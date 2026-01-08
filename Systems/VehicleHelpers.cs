// File: Systems/VehicleHelpers.cs
// Purpose: Shared helpers for classifying service vehicle prefabs (delivery buckets, trailer info).
// Notes:
// - NOT a system.
// - No heavy scanning here anymore (scan moved to PrefabScanSystem via UI button).

namespace DispatchBoss
{
    using Game.Economy;   // Resource
    using Game.Prefabs;   // CarTrailerType, CarTractorData, CarTrailerData, DeliveryTruckData
    using System;
    using Unity.Entities;

    public static class VehicleHelpers
    {
        public enum DeliveryBucket
        {
            Other = 0,
            Semi = 1,
            Van = 2,
            RawMaterials = 3,
            Motorbike = 4,
        }

        /// <summary>
        /// Legacy entry point that used to run a big scan when verbose was enabled.
        /// Kept as a no-op (with a single info line) to avoid accidental heavy logging via the verbose toggle.
        /// Use the Settings button "Run Prefab Scan" instead.
        /// </summary>
        public static void RunServiceVerboseScan(ref SystemState state, PrefabSystem? prefabSystem)
        {
            // Intentionally lightweight now.
            Mod.s_Log.Info($"{Mod.ModTag} Verbose: Prefab scan is now triggered by the Settings button (no auto-scan on verbose toggle).");
        }

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

            if (!entityManager.Exists(prefabEntity))
                return;

            // Tractor component (front unit) can declare what trailer type it expects.
            if (entityManager.HasComponent<CarTractorData>(prefabEntity))
            {
                hasTractor = true;
                CarTractorData tractor = entityManager.GetComponentData<CarTractorData>(prefabEntity);
                tractorTrailerType = tractor.m_TrailerType;
            }

            // Trailer component (rear unit) declares its own type.
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
            // baseCap==0 is valid for tractor-only/helper prefabs (no cargo). Do not bucket these.
            if (baseCap <= 0)
                return DeliveryBucket.Other;

            bool isSemi =
                (hasTractor && tractorTrailerType == CarTrailerType.Semi) ||
                (hasTrailer && trailerTrailerType == CarTrailerType.Semi);

            if (isSemi)
                return DeliveryBucket.Semi;

            // Small-capacity motorbike delivery
            if ((baseCap > 0 && baseCap <= 200) &&
                prefabName.IndexOf("Motorbike", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return DeliveryBucket.Motorbike;
            }

            // Vans (vanilla commonly 4000)
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
    }
}
