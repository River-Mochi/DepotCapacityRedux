// File: Systems/ServiceVehiclesSystem.cs
// Purpose: Apply non-transit service vehicle capacity multipliers (cargo stations, delivery trucks,
//          maintenance vehicles) based on current settings.
// Notes:
// - Run-once system: enabled on city load or when settings Apply() toggles it.
// - Uses PrefabSystem + PrefabBase to read vanilla/base values so results do NOT stack.
// - Writes back vanilla values when scalar is 1x (fixes "stuck doubled" issue).
// - Treats 0 base values as VALID for certain prefab types (tractors, sub-prefabs, etc.) and skips them.

namespace AdjustTransitCapacity
{
    using System;
    using System.Collections.Generic;
    using Colossal.Serialization.Entities;
    using Game;
    using Game.Companies;
    using Game.Prefabs;
    using Game.SceneFlow;
    using Game.Simulation;      // MaintenanceType
    using Unity.Entities;

    public sealed partial class ServiceVehiclesSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem = null!;

        // Prefab base caches (per city) — prevents stacking and keeps results stable.
        private Dictionary<Entity, int> m_CargoStationBaseMaxTransports = null!;
        private Dictionary<Entity, int> m_DeliveryTruckBaseCargoCapacity = null!;
        private Dictionary<Entity, (int Cap, int Rate)> m_MaintenanceBase = null!;
        private Dictionary<Entity, int> m_MaintenanceDepotBaseVehicleCapacity = null!;

        private bool m_DidVerboseScanThisRun; // one-time scan guard when Verbose is enabled

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            m_CargoStationBaseMaxTransports = new Dictionary<Entity, int>();
            m_DeliveryTruckBaseCargoCapacity = new Dictionary<Entity, int>();
            m_MaintenanceBase = new Dictionary<Entity, (int Cap, int Rate)>();
            m_MaintenanceDepotBaseVehicleCapacity = new Dictionary<Entity, int>();

            EntityQuery anyPrefabQuery = SystemAPI.QueryBuilder()
                .WithAll<PrefabData>()
                .WithAny<TransportCompanyData, DeliveryTruckData, MaintenanceVehicleData, MaintenanceDepotData>()
                .Build();

            RequireForUpdate(anyPrefabQuery);

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
            m_MaintenanceBase.Clear();
            m_MaintenanceDepotBaseVehicleCapacity.Clear();
            m_DidVerboseScanThisRun = false;

            Mod.s_Log.Info($"{Mod.ModTag} City Loading Complete -> applying ServiceVehicles settings");
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

            if (verbose && !m_DidVerboseScanThisRun)
            {
                m_DidVerboseScanThisRun = true;
                SystemScan.RunServiceVerboseScan(ref CheckedStateRef, m_PrefabSystem); // helper is try/catch protected
            }

            // -------------------------
            // Maintenance depots: max vehicles (prefabs)
            // -------------------------
            {
                float roadDepotScalar = ClampScalar(settings.RoadMaintenanceDepotScalar);
                float parkDepotScalar = ClampScalar(settings.ParkMaintenanceDepotScalar);

                foreach ((RefRW<MaintenanceDepotData> depotRef, Entity prefabEntity) in SystemAPI
                             .Query<RefRW<MaintenanceDepotData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    ref MaintenanceDepotData data = ref depotRef.ValueRW;

                    MaintenanceType mt = data.m_MaintenanceType;

                    // Skip “None” depots / odd sub-prefabs (log shows these exist).
                    if (mt == MaintenanceType.None)
                    {
                        continue;
                    }

                    int baseVehicles = GetOrCacheMaintenanceDepotBase(prefabEntity, data.m_VehicleCapacity);

                    // 0 is a VALID base for some sub-prefabs; never convert 0 -> 1.
                    if (baseVehicles <= 0 && data.m_VehicleCapacity <= 0)
                    {
                        continue;
                    }

                    bool hasParkFlag = (mt & MaintenanceType.Park) != MaintenanceType.None;
                    bool hasRoadFlags = (mt & (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)) != MaintenanceType.None;

                    bool isPark = hasParkFlag && !hasRoadFlags;
                    string prefabName = GetPrefabName(prefabEntity);

                    // Name fallback only when flags are empty/odd.
                    if (!hasParkFlag && !hasRoadFlags && !string.IsNullOrEmpty(prefabName))
                    {
                        if (prefabName.IndexOf("ParkMaintenance", StringComparison.OrdinalIgnoreCase) >= 0)
                            isPark = true;
                        else if (prefabName.IndexOf("RoadMaintenance", StringComparison.OrdinalIgnoreCase) >= 0)
                            isPark = false;
                    }

                    float scalar = isPark ? parkDepotScalar : roadDepotScalar;
                    int newVehicles = SafeMulIntAllowZero(baseVehicles, scalar);

                    if (newVehicles != data.m_VehicleCapacity)
                    {
                        if (verbose)
                        {
                            string groupLabel = isPark ? "Park" : "Road";
                            Mod.s_Log.Info($"{Mod.ModTag} MaintenanceDepot({groupLabel}) vehicles: '{prefabName}' Type={mt} Base={baseVehicles} x{scalar:0.##} -> {newVehicles}");
                        }

                        data.m_VehicleCapacity = newVehicles;
                    }
                }
            }

