// File: Systems/LaneWearProbeSystem.cs
// Purpose: Verbose probe for LaneCondition.m_Wear to validate lane wear slider behavior.
// Notes:
// - Uses SystemAPI (queries + lookups).
// - Samples a small set of lanes and logs deltas (avoids log spam).
// - Expected time-wear per tick: (1/16) * TimeFactor.

namespace DispatchBoss
{
    using Game;
    using Game.Net;
    using Game.Prefabs;
    using Game.Simulation;
    using System.Collections.Generic;
    using Unity.Entities;
    using Unity.Mathematics;

    public sealed partial class LaneWearProbeSystem : GameSystemBase
    {
        private const int kSamplePerLog = 25;
        private const uint kFramesBetweenLogs = 1024; // throttle hard; adjustable

        private SimulationSystem m_Sim = null!;

        private uint m_LastLogFrame;
        private int m_RoundRobinSkip;

        private readonly Dictionary<Entity, float> m_LastWear = new Dictionary<Entity, float>();

        protected override void OnCreate()
        {
            base.OnCreate();

            m_Sim = World.GetOrCreateSystemManaged<SimulationSystem>();

            // Only run when lanes exist.
            EntityQuery q = SystemAPI.QueryBuilder()
                .WithAll<LaneCondition, PrefabRef>()
                .Build();

            RequireForUpdate(q);
            Enabled = true; // probe can stay enabled; it self-throttles
        }

        protected override void OnUpdate()
        {
            if (Mod.Settings == null || !Mod.Settings.EnableDebugLogging)
                return;

            uint frame = m_Sim.frameIndex;

            if (frame - m_LastLogFrame < kFramesBetweenLogs)
                return;

            m_LastLogFrame = frame;

            var detLookup = SystemAPI.GetComponentLookup<LaneDeteriorationData>(isReadOnly: true);

            int seen = 0;
            int sampled = 0;

            float sumDelta = 0f;
            float maxDelta = 0f;

            // Round-robin sampling without storing entities:
            // skip some lanes each log so it's not inspecting the same first chunk.
            int skip = m_RoundRobinSkip;
            int newSkip = skip;

            foreach (var (condRO, prRO, laneEntity) in SystemAPI
                         .Query<RefRO<LaneCondition>, RefRO<PrefabRef>>()
                         .WithEntityAccess())
            {
                if (skip > 0)
                {
                    skip--;
                    continue;
                }

                seen++;

                LaneCondition cond = condRO.ValueRO;
                PrefabRef pr = prRO.ValueRO;

                float tf = float.NaN;
                float traf = float.NaN;

                if (detLookup.HasComponent(pr.m_Prefab))
                {
                    LaneDeteriorationData det = detLookup[pr.m_Prefab];
                    tf = det.m_TimeFactor;
                    traf = det.m_TrafficFactor;
                }

                float last = m_LastWear.TryGetValue(laneEntity, out float v) ? v : float.NaN;
                float delta = float.IsNaN(last) ? 0f : (cond.m_Wear - last);
                m_LastWear[laneEntity] = cond.m_Wear;

                // From dnSpy: expected time contribution per time-tick is TimeFactor / 16.
                // Don't know how many time-ticks happen between logs, so this is more a scale reference.
                float expectedPerTick = float.IsNaN(tf) ? float.NaN : (tf / 16f);

                sumDelta += delta;
                maxDelta = math.max(maxDelta, delta);

                Mod.s_Log.Info(
                    $"{Mod.ModTag} LaneWearProbe lane={laneEntity.Index}:{laneEntity.Version} " +
                    $"wear={cond.m_Wear:0.###} Δ={delta:0.###} " +
                    $"TimeFactor={Fmt(tf)} TrafficFactor={Fmt(traf)} ExpΔ(Time)/Tick={Fmt(expectedPerTick)}");

                sampled++;
                if (sampled >= kSamplePerLog)
                {
                    newSkip += sampled; // next time, start further in
                    break;
                }
            }

            if (sampled == 0)
            {
                Mod.s_Log.Info($"{Mod.ModTag} LaneWearProbe: no lanes sampled (query empty?)");
                return;
            }

            float avgDelta = sumDelta / sampled;

            Mod.s_Log.Info(
                $"{Mod.ModTag} LaneWearProbe summary: Sampled={sampled} AvgΔ={avgDelta:0.###} MaxΔ={maxDelta:0.###} " +
                $"Frame={frame} NextSkipStart={newSkip}");

            m_RoundRobinSkip = newSkip;
        }

        private static string Fmt(float v) => float.IsNaN(v) ? "n/a" : v.ToString("0.###");
    }
}
