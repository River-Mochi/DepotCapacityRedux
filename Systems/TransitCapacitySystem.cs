// File: Systems/TransitCapacitySystem.cs
// Purpose: apply multipliers for depot max vehicles and passenger max riders.
// PrefabSystem + PrefabBase used for vanilla base values so they never change.

namespace DispatchBoss
{
    using Colossal.Serialization.Entities;
    using Game;
    using Game.Prefabs;
    using Game.SceneFlow;
    using System;
    using System.Collections.Generic;
    using Unity.Entities;

    public sealed partial class TransitCapacitySystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem = null!;

        // Debug: one-time per city summary.
        private HashSet<TransportType> m_SeenDepotTypes = null!;
        private HashSet<TransportType> m_SeenPassengerTypes = null!;
        private bool m_LoggedTypesOnce;

        // Per-type seat summary (BaseSeats/NewSeats ranges), per vehicle prefab.
        private Dictionary<TransportType, SeatSummary> m_PassengerSeatSummary = null!;

        private const int kTramSections = 3;

        private struct SeatSummary
        {
            public bool HasData;
            public int MinBase;
            public int MaxBase;
            public int MinNew;
            public int MaxNew;

            public void AddSample(int baseSeats, int newSeats)
            {
                if (!HasData)
                {
                    HasData = true;
                    MinBase = baseSeats;
                    MaxBase = baseSeats;
                    MinNew = newSeats;
                    MaxNew = newSeats;
                    return;
                }

                if (baseSeats < MinBase) MinBase = baseSeats;
                if (baseSeats > MaxBase) MaxBase = baseSeats;
                if (newSeats < MinNew) MinNew = newSeats;
                if (newSeats > MaxNew) MaxNew = newSeats;
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            m_SeenDepotTypes = new HashSet<TransportType>();
            m_SeenPassengerTypes = new HashSet<TransportType>();
            m_LoggedTypesOnce = false;
            m_PassengerSeatSummary = new Dictionary<TransportType, SeatSummary>();

            // Prefab-only (IMPORTANT) — modifying prefabs, not live entities.
            EntityQuery depotQuery = SystemAPI.QueryBuilder()
                .WithAll<PrefabData, TransportDepotData>()
                .Build();

            EntityQuery vehicleQuery = SystemAPI.QueryBuilder()
                .WithAll<PrefabData, PublicTransportVehicleData>()
                .Build();

            RequireForUpdate(depotQuery);
            RequireForUpdate(vehicleQuery);

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

            m_SeenDepotTypes.Clear();
            m_SeenPassengerTypes.Clear();
            m_LoggedTypesOnce = false;
            m_PassengerSeatSummary.Clear();

            Mod.s_Log.Info($"{Mod.ModTag} City Loading Complete -> applying transit settings");
            Enabled = true;
        }

