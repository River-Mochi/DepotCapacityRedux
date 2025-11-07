// AdjustTransitCapacitySystem.cs
// Purpose: apply multipliers for depot max vehicles and passenger max riders
//          based on current settings. Uses per-entity baselines to avoid stacking.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal.Serialization.Entities; // Purpose, GameMode
    using Game;
    using Game.Prefabs;
    using Unity.Collections;
    using Unity.Entities;

    public sealed partial class AdjustTransitCapacitySystem : GameSystemBase
    {
        // ---- DEBUG ----
        // Set this to true while testing for lots of log output.
        private const bool DebugLogging = true;

        // ---- BASELINE CACHES ----
        // depot entity -> vanilla vehicle capacity (per process, per entity)
        private readonly Dictionary<Entity, int> m_OriginalDepotCapacity =
            new Dictionary<Entity, int>();

        // vehicle entity -> vanilla passenger capacity (per process, per entity)
        private readonly Dictionary<Entity, int> m_OriginalPassengerCapacity =
            new Dictionary<Entity, int>();

        // ---- QUERIES ----
        private EntityQuery m_DepotQuery;
        private EntityQuery m_VehicleQuery;

        // ---- LIFECYCLE: CREATE / DESTROY ----
        protected override void OnCreate()
        {
            base.OnCreate();

            // Query all transport depots (prefabs with TransportDepotData)
            m_DepotQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<TransportDepotData>()
                }
            });

            // Query all public transport vehicles
            m_VehicleQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PublicTransportVehicleData>()
                }
            });

            RequireForUpdate(m_DepotQuery);
            RequireForUpdate(m_VehicleQuery);

            // Run only when explicitly enabled (OnGameLoadingComplete or Setting.Apply).
            Enabled = false;
        }

        /// <summary>
        /// Called after a save or new game has finished loading.
        /// Re-applies settings once in the newly loaded city.
        /// NOTE: baselines are NOT cleared here to avoid stacking between reloads.
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

            if (DebugLogging)
            {
                Mod.Log.Info("[ATC] OnGameLoadingComplete → enabling system for one pass.");
            }

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

            // ---- DEPOTS (5 types: Bus / Taxi / Tram / Train / Subway) ----
            NativeArray<Entity> depots = m_DepotQuery.ToEntityArray(Allocator.Temp);
            try
            {
                for (int i = 0; i < depots.Length; i++)
                {
                    Entity depotEntity = depots[i];
                    TransportDepotData depotData =
                        EntityManager.GetComponentData<TransportDepotData>(depotEntity);

                    // Determine baseline capacity (only once per entity)
                    if (!m_OriginalDepotCapacity.TryGetValue(depotEntity, out int baseCapacity))
                    {
                        baseCapacity = depotData.m_VehicleCapacity;
                        if (baseCapacity < 1)
                        {
                            baseCapacity = 1;
                        }

                        m_OriginalDepotCapacity[depotEntity] = baseCapacity;

                        if (DebugLogging)
                        {
                            Mod.Log.Info(
                                $"[ATC] Depot baseline capture: entity={depotEntity.Index}, " +
                                $"type={depotData.m_TransportType}, base={baseCapacity}");
                        }
                    }

                    float scalar = GetDepotScalar(settings, depotData.m_TransportType);
                    int newCapacity = (int)(baseCapacity * scalar);
                    if (newCapacity < 1)
                    {
                        newCapacity = 1;
                    }

                    if (newCapacity != depotData.m_VehicleCapacity)
                    {
                        if (DebugLogging)
                        {
                            Mod.Log.Info(
                                $"[ATC] Depot apply: entity={depotEntity.Index}, type={depotData.m_TransportType}, " +
                                $"base={baseCapacity}, scalar={scalar:F2}, " +
                                $"old={depotData.m_VehicleCapacity}, new={newCapacity}");
                        }

                        depotData.m_VehicleCapacity = newCapacity;
                        EntityManager.SetComponentData(depotEntity, depotData);
                    }
                }
            }
            finally
            {
                depots.Dispose();
            }

            // ---- PASSENGERS (no taxi capacity change) ----
            NativeArray<Entity> vehicles = m_VehicleQuery.ToEntityArray(Allocator.Temp);
            try
            {
                for (int i = 0; i < vehicles.Length; i++)
                {
                    Entity vehicleEntity = vehicles[i];
                    PublicTransportVehicleData vehicleData =
                        EntityManager.GetComponentData<PublicTransportVehicleData>(vehicleEntity);

                    if (!m_OriginalPassengerCapacity.TryGetValue(vehicleEntity, out int basePassengers))
                    {
                        basePassengers = vehicleData.m_PassengerCapacity;
                        if (basePassengers < 1)
                        {
                            basePassengers = 1;
                        }

                        m_OriginalPassengerCapacity[vehicleEntity] = basePassengers;

                        if (DebugLogging)
                        {
                            Mod.Log.Info(
                                $"[ATC] Vehicle baseline capture: entity={vehicleEntity.Index}, " +
                                $"type={vehicleData.m_TransportType}, base={basePassengers}");
                        }
                    }

                    float scalar = GetPassengerScalar(settings, vehicleData.m_TransportType);
                    int newPassengers = (int)(basePassengers * scalar);
                    if (newPassengers < 1)
                    {
                        newPassengers = 1;
                    }

                    if (newPassengers != vehicleData.m_PassengerCapacity)
                    {
                        if (DebugLogging)
                        {
                            Mod.Log.Info(
                                $"[ATC] Vehicle apply: entity={vehicleEntity.Index}, type={vehicleData.m_TransportType}, " +
                                $"base={basePassengers}, scalar={scalar:F2}, " +
                                $"old={vehicleData.m_PassengerCapacity}, new={newPassengers}");
                        }

                        vehicleData.m_PassengerCapacity = newPassengers;
                        EntityManager.SetComponentData(vehicleEntity, vehicleData);
                    }
                }
            }
            finally
            {
                vehicles.Dispose();
            }

            // Run-once; either Setting.Apply() or city load will enable again.
            Enabled = false;
        }

        // ---- HELPERS: DEPOT SCALAR ----

        /// <summary>
        /// Depot multipliers: 100%–1000%, internal scalar 1.0–10.0.
        /// Any other depot type is left at vanilla (1.0).
        /// </summary>
        private static float GetDepotScalar(Setting settings, TransportType type)
        {
            int percent;
            switch (type)
            {
                case TransportType.Bus:
                    percent = settings.BusDepotPercent;
                    break;
                case TransportType.Taxi:
                    percent = settings.TaxiDepotPercent;
                    break;
                case TransportType.Tram:
                    percent = settings.TramDepotPercent;
                    break;
                case TransportType.Train:
                    percent = settings.TrainDepotPercent;
                    break;
                case TransportType.Subway:
                    percent = settings.SubwayDepotPercent;
                    break;
                default:
                    return 1f;
            }

            if (percent <= 0)
            {
                return 1f;
            }

            float scalar = percent / 100f;

            // Clamp to 1x–10x for safety.
            if (scalar < 1f)
            {
                scalar = 1f;
            }
            else if (scalar > 10f)
            {
                scalar = 10f;
            }

            return scalar;
        }

        // ---- HELPERS: PASSENGER SCALAR ----

        /// <summary>
        /// Passenger multipliers: 100%–1000% → 1.0–10.0.
        /// Taxi is skipped on purpose (game keeps 4 seats).
        /// </summary>
        private static float GetPassengerScalar(Setting settings, TransportType type)
        {
            int percent;
            switch (type)
            {
                case TransportType.Bus:
                    percent = settings.BusPassengerPercent;
                    break;
                case TransportType.Tram:
                    percent = settings.TramPassengerPercent;
                    break;
                case TransportType.Train:
                    percent = settings.TrainPassengerPercent;
                    break;
                case TransportType.Subway:
                    percent = settings.SubwayPassengerPercent;
                    break;
                case TransportType.Ship:
                    percent = settings.ShipPassengerPercent;
                    break;
                case TransportType.Ferry:
                    percent = settings.FerryPassengerPercent;
                    break;
                case TransportType.Airplane:
                    percent = settings.AirplanePassengerPercent;
                    break;
                default:
                    // Includes Taxi → leave at vanilla value.
                    return 1f;
            }

            if (percent <= 0)
            {
                return 1f;
            }

            float scalar = percent / 100f;

            if (scalar < 1f)
            {
                scalar = 1f;
            }
            else if (scalar > 10f)
            {
                scalar = 10f;
            }

            return scalar;
        }
    }
}
