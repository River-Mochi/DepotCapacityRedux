// File: Utils/PrefabScanStatusText.cs
// Purpose: Builds the player-facing prefab scan status string from PrefabScanState data.
// Notes:
// - Centralizes all status words in ONE place.
// - Later, this can be wired to real localization lookup (Phase/FailCode -> localized strings).

namespace DispatchBoss
{
    using System;

    public static class PrefabScanStatusText
    {
        // Status words (English fallback for now).
        // Later: map these via locale keys if you decide to add runtime localization lookup.
        private const string Idle = "Idle";
        private const string Queued = "Queued";
        private const string Running = "Running";
        private const string Done = "Done";
        private const string Failed = "Failed";

        // Failure reasons (English fallback).
        private const string NoCityLoaded = "No city loaded.";

        public static string Format(PrefabScanState.Snapshot s)
        {
            switch (s.Phase)
            {
                case PrefabScanState.Phase.Idle:
                    return Idle;

                case PrefabScanState.Phase.Requested:
                    {
                        TimeSpan elapsed = PrefabScanState.GetElapsedSinceTick(s.RequestTick);
                        return $"{Queued} ({FormatDuration(elapsed)})";
                    }

                case PrefabScanState.Phase.Running:
                    {
                        TimeSpan elapsed = PrefabScanState.GetElapsedSinceTick(s.RunStartTick);
                        return $"{Running} ({FormatDuration(elapsed)})";
                    }

                case PrefabScanState.Phase.Done:
                    {
                        string dur = FormatDuration(s.LastDuration);
                        string ts = s.LastRunFinishedLocal == default
                            ? "unknown time"
                            : s.LastRunFinishedLocal.ToString("yyyy-MM-dd HH:mm:ss");

                        // Keep it compact; the report path is already shown in button desc + written to log.
                        return $"{Done} ({dur} | {ts})";
                    }

                case PrefabScanState.Phase.Failed:
                default:
                    {
                        string reason = s.FailCode switch
                        {
                            PrefabScanState.FailCode.NoCityLoaded => NoCityLoaded,
                            _ => string.Empty
                        };

                        if (!string.IsNullOrEmpty(s.FailDetails))
                        {
                            // Details are typically exception text (not worth translating).
                            return $"{Failed} ({reason} {s.FailDetails})".Trim();
                        }

                        if (!string.IsNullOrEmpty(reason))
                        {
                            return $"{Failed} ({reason})";
                        }

                        return Failed;
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
