// DepotCapacityReduxSystem.cs
// Purpose: apply depot (max vehicles) and passenger (max riders) multipliers
//          from current settings. Clears cached baselines when a new city is
//          loaded because ECS entities are recreated per world.

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
        // depot entity -> original (vanilla) vehicle capacity
        private readonly Dictionary<Entity, int> m_OriginalDepotCapacity =
            new Dictionary<Entity, int>();

        // vehicle entity -> original (vanilla) passenger capacity
        private readonly Dictionary<Entity, int> m_OriginalPassengerCapacity =
            new Dictionary<Entity, int>();

        private EntityQuery m_DepotQuery;
        private EntityQuery m_VehicleQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            // depots present in the world
            m_DepotQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<TransportDepotData>()
                }
            });

            // public transport vehicles present in the world
            m_VehicleQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PublicTransportVehicleData>()
                }
            });

            // run only when both depots and vehicles exist
            RequireForUpdate(m_DepotQuery);
            RequireForUpdate(m_VehicleQuery);

            // first application for the first loaded world
            Enabled = true;
        }

        /// <summary>
        /// Called after a save or new game has finished loading.
        /// Clears cached capacities because entity IDs are different per world.
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

            // new world → old entity baselines are invalid
            m_OriginalDepotCapacity.Clear();
            m_OriginalPassengerCapacity.Clear();

            // re-apply to the newly loaded world
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

            // ---------------------------------------------------------------
            // 1) depot capacities (ONLY the 5 original depot types)
            // ---------------------------------------------------------------
            NativeArray<Entity> depots = m_DepotQuery.ToEntityArray(Allocator.Temp);
            try
            {
                for (int i = 0; i < depots.Length; i++)
                {
                    Entity depotEntity = depots[i];
                    TransportDepotData depotData =
                        EntityManager.GetComponentData<TransportDepotData>(depotEntity);

                    // cache vanilla once per entity
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

            // ---------------------------------------------------------------
            // 2) passenger capacities (all 8 public-transport vehicle types)
            // ---------------------------------------------------------------
            NativeArray<Entity> vehicles = m_VehicleQuery.ToEntityArray(Allocator.Temp);
            try
            {
                for (int i = 0; i < vehicles.Length; i++)
                {
                    Entity vehicleEntity = vehicles[i];
                    PublicTransportVehicleData vehicleData =
                        EntityManager.GetComponentData<PublicTransportVehicleData>(vehicleEntity);

                    // cache vanilla once per entity
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

            // run once; settings.Apply() or city-load will re-enable
            Enabled = false;
        }

        // ---------------------------------------------------------------------
        // Helpers
        // ---------------------------------------------------------------------

        /// <summary>
        /// Depot multipliers: keep behavior to the 5 original depot types only.
        /// Values are 100–1000 → 1.0–10.0.
        /// Any other depot type is left at vanilla (returns 1.0).
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

                // ship / ferry / airplane depots were not in original code → do nothing
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
        /// Passenger multipliers: applied to all city passenger vehicles.
        /// Values are 100–1000 → 1.0–10.0.
        /// </summary>
        private static float GetPassengerScalar(Setting settings, TransportType type)
        {
            float scalar;

            switch (type)
            {
                case TransportType.Bus:
                    scalar = settings.BusPassengerPercent / 100f;
                    break;
                case TransportType.Taxi:
                    scalar = settings.TaxiPassengerPercent / 100f;
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
