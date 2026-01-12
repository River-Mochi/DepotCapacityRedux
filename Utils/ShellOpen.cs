// File: Utils/ShellOpen.cs
// Purpose: Cross-platform-ish folder opening helper for Options UI buttons.
// Notes:
// - First attempt: Unity Application.OpenURL with a file:// URI.
// - Fallback: OS shell open (Windows/macOS/Linux).
// - Safe: catches exceptions; no crash on failure.

namespace DispatchBoss
{
    using Colossal.PSI.Environment;
    using System;
    using System.Diagnostics;
    using System.IO;
    using UnityEngine;

    internal static class ShellOpen
    {
        internal static void OpenFolderSafe(string folderPath, string logLabel)
        {
            try
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    Mod.s_Log.Info($"{Mod.ModTag} {logLabel}: folder path is empty.");
                    return;
                }

                // Normalize to an absolute path early so Uri and OS launch both behave consistently.
                string fullPath = Path.GetFullPath(folderPath);

                if (!Directory.Exists(fullPath))
                {
                    Mod.s_Log.Info($"{Mod.ModTag} {logLabel}: folder not found: {fullPath}");
                    return;
                }

                // Attempt 1: Unity file:// open (best chance to work across platforms).
                if (TryOpenWithUnityFileUrl(fullPath))
                {
                    return;
                }

                // Attempt 2: OS fallback (most reliable on Windows).
                TryOpenWithOsShell(fullPath);
            }
            catch (Exception ex)
            {
                Mod.s_Log.Warn($"{Mod.ModTag} {logLabel}: failed opening folder: {ex.GetType().Name}: {ex.Message}");
            }
        }

        internal static string GetLogsFolder()
        {
            return Path.Combine(EnvPath.kUserDataPath, "Logs");
        }

        internal static string GetModsDataFolder()
        {
            return Path.Combine(EnvPath.kUserDataPath, "ModsData", nameof(DispatchBoss));
        }

        private static bool TryOpenWithUnityFileUrl(string fullPath)
        {
            try
            {
                // Ensure trailing slash for directories to form a clean file URI.
                string path = fullPath;
                if (!path.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) &&
                    !path.EndsWith(Path.AltDirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                {
                    path += Path.DirectorySeparatorChar;
                }

                // new Uri(absolutePath) produces a file:/// URI for local file paths.
                string uri = new Uri(path).AbsoluteUri;

                Application.OpenURL(uri);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void TryOpenWithOsShell(string fullPath)
        {
            try
            {
                RuntimePlatform p = Application.platform;

                // Windows (most players): UseShellExecute launches Explorer via shell association.
                if (p == RuntimePlatform.WindowsPlayer || p == RuntimePlatform.WindowsEditor)
                {
                    var psi = new ProcessStartInfo(fullPath)
                    {
                        UseShellExecute = true,
                        ErrorDialog = false,
                        Verb = "open",
                    };

                    Process.Start(psi);
                    return;
                }

                // macOS: use `open <path>`
                if (p == RuntimePlatform.OSXPlayer || p == RuntimePlatform.OSXEditor)
                {
                    var psi = new ProcessStartInfo("open", QuoteArg(fullPath))
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };

                    Process.Start(psi);
                    return;
                }

                // Linux: use `xdg-open <path>`
                if (p == RuntimePlatform.LinuxPlayer || p == RuntimePlatform.LinuxEditor)
                {
                    var psi = new ProcessStartInfo("xdg-open", QuoteArg(fullPath))
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };

                    Process.Start(psi);
                    return;
                }

                // Unknown platform: try a generic shell execute as a last attempt.
                var psiGeneric = new ProcessStartInfo(fullPath)
                {
                    UseShellExecute = true,
                    ErrorDialog = false,
                };

                Process.Start(psiGeneric);
            }
            catch (Exception ex)
            {
                Mod.s_Log.Warn($"{Mod.ModTag} ShellOpen: OS fallback failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static string QuoteArg(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "\"\"";
            }

            // Basic quoting for paths with spaces.
            if (s.IndexOf(' ') >= 0 || s.IndexOf('\t') >= 0)
            {
                return "\"" + s.Replace("\"", "\\\"") + "\"";
            }

            return s;
        }
    }
}
