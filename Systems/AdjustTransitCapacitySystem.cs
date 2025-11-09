// Systems/AdjustTransitCapacitySystem.cs
// Purpose: apply multipliers for depot max vehicles and passenger max riders
//          based on current settings. PrefabSystem + PrefabBase are used to
//          read vanilla capacities so values never stack and never depend on
//          other runtime changes.

namespace AdjustTransitCapacity
{
    using System;
    using System.Collections.Generic;
    using Colossal.Serialization.Entities; // Purpose, GameMode
    using Game;
    using Game.Prefabs;
    using Game.SceneFlow;
    using Unity.Entities;

    public sealed partial class AdjustTransitCapacitySystem : GameSystemBase
    {
        // PrefabSystem provides PrefabBase, which exposes the original
        // (vanilla) component values for each prefab.
        private PrefabSystem m_PrefabSystem = null!;

        // Debug: one-time per city summary.
        private HashSet<TransportType> m_SeenDepotTypes = null!;
        private HashSet<TransportType> m_SeenPassengerTypes = null!;
        private bool m_LoggedTypesOnce;

        // ---------
        // Lifecycle
        // ---------

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            m_SeenDepotTypes = new HashSet<TransportType>();
            m_SeenPassengerTypes = new HashSet<TransportType>();
            m_LoggedTypesOnce = false;

            // Just set up queries; no heavy work here.
            EntityQuery depotQuery = SystemAPI.QueryBuilder()
                                              .WithAll<TransportDepotData>()
                                              .Build();

            EntityQuery vehicleQuery = SystemAPI.QueryBuilder()
                                                .WithAll<PublicTransportVehicleData>()
                                                .Build();

            RequireForUpdate(depotQuery);
            RequireForUpdate(vehicleQuery);

            // Run only when explicitly enabled (game load or settings change).
            Enabled = false;
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            // Only react for real gameplay (new or loaded city).
            bool isRealGame =
                mode == GameMode.Game &&
                (purpose == Purpose.NewGame || purpose == Purpose.LoadGame);

            if (!isRealGame)
            {
                return;
            }

            m_SeenDepotTypes.Clear();
            m_SeenPassengerTypes.Clear();
            m_LoggedTypesOnce = false;

            Mod.Log.Info($"{Mod.ModTag} City Loading Complete -> applying ATC settings");

            // Schedule one run for this city.
            Enabled = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        // -----------
        // Main update
        // -----------

        protected override void OnUpdate()
        {
            // Extra safety: never do work outside gameplay, even if someone
            // accidentally toggled Enabled from elsewhere.
            GameManager gm = GameManager.instance;
            if (gm == null || !gm.gameMode.IsGame())
            {
                bool debugBail = Mod.Settings?.EnableDebugLogging == true;
                if (debugBail)
                {
                    GameMode mode = gm != null ? gm.gameMode : GameMode.None;
                    Mod.Log.Info(
                        $"{Mod.ModTag} Debug: OnUpdate bail; gameMode={mode} (not Game) → disabling system.");
                }

                Enabled = false;
                return;
            }

            if (Mod.Settings == null)
            {
                Enabled = false;
                return;
            }

            Setting settings = Mod.Settings;
            bool debug = settings.EnableDebugLogging;

            // DEPOTS (Bus / Taxi / Tram / Train / Subway only)
            foreach ((RefRW<TransportDepotData> depotRef, Entity entity) in SystemAPI
                         .Query<RefRW<TransportDepotData>>()
                         .WithEntityAccess())
            {
                ref TransportDepotData depotData = ref depotRef.ValueRW;

                // Ignore depots with TransportType we don't support (None, Rocket, etc.).
                if (!IsHandledDepotType(depotData.m_TransportType))
                {
                    continue;
                }

                // Track which types were actually present in this city.
                m_SeenDepotTypes.Add(depotData.m_TransportType);

                float scalar = GetDepotScalar(settings, depotData.m_TransportType);

                // Get vanilla base from PrefabBase via PrefabSystem.
                int baseCapacity;
                if (!TryGetDepotBaseCapacity(entity, out baseCapacity))
                {
                    if (debug)
                    {
                        Mod.Log.Warn(
                            $"{Mod.ModTag} Depot: failed to get BaseDepot from prefab " +
                            $"for entity={entity.Index}:{entity.Version}, type={depotData.m_TransportType}. " +
                            "Falling back to current depot vehicle capacity.");
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
                            $"type={depotData.m_TransportType} BaseDepot={baseCapacity} scalar={scalar:F2} " +
                            $"OldDepot={depotData.m_VehicleCapacity} NewDepot={newCapacity}");
                    }

                    depotData.m_VehicleCapacity = newCapacity;
                }
            }

