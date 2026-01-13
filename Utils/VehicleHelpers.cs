// File: Utils/VehicleHelpers.cs
// Purpose: shared helpers for prefab classification (delivery buckets, tractor/trailer info).
// Notes:
// - Not a system (no OnUpdate).
// - Provides BOTH EntityManager-based and ComponentLookup-based readers.
// - Prefer the lookup overload when calling from a system that already uses SystemAPI.

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

        // Preferred for systems: pass lookups from SystemAPI.GetComponentLookup<T>(true)
        public static void GetTrailerTypeInfo(
            in ComponentLookup<CarTractorData> tractorLookup,
            in ComponentLookup<CarTrailerData> trailerLookup,
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

            if (tractorLookup.TryGetComponent(prefabEntity, out CarTractorData tractor))
            {
                hasTractor = true;
                tractorTrailerType = tractor.m_TrailerType;
            }

            if (trailerLookup.TryGetComponent(prefabEntity, out CarTrailerData trailer))
            {
                hasTrailer = true;
                trailerTrailerType = trailer.m_TrailerType;
            }
        }

        // Fallback for non-system callers:
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
            int vanillaCargoCapacity,
            Resource transportedResources,
            bool hasTractor,
            CarTrailerType tractorTrailerType,
            bool hasTrailer,
            CarTrailerType trailerTrailerType)
        {
            if (vanillaCargoCapacity <= 0)
                return DeliveryBucket.Other;

            string name = prefabName ?? string.Empty;

            bool isSemi =
                (hasTractor && tractorTrailerType == CarTrailerType.Semi) ||
                (hasTrailer && trailerTrailerType == CarTrailerType.Semi);

            if (isSemi)
                return DeliveryBucket.Semi;

            if (name.IndexOf("Motorbike", StringComparison.OrdinalIgnoreCase) >= 0 &&
                vanillaCargoCapacity > 0 &&
                vanillaCargoCapacity <= 200)
            {
                return DeliveryBucket.Motorbike;
            }

            if (vanillaCargoCapacity == 4000 ||
                name.IndexOf("DeliveryVan", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return DeliveryBucket.Van;
            }

            const Resource rawMask =
                Resource.Oil |
                Resource.Coal |
                Resource.Ore |
                Resource.Stone;

            if ((transportedResources & rawMask) != 0)
                return DeliveryBucket.RawMaterials;

            if (vanillaCargoCapacity == 20000 ||
                name.IndexOf("OilTruck", StringComparison.OrdinalIgnoreCase) >= 0 ||
                name.IndexOf("CoalTruck", StringComparison.OrdinalIgnoreCase) >= 0 ||
                name.IndexOf("OreTruck", StringComparison.OrdinalIgnoreCase) >= 0 ||
                name.IndexOf("StoneTruck", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return DeliveryBucket.RawMaterials;
            }

            return DeliveryBucket.Other;
        }
    }
}

