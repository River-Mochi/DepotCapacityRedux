// File: Systems/LaneWearProbeSystem.cs
// Purpose: Verbose probe for LaneCondition.m_Wear to validate lane wear slider behavior.
// Notes:
// - Samples lanes in the current UpdateFrame group (matches NetDeteriorationSystem cadence).
// - Stores a small per-group sample set so deltas become meaningful after groups cycle.
// - UpdateFrame is a shared component (ISharedComponentData), so we read it via EntityManager.GetSharedComponentData.

namespace DispatchBoss
{
    using Game;
    using Game.Net;
    using Game.Prefabs;
    using Game.Simulation;
    using System;
    using Unity.Entities;
    using Unity.Mathematics;

    [UpdateAfter(typeof(Game.Simulation.NetDeteriorationSystem))]
    public sealed partial class LaneWearProbeSystem : GameSystemBase
    {
        private const int kUpdatesPerDay = 16;     // NetDeteriorationSystem.kUpdatesPerDay
        private const int kGroupCount = 16;        // lane update group count
        private const int kSamplesPerGroup = 3;    // keep tiny to avoid log spam

        // Log once per NetDeteriorationSystem run (1024 frames). This means deltas for a given group
        // become non-zero after that group repeats (≈16 logs ≈ 4–5 minutes at ~60fps).
        private const uint kFramesBetweenLogs = 1024;

        private SimulationSystem m_Sim = null!;

        private uint m_LastLogFrame;

        // Per-group samples (group * kSamplesPerGroup + i)
        private Entity[] m_Samples = null!;
        private float[] m_LastWear = null!;
        private bool[] m_HasLast = null!;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_Sim = World.GetOrCreateSystemManaged<SimulationSystem>();

            m_Samples = new Entity[kGroupCount * kSamplesPerGroup];
            m_LastWear = new float[kGroupCount * kSamplesPerGroup];
            m_HasLast = new bool[kGroupCount * kSamplesPerGroup];

            // Only run when lanes exist.
            EntityQuery q = SystemAPI.QueryBuilder()
                .WithAll<LaneCondition, PrefabRef, UpdateFrame>()
                .Build();

            RequireForUpdate(q);
        }

        protected override void OnUpdate()
        {
            if (Mod.Settings == null || !Mod.Settings.EnableDebugLogging)
                return;

            uint frame = m_Sim.frameIndex;
            if (frame - m_LastLogFrame < kFramesBetweenLogs)
                return;

            m_LastLogFrame = frame;

            uint groupU = SimulationUtils.GetUpdateFrame(frame, kUpdatesPerDay, kGroupCount);
            int group = (int)groupU;
            UpdateFrame target = new UpdateFrame(groupU);

            var detLookup = SystemAPI.GetComponentLookup<LaneDeteriorationData>(isReadOnly: true);

            EnsureSamplesForGroup(group, target);

            int baseIndex = group * kSamplesPerGroup;

            float sumDelta = 0f;
            float maxAbsDelta = 0f;
            int logged = 0;

            for (int i = 0; i < kSamplesPerGroup; i++)
            {
                int idx = baseIndex + i;
                Entity lane = m_Samples[idx];

                if (lane == Entity.Null || !EntityManager.Exists(lane) || !EntityManager.HasComponent<LaneCondition>(lane))
                    continue;

                LaneCondition cond = EntityManager.GetComponentData<LaneCondition>(lane);
                PrefabRef pr = EntityManager.GetComponentData<PrefabRef>(lane);

                float tf = float.NaN;
                float traf = float.NaN;

                if (detLookup.HasComponent(pr.m_Prefab))
                {
                    LaneDeteriorationData det = detLookup[pr.m_Prefab];
                    tf = det.m_TimeFactor;
                    traf = det.m_TrafficFactor;
                }

                float delta = 0f;
                if (m_HasLast[idx])
                    delta = cond.m_Wear - m_LastWear[idx];

                m_LastWear[idx] = cond.m_Wear;
                m_HasLast[idx] = true;

                sumDelta += delta;
                maxAbsDelta = math.max(maxAbsDelta, math.abs(delta));
                logged++;

                float expectedPerTick = float.IsNaN(tf) ? float.NaN : (tf / 16f);

                Mod.s_Log.Info(
                    $"{Mod.ModTag} [LaneWearProbe g={group}] lane={lane.Index}:{lane.Version} " +
                    $"wear={cond.m_Wear:0.###} Δ={delta:0.###} " +
                    $"Prefab={pr.m_Prefab.Index}:{pr.m_Prefab.Version} TF={Fmt(tf)} TrF={Fmt(traf)} ExpΔ(Time)/Tick={Fmt(expectedPerTick)}");
            }

            if (logged == 0)
            {
                Mod.s_Log.Info($"{Mod.ModTag} [LaneWearProbe g={group}] no samples found (no lanes in this group?) Frame={frame}");
                return;
            }

            float avg = sumDelta / logged;

            Mod.s_Log.Info(
                $"{Mod.ModTag} [LaneWearProbe g={group}] summary: Logged={logged} AvgΔ={avg:0.###} Max|Δ|={maxAbsDelta:0.###} Frame={frame}");
        }

        private void EnsureSamplesForGroup(int group, UpdateFrame target)
        {
            int baseIndex = group * kSamplesPerGroup;

            // If all present and valid, keep them (stable sampling).
            bool allValid = true;
            for (int i = 0; i < kSamplesPerGroup; i++)
            {
                Entity e = m_Samples[baseIndex + i];
                if (e == Entity.Null || !EntityManager.Exists(e))
                {
                    allValid = false;
                    break;
                }
            }

            if (allValid)
                return;

            // Refill samples for this group.
            for (int i = 0; i < kSamplesPerGroup; i++)
            {
                m_Samples[baseIndex + i] = Entity.Null;
                m_HasLast[baseIndex + i] = false;
                m_LastWear[baseIndex + i] = 0f;
            }

            int filled = 0;

            foreach ((RefRO<LaneCondition> _, RefRO<PrefabRef> __, Entity laneEntity) in SystemAPI
                         .Query<RefRO<LaneCondition>, RefRO<PrefabRef>>()
                         .WithAll<UpdateFrame>()
                         .WithEntityAccess())
            {
                // UpdateFrame is shared: compare via GetSharedComponentData.
                UpdateFrame uf = EntityManager.GetSharedComponentData<UpdateFrame>(laneEntity);
                if (!uf.Equals(target))
                    continue;

                bool dup = false;
                for (int i = 0; i < filled; i++)
                {
                    if (m_Samples[baseIndex + i] == laneEntity)
                    {
                        dup = true;
                        break;
                    }
                }

                if (dup)
                    continue;

                m_Samples[baseIndex + filled] = laneEntity;
                filled++;

                if (filled >= kSamplesPerGroup)
                    break;
            }
        }

        private static string Fmt(float v) => float.IsNaN(v) ? "n/a" : v.ToString("0.###");
    }
}
