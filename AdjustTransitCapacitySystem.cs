// AdjustTransitCapacitySystem.cs
// Purpose: apply multipliers for depot max vehicles and passenger max riders
//          based on current settings. PrefabSystem + PrefabBase are used to
//          read vanilla capacities so values never stack and never depend on
//          other runtime changes.

namespace AdjustTransitCapacity
{
    using Colossal.Serialization.Entities; // Purpose, GameMode
    using Game;
    using Game.Prefabs;
    using Unity.Entities;

    public sealed partial class AdjustTransitCapacitySystem : GameSystemBase
    {
        // PrefabSystem provides PrefabBase, which exposes the original
        // (vanilla) component values for each prefab.
        private PrefabSystem m_PrefabSystem = null!;

        // ---- LIFECYCLE ----
        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

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

            Mod.Log.Info($"{Mod.ModTag} AdjustTransitCapacitySystem: GameLoadingComplete → reapply settings");

            Enabled = true;
        }

        protected override void OnDestroy()
        {
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

            // ---- DEPOTS (Bus / Taxi / Tram / Train / Subway) ----
            foreach (var (depotRef, entity) in SystemAPI
                         .Query<RefRW<TransportDepotData>>()
                         .WithEntityAccess())
            {
                ref TransportDepotData depotData = ref depotRef.ValueRW;

                float scalar = GetDepotScalar(settings, depotData.m_TransportType);

                // Get vanilla base from PrefabBase via PrefabSystem.
                int baseCapacity;
                if (!TryGetDepotBaseCapacity(entity, out baseCapacity))
                {
                    if (debug)
                    {
                        Mod.Log.Warn(
                            $"{Mod.ModTag} Depot: failed to get base capacity " +
                            $"for entity={entity.Index}:{entity.Version}, type={depotData.m_TransportType}. " +
                            "Falling back to current component value.");
                    }

                    baseCapacity = depotData.m_VehicleCapacity;
                }

                if (baseCapacity < 1)
                {
                    baseCapacity = 1;
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

            // ---- PASSENGERS (affect vehicle capacity only; taxi seats unchanged) ----
            foreach (var (vehicleRef, entity) in SystemAPI
                         .Query<RefRW<PublicTransportVehicleData>>()
                         .WithEntityAccess())
            {
                ref PublicTransportVehicleData vehicleData = ref vehicleRef.ValueRW;

                float scalar = GetPassengerScalar(settings, vehicleData.m_TransportType);

                // Get vanilla base from PrefabBase via PrefabSystem.
                int basePassengers;
                if (!TryGetPassengerBaseCapacity(entity, out basePassengers))
                {
                    if (debug)
                    {
                        Mod.Log.Warn(
                            $"{Mod.ModTag} Vehicle: failed to get base passenger capacity " +
                            $"for entity={entity.Index}:{entity.Version}, type={vehicleData.m_TransportType}. " +
                            "Falling back to current component value.");
                    }

                    basePassengers = vehicleData.m_PassengerCapacity;
                }

                if (basePassengers < 1)
                {
                    basePassengers = 1;
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

        // ---- PREFAB HELPERS: READ VANILLA FROM PrefabBase ----

        private bool TryGetDepotBaseCapacity(Entity entity, out int baseCapacity)
        {
            baseCapacity = 0;

            if (m_PrefabSystem == null)
            {
                return false;
            }

            if (!m_PrefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
            {
                return false;
            }

            if (!prefabBase.TryGet(out TransportDepot depotComponent))
            {
                return false;
            }

            baseCapacity = depotComponent.m_VehicleCapacity;
            return true;
        }

        private bool TryGetPassengerBaseCapacity(Entity entity, out int basePassengers)
        {
            basePassengers = 0;

            if (m_PrefabSystem == null)
            {
                return false;
            }

            if (!m_PrefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
            {
                return false;
            }

            if (!prefabBase.TryGet(out PublicTransport publicTransport))
            {
                return false;
            }

            basePassengers = publicTransport.m_PassengerCapacity;
            return true;
        }

        // ---- DEPOT / PASSENGER SCALAR HELPERS ----

        // Depot multipliers: 1.0–10.0x. Any other depot type is left at vanilla.
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

        // Passenger multipliers: 1.0–10.0x. Taxi seats are not changed.
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
                    // Includes Taxi → leave passenger seats at vanilla.
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
