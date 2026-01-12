// File: Systems/MaintenanceSystem.cs
// Purpose: Apply park/road maintenance vehicle and depot multipliers based on current settings.
// Notes:
// - Run-once system: enabled on city load or when settings Apply() enables it.
// - Uses PrefabSystem + PrefabBase to read vanilla/base values so results do NOT stack.
// - Includes name-based fallback for depots with MaintenanceType.None (extra garages / sub-prefabs).

namespace DispatchBoss
{
    using Colossal.Serialization.Entities;
    using Game;
    using Game.Prefabs;
    using Game.SceneFlow;
    using Game.Simulation; // MaintenanceType
    using System;
    using System.Collections.Generic;
    using Unity.Entities;

    public sealed partial class MaintenanceSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem = null!;

        // Prefab base caches (per city/session) — prevents stacking.
        private Dictionary<Entity, (int Cap, int Rate)> m_MaintenanceVehicleBase = null!;
        private Dictionary<Entity, int> m_MaintenanceDepotBaseVehicleCapacity = null!;

        private static bool s_LoggedPrefabNameException;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            m_MaintenanceVehicleBase = new Dictionary<Entity, (int Cap, int Rate)>();
            m_MaintenanceDepotBaseVehicleCapacity = new Dictionary<Entity, int>();

            EntityQuery q = SystemAPI.QueryBuilder()
                .WithAll<PrefabData>()
                .WithAny<MaintenanceVehicleData, MaintenanceDepotData>()
                .Build();

            RequireForUpdate(q);

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

            m_MaintenanceVehicleBase.Clear();
            m_MaintenanceDepotBaseVehicleCapacity.Clear();

