// File: Systems/IndustrySystem.cs
// Purpose: Apply industry/logistics tuning based on current settings:
//          - Extractor fleet max trucks (TransportCompanyData.m_MaxTransports for industrial companies)
//          - Cargo station max fleet (TransportCompanyData.m_MaxTransports for CargoTransportStationData)
//          - Delivery truck cargo capacity (DeliveryTruckData.m_CargoCapacity)
// Notes:
// - Run-once system: enabled on city load or when settings Apply() enables it.
// - Uses PrefabSystem + PrefabBase to read vanilla/base values (no stacking).
// - Extractor targeting tightened to known industrial company prefab names + safe fallback for Industrial_*Extractor*.

namespace DispatchBoss
{
    using Colossal.Serialization.Entities;
    using Game;
    using Game.Companies;
    using Game.Prefabs;
    using Game.SceneFlow;
    using System;
    using System.Collections.Generic;
    using Unity.Entities;

    public sealed partial class IndustrySystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem = null!;

        // Prefab base caches (per city/session) — prevents stacking.
        private Dictionary<Entity, int> m_CargoStationBaseMaxTransports = null!;
        private Dictionary<Entity, int> m_DeliveryTruckBaseCargoCapacity = null!;
        private Dictionary<Entity, int> m_ExtractorCompanyBaseMaxTransports = null!;

        private static bool s_LoggedPrefabNameException;

