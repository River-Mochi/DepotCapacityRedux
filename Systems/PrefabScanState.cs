// File: Systems/PrefabScanState.cs
// Purpose: Shared scan state for PrefabScanSystem + Settings UI status text.
// Notes:
// - Managed-only state (RAM). Not saved.
// - Guards against spam clicks, and lightweight running timer for UI refresh.

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

        private static readonly object s_Lock = new object();

        private static Phase s_Phase = Phase.Idle;

        private static long s_RequestTick;
        private static long s_RunStartTick;

        private static DateTime s_LastRunStartedLocal;
        private static DateTime s_LastRunFinishedLocal;

        private static TimeSpan s_LastDuration;
        private static string s_Message = "Idle";

        public static Phase CurrentPhase
        {
            get
            {
                lock (s_Lock) return s_Phase;
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
                s_Message = "Queued";
                return true;
            }
        }

        public static void MarkRunning()
        {
            lock (s_Lock)
            {
                s_Phase = Phase.Running;
                s_RunStartTick = Stopwatch.GetTimestamp();
                s_LastRunStartedLocal = DateTime.Now;
                s_Message = "Running";
            }
        }

        public static void MarkDone(TimeSpan duration, string message)
        {
            lock (s_Lock)
            {
                s_Phase = Phase.Done;
                s_LastDuration = duration;
                s_LastRunFinishedLocal = DateTime.Now;
                s_Message = message;
            }
        }

        public static void MarkFailed(string message)
        {
            lock (s_Lock)
            {
                s_Phase = Phase.Failed;
                s_Message = string.IsNullOrEmpty(message) ? "Failed" : message;
            }
        }

        public static string GetStatusText()
        {
            lock (s_Lock)
            {
                switch (s_Phase)
                {
                    case Phase.Idle:
                        return "Scan Status: idle";

                    case Phase.Requested:
                        return $"Scan Status: queued ({GetElapsedText(s_RequestTick)})";

                    case Phase.Running:
                        return $"Scan Status: running ({GetElapsedText(s_RunStartTick)})";

                    case Phase.Done:
                        return $"Scan Status: done (Time Taken {FormatDuration(s_LastDuration)} | {s_LastRunFinishedLocal:yyyy-MM-dd HH:mm:ss})";

                    case Phase.Failed:
                        return $"Scan Status: {s_Message}";

                    default:
                        return "Scan Status: unknown";
                }
            }
        }

        private static string GetElapsedText(long startTick)
        {
            if (startTick <= 0)
            {
                return "00:00:00";
            }

            long now = Stopwatch.GetTimestamp();
            long delta = now - startTick;
            if (delta < 0) delta = 0;

            double seconds = delta / (double)Stopwatch.Frequency;
            if (seconds < 0) seconds = 0;

            return FormatDuration(TimeSpan.FromSeconds(seconds));
        }

        private static string FormatDuration(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
            {
                return ts.ToString(@"hh\:mm\:ss");
            }

            return ts.ToString(@"mm\:ss");
        }
    }
}
