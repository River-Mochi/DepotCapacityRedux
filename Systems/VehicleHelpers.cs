// File: Systems/VehicleHelpers.cs
// Purpose: Shared helpers for classifying service vehicle prefabs (delivery buckets, tractor/trailer info).
// Notes:
// - Not a system (no OnUpdate).
// - Reusable (classification and component reads only).
// - Scanning/reporting belongs in PrefabScanSystem (triggered by UI button).

namespace DispatchBoss
{
    using Game.Economy;   // Resource
    using Game.Prefabs;   // CarTrailerType, CarTractorData, CarTrailerData
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

            // Tractor component (front unit) declares what trailer type it expects.
            if (entityManager.HasComponent<CarTractorData>(prefabEntity))
            {
                hasTractor = true;
                CarTractorData tractor = entityManager.GetComponentData<CarTractorData>(prefabEntity);
                tractorTrailerType = tractor.m_TrailerType;
            }

            // Trailer component (rear unit) declares its own trailer type.
            if (entityManager.HasComponent<CarTrailerData>(prefabEntity))
            {
                hasTrailer = true;
                CarTrailerData trailer = entityManager.GetComponentData<CarTrailerData>(prefabEntity);
                trailerTrailerType = trailer.m_TrailerType;
            }
        }

        public static DeliveryBucket ClassifyDeliveryTruckPrefab(
            string prefabName,
            int vanillaCargoCapacity,
            Resource transportedResources,
            bool hasTractor,
            CarTrailerType tractorTrailerType,
            bool hasTrailer,
            CarTrailerType trailerTrailerType)
        {
            // Capacity <= 0 is valid for tractor-only/helper prefabs (no cargo payload).
            // These should not be bucketed into cargo scaling groups.
            if (vanillaCargoCapacity <= 0)
                return DeliveryBucket.Other;

            // Semis are identified by trailer type (tractor expecting Semi, or trailer being Semi).
            bool isSemi =
                (hasTractor && tractorTrailerType == CarTrailerType.Semi) ||
                (hasTrailer && trailerTrailerType == CarTrailerType.Semi);

            if (isSemi)
                return DeliveryBucket.Semi;

            // Motorbike delivery
            if (vanillaCargoCapacity > 0 &&
                vanillaCargoCapacity <= 200 &&
                prefabName.IndexOf("Motorbike", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return DeliveryBucket.Motorbike;
            }

            // Vans (vanilla commonly 4000).
            if (vanillaCargoCapacity == 4000 ||
                prefabName.IndexOf("DeliveryVan", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return DeliveryBucket.Van;
            }

            // Raw materials dump trucks (vanilla commonly 20000).
            // Name checks help when capacity differs due to other mods or prefab variants.
            if (vanillaCargoCapacity == 20000 ||
                prefabName.IndexOf("OilTruck", StringComparison.OrdinalIgnoreCase) >= 0 ||
                prefabName.IndexOf("CoalTruck", StringComparison.OrdinalIgnoreCase) >= 0 ||
                prefabName.IndexOf("OreTruck", StringComparison.OrdinalIgnoreCase) >= 0 ||
                prefabName.IndexOf("StoneTruck", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return DeliveryBucket.RawMaterials;
            }

            return DeliveryBucket.Other;
        }
    }
}
