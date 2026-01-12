// File: Systems/IndustrySystem.cs
// Purpose: Apply industry/logistics tuning based on current settings:
//          - Extractor fleet max trucks (TransportCompanyData.m_MaxTransports for industrial companies)
//          - Cargo station max fleet (TransportCompanyData.m_MaxTransports for CargoTransportStationData)
//          - Delivery truck cargo capacity (DeliveryTruckData.m_CargoCapacity)

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
                // Cargo station slider is 1x..5x. Clamp to that range.
                float scalar = ScalarMath.ClampScalar(settings.CargoStationMaxTrucksScalar, Setting.CargoStationMinScalar, Setting.CargoStationMaxScalar);

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

                    int newMax = ScalarMath.ScaleIntRoundedAllowZeroMin1(baseMax, scalar);

                    if (newMax != company.m_MaxTransports)
                    {
                        if (verbose)
                        {
                            Mod.s_Log.Info($"{Mod.ModTag} CargoStation max trucks: '{PrefabNameUtil.GetNameSafe(m_PrefabSystem, prefabEntity)}' Base={baseMax} x{scalar:0.##} -> {newMax}");
                        }

                        company.m_MaxTransports = newMax;
                    }
                }
            }

            // -------------------------
            // Delivery trucks: buckets (semi / vans / raw materials / motorbikes)
            // -------------------------
            {
                float semiScalar = ScalarMath.ClampScalar(settings.SemiTruckCargoScalar, Setting.ServiceMinScalar, Setting.ServiceMaxScalar);
                float vanScalar = ScalarMath.ClampScalar(settings.DeliveryVanCargoScalar, Setting.ServiceMinScalar, Setting.ServiceMaxScalar);
                float rawScalar = ScalarMath.ClampScalar(settings.OilTruckCargoScalar, Setting.ServiceMinScalar, Setting.ServiceMaxScalar);
                float mbikeScalar = ScalarMath.ClampScalar(settings.MotorbikeDeliveryCargoScalar, Setting.ServiceMinScalar, Setting.ServiceMaxScalar);

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

                    string prefabName = PrefabNameUtil.GetNameSafe(m_PrefabSystem, prefabEntity);

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

                    int newCap = ScalarMath.ScaleIntRoundedAllowZeroMin1(baseCap, scalar);

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
                // Also clamp extractors to 1x..5x (same slider range).
                float scalarF = ScalarMath.ClampScalar(
                      settings.ExtractorMaxTrucksScalar,
                      Setting.CargoStationMinScalar,
                      Setting.CargoStationMaxScalar);

                int scalar = (int)Math.Round(scalarF);
                scalar = ScalarMath.ClampInt(scalar, 1, (int)Setting.CargoStationMaxScalar);

                int matched = 0;
                int changed = 0;
                int skippedZero = 0;

                foreach ((RefRW<TransportCompanyData> tcRef, Entity prefabEntity) in SystemAPI
                             .Query<RefRW<TransportCompanyData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    string name = PrefabNameUtil.GetNameSafe(m_PrefabSystem, prefabEntity);
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

                if (verbose || changed > 0)
                {
                    Mod.s_Log.Info($"{Mod.ModTag} Extractor trucks: scalar={scalar} matched={matched} changed={changed} skippedZero={skippedZero}");
                }
            }

            Enabled = false;
        }

        private static bool IsTargetIndustrialCompany(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (s_KnownIndustrialCompanies.Contains(name))
            {
                return true;
            }

            if (name.StartsWith("Industrial_", StringComparison.OrdinalIgnoreCase) &&
                name.IndexOf("Extractor", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            return false;
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

            if (!PrefabComponentUtil.TryGetComponent(m_PrefabSystem, prefabEntity, out CargoTransportStation station))
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

            if (!PrefabComponentUtil.TryGetComponent(m_PrefabSystem, prefabEntity, out Game.Prefabs.DeliveryTruck truck))
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

            baseMax = currentValue;

            m_ExtractorCompanyBaseMaxTransports[prefabEntity] = baseMax;
            return baseMax;
        }
    }
}
