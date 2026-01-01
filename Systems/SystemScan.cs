// File: Systems/SystemScan.cs
// Purpose: Centralized verbose prefab scanning + classification helpers for Adjust Transit Capacity.
// Notes:
// - Helper (NOT a system). Called from GameSystemBase systems when Verbose is enabled.
// - Uses EntityQueryBuilder.Build(ref SystemState) (Entities 1.3).
// - Logs are deduped and capped so Verbose remains usable.

namespace AdjustTransitCapacity
{
    using System;
    using System.Collections.Generic;
    using Game.Economy;
    using Game.Prefabs;
    using Unity.Collections;
    using Unity.Entities;

    public static class SystemScan
    {
        public enum DeliveryBucket
        {
            Other = 0,
            Semi = 1,
            Van = 2,
            RawMaterials = 3,
            Motorbike = 4,
        }

        // Keep this conservative; keyword scan can explode logs fast.
        private const int kMaxKeywordMatches = 600;

        public static void RunServiceVerboseScan(ref SystemState state, PrefabSystem? prefabSystem)
        {
            // Null shouldn’t normally happen (PrefabSystem exists in Game world),
            // but accepting nullable avoids NRE risk + satisfies nullability analysis.
            if (prefabSystem == null)
            {
                Mod.s_Log.Warn($"{Mod.ModTag} Verbose: SystemScan skipped (PrefabSystem is null)");
                return;
            }

            try
            {
                Mod.s_Log.Info($"{Mod.ModTag} Verbose: SystemScan starting (Services)");

                ScanDeliveryTruckPrefabs(ref state, prefabSystem);
                ScanMaintenanceVehiclePrefabs(ref state, prefabSystem);
                ScanMaintenanceDepotPrefabs(ref state, prefabSystem);
                ScanCargoStationPrefabs(ref state, prefabSystem);

                ScanPrefabNamesForKeywords(ref state, prefabSystem);

                Mod.s_Log.Info($"{Mod.ModTag} Verbose: SystemScan complete (Services)");
            }
            catch (Exception ex)
            {
                Mod.s_Log.Warn($"{Mod.ModTag} Verbose: SystemScan failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        // ------------------------------------------------------------
        // Helpers used by ServiceVehiclesSystem
        // ------------------------------------------------------------

        public static void GetTrailerTypeInfo(
            EntityManager entityManager,
            Entity prefabEntity,
            out bool hasTractor,
            out CarTrailerType tractorTrailerType,
            out bool hasTrailer,
            out CarTrailerType trailerTrailerType)
        {
            hasTractor = false;
            hasTrailer = false;

            tractorTrailerType = CarTrailerType.None;
            trailerTrailerType = CarTrailerType.None;

            // EntityManager.Exists is cheap and prevents edge-case exceptions if a query result is stale.
            if (!entityManager.Exists(prefabEntity))
            {
                return;
            }

            if (entityManager.HasComponent<CarTractorData>(prefabEntity))
            {
                hasTractor = true;
                CarTractorData tractor = entityManager.GetComponentData<CarTractorData>(prefabEntity);
                tractorTrailerType = tractor.m_TrailerType;
            }

            if (entityManager.HasComponent<CarTrailerData>(prefabEntity))
            {
                hasTrailer = true;
                CarTrailerData trailer = entityManager.GetComponentData<CarTrailerData>(prefabEntity);
                trailerTrailerType = trailer.m_TrailerType;
            }
        }

        public static DeliveryBucket ClassifyDeliveryTruckPrefab(
            string prefabName,
            int baseCap,
            Resource transportedResources,
            bool hasTractor,
            CarTrailerType tractorTrailerType,
            bool hasTrailer,
            CarTrailerType trailerTrailerType)
        {
            // IMPORTANT: baseCap==0 is valid for “tractor-only” prefabs (no cargo).
            // We do NOT want to bucket these into anything that might later “force” capacity.
            if (baseCap <= 0)
            {
                return DeliveryBucket.Other;
            }

            bool isSemi =
                (hasTractor && tractorTrailerType == CarTrailerType.Semi) ||
                (hasTrailer && trailerTrailerType == CarTrailerType.Semi);

            if (isSemi)
            {
                return DeliveryBucket.Semi;
            }

            // Small-capacity motorbike delivery
            if ((baseCap > 0 && baseCap <= 200) &&
                prefabName.IndexOf("Motorbike", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return DeliveryBucket.Motorbike;
            }

            // Vans (vanilla is typically 4000)
            if (baseCap == 4000 ||
                prefabName.IndexOf("DeliveryVan", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return DeliveryBucket.Van;
            }

            // Raw materials dump trucks (vanilla often 20000)
            if (baseCap == 20000 ||
                prefabName.IndexOf("OilTruck", StringComparison.OrdinalIgnoreCase) >= 0 ||
                prefabName.IndexOf("CoalTruck", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return DeliveryBucket.RawMaterials;
            }

            return DeliveryBucket.Other;
        }

        // ------------------------------------------------------------
        // Verbose scans (per prefab category)
        // ------------------------------------------------------------

        private static void ScanDeliveryTruckPrefabs(ref SystemState state, PrefabSystem prefabSystem)
        {
            EntityManager em = state.EntityManager;

            using var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<PrefabData>()
                .WithAll<DeliveryTruckData>();

            EntityQuery query = builder.Build(ref state);
            using NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp);

            int total = 0, semi = 0, van = 0, raw = 0, bike = 0, other = 0;

            Mod.s_Log.Info($"{Mod.ModTag} Verbose: DeliveryTruck prefab scan (count={entities.Length})");

            for (int i = 0; i < entities.Length; i++)
            {
                Entity prefabEntity = entities[i];
                total++;

                DeliveryTruckData data = em.GetComponentData<DeliveryTruckData>(prefabEntity);

                string name = GetPrefabName(prefabSystem, prefabEntity);
                int baseCap = TryGetDeliveryTruckBase(prefabSystem, prefabEntity, fallback: data.m_CargoCapacity);

                GetTrailerTypeInfo(
                    em,
                    prefabEntity,
                    out bool hasTractor,
                    out CarTrailerType tractorType,
                    out bool hasTrailer,
                    out CarTrailerType trailerType);

                DeliveryBucket bucket = ClassifyDeliveryTruckPrefab(
                    name,
                    baseCap,
                    data.m_TransportedResources,
                    hasTractor,
                    tractorType,
                    hasTrailer,
                    trailerType);

                switch (bucket)
                {
                    case DeliveryBucket.Semi:
                        semi++;
                        break;
                    case DeliveryBucket.Van:
                        van++;
                        break;
                    case DeliveryBucket.RawMaterials:
                        raw++;
                        break;
                    case DeliveryBucket.Motorbike:
                        bike++;
                        break;
                    default:
                        other++;
                        break;
                }

                string tractorInfo = hasTractor ? tractorType.ToString() : "none";
                string trailerInfo = hasTrailer ? trailerType.ToString() : "none";

                Mod.s_Log.Info(
                    $"{Mod.ModTag} Verbose: DeliveryPrefab '{name}' " +
                    $"BaseCap={baseCap} CurCap={data.m_CargoCapacity} Resources={data.m_TransportedResources} " +
                    $"TractorType={tractorInfo} TrailerType={trailerInfo} Bucket={bucket}");
            }

            Mod.s_Log.Info(
                $"{Mod.ModTag} Verbose: DeliveryTruck summary: Total={total} Semi={semi} Van={van} RawMaterials={raw} Motorbike={bike} Other={other}");
        }

        private static void ScanMaintenanceVehiclePrefabs(ref SystemState state, PrefabSystem prefabSystem)
        {
            EntityManager em = state.EntityManager;

            using var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<PrefabData>()
                .WithAll<MaintenanceVehicleData>();

            EntityQuery query = builder.Build(ref state);
            using NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp);

            Mod.s_Log.Info($"{Mod.ModTag} Verbose: MaintenanceVehicle prefab scan (count={entities.Length})");

            for (int i = 0; i < entities.Length; i++)
            {
                Entity prefabEntity = entities[i];
                MaintenanceVehicleData data = em.GetComponentData<MaintenanceVehicleData>(prefabEntity);

                string name = GetPrefabName(prefabSystem, prefabEntity);

                int baseCap = data.m_MaintenanceCapacity;
                int baseRate = data.m_MaintenanceRate;

                // Prefer prefab base values (non-stacking).
                if (prefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase) &&
                    prefabBase.TryGet(out Game.Prefabs.MaintenanceVehicle mv))
                {
                    baseCap = mv.m_MaintenanceCapacity;
                    baseRate = mv.m_MaintenanceRate;
                }

                Mod.s_Log.Info(
                    $"{Mod.ModTag} Verbose: MaintenanceVehicle '{name}' Type={data.m_MaintenanceType} " +
                    $"BaseCap={baseCap} CurCap={data.m_MaintenanceCapacity} BaseRate={baseRate} CurRate={data.m_MaintenanceRate}");
            }
        }

        private static void ScanMaintenanceDepotPrefabs(ref SystemState state, PrefabSystem prefabSystem)
        {
            EntityManager em = state.EntityManager;

            using var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<PrefabData>()
                .WithAll<MaintenanceDepotData>();

            EntityQuery query = builder.Build(ref state);
            using NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp);

            Mod.s_Log.Info($"{Mod.ModTag} Verbose: MaintenanceDepot prefab scan (count={entities.Length})");

            for (int i = 0; i < entities.Length; i++)
            {
                Entity prefabEntity = entities[i];
                MaintenanceDepotData data = em.GetComponentData<MaintenanceDepotData>(prefabEntity);

                string name = GetPrefabName(prefabSystem, prefabEntity);

                int baseVehicles = data.m_VehicleCapacity;

                if (prefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase) &&
                    prefabBase.TryGet(out Game.Prefabs.MaintenanceDepot depot))
                {
                    baseVehicles = depot.m_VehicleCapacity;
                }

                Mod.s_Log.Info(
                    $"{Mod.ModTag} Verbose: MaintenanceDepot '{name}' Type={data.m_MaintenanceType} " +
                    $"BaseVehicles={baseVehicles} CurVehicles={data.m_VehicleCapacity}");
            }
        }

        private static void ScanCargoStationPrefabs(ref SystemState state, PrefabSystem prefabSystem)
        {
            EntityManager em = state.EntityManager;

            using var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<PrefabData>()
                .WithAll<CargoTransportStationData>()
                .WithAll<Game.Companies.TransportCompanyData>();

            EntityQuery query = builder.Build(ref state);
            using NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp);

            Mod.s_Log.Info($"{Mod.ModTag} Verbose: CargoStation prefab scan (count={entities.Length})");

            for (int i = 0; i < entities.Length; i++)
            {
                Entity prefabEntity = entities[i];
                var company = em.GetComponentData<Game.Companies.TransportCompanyData>(prefabEntity);

                string name = GetPrefabName(prefabSystem, prefabEntity);

                int baseMax = company.m_MaxTransports;

                if (prefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase) &&
                    prefabBase.TryGet(out CargoTransportStation station))
                {
                    baseMax = station.transports;
                }

                Mod.s_Log.Info(
                    $"{Mod.ModTag} Verbose: CargoStation '{name}' BaseMaxTransports={baseMax} CurMaxTransports={company.m_MaxTransports}");
            }
        }

