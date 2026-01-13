// File: Utils/PrefabScanStatusText.cs
// Purpose: Builds the player-facing prefab scan status string from PrefabScanState data.
// Notes:
// - Uses localization keys with English fallbacks.
// - Does not translate FailDetails (usually exception text).

namespace DispatchBoss
{
    using System;

    public static class PrefabScanStatusText
    {
        // Locale keys (templates)
        private const string KeyIdle = "DB_SCAN_IDLE";
        private const string KeyQueuedFmt = "DB_SCAN_QUEUED_FMT";     // "{0}" = elapsed
        private const string KeyRunningFmt = "DB_SCAN_RUNNING_FMT";   // "{0}" = elapsed
        private const string KeyDoneFmt = "DB_SCAN_DONE_FMT";         // "{0}" = duration, "{1}" = timestamp
        private const string KeyFailed = "DB_SCAN_FAILED";
        private const string KeyFailNoCity = "DB_SCAN_FAIL_NO_CITY";
        private const string KeyUnknownTime = "DB_SCAN_UNKNOWN_TIME";

        public static string Format(PrefabScanState.Snapshot s)
        {
            switch (s.Phase)
            {
                case PrefabScanState.Phase.Idle:
                    return Mod.L(KeyIdle, "Idle");

                case PrefabScanState.Phase.Requested:
                    {
                        TimeSpan elapsed = PrefabScanState.GetElapsedSinceTick(s.RequestTick);
                        return string.Format(
                            Mod.L(KeyQueuedFmt, "Queued ({0})"),
                            FormatDuration(elapsed));
                    }

                case PrefabScanState.Phase.Running:
                    {
                        TimeSpan elapsed = PrefabScanState.GetElapsedSinceTick(s.RunStartTick);
                        return string.Format(
                            Mod.L(KeyRunningFmt, "Running ({0})"),
                            FormatDuration(elapsed));
                    }

                case PrefabScanState.Phase.Done:
                    {
                        string dur = FormatDuration(s.LastDuration);

                        string ts = s.LastRunFinishedLocal == default
                            ? Mod.L(KeyUnknownTime, "unknown time")
                            : s.LastRunFinishedLocal.ToString("yyyy-MM-dd HH:mm:ss");

                        // Compact here; report path shown elsewhere.
                        return string.Format(
                            Mod.L(KeyDoneFmt, "Done ({0} | {1})"),
                            dur,
                            ts);
                    }

                case PrefabScanState.Phase.Failed:
                default:
                    {
                        string failed = Mod.L(KeyFailed, "Failed");

                        string reason = s.FailCode switch
                        {
                            PrefabScanState.FailCode.NoCityLoaded => Mod.L(KeyFailNoCity, "LOAD CITY FIRST"),
                            _ => string.Empty
                        };

                        if (!string.IsNullOrEmpty(s.FailDetails))
                        {
                            // Fails are typically Exception text (not worth translating).
                            if (!string.IsNullOrEmpty(reason))
                                return $"{failed} ({reason} {s.FailDetails})".Trim();

                            return $"{failed} ({s.FailDetails})".Trim();
                        }

                        if (!string.IsNullOrEmpty(reason))
                        {
                            return $"{failed} - {reason}";
                        }

                        return failed;
                    }
            }
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