        // TransportCompany prefabs
        private static readonly HashSet<string> s_KnownIndustrialCompanies = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Industrial_FishExtractor", "Industrial_ForestryExtractor", "Industrial_GrainExtractor", "Industrial_OreExtractor", "Industrial_OilExtractor",
            "Industrial_VegetableExtractor", "Industrial_LivestockExtractor", "Industrial_CottonExtractor", "Industrial_CoalMine",
            "Industrial_StoneQuarry", "Industrial_MineralPlant", "Industrial_WarehouseStone", "Industrial_WarehouseCoal", "Industrial_WarehouseMinerals",
        };

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            m_CargoStationBaseMaxTransports = new Dictionary<Entity, int>();
            m_DeliveryTruckBaseCargoCapacity = new Dictionary<Entity, int>();
            m_ExtractorCompanyBaseMaxTransports = new Dictionary<Entity, int>();

            EntityQuery anyRelevantPrefabQuery = SystemAPI.QueryBuilder()
                .WithAll<PrefabData>()
                .WithAny<TransportCompanyData, DeliveryTruckData>()
                .Build();

            RequireForUpdate(anyRelevantPrefabQuery);

            Enabled = false;
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            bool isRealGame =
                mode == GameMode.Game &&
                (purpose == Purpose.NewGame || purpose == Purpose.LoadGame);

            if (!isRealGame)
            {
                return;
            }

            m_CargoStationBaseMaxTransports.Clear();
            m_DeliveryTruckBaseCargoCapacity.Clear();
            m_ExtractorCompanyBaseMaxTransports.Clear();

            Mod.s_Log.Info($"{Mod.ModTag} City Loading Complete -> applying Industry settings");
            Enabled = true;
        }

        protected override void OnUpdate()
        {
            GameManager gm = GameManager.instance;
            if (gm == null || !gm.gameMode.IsGame())
            {
                Enabled = false;
                return;
            }

            if (Mod.Settings == null)
            {
                Enabled = false;
                return;
            }

            Setting settings = Mod.Settings;
            bool verbose = settings.EnableDebugLogging;

            // -------------------------
            // Cargo Stations: max trucks (TransportCompanyData.m_MaxTransports)
            // -------------------------
            {
                float scalar = ClampServiceScalar(settings.CargoStationMaxTrucksScalar);

                foreach ((RefRW<TransportCompanyData> companyRef, Entity prefabEntity) in SystemAPI
                             .Query<RefRW<TransportCompanyData>>()
                             .WithAll<CargoTransportStationData, PrefabData>()
                             .WithEntityAccess())
                {
                    ref TransportCompanyData company = ref companyRef.ValueRW;

                    int baseMax = GetOrCacheCargoStationBase(prefabEntity, company.m_MaxTransports);

                    if (baseMax <= 0 && company.m_MaxTransports <= 0)
                    {
                        continue;
                    }

                    int newMax = SafeMulIntAllowZero(baseMax, scalar);

                    if (newMax != company.m_MaxTransports)
                    {
                        if (verbose)
                        {
                            Mod.s_Log.Info($"{Mod.ModTag} CargoStation max trucks: '{GetPrefabName(prefabEntity)}' Base={baseMax} x{scalar:0.##} -> {newMax}");
                        }

                        company.m_MaxTransports = newMax;
                    }
                }
            }

            // -------------------------
            // Delivery trucks: buckets (semi / vans / raw materials / motorbikes)
            // -------------------------
            {
                float semiScalar = ClampServiceScalar(settings.SemiTruckCargoScalar);
                float vanScalar = ClampServiceScalar(settings.DeliveryVanCargoScalar);
                float rawScalar = ClampServiceScalar(settings.OilTruckCargoScalar);
                float mbikeScalar = ClampServiceScalar(settings.MotorbikeDeliveryCargoScalar);

                foreach ((RefRW<DeliveryTruckData> truckRef, Entity prefabEntity) in SystemAPI
                             .Query<RefRW<DeliveryTruckData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    ref DeliveryTruckData data = ref truckRef.ValueRW;

                    int baseCap = GetOrCacheDeliveryTruckBase(prefabEntity, data.m_CargoCapacity);

                    if (baseCap <= 0 && data.m_CargoCapacity <= 0)
                    {
                        continue;
                    }

                    string prefabName = GetPrefabName(prefabEntity);

                    VehicleHelpers.GetTrailerTypeInfo(
                        EntityManager,
                        prefabEntity,
                        out bool hasTractor,
                        out CarTrailerType tractorType,
                        out bool hasTrailer,
                        out CarTrailerType trailerType);

                    VehicleHelpers.DeliveryBucket bucket = VehicleHelpers.ClassifyDeliveryTruckPrefab(
                        prefabName,
                        baseCap,
                        data.m_TransportedResources,
                        hasTractor,
                        tractorType,
                        hasTrailer,
                        trailerType);

                    float scalar =
                        bucket == VehicleHelpers.DeliveryBucket.Semi ? semiScalar :
                        bucket == VehicleHelpers.DeliveryBucket.Van ? vanScalar :
                        bucket == VehicleHelpers.DeliveryBucket.RawMaterials ? rawScalar :
                        bucket == VehicleHelpers.DeliveryBucket.Motorbike ? mbikeScalar :
                        1f;

                    int newCap = SafeMulIntAllowZero(baseCap, scalar);

                    if (newCap != data.m_CargoCapacity)
                    {
                        if (verbose)
                        {
                            Mod.s_Log.Info($"{Mod.ModTag} Delivery cargo: '{prefabName}' Bucket={bucket} Base={baseCap} x{scalar:0.##} -> {newCap} Resources={data.m_TransportedResources}");
                        }

                        data.m_CargoCapacity = newCap;
                    }
                }
            }

            // -------------------------
            // Industrial extractor transport companies: max fleet (TransportCompanyData.m_MaxTransports)
            // -------------------------
            {
                float scalarF = settings.ExtractorMaxTrucksScalar;
                if (scalarF <= 0f)
                {
                    scalarF = 1f;
                }

                int scalar = (int)Math.Round(scalarF);
                if (scalar < 1) scalar = 1;
                if (scalar > (int)Setting.CargoStationMaxScalar) scalar = (int)Setting.CargoStationMaxScalar;

                int matched = 0;
                int changed = 0;
                int skippedZero = 0;

                foreach ((RefRW<TransportCompanyData> tcRef, Entity prefabEntity) in SystemAPI
                             .Query<RefRW<TransportCompanyData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    string name = GetPrefabName(prefabEntity);
                    if (!IsTargetIndustrialCompany(name))
                    {
                        continue;
                    }

                    ref TransportCompanyData tc = ref tcRef.ValueRW;

                    int baseMax = GetOrCacheExtractorCompanyBase(prefabEntity, tc.m_MaxTransports);

                    if (baseMax == 0)
                    {
                        skippedZero++;
                        continue;
                    }

                    matched++;

                    int desired = baseMax * scalar;
                    if (desired < 0) desired = 0;

                    if (tc.m_MaxTransports != desired)
                    {
                        tc.m_MaxTransports = desired;
                        changed++;

                        if (verbose)
                        {
                            Mod.s_Log.Info($"{Mod.ModTag} Extractor trucks: '{name}' Base={baseMax} x{scalar} -> {desired}");
                        }
                    }
                }

                Mod.s_Log.Info($"{Mod.ModTag} Extractor trucks: scalar={scalar} matched={matched} changed={changed} skippedZero={skippedZero}");
            }

            Enabled = false;
        }

        // -------------------------
        // Targeting
        // -------------------------

        private static bool IsTargetIndustrialCompany(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Known set (from scan report)
            if (s_KnownIndustrialCompanies.Contains(name))
            {
                return true;
            }

            // Safe fallback: future additions that follow the obvious naming pattern
            if (name.StartsWith("Industrial_", StringComparison.OrdinalIgnoreCase) &&
                name.IndexOf("Extractor", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            return false;
        }

        // -------------------------
        // Helpers
        // -------------------------

        private static float ClampServiceScalar(float scalar)
        {
            if (scalar < Setting.ServiceMinScalar)
                return Setting.ServiceMinScalar;
            if (scalar > Setting.ServiceMaxScalar)
                return Setting.ServiceMaxScalar;
            return scalar;
        }

        private static int SafeMulIntAllowZero(int baseValue, float scalar)
        {
            if (baseValue <= 0)
                return 0;

            int v = (int)(baseValue * scalar);
            return v < 1 ? 1 : v;
        }

        private int GetOrCacheCargoStationBase(Entity prefabEntity, int currentValue)
        {
            if (m_CargoStationBaseMaxTransports.TryGetValue(prefabEntity, out int baseMax))
            {
                return baseMax;
            }

            int vanilla;
            if (TryGetCargoStationVanillaMax(prefabEntity, out vanilla) && vanilla > 0)
                baseMax = vanilla;
            else
                baseMax = currentValue;

            m_CargoStationBaseMaxTransports[prefabEntity] = baseMax;
            return baseMax;
        }

        private bool TryGetCargoStationVanillaMax(Entity prefabEntity, out int baseMax)
        {
            baseMax = 0;

            if (m_PrefabSystem == null)
                return false;
            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
                return false;
            if (!prefabBase.TryGet(out CargoTransportStation station))
                return false;

            baseMax = station.transports;
            return true;
        }

        private int GetOrCacheDeliveryTruckBase(Entity prefabEntity, int currentValue)
        {
            if (m_DeliveryTruckBaseCargoCapacity.TryGetValue(prefabEntity, out int baseCap))
            {
                return baseCap;
            }

            int vanilla;
            if (TryGetDeliveryTruckVanillaCargo(prefabEntity, out vanilla) && vanilla >= 0)
                baseCap = vanilla;
            else
                baseCap = currentValue;

            m_DeliveryTruckBaseCargoCapacity[prefabEntity] = baseCap;
            return baseCap;
        }

        private bool TryGetDeliveryTruckVanillaCargo(Entity prefabEntity, out int baseCap)
        {
            baseCap = 0;

            if (m_PrefabSystem == null)
                return false;
            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
                return false;
            if (!prefabBase.TryGet(out Game.Prefabs.DeliveryTruck truck))
                return false;

            baseCap = truck.m_CargoCapacity;
            return true;
        }

        private int GetOrCacheExtractorCompanyBase(Entity prefabEntity, int currentValue)
        {
            if (m_ExtractorCompanyBaseMaxTransports.TryGetValue(prefabEntity, out int baseMax))
            {
                return baseMax;
            }

            // Capture the first value seen this session (usually vanilla or whatever another mod set).
            baseMax = currentValue;

            m_ExtractorCompanyBaseMaxTransports[prefabEntity] = baseMax;
            return baseMax;
        }

        private string GetPrefabName(Entity prefabEntity)
        {
            try
            {
                if (m_PrefabSystem != null && m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
                {
                    return prefabBase.name ?? "(unnamed)";
                }
            }
            catch (Exception ex)
            {
                if (!s_LoggedPrefabNameException)
                {
                    s_LoggedPrefabNameException = true;
                    Mod.s_Log.Warn($"{Mod.ModTag} GetPrefabName failed once: {ex.GetType().Name}: {ex.Message}");
                }
            }

            return $"PrefabEntity={prefabEntity.Index}:{prefabEntity.Version}";
        }
    }
}