            // PASSENGERS (Bus / Tram / Train / Subway / Ship / Ferry / Airplane)
            // Taxi seats stay vanilla 4; Prison vans are skipped explicitly.
            foreach ((RefRW<PublicTransportVehicleData> vehicleRef, Entity entity) in SystemAPI
                         .Query<RefRW<PublicTransportVehicleData>>()
                         .WithEntityAccess())
            {
                ref PublicTransportVehicleData vehicleData = ref vehicleRef.ValueRW;

                // Only touch specific transport types we actually expose sliders for.
                if (!IsHandledPassengerType(vehicleData.m_TransportType))
                {
                    // This also leaves Taxi and any weird special vehicles completely vanilla.
                    continue;
                }

                // Track which types were actually present in this city.
                m_SeenPassengerTypes.Add(vehicleData.m_TransportType);

                // Hard skip Prison Vans even though they use TransportType.Bus.
                if (IsPrisonVan(entity))
                {
                    if (debug)
                    {
                        Mod.Log.Info(
                            $"{Mod.ModTag} Vehicle skip: entity={entity.Index}:{entity.Version} " +
                            "PrisonVan prefab detected → leaving seats vanilla.");
                    }

                    continue;
                }

                float scalar = GetPassengerScalar(settings, vehicleData.m_TransportType);

                // Get vanilla base from PrefabBase via PrefabSystem.
                int basePassengers;
                if (!TryGetPassengerBaseCapacity(entity, out basePassengers))
                {
                    if (debug)
                    {
                        Mod.Log.Warn(
                            $"{Mod.ModTag} Vehicle: failed to get BaseSeats from prefab " +
                            $"for entity={entity.Index}:{entity.Version}, type={vehicleData.m_TransportType}. " +
                            "Falling back to current passenger capacity.");
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
                            $"{Mod.ModTag} Passengers apply: entity={entity.Index}:{entity.Version} " +
                            $"type={vehicleData.m_TransportType} BaseSeats={basePassengers} scalar={scalar:F2} " +
                            $"OldSeats={vehicleData.m_PassengerCapacity} NewSeats={newPassengers}");
                    }

                    vehicleData.m_PassengerCapacity = newPassengers;
                }
            }

            // One-time debug summary per city of what we actually touched.
            if (debug && !m_LoggedTypesOnce)
            {
                m_LoggedTypesOnce = true;

                string depotSummary = m_SeenDepotTypes.Count > 0
                    ? string.Join(", ", m_SeenDepotTypes)
                    : "(none)";

                string passengerSummary = m_SeenPassengerTypes.Count > 0
                    ? string.Join(", ", m_SeenPassengerTypes)
                    : "(none)";

                Mod.Log.Info(
                    $"{Mod.ModTag} Debug: City Summary -> DepotTypes=[{depotSummary}] " +
                    $"PassengerTypes=[{passengerSummary}]");
            }

            // Run-once; either settings changes or city load will enable again.
            Enabled = false;
        }

        // ------------------------------
        // Helpers: type whitelists / ids
        // ------------------------------

        private static bool IsHandledDepotType(TransportType type)
        {
            switch (type)
            {
                case TransportType.Bus:
                case TransportType.Taxi:
                case TransportType.Tram:
                case TransportType.Train:
                case TransportType.Subway:
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsHandledPassengerType(TransportType type)
        {
            switch (type)
            {
                case TransportType.Bus:
                case TransportType.Tram:
                case TransportType.Train:
                case TransportType.Subway:
                case TransportType.Ship:
                case TransportType.Ferry:
                case TransportType.Airplane:
                    return true;
                default:
                    return false;
            }
        }

        // Helper: identify PrisonVan from its prefab
        private bool IsPrisonVan(Entity entity)
        {
            if (m_PrefabSystem == null)
            {
                return false;
            }

            if (!m_PrefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
            {
                return false;
            }

            string name = prefabBase.name;
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Be robust to suffix changes: "PrisonVan01", "PrisonVan02", etc.
            return name.IndexOf("PrisonVan", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        // -----------------------------
        // Prefab helpers: vanilla base
        // -----------------------------

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

            // Vanilla base seats from prefab.
            basePassengers = publicTransport.m_PassengerCapacity;
            return true;
        }

        // -----------------------------
        // Scalar helpers (percent → x)
        // -----------------------------

        private static float GetDepotScalar(Setting settings, TransportType type)
        {
            float percent;

            switch (type)
            {
                case TransportType.Bus:
                    percent = settings.BusDepotScalar;
                    break;
                case TransportType.Taxi:
                    percent = settings.TaxiDepotScalar;
                    break;
                case TransportType.Tram:
                    percent = settings.TramDepotScalar;
                    break;
                case TransportType.Train:
                    percent = settings.TrainDepotScalar;
                    break;
                case TransportType.Subway:
                    percent = settings.SubwayDepotScalar;
                    break;
                default:
                    return 1f;
            }

            if (percent < Setting.MinPercent)
            {
                percent = Setting.MinPercent;
            }
            else if (percent > Setting.MaxPercent)
            {
                percent = Setting.MaxPercent;
            }

            return percent / 100f;
        }

        private static float GetPassengerScalar(Setting settings, TransportType type)
        {
            float percent;

            switch (type)
            {
                case TransportType.Bus:
                    percent = settings.BusPassengerScalar;
                    break;
                case TransportType.Tram:
                    percent = settings.TramPassengerScalar;
                    break;
                case TransportType.Train:
                    percent = settings.TrainPassengerScalar;
                    break;
                case TransportType.Subway:
                    percent = settings.SubwayPassengerScalar;
                    break;
                case TransportType.Ship:
                    percent = settings.ShipPassengerScalar;
                    break;
                case TransportType.Ferry:
                    percent = settings.FerryPassengerScalar;
                    break;
                case TransportType.Airplane:
                    percent = settings.AirplanePassengerScalar;
                    break;
                default:
                    // Includes Taxi and any special types (rockets, etc.) -> leave vanilla.
                    return 1f;
            }

            if (percent < Setting.MinPercent)
            {
                percent = Setting.MinPercent;
            }
            else if (percent > Setting.MaxPercent)
            {
                percent = Setting.MaxPercent;
            }

            return percent / 100f;
        }
    }
}
