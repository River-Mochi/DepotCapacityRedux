// File: Systems/VehicleCountPolicyTunerSystem.cs
// Purpose: Optional Toggle to adjust VehicleCountPolicy VehicleInterval modifier so the vanilla transit line panel
//          can reach as low as 1 vehicle, while keeping maximums from going too high.
// Notes:
// - Global policy edit (affects all transit line types using VehicleCountPolicy).
// - One-shot: runs after city load, and whenever Settings.Apply enables it.
// - Toggle OFF restores the original policy values captured at first run (per session).

namespace DispatchBoss
{
    using Colossal.Mathematics;              // Bounds1
    using Colossal.Serialization.Entities;   // Purpose
    using Game;                              // GameMode
    using Game.Prefabs;                      // PrefabSystem, UITransportConfiguration*
    using Game.Routes;                       // RouteModifierData, RouteModifierType
    using Unity.Entities;                    // EntityQuery, ComponentType, DynamicBuffer

    public sealed partial class VehicleCountPolicyTunerSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem = null!;
        private EntityQuery m_ConfigQuery;

        private static bool s_HasOriginal;
        private static Bounds1 s_OriginalVehicleIntervalRange;
        private static ModifierValueMode s_OriginalVehicleIntervalMode;

        private const float kFewerVehiclesApplied = 22f;
        private const float kMoreVehiclesApplied = -0.84f;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            m_ConfigQuery = GetEntityQuery(ComponentType.ReadOnly<UITransportConfigurationData>());

            RequireForUpdate(m_ConfigQuery);
            Enabled = false;
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            bool isRealGame =
                mode == GameMode.Game &&
                (purpose == Purpose.NewGame || purpose == Purpose.LoadGame);

            if (!isRealGame)
                return;

