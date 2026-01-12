// File: Utils/ShellOpen.cs
// Purpose: Cross-platform-ish folder opening helper for Options UI buttons.
// Notes:
// - Uses Application.OpenURL with file:// URI.
// - Safe: catches exceptions; no crash on failure.

namespace DispatchBoss
{
    using Colossal.PSI.Environment;
    using System;
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
                    Mod.s_Log.Warn($"{Mod.ModTag} {logLabel}: folder path is empty.");
                    return;
                }

                if (!Directory.Exists(folderPath))
                {
                    Mod.s_Log.Warn($"{Mod.ModTag} {logLabel}: folder not found: {folderPath}");
                    return;
                }

                var uri = new Uri(folderPath);
                Application.OpenURL(uri.AbsoluteUri);
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
    }
}
