// File: Systems/PrefabScanState.cs
// Purpose: Shared scan state for PrefabScanSystem + Settings UI status text.
// Notes:
// - Managed-only state (RAM). Not saved.
// - Guards against spam clicks, and provides running timer + last result metadata.

namespace DispatchBoss
{
    using System;
    using System.Diagnostics;

    public static class PrefabScanState
    {
        public enum Phase
        {
            Idle = 0,
            Requested = 1,
            Running = 2,
            Done = 3,
            Failed = 4,
        }

        public enum FailCode
        {
            None = 0,
            NoCityLoaded = 1,
            Exception = 2,
            Unknown = 3,
        }

        public readonly struct Snapshot
        {
            public Snapshot(
                Phase phase,
                FailCode failCode,
                string? failDetails,
                long requestTick,
                long runStartTick,
                DateTime lastRunFinishedLocal,
                TimeSpan lastDuration,
                string? lastReportPath)
            {
                Phase = phase;
                FailCode = failCode;
                FailDetails = failDetails;

                RequestTick = requestTick;
                RunStartTick = runStartTick;

                LastRunFinishedLocal = lastRunFinishedLocal;
                LastDuration = lastDuration;
                LastReportPath = lastReportPath;
            }

            public Phase Phase { get; }
            public FailCode FailCode { get; }
            public string? FailDetails { get; }

            public long RequestTick { get; }
            public long RunStartTick { get; }

            public DateTime LastRunFinishedLocal { get; }
            public TimeSpan LastDuration { get; }
            public string? LastReportPath { get; }
        }

        private static readonly object s_Lock = new object();

        private static Phase s_Phase = Phase.Idle;

        private static FailCode s_FailCode = FailCode.None;
        private static string? s_FailDetails;

        private static long s_RequestTick;
        private static long s_RunStartTick;

        private static DateTime s_LastRunFinishedLocal;
        private static TimeSpan s_LastDuration;

        private static string? s_LastReportPath;

        public static Phase CurrentPhase
        {
            get
            {
                lock (s_Lock) return s_Phase;
            }
        }

        public static Snapshot GetSnapshot()
        {
            lock (s_Lock)
            {
                return new Snapshot(
                    s_Phase,
                    s_FailCode,
                    s_FailDetails,
                    s_RequestTick,
                    s_RunStartTick,
                    s_LastRunFinishedLocal,
                    s_LastDuration,
                    s_LastReportPath);
            }
        }

        public static bool RequestScan()
        {
            lock (s_Lock)
            {
                if (s_Phase == Phase.Requested || s_Phase == Phase.Running)
                {
                    return false;
                }

                s_Phase = Phase.Requested;
                s_RequestTick = Stopwatch.GetTimestamp();

                s_FailCode = FailCode.None;
                s_FailDetails = null;

                return true;
            }
        }

        public static void MarkRunning()
        {
            lock (s_Lock)
            {
                s_Phase = Phase.Running;
                s_RunStartTick = Stopwatch.GetTimestamp();

                s_FailCode = FailCode.None;
                s_FailDetails = null;
            }
        }

        public static void MarkDone(TimeSpan duration, string reportPath)
        {
            lock (s_Lock)
            {
                s_Phase = Phase.Done;
                s_LastDuration = duration;
                s_LastRunFinishedLocal = DateTime.Now;
                s_LastReportPath = reportPath;

                s_FailCode = FailCode.None;
                s_FailDetails = null;
            }
        }

        public static void MarkFailed(FailCode code, string? details)
        {
            lock (s_Lock)
            {
                s_Phase = Phase.Failed;
                s_FailCode = code == FailCode.None ? FailCode.Unknown : code;
                s_FailDetails = details;

                // Keep last report info intact (sometimes useful even if a later run fails).
            }
        }

        public static TimeSpan GetElapsedSinceTick(long startTick)
        {
            if (startTick <= 0)
            {
                return TimeSpan.Zero;
            }

            long now = Stopwatch.GetTimestamp();
            long delta = now - startTick;
            if (delta < 0) delta = 0;

            double seconds = delta / (double)Stopwatch.Frequency;
            if (seconds < 0) seconds = 0;

            return TimeSpan.FromSeconds(seconds);
        }
    }
}
