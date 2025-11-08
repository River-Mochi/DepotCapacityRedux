// AdjustTransitCapacitySystem.cs
// Purpose: apply multipliers for depot max vehicles and passenger max riders
//          based on current settings. Uses per-entity baselines and SystemAPI
//          queries so values never "stack" across reloads or setting changes.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal.Serialization.Entities; // Purpose, GameMode
    using Game;
    using Game.Prefabs;
    using Unity.Entities;

    public sealed partial class AdjustTransitCapacitySystem : GameSystemBase
    {
        // ---- BASELINE CACHES ----
        // depot entity -> vanilla vehicle capacity
        private readonly Dictionary<Entity, int> m_OriginalDepotCapacity =
            new Dictionary<Entity, int>();

        // vehicle entity -> vanilla passenger capacity
        private readonly Dictionary<Entity, int> m_OriginalPassengerCapacity =
            new Dictionary<Entity, int>();

        // ---- LIFECYCLE: CREATE / DESTROY ----
        protected override void OnCreate()
        {
            base.OnCreate();

            // Only run when there are depots or vehicles in the world.
            // Build queries via SystemAPI.QueryBuilder just for RequireForUpdate.
            var depotQuery = SystemAPI.QueryBuilder()
                                      .WithAll<TransportDepotData>()
                                      .Build();
            var vehicleQuery = SystemAPI.QueryBuilder()
                                        .WithAll<PublicTransportVehicleData>()
                                        .Build();

            RequireForUpdate(depotQuery);
            RequireForUpdate(vehicleQuery);

            // Run only when explicitly enabled (game load or settings Apply).
            Enabled = false;
        }

        /// <summary>
        /// Called after a save or new game has finished loading.
        /// Clears cached baselines because entity IDs and vanilla values
        /// can differ between cities.
        /// </summary>
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

            m_OriginalDepotCapacity.Clear();
            m_OriginalPassengerCapacity.Clear();

            Mod.Log.Info($"{Mod.ModTag} AdjustTransitCapacitySystem: GameLoadingComplete → reapply settings");

            Enabled = true;
        }

        protected override void OnDestroy()
        {
            m_OriginalDepotCapacity.Clear();
            m_OriginalPassengerCapacity.Clear();
            base.OnDestroy();
        }

        // ---- MAIN UPDATE ----
        protected override void OnUpdate()
        {
            if (Mod.Settings == null)
            {
                Enabled = false;
                return;
            }

            Setting settings = Mod.Settings;
            bool debug = settings.EnableDebugLogging;

            // ---- DEPOTS (5 types: Bus / Taxi / Tram / Train / Subway) ----
            foreach (var (depotRef, entity) in SystemAPI
                         .Query<RefRW<TransportDepotData>>()
                         .WithEntityAccess())
            {
                ref TransportDepotData depotData = ref depotRef.ValueRW;

                // Capture vanilla capacity once per entity.
                if (!m_OriginalDepotCapacity.TryGetValue(entity, out int baseCapacity))
                {
                    baseCapacity = depotData.m_VehicleCapacity;
                    if (baseCapacity < 1)
                    {
                        baseCapacity = 1;
                    }

                    m_OriginalDepotCapacity[entity] = baseCapacity;

                    if (debug)
                    {
                        Mod.Log.Info(
                            $"{Mod.ModTag} Depot baseline set: entity={entity.Index}:{entity.Version} " +
                            $"type={depotData.m_TransportType} baseCapacity={baseCapacity}");
                    }
                }

                float scalar = GetDepotScalar(settings, depotData.m_TransportType);

                // If scalar is exactly 1, leave vanilla.
                if (scalar == 1f)
                {
                    continue;
                }

                int newCapacity = (int)(baseCapacity * scalar);
                if (newCapacity < 1)
                {
                    newCapacity = 1;
                }

                if (newCapacity != depotData.m_VehicleCapacity)
                {
                    if (debug)
                    {
                        Mod.Log.Info(
                            $"{Mod.ModTag} Depot apply: entity={entity.Index}:{entity.Version} " +
                            $"type={depotData.m_TransportType} base={baseCapacity} scalar={scalar:F2} " +
                            $"old={depotData.m_VehicleCapacity} new={newCapacity}");
                    }

                    depotData.m_VehicleCapacity = newCapacity;
                }
            }

            // ---- PASSENGERS (no taxi capacity change) ----
            foreach (var (vehicleRef, entity) in SystemAPI
                         .Query<RefRW<PublicTransportVehicleData>>()
                         .WithEntityAccess())
            {
                ref PublicTransportVehicleData vehicleData = ref vehicleRef.ValueRW;

                // Capture vanilla passenger capacity once per entity.
                if (!m_OriginalPassengerCapacity.TryGetValue(entity, out int basePassengers))
                {
                    basePassengers = vehicleData.m_PassengerCapacity;
                    if (basePassengers < 1)
                    {
                        basePassengers = 1;
                    }

                    m_OriginalPassengerCapacity[entity] = basePassengers;

                    if (debug)
                    {
                        Mod.Log.Info(
                            $"{Mod.ModTag} Vehicle baseline set: entity={entity.Index}:{entity.Version} " +
                            $"type={vehicleData.m_TransportType} basePassengers={basePassengers}");
                    }
                }

                float scalar = GetPassengerScalar(settings, vehicleData.m_TransportType);

                // For Taxi and any unsupported type, GetPassengerScalar returns 1 → no change.
                if (scalar == 1f)
                {
                    continue;
                }

                int newPassengers = (int)(basePassengers * scalar);
                if (newPassengers < 1)
                {
                    newPassengers = 1;
                }

                if (newPassengers != vehicleData.m_PassengerCapacity)
                {
                    if (debug)
                    {
                        Mod.Log.Info(
                            $"{Mod.ModTag} Vehicle apply: entity={entity.Index}:{entity.Version} " +
                            $"type={vehicleData.m_TransportType} base={basePassengers} scalar={scalar:F2} " +
                            $"old={vehicleData.m_PassengerCapacity} new={newPassengers}");
                    }

                    vehicleData.m_PassengerCapacity = newPassengers;
                }
            }

            // Run-once; either Setting.Apply() or city load will enable again.
            Enabled = false;
        }

        // ---- HELPERS: DEPOT SCALAR ----

        /// <summary>
        /// Depot multipliers: 1.0–10.0x.
        /// Any other depot type is left at vanilla (1.0).
        /// Targets 5 depot types.
        /// </summary>
        private static float GetDepotScalar(Setting settings, TransportType type)
        {
            float scalar;
            switch (type)
            {
                case TransportType.Bus:
                    scalar = settings.BusDepotScalar;
                    break;
                case TransportType.Taxi:
                    scalar = settings.TaxiDepotScalar;
                    break;
                case TransportType.Tram:
                    scalar = settings.TramDepotScalar;
                    break;
                case TransportType.Train:
                    scalar = settings.TrainDepotScalar;
                    break;
                case TransportType.Subway:
                    scalar = settings.SubwayDepotScalar;
                    break;
                default:
                    return 1f;
            }

            // Clamp to 1x–10x for safety, using Setting constants.
            if (scalar < Setting.MinScalar)
            {
                scalar = Setting.MinScalar;
            }
            else if (scalar > Setting.MaxScalar)
            {
                scalar = Setting.MaxScalar;
            }

            return scalar;
        }

        // ---- HELPERS: PASSENGER SCALAR ----

        /// <summary>
        /// Passenger multipliers: 1.0–10.0x.
        /// Taxi is skipped on purpose (game keeps 4 seats).
        /// </summary>
        private static float GetPassengerScalar(Setting settings, TransportType type)
        {
            float scalar;
            switch (type)
            {
                case TransportType.Bus:
                    scalar = settings.BusPassengerScalar;
                    break;
                case TransportType.Tram:
                    scalar = settings.TramPassengerScalar;
                    break;
                case TransportType.Train:
                    scalar = settings.TrainPassengerScalar;
                    break;
                case TransportType.Subway:
                    scalar = settings.SubwayPassengerScalar;
                    break;
                case TransportType.Ship:
                    scalar = settings.ShipPassengerScalar;
                    break;
                case TransportType.Ferry:
                    scalar = settings.FerryPassengerScalar;
                    break;
                case TransportType.Airplane:
                    scalar = settings.AirplanePassengerScalar;
                    break;
                default:
                    // Includes Taxi → leave at vanilla value.
                    return 1f;
            }

            if (scalar < Setting.MinScalar)
            {
                scalar = Setting.MinScalar;
            }
            else if (scalar > Setting.MaxScalar)
            {
                scalar = Setting.MaxScalar;
            }

            return scalar;
        }
    }
}