        protected override void OnUpdate()
        {
            GameManager gm = GameManager.instance;
            if (gm == null || !gm.gameMode.IsGame())
            {
                if (Mod.Settings != null && Mod.Settings.EnableDebugLogging)
                {
                    GameMode mode = gm != null ? gm.gameMode : GameMode.None;
                    Mod.s_Log.Info($"{Mod.ModTag} Debug: TransitCapacitySystem bail; gameMode={mode} (not Game) -> disabling system.");
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

            // DEPOTS (Bus / Taxi /  Tram / Train / Subway only) — prefab-only
            foreach ((RefRW<TransportDepotData> depotRef, Entity entity) in SystemAPI
                         .Query<RefRW<TransportDepotData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                ref TransportDepotData depotData = ref depotRef.ValueRW;

                if (!IsHandledDepotType(depotData.m_TransportType))
                {
                    continue;
                }

                m_SeenDepotTypes.Add(depotData.m_TransportType);

                float scalar = GetDepotScalar(settings, depotData.m_TransportType);

                int baseCapacity;
                if (!TryGetDepotBaseCapacity(entity, out baseCapacity))
                {
                    if (debug)
                    {
                        Mod.s_Log.Warn(
                            $"{Mod.ModTag} Depot: failed to read vanilla base capacity for entity={entity.Index}:{entity.Version}, type={depotData.m_TransportType}. " +
                            "Falling back to current depot vehicle capacity.");
                    }

                    baseCapacity = depotData.m_VehicleCapacity;
                }

                if (baseCapacity < 1)
                    baseCapacity = 1;

                int newCapacity = (int)(baseCapacity * scalar);
                if (newCapacity < 1)
                    newCapacity = 1;

                if (newCapacity != depotData.m_VehicleCapacity)
                {
                    if (debug)
                    {
                        Mod.s_Log.Info(
                            $"{Mod.ModTag} Depot apply: entity={entity.Index}:{entity.Version} " +
                            $"type={depotData.m_TransportType} BaseDepot={baseCapacity} scalar={scalar:F2} " +
                            $"OldDepot={depotData.m_VehicleCapacity} NewDepot={newCapacity}");
                    }

                    depotData.m_VehicleCapacity = newCapacity;
                }
            }

            // PASSENGERS (Bus / Tram / Train / Subway / Ship / Ferry / Airplane) — prefab-only
            foreach ((RefRW<PublicTransportVehicleData> vehicleRef, Entity entity) in SystemAPI
                         .Query<RefRW<PublicTransportVehicleData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                ref PublicTransportVehicleData vehicleData = ref vehicleRef.ValueRW;

                if (!IsHandledPassengerType(vehicleData.m_TransportType))
                {
                    continue;
                }

                m_SeenPassengerTypes.Add(vehicleData.m_TransportType);

                if (IsPrisonVan(entity))
                {
                    if (debug)
                    {
                        Mod.s_Log.Info($"{Mod.ModTag} Vehicle skip: {GetPrefabNameSafe(entity)} PrisonVan detected -> leaving seats vanilla.");
                    }

                    continue;
                }

                float scalar = GetPassengerScalar(settings, vehicleData.m_TransportType);

                int basePassengers;
                if (!TryGetPassengerBaseCapacity(entity, out basePassengers))
                {
                    if (debug)
                    {
                        Mod.s_Log.Warn(
                            $"{Mod.ModTag} Vehicle: failed to read vanilla base seats for entity={entity.Index}:{entity.Version}, type={vehicleData.m_TransportType}. " +
                            "Falling back to current passenger capacity.");
                    }

                    basePassengers = vehicleData.m_PassengerCapacity;
                }

                if (basePassengers < 1)
                    basePassengers = 1;

                int newPassengers = (int)(basePassengers * scalar);
                if (newPassengers < 1)
                    newPassengers = 1;

                if (newPassengers != vehicleData.m_PassengerCapacity)
                {
                    if (debug)
                    {
                        Mod.s_Log.Info(
                            $"{Mod.ModTag} Passengers apply: entity={entity.Index}:{entity.Version} " +
                            $"type={vehicleData.m_TransportType} BaseSeats={basePassengers} scalar={scalar:F2} " +
                            $"OldSeats={vehicleData.m_PassengerCapacity} NewSeats={newPassengers}");
                    }

                    vehicleData.m_PassengerCapacity = newPassengers;
                }

                if (debug)
                {
                    if (!m_PassengerSeatSummary.TryGetValue(vehicleData.m_TransportType, out SeatSummary summary))
                    {
                        summary = new SeatSummary();
                    }

                    summary.AddSample(basePassengers, newPassengers);
                    m_PassengerSeatSummary[vehicleData.m_TransportType] = summary;
                }
            }

            if (debug && !m_LoggedTypesOnce)
            {
                m_LoggedTypesOnce = true;

                string depotSummary = m_SeenDepotTypes.Count > 0 ? string.Join(", ", m_SeenDepotTypes) : "(none)";
                string passengerSummary = m_SeenPassengerTypes.Count > 0 ? string.Join(", ", m_SeenPassengerTypes) : "(none)";

                Mod.s_Log.Info($"{Mod.ModTag} Debug: City Summary -> DepotTypes=[{depotSummary}] PassengerTypes=[{passengerSummary}]");

                if (m_PassengerSeatSummary.Count > 0)
                {
                    foreach (KeyValuePair<TransportType, SeatSummary> kvp in m_PassengerSeatSummary)
                    {
                        TransportType type = kvp.Key;
                        SeatSummary summary = kvp.Value;
                        if (!summary.HasData)
                        {
                            continue;
                        }

                        float scalar = GetPassengerScalar(settings, type);
                        float percent = scalar * 100f;

                        if (type == TransportType.Tram)
                        {
                            int perSectionBase = summary.MinBase;
                            int perSectionNew = summary.MinNew;
                            int totalNew = perSectionNew * kTramSections;

                            Mod.s_Log.Info(
                                $"{Mod.ModTag} Debug: Tram passengers scaled {percent:F0}%, " +
                                $"{perSectionBase} -> {perSectionNew} x {kTramSections} sections = {totalNew} (per vehicle prefab type)");
                        }
                        else if (summary.MinBase == summary.MaxBase && summary.MinNew == summary.MaxNew)
                        {
                            Mod.s_Log.Info($"{Mod.ModTag} Debug: {type} passengers scaled {percent:F0}%, {summary.MinBase} -> {summary.MinNew} (per vehicle prefab type)");
                        }
                        else
                        {
                            Mod.s_Log.Info($"{Mod.ModTag} Debug: {type} passengers scaled {percent:F0}%, {summary.MinBase}-{summary.MaxBase} -> {summary.MinNew}-{summary.MaxNew} (per vehicle prefab types)");
                        }
                    }
                }
            }

            Enabled = false; // run-once behavior
        }

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

        private bool IsPrisonVan(Entity entity)
        {
            if (m_PrefabSystem == null)
                return false;

            if (!m_PrefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
                return false;

            string name = prefabBase.name;
            if (string.IsNullOrEmpty(name))
                return false;

            return name.IndexOf("PrisonVan", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private string GetPrefabNameSafe(Entity entity)
        {
            if (m_PrefabSystem == null)
                return $"PrefabEntity={entity.Index}:{entity.Version}";

            if (!m_PrefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
                return $"PrefabEntity={entity.Index}:{entity.Version}";

            return prefabBase.name ?? "(unnamed)";
        }

        private bool TryGetDepotBaseCapacity(Entity entity, out int baseCapacity)
        {
            baseCapacity = 0;

            if (m_PrefabSystem == null)
                return false;

            if (!m_PrefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
                return false;

            if (!prefabBase.TryGet(out TransportDepot depotComponent))
                return false;

            baseCapacity = depotComponent.m_VehicleCapacity;
            return true;
        }

        private bool TryGetPassengerBaseCapacity(Entity entity, out int basePassengers)
        {
            basePassengers = 0;

            if (m_PrefabSystem == null)
                return false;

            if (!m_PrefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
                return false;

            if (!prefabBase.TryGet(out PublicTransport publicTransport))
                return false;

            basePassengers = publicTransport.m_PassengerCapacity;
            return true;
        }

        private static float GetDepotScalar(Setting settings, TransportType type)
        {
            float percent;
            switch (type)
            {
                case TransportType.Bus: percent = settings.BusDepotScalar; break;
                case TransportType.Taxi: percent = settings.TaxiDepotScalar; break;
                case TransportType.Tram: percent = settings.TramDepotScalar; break;
                case TransportType.Train: percent = settings.TrainDepotScalar; break;
                case TransportType.Subway: percent = settings.SubwayDepotScalar; break;
                default: return 1f;
            }

            if (percent < Setting.DepotMinPercent) percent = Setting.DepotMinPercent;
            else if (percent > Setting.MaxPercent) percent = Setting.MaxPercent;

            return percent / 100f;
        }

        private static float GetPassengerScalar(Setting settings, TransportType type)
        {
            float percent;
            switch (type)
            {
                case TransportType.Bus: percent = settings.BusPassengerScalar; break;
                case TransportType.Tram: percent = settings.TramPassengerScalar; break;
                case TransportType.Train: percent = settings.TrainPassengerScalar; break;
                case TransportType.Subway: percent = settings.SubwayPassengerScalar; break;
                case TransportType.Ship: percent = settings.ShipPassengerScalar; break;
                case TransportType.Ferry: percent = settings.FerryPassengerScalar; break;
                case TransportType.Airplane: percent = settings.AirplanePassengerScalar; break;
                default: return 1f;
            }

            if (percent < Setting.PassengerMinPercent) percent = Setting.PassengerMinPercent;
            else if (percent > Setting.MaxPercent) percent = Setting.MaxPercent;

            return percent / 100f;
        }
    }
}