            Mod.s_Log.Info($"{Mod.ModTag} City Loading Complete -> applying Maintenance settings");
            Enabled = true;
        }

        protected override void OnUpdate()
        {
            GameManager gm = GameManager.instance;
            if (gm == null || !gm.gameMode.IsGame())
            {
                Enabled = false;
                return;
            }

            if (Mod.Settings == null)
            {
                Enabled = false;
                return;
            }

            Setting settings = Mod.Settings;
            bool verbose = settings.EnableDebugLogging;

            // -------------------------
            // Maintenance depots: max vehicles (prefabs)
            // -------------------------
            {
                float roadDepotScalar = PercentToScalar(settings.RoadMaintenanceDepotScalar, Setting.MaintenanceMinPercent, Setting.MaintenanceMaxPercent);
                float parkDepotScalar = PercentToScalar(settings.ParkMaintenanceDepotScalar, Setting.MaintenanceMinPercent, Setting.MaintenanceMaxPercent);

                foreach ((RefRW<MaintenanceDepotData> depotRef, Entity prefabEntity) in SystemAPI
                             .Query<RefRW<MaintenanceDepotData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    ref MaintenanceDepotData data = ref depotRef.ValueRW;

                    MaintenanceType mt = data.m_MaintenanceType;

                    string prefabName = GetPrefabName(prefabEntity);

                    bool hasParkFlag = (mt & MaintenanceType.Park) != MaintenanceType.None;
                    bool hasRoadFlags = (mt & (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)) != MaintenanceType.None;

                    // Some depot sub-prefabs / upgrades report Type=None. Use name fallback.
                    bool isPark;
                    bool isRecognized;

                    if (mt == MaintenanceType.None)
                    {
                        if (prefabName.IndexOf("ParkMaintenanceDepot", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            isPark = true;
                            isRecognized = true;
                        }
                        else if (prefabName.IndexOf("RoadMaintenanceDepot", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            isPark = false;
                            isRecognized = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        isPark = hasParkFlag && !hasRoadFlags;
                        isRecognized = hasParkFlag || hasRoadFlags;

                        if (!isRecognized && !string.IsNullOrEmpty(prefabName))
                        {
                            if (prefabName.IndexOf("ParkMaintenanceDepot", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                isPark = true;
                                isRecognized = true;
                            }
                            else if (prefabName.IndexOf("RoadMaintenanceDepot", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                isPark = false;
                                isRecognized = true;
                            }
                        }

                        if (!isRecognized)
                        {
                            continue;
                        }
                    }

                    int baseVehicles = GetOrCacheMaintenanceDepotBase(prefabEntity, data.m_VehicleCapacity);

                    if (baseVehicles <= 0 && data.m_VehicleCapacity <= 0)
                    {
                        continue;
                    }

                    float scalar = isPark ? parkDepotScalar : roadDepotScalar;
                    int newVehicles = SafeMulIntAllowZero(baseVehicles, scalar);

                    // If base is >0 and scalar tiny, avoid collapsing to 1 vehicle.
                    if (baseVehicles > 0 && newVehicles == 1)
                        newVehicles = 2;

                    if (newVehicles != data.m_VehicleCapacity)
                    {
                        if (verbose)
                        {
                            string groupLabel = isPark ? "Park" : "Road";
                            Mod.s_Log.Info($"{Mod.ModTag} MaintenanceDepot({groupLabel}) vehicles: '{prefabName}' Type={mt} Base={baseVehicles} x{scalar:0.##} -> {newVehicles}");
                        }

                        data.m_VehicleCapacity = newVehicles;
                    }
                }
            }

            // -------------------------
            // Maintenance vehicles: capacity + rate (prefabs)
            // -------------------------
            {
                float roadCapScalar = PercentToScalar(settings.RoadMaintenanceVehicleCapacityScalar, Setting.MaintenanceMinPercent, Setting.MaintenanceMaxPercent);
                float roadRateScalar = PercentToScalar(settings.RoadMaintenanceVehicleRateScalar, Setting.MaintenanceMinPercent, Setting.MaintenanceMaxPercent);

                float parkCapScalar = PercentToScalar(settings.ParkMaintenanceVehicleCapacityScalar, Setting.MaintenanceMinPercent, Setting.MaintenanceMaxPercent);
                float parkRateScalar = PercentToScalar(settings.ParkMaintenanceVehicleRateScalar, Setting.MaintenanceMinPercent, Setting.MaintenanceMaxPercent);

                foreach ((RefRW<MaintenanceVehicleData> mvRef, Entity prefabEntity) in SystemAPI
                             .Query<RefRW<MaintenanceVehicleData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    ref MaintenanceVehicleData data = ref mvRef.ValueRW;

                    MaintenanceType mt = data.m_MaintenanceType;
                    if (mt == MaintenanceType.None)
                    {
                        continue;
                    }

                    string prefabName = GetPrefabName(prefabEntity);

                    bool hasParkFlag = (mt & MaintenanceType.Park) != MaintenanceType.None;
                    bool hasRoadFlags = (mt & (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)) != MaintenanceType.None;

                    bool isPark = hasParkFlag && !hasRoadFlags;

                    if (!hasParkFlag && !hasRoadFlags && !string.IsNullOrEmpty(prefabName))
                    {
                        if (prefabName.IndexOf("ParkMaintenance", StringComparison.OrdinalIgnoreCase) >= 0)
                            isPark = true;
                        else if (prefabName.IndexOf("RoadMaintenance", StringComparison.OrdinalIgnoreCase) >= 0)
                            isPark = false;
                    }

                    float capScalar = isPark ? parkCapScalar : roadCapScalar;
                    float rateScalar = isPark ? parkRateScalar : roadRateScalar;

                    (int Cap, int Rate) baseVals = GetOrCacheMaintenanceVehicleBase(prefabEntity, data.m_MaintenanceCapacity, data.m_MaintenanceRate);

                    int newCap = SafeMulInt(baseVals.Cap, capScalar);
                    int newRate = SafeMulInt(baseVals.Rate, rateScalar);

                    if (newCap != data.m_MaintenanceCapacity)
                    {
                        if (verbose)
                        {
                            string groupLabel = isPark ? "Park" : "Road";
                            Mod.s_Log.Info($"{Mod.ModTag} Maintenance({groupLabel}) cap: '{prefabName}' Type={mt} Base={baseVals.Cap} x{capScalar:0.##} -> {newCap}");
                        }

                        data.m_MaintenanceCapacity = newCap;
                    }

                    if (newRate != data.m_MaintenanceRate)
                    {
                        if (verbose)
                        {
                            string groupLabel = isPark ? "Park" : "Road";
                            Mod.s_Log.Info($"{Mod.ModTag} Maintenance({groupLabel}) rate: '{prefabName}' Type={mt} Base={baseVals.Rate} x{rateScalar:0.##} -> {newRate}");
                        }

                        data.m_MaintenanceRate = newRate;
                    }
                }
            }

            Enabled = false;
        }

        // -------------------------
        // Helpers
        // -------------------------

        private static float ClampPercent(float v, float min, float max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        private static float PercentToScalar(float percent, float minPercent, float maxPercent)
        {
            percent = ClampPercent(percent, minPercent, maxPercent);
            return percent / 100f;
        }

        private static int SafeMulInt(int baseValue, float scalar)
        {
            if (baseValue < 1)
                baseValue = 1;

            int v = (int)(baseValue * scalar);
            return v < 1 ? 1 : v;
        }

        private static int SafeMulIntAllowZero(int baseValue, float scalar)
        {
            if (baseValue <= 0)
                return 0;

            int v = (int)(baseValue * scalar);
            return v < 1 ? 1 : v;
        }

        private int GetOrCacheMaintenanceDepotBase(Entity prefabEntity, int currentValue)
        {
            if (m_MaintenanceDepotBaseVehicleCapacity.TryGetValue(prefabEntity, out int baseVehicles))
            {
                return baseVehicles;
            }

            int vanilla;
            if (TryGetMaintenanceDepotVanilla(prefabEntity, out vanilla) && vanilla >= 0)
                baseVehicles = vanilla;
            else
                baseVehicles = currentValue;

            m_MaintenanceDepotBaseVehicleCapacity[prefabEntity] = baseVehicles;
            return baseVehicles;
        }

        private bool TryGetMaintenanceDepotVanilla(Entity prefabEntity, out int baseVehicles)
        {
            baseVehicles = 0;

            if (m_PrefabSystem == null)
                return false;
            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
                return false;
            if (!prefabBase.TryGet(out Game.Prefabs.MaintenanceDepot depot))
                return false;

            baseVehicles = depot.m_VehicleCapacity;
            return true;
        }

        private (int Cap, int Rate) GetOrCacheMaintenanceVehicleBase(Entity prefabEntity, int currentCap, int currentRate)
        {
            if (m_MaintenanceVehicleBase.TryGetValue(prefabEntity, out (int Cap, int Rate) baseVals))
            {
                return baseVals;
            }

            int baseCap;
            int baseRate;

            if (!TryGetMaintenanceVehicleVanilla(prefabEntity, out baseCap, out baseRate))
            {
                baseCap = currentCap;
                baseRate = currentRate;
            }

            if (baseCap < 1) baseCap = 1;
            if (baseRate < 1) baseRate = 1;

            baseVals = (baseCap, baseRate);
            m_MaintenanceVehicleBase[prefabEntity] = baseVals;
            return baseVals;
        }

        private bool TryGetMaintenanceVehicleVanilla(Entity prefabEntity, out int baseCap, out int baseRate)
        {
            baseCap = 0;
            baseRate = 0;

            if (m_PrefabSystem == null)
                return false;
            if (!m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
                return false;
            if (!prefabBase.TryGet(out Game.Prefabs.MaintenanceVehicle mv))
                return false;

            baseCap = mv.m_MaintenanceCapacity;
            baseRate = mv.m_MaintenanceRate;
            return true;
        }

        private string GetPrefabName(Entity prefabEntity)
        {
            try
            {
                if (m_PrefabSystem != null && m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase))
                {
                    return prefabBase.name ?? "(unnamed)";
                }
            }
            catch (Exception ex)
            {
                if (!s_LoggedPrefabNameException)
                {
                    s_LoggedPrefabNameException = true;
                    Mod.s_Log.Warn($"{Mod.ModTag} GetPrefabName failed once: {ex.GetType().Name}: {ex.Message}");
                }
            }

            return $"PrefabEntity={prefabEntity.Index}:{prefabEntity.Version}";
        }
    }
}