            Enabled = true;
        }

        protected override void OnUpdate()
        {
            Enabled = false;

            if (m_ConfigQuery.IsEmptyIgnoreFilter)
                return;

            if (Mod.Settings == null)
                return;

            bool verbose = Mod.Settings.EnableDebugLogging;

            UITransportConfigurationPrefab config =
                m_PrefabSystem.GetSingletonPrefab<UITransportConfigurationPrefab>(m_ConfigQuery);

            Entity policyEntity = m_PrefabSystem.GetEntity(config.m_VehicleCountPolicy);

            if (policyEntity == Entity.Null || !SystemAPI.Exists(policyEntity))
            {
                Mod.s_Log.Warn($"{Mod.ModTag} VehicleCountPolicyTuner: could not resolve VehicleCountPolicy entity.");
                return;
            }

            if (!SystemAPI.HasBuffer<RouteModifierData>(policyEntity))
            {
                Mod.s_Log.Warn($"{Mod.ModTag} VehicleCountPolicyTuner: VehicleCountPolicy has no RouteModifierData buffer.");
                return;
            }

            bool enable = Mod.Settings.EnableLineVehicleCountTuner;

            DynamicBuffer<RouteModifierData> buf = SystemAPI.GetBuffer<RouteModifierData>(policyEntity);

            bool found = false;
            bool changed = false;

            for (int i = 0; i < buf.Length; i++)
            {
                RouteModifierData item = buf[i];
                if (item.m_Type != RouteModifierType.VehicleInterval)
                    continue;

                found = true;

                if (!s_HasOriginal)
                {
                    s_HasOriginal = true;
                    s_OriginalVehicleIntervalRange = item.m_Range;
                    s_OriginalVehicleIntervalMode = item.m_Mode;

                    Mod.s_Log.Info(
                        $"{Mod.ModTag} VehicleCountPolicyTuner: captured original VehicleInterval " +
                        $"mode={item.m_Mode} range={item.m_Range.min:F3}..{item.m_Range.max:F3} (input space)");
                }

                if (!enable)
                {
                    bool needRestore =
                        item.m_Range.min != s_OriginalVehicleIntervalRange.min ||
                        item.m_Range.max != s_OriginalVehicleIntervalRange.max ||
                        item.m_Mode != s_OriginalVehicleIntervalMode;

                    if (needRestore)
                    {
                        Bounds1 oldRange = item.m_Range;
                        ModifierValueMode oldMode = item.m_Mode;

                        item.m_Range = s_OriginalVehicleIntervalRange;
                        item.m_Mode = s_OriginalVehicleIntervalMode;

                        buf[i] = item;
                        changed = true;

                        Mod.s_Log.Info(
                            $"{Mod.ModTag} VehicleCountPolicyTuner: DISABLED -> restore VehicleInterval " +
                            $"mode {oldMode} -> {item.m_Mode}, " +
                            $"range {oldRange.min:F3}..{oldRange.max:F3} -> {item.m_Range.min:F3}..{item.m_Range.max:F3} (input space)");
                    }
                    else if (verbose)
                    {
                        Mod.s_Log.Info($"{Mod.ModTag} VehicleCountPolicyTuner: DISABLED -> already original (no change).");
                    }

                    continue;
                }

                if (item.m_Mode == ModifierValueMode.InverseRelative)
                {
                    Bounds1 desired = BuildInverseRelativeInputRange(kFewerVehiclesApplied, kMoreVehiclesApplied);

                    if (item.m_Range.min != desired.min || item.m_Range.max != desired.max)
                    {
                        Bounds1 oldRange = item.m_Range;

                        item.m_Range = desired;
                        buf[i] = item;
                        changed = true;

                        float appliedAtMinInput = InverseRelativeAppliedFromInput(desired.min);
                        float appliedAtMaxInput = InverseRelativeAppliedFromInput(desired.max);

                        Mod.s_Log.Info(
                            $"{Mod.ModTag} VehicleCountPolicyTuner: ENABLED -> VehicleInterval input range " +
                            $"{oldRange.min:F3}..{oldRange.max:F3} -> {desired.min:F3}..{desired.max:F3} (InverseRelative). " +
                            $"This maps to appliedΔ at endpoints: inputMin→{appliedAtMinInput:F3}, inputMax→{appliedAtMaxInput:F3}. " +
                            $"Targets: fewer={kFewerVehiclesApplied:F1}, more={kMoreVehiclesApplied:F2}");
                    }
                    else if (verbose)
                    {
                        Mod.s_Log.Info($"{Mod.ModTag} VehicleCountPolicyTuner: ENABLED -> already in desired state (no change).");
                    }
                }
                else if (item.m_Mode == ModifierValueMode.Relative)
                {
                    Mod.s_Log.Warn(
                        $"{Mod.ModTag} VehicleCountPolicyTuner: VehicleInterval mode is Relative; leaving unchanged. " +
                        $"Range={item.m_Range.min:F3}..{item.m_Range.max:F3}");
                }
                else
                {
                    Mod.s_Log.Warn(
                        $"{Mod.ModTag} VehicleCountPolicyTuner: VehicleInterval mode is {item.m_Mode}; not modifying. " +
                        $"Range={item.m_Range.min:F3}..{item.m_Range.max:F3}");
                }

                // Only one entry expected; bail early once handled.
                break;
            }

            if (!found)
            {
                Mod.s_Log.Warn($"{Mod.ModTag} VehicleCountPolicyTuner: no VehicleInterval RouteModifierData entry found.");
            }
            else if (!changed && verbose)
            {
                Mod.s_Log.Info($"{Mod.ModTag} VehicleCountPolicyTuner: completed (no change). enable={enable}");
            }
        }

        private static Bounds1 BuildInverseRelativeInputRange(float fewerVehiclesApplied, float moreVehiclesApplied)
        {
            float inputForFewer = InverseRelativeInputFromApplied(fewerVehiclesApplied);
            float inputForMore = InverseRelativeInputFromApplied(moreVehiclesApplied);

            float inputMin = inputForFewer;
            float inputMax = inputForMore;

            if (inputMin > inputMax)
            {
                float t = inputMin;
                inputMin = inputMax;
                inputMax = t;
            }

            if (inputMin <= -0.999f) inputMin = -0.999f;

            return new Bounds1(inputMin, inputMax);
        }

        private static float InverseRelativeInputFromApplied(float applied)
        {
            return (-applied) / (1f + applied);
        }

        private static float InverseRelativeAppliedFromInput(float input)
        {
            return (-input) / (1f + input);
        }
    }
}