        // ------------------------------------------------------------
        // Keyword scan (deduped + capped)
        // ------------------------------------------------------------

        private static void ScanPrefabNamesForKeywords(ref SystemState state, PrefabSystem prefabSystem)
        {
            EntityManager em = state.EntityManager;

            // Discovery keywords (avoid super-generic words; they match thousands of prefabs).
            string[] keywords = new[]
            {
                // Delivery vehicles
                "oiltruck", "coaltruck", "deliveryvan", "trucktractor", "motorbikedelivery", "truck",

                // Maintenance
                "roadmaintenance", "parkmaintenance", "snowplow",

                // Waste-ish probes (name only)
                "industrialwaste", "hazard", "wasteprocessing",

                // Other probes you mentioned
                "maintenancevehicle", "maintenance",

                // Postal (identify/exclude later if ever needed)
                "postvan", "posttruck"
            };

            using var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<PrefabData>();

            EntityQuery query = builder.Build(ref state);
            using NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp);

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int matches = 0;

            Mod.s_Log.Info($"{Mod.ModTag} Verbose: Prefab keyword scan (prefabs={entities.Length}, cap={kMaxKeywordMatches})");

            for (int i = 0; i < entities.Length; i++)
            {
                if (matches >= kMaxKeywordMatches)
                {
                    Mod.s_Log.Warn($"{Mod.ModTag} Verbose: Prefab keyword scan hit cap ({kMaxKeywordMatches}). Narrow keywords or raise cap if needed.");
                    break;
                }

                Entity prefabEntity = entities[i];
                string name = GetPrefabName(prefabSystem, prefabEntity);
                if (string.IsNullOrEmpty(name) || name == "(unnamed)")
                {
                    continue;
                }

                string lower = name.ToLowerInvariant();

                string? hit = null;
                for (int k = 0; k < keywords.Length; k++)
                {
                    if (lower.Contains(keywords[k]))
                    {
                        hit = keywords[k];
                        break;
                    }
                }

                if (hit == null)
                {
                    continue;
                }

                // Dedupe by prefab name (most useful for logs).
                if (!seen.Add(name))
                {
                    continue;
                }

                matches++;

                Mod.s_Log.Info($"{Mod.ModTag} Verbose: KeywordMatch '{name}' (hit='{hit}')"); // compact header

                // Add useful details only when present (prevents log spam).
                if (em.HasComponent<DeliveryTruckData>(prefabEntity))
                {
                    DeliveryTruckData dt = em.GetComponentData<DeliveryTruckData>(prefabEntity);
                    int baseCap = TryGetDeliveryTruckBase(prefabSystem, prefabEntity, fallback: dt.m_CargoCapacity);
                    Mod.s_Log.Info($"{Mod.ModTag} Verbose:  - DeliveryTruckData BaseCap={baseCap} CurCap={dt.m_CargoCapacity} Resources={dt.m_TransportedResources}");
                }

                if (em.HasComponent<MaintenanceVehicleData>(prefabEntity))
                {
                    MaintenanceVehicleData mv = em.GetComponentData<MaintenanceVehicleData>(prefabEntity);
                    int baseCap = mv.m_MaintenanceCapacity, baseRate = mv.m_MaintenanceRate;

                    if (prefabSystem.TryGetPrefab(prefabEntity, out PrefabBase pb) &&
                        pb.TryGet(out Game.Prefabs.MaintenanceVehicle mvBase))
                    {
                        baseCap = mvBase.m_MaintenanceCapacity;
                        baseRate = mvBase.m_MaintenanceRate;
                    }

                    Mod.s_Log.Info($"{Mod.ModTag} Verbose:  - MaintenanceVehicleData Type={mv.m_MaintenanceType} BaseCap={baseCap} CurCap={mv.m_MaintenanceCapacity} BaseRate={baseRate} CurRate={mv.m_MaintenanceRate}");
                }

                if (em.HasComponent<MaintenanceDepotData>(prefabEntity))
                {
                    MaintenanceDepotData md = em.GetComponentData<MaintenanceDepotData>(prefabEntity);
                    int baseVehicles = md.m_VehicleCapacity;

                    if (prefabSystem.TryGetPrefab(prefabEntity, out PrefabBase pb2) &&
                        pb2.TryGet(out Game.Prefabs.MaintenanceDepot depotBase))
                    {
                        baseVehicles = depotBase.m_VehicleCapacity;
                    }

                    Mod.s_Log.Info($"{Mod.ModTag} Verbose:  - MaintenanceDepotData Type={md.m_MaintenanceType} BaseVehicles={baseVehicles} CurVehicles={md.m_VehicleCapacity}");
                }

                if (em.HasComponent<CargoTransportStationData>(prefabEntity) &&
                    em.HasComponent<Game.Companies.TransportCompanyData>(prefabEntity))
                {
                    var company = em.GetComponentData<Game.Companies.TransportCompanyData>(prefabEntity);
                    int baseMax = company.m_MaxTransports;

                    if (prefabSystem.TryGetPrefab(prefabEntity, out PrefabBase pb3) &&
                        pb3.TryGet(out CargoTransportStation station))
                    {
                        baseMax = station.transports;
                    }

                    Mod.s_Log.Info($"{Mod.ModTag} Verbose:  - CargoStation BaseMaxTransports={baseMax} CurMaxTransports={company.m_MaxTransports}");
                }
            }

            Mod.s_Log.Info($"{Mod.ModTag} Verbose: Prefab keyword scan complete (uniqueMatches={matches}, cap={kMaxKeywordMatches})");
        }

        // ------------------------------------------------------------
        // Utilities
        // ------------------------------------------------------------

        private static string GetPrefabName(PrefabSystem? prefabSystem, Entity prefabEntity)
        {
            try
            {
                if (prefabSystem != null && prefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
                {
                    return prefabBase.name ?? "(unnamed)";
                }
            }
            catch
            {
                // ignore (verbose helper; never crash a city)
            }

            return $"PrefabEntity={prefabEntity.Index}:{prefabEntity.Version}";
        }

        private static int TryGetDeliveryTruckBase(PrefabSystem? prefabSystem, Entity prefabEntity, int fallback)
        {
            try
            {
                if (prefabSystem != null && prefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
                {
                    if (prefabBase.TryGet(out Game.Prefabs.DeliveryTruck truck))
                    {
                        int v = truck.m_CargoCapacity;
                        return v >= 0 ? v : fallback; // allow 0 (tractors) as valid base
                    }
                }
            }
            catch
            {
                // ignore
            }

            return fallback;
        }
    }
}
