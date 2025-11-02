namespace DepotCapacityRedux
{
    using System.Collections.Generic;
    using Game;
    using Game.Prefabs;
    using Unity.Collections;
    using Unity.Entities;

    public sealed partial class DepotCapacityReduxSystem : GameSystemBase
    {
        // depot entity -> original depot capacity
        private readonly Dictionary<Entity, int> m_OriginalDepotCapacity = new Dictionary<Entity, int>();

        // vehicle entity -> original passenger capacity
        private readonly Dictionary<Entity, int> m_OriginalPassengerCapacity = new Dictionary<Entity, int>();

        private EntityQuery m_DepotQuery;
        private EntityQuery m_VehicleQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_DepotQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<TransportDepotData>()
                }
            });

            m_VehicleQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PublicTransportVehicleData>()
                }
            });

            RequireForUpdate(m_DepotQuery);
            RequireForUpdate(m_VehicleQuery);

            Enabled = true;
        }

        protected override void OnUpdate()
        {
            if (Mod.Settings == null)
            {
                Enabled = false;
                return;
            }

            var settings = Mod.Settings;

            // 1) apply depot capacity
            var depots = m_DepotQuery.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < depots.Length; i++)
            {
                var entity = depots[i];
                var data = EntityManager.GetComponentData<TransportDepotData>(entity);

                if (!m_OriginalDepotCapacity.TryGetValue(entity, out var baseCapacity))
                {
                    baseCapacity = data.m_VehicleCapacity;
                    m_OriginalDepotCapacity[entity] = baseCapacity;
                }

                var scalar = GetDepotScalar(settings, data.m_TransportType);
                if (scalar <= 0f)
                {
                    continue;
                }

                var newCapacity = (int)(baseCapacity * scalar);
                if (newCapacity != data.m_VehicleCapacity)
                {
                    data.m_VehicleCapacity = newCapacity;
                    EntityManager.SetComponentData(entity, data);
                }
            }
            depots.Dispose();

            // 2) apply passenger capacity
            var vehicles = m_VehicleQuery.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < vehicles.Length; i++)
            {
                var entity = vehicles[i];
                var data = EntityManager.GetComponentData<PublicTransportVehicleData>(entity);

                if (!m_OriginalPassengerCapacity.TryGetValue(entity, out var basePassengers))
                {
                    basePassengers = data.m_PassengerCapacity;
                    m_OriginalPassengerCapacity[entity] = basePassengers;
                }

                var scalar = GetPassengerScalar(settings, data.m_TransportType);
                if (scalar <= 0f)
                {
                    continue;
                }

                var newPassengers = (int)(basePassengers * scalar);
                if (newPassengers != data.m_PassengerCapacity)
                {
                    data.m_PassengerCapacity = newPassengers;
                    EntityManager.SetComponentData(entity, data);
                }
            }
            vehicles.Dispose();

            // run-once; Setting.Apply() will re-enable
            Enabled = false;
        }

        private static float GetDepotScalar(Setting settings, TransportType type)
        {
            switch (type)
            {
                case TransportType.Bus:
                    return settings.BusDepotPercent / 100f;
                case TransportType.Taxi:
                    return settings.TaxiDepotPercent / 100f;
                case TransportType.Tram:
                    return settings.TramDepotPercent / 100f;
                case TransportType.Train:
                    return settings.TrainDepotPercent / 100f;
                case TransportType.Subway:
                    return settings.SubwayDepotPercent / 100f;
                case TransportType.Ship:
                    return settings.ShipDepotPercent / 100f;
                case TransportType.Ferry:
                    return settings.FerryDepotPercent / 100f;
                case TransportType.Airplane:
                    return settings.AirplaneDepotPercent / 100f;
                default:
                    return 0f;
            }
        }

        private static float GetPassengerScalar(Setting settings, TransportType type)
        {
            switch (type)
            {
                case TransportType.Bus:
                    return settings.BusPassengerPercent / 100f;
                case TransportType.Taxi:
                    return settings.TaxiPassengerPercent / 100f;
                case TransportType.Tram:
                    return settings.TramPassengerPercent / 100f;
                case TransportType.Train:
                    return settings.TrainPassengerPercent / 100f;
                case TransportType.Subway:
                    return settings.SubwayPassengerPercent / 100f;
                case TransportType.Ship:
                    return settings.ShipPassengerPercent / 100f;
                case TransportType.Ferry:
                    return settings.FerryPassengerPercent / 100f;
                case TransportType.Airplane:
                    return settings.AirplanePassengerPercent / 100f;
                default:
                    return 0f;
            }
        }
    }
}
