// File: Systems/LaneWearSystem.cs
// Purpose: Apply RoadWearScalar (percent) to LaneDeteriorationData.m_TimeFactor (prefab lane deterioration settings).
// Notes:
// - Run-once system: enabled on city load or when settings Apply() enables it.
// - Caches original m_TimeFactor per prefab entity so changes do not stack.
// - Affects how quickly lanes accumulate deterioration (wear) over time.

namespace DispatchBoss
{
    using Colossal.Serialization.Entities;
    using Game;
    using Game.Prefabs;
    using Game.SceneFlow;
    using System;
    using System.Collections.Generic;
    using Unity.Entities;

    public sealed partial class LaneWearSystem : GameSystemBase
    {
        // Base (vanilla/current-session-original) time factor per prefab entity.
        // Key: prefab entity that has LaneDeteriorationData.
        // Value: captured lane.m_TimeFactor before any scaling is applied this session.
        private readonly Dictionary<Entity, float> m_BaseTimeFactor = new Dictionary<Entity, float>();

        protected override void OnCreate()
        {
            base.OnCreate();

            // Only run if there are prefab entities that actually have lane deterioration settings.
            // This is NOT per-road-segment data; it is prefab-level configuration used by many segments.
            EntityQuery q = SystemAPI.QueryBuilder()
                .WithAll<PrefabData, LaneDeteriorationData>()
                .Build();

            RequireForUpdate(q);

            // One-shot behavior: disabled by default; enabled by Apply() or by game-load hook below.
            Enabled = false;
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            // Only apply when a real city is being played/loaded.
            bool isRealGame =
                mode == GameMode.Game &&
                (purpose == Purpose.NewGame || purpose == Purpose.LoadGame);

            if (!isRealGame)
            {
                return;
            }

            // Clear cached bases so the first run after loading captures fresh "original" values
            // for this session (prevents accidental carryover between different city loads).
            m_BaseTimeFactor.Clear();

            // Enable so OnUpdate runs once right after loading completes.
            Enabled = true;
        }

        protected override void OnUpdate()
        {
            // Safety: only operate in an active city.
            GameManager gm = GameManager.instance;
            if (gm == null || !gm.gameMode.IsGame())
            {
                Enabled = false;
                return;
            }

            // Safety: settings must exist.
            if (Mod.Settings == null)
            {
                Enabled = false;
                return;
            }

            bool verbose = Mod.Settings.EnableDebugLogging;

            float percent = Mod.Settings.RoadWearScalar; // Percent based, 100 = vanilla, 200 = 2x faster wear.

            // Hard clamp so UI changes (or corrupted values) cannot push beyond supported range.
            if (percent < Setting.RoadWearMinPercent) percent = Setting.RoadWearMinPercent;
            if (percent > Setting.RoadWearMaxPercent) percent = Setting.RoadWearMaxPercent;

            // Convert percent to multiplier.
            float scalar = percent / 100f;  // Example: 200% => 2.0f, 50% => 0.5f

            int total = 0;
            int changed = 0;

            foreach ((RefRW<LaneDeteriorationData> laneRef, Entity e) in SystemAPI
                         .Query<RefRW<LaneDeteriorationData>>()
                         .WithAll<PrefabData>()
                         .WithEntityAccess())
            {
                total++;

                ref LaneDeteriorationData lane = ref laneRef.ValueRW;

                if (!m_BaseTimeFactor.TryGetValue(e, out float baseTf))
                {
                    baseTf = lane.m_TimeFactor;
                    m_BaseTimeFactor[e] = baseTf;
                }

                float desired = baseTf * scalar;

                if (desired < 0.0001f)
                {
                    desired = 0.0001f;
                }

                if (Math.Abs(lane.m_TimeFactor - desired) > 0.00001f)
                {
                    lane.m_TimeFactor = desired;
                    changed++;
                }
            }

            if (verbose)
            {
                Mod.s_Log.Info($"{Mod.ModTag} Lane wear: RoadWearScalar={percent:0.#}% Scalar={scalar:0.###} TotalPrefabs={total} Changed={changed}");
            }

            Enabled = false;
        }
    }
}
