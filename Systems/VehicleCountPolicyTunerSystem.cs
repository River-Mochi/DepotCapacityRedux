// File: Systems/VehicleCountPolicyTunerSystem.cs
// Purpose: (Optional Toggle) Adjust VehicleCountPolicy VehicleInterval modifier so the vanilla transit line panel
//          can reach as low as 1 vehicle, while letting maximums go a bit higher (still capped).
// Notes:
// - Global policy edit (affects all transit line types using VehicleCountPolicy).
// - Runs once after city load, and once whenever Settings.Apply enables it.
// - Toggle OFF restores the original policy values captured at first run (per city load).

namespace DispatchBoss
{
    using Colossal.Mathematics;              // Bounds1
    using Colossal.Serialization.Entities;   // Purpose
    using Game;                              // GameMode
    using Game.Prefabs;                      // PrefabSystem, UITransportConfiguration*
    using Game.Routes;                       // RouteModifierData, RouteModifierType
    using System;
    using Unity.Entities;                    // EntityQuery, ComponentType, DynamicBuffer

    public sealed partial class VehicleCountPolicyTunerSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem = null!;
        private EntityQuery m_ConfigQuery;
        private bool m_Done;

        // Store original so disabling the toggle undoes the change.
        // Static is OK, but MUST reset on each city load to avoid cross-city contamination in one game session.
        private static bool s_HasOriginal;
        private static Bounds1 s_OriginalVehicleIntervalRange;
        private static ModifierValueMode s_OriginalVehicleIntervalMode;

        // ---- TUNING (no Harmony) ----
        // Goals:
        // - Still able to hit "1 vehicle" minimum on all lines → needs a strong "fewer vehicles" side (longer interval).
        // - Allow a somewhat higher max count than before → allow a slightly shorter interval (more negative applied).
        //
        // If max values still too low: make kMoreVehiclesApplied more negative (e.g. -0.82f or -0.85f).
        // If it doesn't reach 1 vehicle on some lines: increase kFewerVehiclesApplied (e.g. 26f or 30f).
        private const float kFewerVehiclesApplied = 24f;   // helps reach 1 vehicle
        private const float kMoreVehiclesApplied = -0.80f; // was -0.75f; allows slightly higher max vehicles

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            m_ConfigQuery = GetEntityQuery(ComponentType.ReadOnly<UITransportConfigurationData>());

            RequireForUpdate(m_ConfigQuery);
            Enabled = false; // run-once after load, and when Settings.Apply enables us
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

            // IMPORTANT: reset original capture per city load.
            s_HasOriginal = false;
            s_OriginalVehicleIntervalRange = default;
            s_OriginalVehicleIntervalMode = default;

