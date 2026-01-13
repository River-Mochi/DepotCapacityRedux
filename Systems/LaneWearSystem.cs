// File: Systems/LaneWearSystem.cs
// Purpose: Apply RoadWearScalar (percent) to BOTH LaneDeteriorationData.m_TimeFactor and m_TrafficFactor (prefab lane deterioration settings).
// Notes:
// - Run-once system: enabled on city load or when settings Apply() enables it.
// - Caches original m_TimeFactor + m_TrafficFactor per prefab entity so changes do not stack.
// - Affects how quickly lanes accumulate deterioration from BOTH time and traffic.

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
        private struct BaseFactors
        {
            public float Time;
            public float Traffic;
        }

        // Base (vanilla/current-session-original) factors per prefab entity (LaneDeteriorationData).
        private readonly Dictionary<Entity, BaseFactors> m_Base = new Dictionary<Entity, BaseFactors>();

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
                return;

            m_Base.Clear();
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

            float percent = Mod.Settings.RoadWearScalar; // 100 = vanilla
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

                if (!m_Base.TryGetValue(e, out BaseFactors baseF))
                {
                    baseF = new BaseFactors
                    {
                        Time = lane.m_TimeFactor,
                        Traffic = lane.m_TrafficFactor,
                    };
                    m_Base[e] = baseF;
                }

                float desiredTime = baseF.Time * scalar;
                float desiredTraffic = baseF.Traffic * scalar;

                // Keep tiny positives so “0” doesn’t effectively freeze wear forever unless you explicitly want that behavior.
                if (desiredTime < 0.0001f) desiredTime = 0.0001f;
                if (desiredTraffic < 0.0001f) desiredTraffic = 0.0001f;

                bool any = false;

                if (Math.Abs(lane.m_TimeFactor - desiredTime) > 0.00001f)
                {
                    lane.m_TimeFactor = desiredTime;
                    any = true;
                }

                if (Math.Abs(lane.m_TrafficFactor - desiredTraffic) > 0.00001f)
                {
                    lane.m_TrafficFactor = desiredTraffic;
                    any = true;
                }

                if (any) changed++;
            }

            if (verbose)
            {
                Mod.s_Log.Info(
                    $"{Mod.ModTag} Lane wear: RoadWearScalar={percent:0.#}% Scalar={scalar:0.###} " +
                    $"Prefabs={total} Changed={changed} (scaled TimeFactor + TrafficFactor)");
            }

            Enabled = false;
        }
    }
}
