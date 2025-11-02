// DepotCapacityReduxSystem.cs
// Purpose: apply multipliers for Depot max vehicles and Passenger max riders
//          based on current settings. Safe across city loads: caches are cleared

namespace DepotCapacityRedux
{
    using System.Collections.Generic;
    using Colossal.Serialization.Entities; // Purpose, GameMode
    using Game;
    using Game.Prefabs;
    using Unity.Collections;
    using Unity.Entities;

    public sealed partial class DepotCapacityReduxSystem : GameSystemBase
    {
        // depot entity -> vanilla vehicle capacity
        private readonly Dictionary<Entity, int> m_OriginalDepotCapacity =
            new Dictionary<Entity, int>();

        // vehicle entity -> vanilla passenger capacity
        private readonly Dictionary<Entity, int> m_OriginalPassengerCapacity =
            new Dictionary<Entity, int>();

        private EntityQuery m_DepotQuery;
        private EntityQuery m_VehicleQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            // query all transport depots
            m_DepotQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<TransportDepotData>()
                }
            });

            // query all public transport vehicles
            m_VehicleQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PublicTransportVehicleData>()
                }
            });

            RequireForUpdate(m_DepotQuery);
            RequireForUpdate(m_VehicleQuery);

            // initial run (in case options were saved already)
            Enabled = true;
        }

        /// <summary>
        /// Called after a save or new game has finished loading.
        /// Clears cached capacities because entity IDs are different per city.
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

            // entities are rebuilt per city -> cached baselines are no longer valid
            m_OriginalDepotCapacity.Clear();
            m_OriginalPassengerCapacity.Clear();

            // re-apply to newly loaded city
            Enabled = true;
        }

        protected override void OnDestroy()
        {
            m_OriginalDepotCapacity.Clear();
            m_OriginalPassengerCapacity.Clear();
            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
            if (Mod.Settings == null)
            {
                Enabled = false;
                return;
            }

            Setting settings = Mod.Settings;

            //
            // 1) DEPOTS 5 types (Bus/Taxi/Tram/Train/Subway)
            //
            NativeArray<Entity> depots = m_DepotQuery.ToEntityArray(Allocator.Temp);
            try
            {
                for (int i = 0; i < depots.Length; i++)
                {
                    Entity depotEntity = depots[i];
                    TransportDepotData depotData =
                        EntityManager.GetComponentData<TransportDepotData>(depotEntity);

                    if (!m_OriginalDepotCapacity.TryGetValue(depotEntity, out int baseCapacity))
                    {
                        baseCapacity = depotData.m_VehicleCapacity;
                        if (baseCapacity < 1)
                        {
                            baseCapacity = 1;
                        }

                        m_OriginalDepotCapacity[depotEntity] = baseCapacity;
                    }

                    float scalar = GetDepotScalar(settings, depotData.m_TransportType);
                    int newCapacity = (int)(baseCapacity * scalar);

                    if (newCapacity < 1)
                    {
                        newCapacity = 1;
                    }

                    if (newCapacity != depotData.m_VehicleCapacity)
                    {
                        depotData.m_VehicleCapacity = newCapacity;
                        EntityManager.SetComponentData(depotEntity, depotData);
                    }
                }
            }
            finally
            {
                depots.Dispose();
            }

            //
            // 2) PASSENGERS (no taxi capacity change)
            //
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
                    }

                    float scalar = GetPassengerScalar(settings, vehicleData.m_TransportType);
                    int newPassengers = (int)(basePassengers * scalar);

                    if (newPassengers < 1)
                    {
                        newPassengers = 1;
                    }

                    if (newPassengers != vehicleData.m_PassengerCapacity)
                    {
                        vehicleData.m_PassengerCapacity = newPassengers;
                        EntityManager.SetComponentData(vehicleEntity, vehicleData);
                    }
                }
            }
            finally
            {
                vehicles.Dispose();
            }

            // run-once; either settings.Apply() or city load will enable again
            Enabled = false;
        }

        //
        // Helpers
        //

        /// <summary>
        /// Depot multipliers: 100%–1000%, internal scalar 1.0–10.0.
        /// Any other depot type is left at vanilla (1.0)
        /// targets 5 depot types
        /// </summary>
        private static float GetDepotScalar(Setting settings, TransportType type)
        {
            float scalar;
            switch (type)
            {
                case TransportType.Bus:
                    scalar = settings.BusDepotPercent / 100f;
                    break;
                case TransportType.Taxi:
                    scalar = settings.TaxiDepotPercent / 100f;
                    break;
                case TransportType.Tram:
                    scalar = settings.TramDepotPercent / 100f;
                    break;
                case TransportType.Train:
                    scalar = settings.TrainDepotPercent / 100f;
                    break;
                case TransportType.Subway:
                    scalar = settings.SubwayDepotPercent / 100f;
                    break;
                default:
                    return 1f;
            }

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

        /// <summary>
        /// Passenger multipliers: 100%–1000% → 1.0–10.0.
        /// Taxi is skipped on purpose (game keeps 4 seats).
        /// </summary>
        private static float GetPassengerScalar(Setting settings, TransportType type)
        {
            float scalar;
            switch (type)
            {
                case TransportType.Bus:
                    scalar = settings.BusPassengerPercent / 100f;
                    break;
                case TransportType.Tram:
                    scalar = settings.TramPassengerPercent / 100f;
                    break;
                case TransportType.Train:
                    scalar = settings.TrainPassengerPercent / 100f;
                    break;
                case TransportType.Subway:
                    scalar = settings.SubwayPassengerPercent / 100f;
                    break;
                case TransportType.Ship:
                    scalar = settings.ShipPassengerPercent / 100f;
                    break;
                case TransportType.Ferry:
                    scalar = settings.FerryPassengerPercent / 100f;
                    break;
                case TransportType.Airplane:
                    scalar = settings.AirplanePassengerPercent / 100f;
                    break;
                default:
                    // includes Taxi → leave at vanilla value
                    return 1f;
            }

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