            m_Done = false;
            Enabled = true;
        }

        protected override void OnUpdate()
        {
            if (m_Done || m_ConfigQuery.IsEmptyIgnoreFilter)
            {
                Enabled = false;
                return;
            }

            if (Mod.Settings == null)
            {
                Enabled = false;
                return;
            }

            UITransportConfigurationPrefab config =
                m_PrefabSystem.GetSingletonPrefab<UITransportConfigurationPrefab>(m_ConfigQuery);

            Entity policyEntity = m_PrefabSystem.GetEntity(config.m_VehicleCountPolicy);

            if (policyEntity == Entity.Null || !EntityManager.Exists(policyEntity))
            {
                Mod.s_Log.Warn($"{Mod.ModTag} VehicleCountPolicyTuner: could not resolve VehicleCountPolicy entity.");
                Enabled = false;
                return;
            }

            if (!EntityManager.HasBuffer<RouteModifierData>(policyEntity))
            {
                Mod.s_Log.Warn($"{Mod.ModTag} VehicleCountPolicyTuner: VehicleCountPolicy has no RouteModifierData buffer.");
                Enabled = false;
                return;
            }

            bool enable = Mod.Settings.EnableLineVehicleCountTuner;

            DynamicBuffer<RouteModifierData> buf = EntityManager.GetBuffer<RouteModifierData>(policyEntity);

            bool found = false;
            bool changed = false;

            for (int i = 0; i < buf.Length; i++)
            {
                RouteModifierData item = buf[i];
                if (item.m_Type != RouteModifierType.VehicleInterval)
                {
                    continue;
                }

                found = true;

                // Capture original once (whatever it was before our mod changes it) per city load.
                if (!s_HasOriginal)
                {
                    s_HasOriginal = true;
                    s_OriginalVehicleIntervalRange = item.m_Range;
                    s_OriginalVehicleIntervalMode = item.m_Mode;

                    Mod.s_Log.Info(
                        $"{Mod.ModTag} VehicleCountPolicyTuner: captured original VehicleInterval mode={item.m_Mode} " +
                        $"range={item.m_Range.min:F3}..{item.m_Range.max:F3}");
                }

                if (!enable)
                {
                    // Restore original (range + mode)
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
                            $"range {oldRange.min:F3}..{oldRange.max:F3} -> {item.m_Range.min:F3}..{item.m_Range.max:F3}");
                    }

                    continue;
                }

                // Enabled: apply a sane widening (best supported is InverseRelative).
                if (item.m_Mode == ModifierValueMode.InverseRelative)
                {
                    Bounds1 desired = BuildInverseRelativeInputRange(kFewerVehiclesApplied, kMoreVehiclesApplied);

                    if (item.m_Range.min != desired.min || item.m_Range.max != desired.max)
                    {
                        Bounds1 oldRange = item.m_Range;

                        item.m_Range = desired;
                        buf[i] = item;
                        changed = true;

                        Mod.s_Log.Info(
                            $"{Mod.ModTag} VehicleCountPolicyTuner: ENABLED -> set VehicleInterval " +
                            $"{oldRange.min:F3}..{oldRange.max:F3} -> {desired.min:F3}..{desired.max:F3} (mode={item.m_Mode})");
                    }
                }
                else if (item.m_Mode == ModifierValueMode.Relative)
                {
                    Mod.s_Log.Warn(
                        $"{Mod.ModTag} VehicleCountPolicyTuner: VehicleInterval mode is Relative; leaving unchanged to avoid runaway max counts. " +
                        $"Range={item.m_Range.min:F3}..{item.m_Range.max:F3}");
                }
                else
                {
                    Mod.s_Log.Warn(
                        $"{Mod.ModTag} VehicleCountPolicyTuner: VehicleInterval mode is {item.m_Mode}; not modifying. " +
                        $"Range={item.m_Range.min:F3}..{item.m_Range.max:F3}");
                }
            }

            if (!found)
            {
                Mod.s_Log.Warn($"{Mod.ModTag} VehicleCountPolicyTuner: no VehicleInterval RouteModifierData entry found.");
            }
            else if (!changed)
            {
                Mod.s_Log.Info($"{Mod.ModTag} VehicleCountPolicyTuner: no change needed (already in desired state).");
            }

            m_Done = true;
            Enabled = false;
        }

        private static Bounds1 BuildInverseRelativeInputRange(float fewerVehiclesApplied, float moreVehiclesApplied)
        {
            // InverseRelative uses transform: f(x) = 1/(1+x) - 1, and f is its own inverse.
            // We want slider-min (fewer vehicles) to map to a positive applied delta,
            // and slider-max (more vehicles) to map to a negative applied delta.
            float inputMin = InverseRelativeInputFromApplied(fewerVehiclesApplied);
            float inputMax = InverseRelativeInputFromApplied(moreVehiclesApplied);

            if (inputMin > inputMax)
            {
                float t = inputMin;
                inputMin = inputMax;
                inputMax = t;
            }

            // Safety: avoid exactly -1 (would explode).
            if (inputMin <= -0.999f) inputMin = -0.999f;

            return new Bounds1(inputMin, inputMax);
        }

        private static float InverseRelativeInputFromApplied(float applied)
        {
            // f(x) = -x/(1+x)  (equivalent to 1/(1+x)-1, simplified)
            // And f is self-inverse, so inputFromApplied = f(applied).
            return (-applied) / (1f + applied);
        }
    }
}
