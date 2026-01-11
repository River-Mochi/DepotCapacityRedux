// File: Systems/LaneWearSystem.cs
// Purpose: Apply RoadWearScalar (percent) to LaneDeteriorationData.m_TimeFactor (prefabs).
// Notes:
// - Run-once system: enabled on city load or when settings Apply() enables it.
// - Caches original m_TimeFactor per prefab entity so changes don't stack.
// - This affects how quickly lanes accumulate deterioration (wear).

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
        private readonly Dictionary<Entity, float> m_BaseTimeFactor = new Dictionary<Entity, float>();

        protected override void OnCreate()
        {
            base.OnCreate();

            EntityQuery q = SystemAPI.QueryBuilder()
                .WithAll<PrefabData, LaneDeteriorationData>()
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

            // Keep originals for the session; clear so first run captures fresh bases for this loaded city/session.
            m_BaseTimeFactor.Clear();
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

            bool verbose = Mod.Settings.EnableDebugLogging;

            float percent = Mod.Settings.RoadWearScalar;
            if (percent < Setting.RoadWearMinPercent) percent = Setting.RoadWearMinPercent;
            if (percent > Setting.RoadWearMaxPercent) percent = Setting.RoadWearMaxPercent;

            float scalar = percent / 100f;

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

                // avoid collapse to 0
                if (desired < 0.0001f)
                    desired = 0.0001f;

                if (Math.Abs(lane.m_TimeFactor - desired) > 0.00001f)
                {
                    lane.m_TimeFactor = desired;
                    changed++;
                }
            }

            if (verbose)
            {
                Mod.s_Log.Info($"{Mod.ModTag} Lane wear: RoadWearScalar={percent:0.#}% scalar={scalar:0.###} total={total} changed={changed}");
            }

            Enabled = false; // run-once behavior
        }
    }
}