            // -------------------------
            // Cargo Stations: max trucks (TransportCompanyData.m_MaxTransports)
            // -------------------------
            {
                float scalar = ClampScalar(settings.CargoStationMaxTrucksScalar);

                foreach ((RefRW<TransportCompanyData> companyRef, Entity prefabEntity) in SystemAPI
                             .Query<RefRW<TransportCompanyData>>()
                             .WithAll<CargoTransportStationData, PrefabData>()
                             .WithEntityAccess())
                {
                    ref TransportCompanyData company = ref companyRef.ValueRW;

                    int baseMax = GetOrCacheCargoStationBase(prefabEntity, company.m_MaxTransports);

                    // 0 is valid for many cargo-harbor sub-prefabs; do not force 1.
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
                float semiScalar = ClampScalar(settings.SemiTruckCargoScalar);
                float vanScalar = ClampScalar(settings.DeliveryVanCargoScalar);
                float rawScalar = ClampScalar(settings.OilTruckCargoScalar);            // UI label may change; keep data stable
                float bikeScalar = ClampScalar(settings.MotorbikeDeliveryCargoScalar);

                foreach ((RefRW<DeliveryTruckData> truckRef, Entity prefabEntity) in SystemAPI
                             .Query<RefRW<DeliveryTruckData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    ref DeliveryTruckData data = ref truckRef.ValueRW;

                    int baseCap = GetOrCacheDeliveryTruckBase(prefabEntity, data.m_CargoCapacity);

                    // Tractor-only / helper prefabs can legitimately be 0 capacity. Never force to 1.
                    if (baseCap <= 0 && data.m_CargoCapacity <= 0)
                    {
                        continue;
                    }

                    string prefabName = GetPrefabName(prefabEntity);

                    // Trailer info via EntityManager (safe + component-checked in SystemScan).
                    SystemScan.GetTrailerTypeInfo(
                        EntityManager,
                        prefabEntity,
                        out bool hasTractor,
                        out CarTrailerType tractorType,
                        out bool hasTrailer,
                        out CarTrailerType trailerType);

                    SystemScan.DeliveryBucket bucket = SystemScan.ClassifyDeliveryTruckPrefab(
                        prefabName,
                        baseCap,
                        data.m_TransportedResources,
                        hasTractor,
                        tractorType,
                        hasTrailer,
                        trailerType);

                    float scalar =
                        bucket == SystemScan.DeliveryBucket.Semi ? semiScalar :
                        bucket == SystemScan.DeliveryBucket.Van ? vanScalar :
                        bucket == SystemScan.DeliveryBucket.RawMaterials ? rawScalar :
                        bucket == SystemScan.DeliveryBucket.Motorbike ? bikeScalar :
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
            // Maintenance vehicles: capacity + rate (prefabs)
            // -------------------------
            {
                float roadCapScalar = ClampScalar(settings.RoadMaintenanceVehicleCapacityScalar);
                float roadRateScalar = ClampScalar(settings.RoadMaintenanceVehicleRateScalar);

                float parkCapScalar = ClampScalar(settings.ParkMaintenanceVehicleCapacityScalar);
                float parkRateScalar = ClampScalar(settings.ParkMaintenanceVehicleRateScalar);

                foreach ((RefRW<MaintenanceVehicleData> mvRef, Entity prefabEntity) in SystemAPI
                             .Query<RefRW<MaintenanceVehicleData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    ref MaintenanceVehicleData data = ref mvRef.ValueRW;

                    MaintenanceType mt = data.m_MaintenanceType;
                    if (mt == MaintenanceType.None)
                    {
                        continue; // defensive; shouldn’t normally exist, but costs nothing to guard
                    }

                    string prefabName = GetPrefabName(prefabEntity);

                    bool hasParkFlag = (mt & MaintenanceType.Park) != MaintenanceType.None;
                    bool hasRoadFlags = (mt & (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)) != MaintenanceType.None;

                    bool isPark = hasParkFlag && !hasRoadFlags;

                    // Name fallback only if flags are ambiguous.
                    if (!hasParkFlag && !hasRoadFlags && !string.IsNullOrEmpty(prefabName))
                    {
                        if (prefabName.IndexOf("ParkMaintenance", StringComparison.OrdinalIgnoreCase) >= 0)
                            isPark = true;
                        else if (prefabName.IndexOf("RoadMaintenance", StringComparison.OrdinalIgnoreCase) >= 0)
                            isPark = false;
                    }

                    float capScalar = isPark ? parkCapScalar : roadCapScalar;
                    float rateScalar = isPark ? parkRateScalar : roadRateScalar;

                    (int Cap, int Rate) baseVals = GetOrCacheMaintenanceBase(prefabEntity, data.m_MaintenanceCapacity, data.m_MaintenanceRate);

                    int newCap = SafeMulInt(baseVals.Cap, capScalar);
                    int newRate = SafeMulInt(baseVals.Rate, rateScalar);

                    if (newCap != data.m_MaintenanceCapacity)
                    {
                        if (verbose)
                        {
                            string groupLabel = isPark ? "Park" : "Road";
                            Mod.s_Log.Info($"{Mod.ModTag} Maintenance({groupLabel}) cap: '{prefabName}' Type={mt} Base={baseVals.Cap} x{capScalar:0.##} -> {newCap}");
                        }

                        data.m_MaintenanceCapacity = newCap;
                    }

                    if (newRate != data.m_MaintenanceRate)
                    {
                        if (verbose)
                        {
                            string groupLabel = isPark ? "Park" : "Road";
                            Mod.s_Log.Info($"{Mod.ModTag} Maintenance({groupLabel}) rate: '{prefabName}' Type={mt} Base={baseVals.Rate} x{rateScalar:0.##} -> {newRate}");
                        }

                        data.m_MaintenanceRate = newRate;
                    }
                }
            }

            Enabled = false; // run-once behavior
        }

        // -------------------------
        // Helpers
        // -------------------------

        private static float ClampScalar(float scalar)
        {
            if (scalar < Setting.ServiceMinScalar)
                return Setting.ServiceMinScalar;
            if (scalar > Setting.ServiceMaxScalar)
                return Setting.ServiceMaxScalar;
            return scalar;
        }

        private static int SafeMulInt(int baseValue, float scalar)
        {
            if (baseValue < 1)
                baseValue = 1;            // for systems where 0 is never meaningful
            int v = (int)(baseValue * scalar);
            return v < 1 ? 1 : v;
        }

        private static int SafeMulIntAllowZero(int baseValue, float scalar)
        {
            if (baseValue <= 0)
                return 0;                // preserve 0 (tractor/sub-prefab/etc.)
            int v = (int)(baseValue * scalar);
            return v < 1 ? 1 : v;
        }

        private int GetOrCacheCargoStationBase(Entity prefabEntity, int currentValue)
        {
            if (m_CargoStationBaseMaxTransports.TryGetValue(prefabEntity, out int baseMax))
            {
                return baseMax;
            }

            int vanilla = 0;
            if (TryGetCargoStationVanillaMax(prefabEntity, out vanilla) && vanilla > 0)
                baseMax = vanilla;
            else
                baseMax = currentValue; // can be 0 for sub-prefabs; that’s valid

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

            int vanilla = 0;
            if (TryGetDeliveryTruckVanillaCargo(prefabEntity, out vanilla) && vanilla >= 0)
                baseCap = vanilla; // allow 0 (tractor-only) as a valid vanilla base
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

        private int GetOrCacheMaintenanceDepotBase(Entity prefabEntity, int currentValue)
        {
            if (m_MaintenanceDepotBaseVehicleCapacity.TryGetValue(prefabEntity, out int baseVehicles))
            {
                return baseVehicles;
            }

            int vanilla = 0;
            if (TryGetMaintenanceDepotVanilla(prefabEntity, out vanilla) && vanilla >= 0)
                baseVehicles = vanilla; // allow 0 as valid
            else
                baseVehicles = currentValue;

            m_MaintenanceDepotBaseVehicleCapacity[prefabEntity] = baseVehicles;
            return baseVehicles;
        }

        private bool TryGetMaintenanceDepotVanilla(Entity prefabEntity, out int baseVehicles)
        {
            baseVehicles = 0;
            if (m_PrefabSystem == null)
                return false;
            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
                return false;
            if (!prefabBase.TryGet(out Game.Prefabs.MaintenanceDepot depot))
                return false;

            baseVehicles = depot.m_VehicleCapacity;
            return true;
        }

        private (int Cap, int Rate) GetOrCacheMaintenanceBase(Entity prefabEntity, int currentCap, int currentRate)
        {
            if (m_MaintenanceBase.TryGetValue(prefabEntity, out (int Cap, int Rate) baseVals))
            {
                return baseVals;
            }

            int baseCap = 0, baseRate = 0;

            if (TryGetMaintenanceVanilla(prefabEntity, out baseCap, out baseRate))
            {
                // ok
            }
            else
            {
                baseCap = currentCap;
                baseRate = currentRate;
            }

            if (baseCap < 1)
                baseCap = 1;   // maintenance vehicles should never be 0 capacity
            if (baseRate < 1)
                baseRate = 1;

            baseVals = (baseCap, baseRate);
            m_MaintenanceBase[prefabEntity] = baseVals;
            return baseVals;
        }

        private bool TryGetMaintenanceVanilla(Entity prefabEntity, out int baseCap, out int baseRate)
        {
            baseCap = 0;
            baseRate = 0;

            if (m_PrefabSystem == null)
                return false;
            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
                return false;
            if (!prefabBase.TryGet(out Game.Prefabs.MaintenanceVehicle mv))
                return false;

            baseCap = mv.m_MaintenanceCapacity;
            baseRate = mv.m_MaintenanceRate;
            return true;
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
            catch
            {
                // ignore (never crash a city because of logging)
            }

            return $"PrefabEntity={prefabEntity.Index}:{prefabEntity.Version}";
        }
    }
}
