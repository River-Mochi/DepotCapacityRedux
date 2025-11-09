// Settings/Setting.cs
// Options UI + saved settings for Adjust Transit Capacity.

namespace AdjustTransitCapacity
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Colossal.IO.AssetDatabase;
    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;
    using Game.Settings;
    using Game.UI;
    using Unity.Entities;
    using UnityEngine;

    /// <summary>
    /// ATC options: depot/passenger percent sliders, about info, links, and debug toggle.
    /// </summary>
    [FileLocation("ModsSettings/AdjustTransitCapacity/AdjustTransitCapacity")]
    [SettingsUITabOrder(
        ActionsTab,
        AboutTab
    )]
    [SettingsUIGroupOrder(
        DepotGroup,
        PassengerGroup,
        AboutInfoGroup,
        AboutLinksGroup,
        DebugGroup,
        LogGroup
    )]
    [SettingsUIShowGroupName(
        DepotGroup,
        PassengerGroup,
        AboutLinksGroup,
        DebugGroup,
        LogGroup
    )]
    public sealed class Setting : ModSetting
    {
        // Tabs
        public const string ActionsTab = "Actions";
        public const string AboutTab = "About";

        // Groups (Actions tab)
        public const string DepotGroup = "DepotCapacity";
        public const string PassengerGroup = "PassengerCapacity";

        // Groups (About tab)
        public const string AboutInfoGroup = "AboutInfo";
        public const string AboutLinksGroup = "AboutLinks";
        public const string DebugGroup = "Debug";
        public const string LogGroup = "Log";

        // Slider range in percent: 100–1000 (100% = vanilla 1.0x, 1000% = 10.0x)
        public const float MinPercent = 100f;
        public const float MaxPercent = 1000f;
        public const float StepPercent = 20f;

        // External links
        private const string UrlParadox =
            "https://mods.paradoxplaza.com/uploaded?orderBy=desc&sortBy=best&time=alltime";

        private const string UrlDiscord =
            "https://discord.gg/HTav7ARPs2";

        public Setting(IMod mod)
            : base(mod)
        {
            // Brand-new settings file -> populate defaults.
            if (BusDepotScalar == 0f)
            {
                SetDefaults();
            }
        }

        // ----------------
        // Defaults / Apply
        // ----------------

        public override void SetDefaults()
        {
            ResetDepotToVanilla();
            ResetPassengerToVanilla();

            // Verbose logging off by default.
            EnableDebugLogging = false;
        }

        public override void Apply()
        {
            base.Apply();

            // Always save slider values, but only try to apply them
            // when actual gameplay is running (a city is loaded).
            GameManager? gm = GameManager.instance;
            if (gm == null || !gm.gameMode.IsGame())
            {
                // Main menu / editor: settings are saved, first use will be
                // when a GameMode.Game city finishes loading.
                return;
            }

            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
            {
                return;
            }

            AdjustTransitCapacitySystem system =
                world.GetExistingSystemManaged<AdjustTransitCapacitySystem>();
            if (system != null)
            {
                // Run one more tick in the CURRENT city to reapply new slider values.
                system.Enabled = true;
            }
        }

        // ------------------------
        // Actions tab: depot (max)
        // ------------------------

        // Stored as percent: 100–1000. Runtime scalar = value / 100f.

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public float BusDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public float TaxiDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public float TramDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public float TrainDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public float SubwayDepotScalar
        {
            get; set;
        }

        [SettingsUIButtonGroup(DepotGroup)]
        [SettingsUIButton]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public bool ResetDepotToVanillaButton
        {
            set
            {
                if (!value)
                {
                    return;
                }

                ResetDepotToVanilla();
                Apply();
            }
        }

        // -----------------------------
        // Actions tab: passengers (max)
        // -----------------------------

        // Taxi passenger capacity is not changed (CS2 keeps 4 seats).
        // Stored as percent: 100–1000. Runtime scalar = value / 100f.

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float BusPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float TramPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float TrainPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float SubwayPassengerScalar
        {
            get; set;
        }

        // Passenger-only types (not depots).

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float ShipPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float FerryPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float AirplanePassengerScalar
        {
            get; set;
        }

        [SettingsUIButtonGroup(PassengerGroup)]
        [SettingsUIButton]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public bool ResetPassengerToVanillaButton
        {
            set
            {
                if (!value)
                {
                    return;
                }

                ResetPassengerToVanilla();
                Apply();
            }
        }

        // --------------------
        // About tab: info/link
        // --------------------

        [SettingsUISection(AboutTab, AboutInfoGroup)]
        public string ModNameDisplay => $"{Mod.ModName} {Mod.ModTag}";

        [SettingsUISection(AboutTab, AboutInfoGroup)]
        public string ModVersionDisplay => Mod.ModVersion;

        // About tab: links

        [SettingsUIButtonGroup(AboutLinksGroup)]
        [SettingsUIButton]
        [SettingsUISection(AboutTab, AboutLinksGroup)]
        public bool OpenParadoxMods
        {
            set
            {
                if (!value)
                {
                    return;
                }

                try
                {
                    Application.OpenURL(UrlParadox);
                }
                catch (Exception)
                {
                    // ignore
                }
            }
        }

        [SettingsUIButtonGroup(AboutLinksGroup)]
        [SettingsUIButton]
        [SettingsUISection(AboutTab, AboutLinksGroup)]
        public bool OpenDiscord
        {
            set
            {
                if (!value)
                {
                    return;
                }

                try
                {
                    Application.OpenURL(UrlDiscord);
                }
                catch (Exception)
                {
                    // ignore
                }
            }
        }

        // About tab: debug toggle

        [SettingsUISection(AboutTab, DebugGroup)]
        public bool EnableDebugLogging
        {
            get; set;
        }

        // About tab: Open Log Button
        // Opens the log file if it exists, or the log folder if not.

        [SettingsUIButtonGroup(LogGroup)]
        [SettingsUIButton]
        [SettingsUISection(AboutTab, LogGroup)]
        public bool OpenLogButton
        {
            set
            {
                if (!value)
                {
                    return;
                }

                try
                {
                    // 1. Prefer actual logPath from the logger if we can get it.
                    string? logPath = null;

                    if (Mod.Log is UnityLogger unityLogger &&
                        !string.IsNullOrEmpty(unityLogger.logPath))
                    {
                        logPath = unityLogger.logPath;
                    }
                    else
                    {
                        // Fallback orig method path
                        string logsDir = Path.Combine(Application.persistentDataPath, "Logs");
                        logPath = Path.Combine(logsDir, "AdjustTransitCapacity.log");
                    }

                    // 2. Try to open the log file.
                    if (!string.IsNullOrEmpty(logPath) && File.Exists(logPath))
                    {
                        OpenWithUnityFileUrl(logPath);
                        return;
                    }

                    // 3. If no file, try to open the folder.
                    string? folder = Path.GetDirectoryName(logPath ?? string.Empty);

                    if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
                    {
                        OpenWithUnityFileUrl(folder, isDirectory: true);
                        return;
                    }

                    Mod.Log.Info($"{Mod.ModTag} OpenLogButton: no log file yet, and log folder not found.");
                }
                catch (Exception ex)
                {
                    // Unity failed for some reason – last resort: Windows shell.
                    try
                    {
                        string logsDir = Path.Combine(Application.persistentDataPath, "Logs");
                        string logPath = Path.Combine(logsDir, "AdjustTransitCapacity.log");

                        if (File.Exists(logPath))
                        {
                            var psi = new ProcessStartInfo(logPath)
                            {
                                UseShellExecute = true,
                                ErrorDialog = false,
                                Verb = "open"
                            };

                            Process.Start(psi);
                        }
                        else if (Directory.Exists(logsDir))
                        {
                            var psi2 = new ProcessStartInfo(logsDir)
                            {
                                UseShellExecute = true,
                                ErrorDialog = false,
                                Verb = "open"
                            };

                            Process.Start(psi2);
                        }
                    }
                    catch
                    {
                        // Don't crash the options UI, just log the problem.
                    }

                    Mod.Log.Warn($"{Mod.ModTag} OpenLogButton failed: {ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        // ----------------
        // Helpers: logging
        // ----------------

        // Helper: open a file or folder via Unity, using a file:/// URI.
        private static void OpenWithUnityFileUrl(string path, bool isDirectory = false)
        {
            // Normalize to forward slashes for URI.
            string normalized = path.Replace('\\', '/');

            // Some platforms like a trailing slash for directories.
            if (isDirectory && !normalized.EndsWith("/", StringComparison.Ordinal))
            {
                normalized += "/";
            }

            string uri = "file:///" + normalized;
            Application.OpenURL(uri);
        }

        // -------------------------
        // Helpers: slider defaults
        // -------------------------

        public void ResetDepotToVanilla()
        {
            BusDepotScalar = 100f;
            TaxiDepotScalar = 100f;
            TramDepotScalar = 100f;
            TrainDepotScalar = 100f;
            SubwayDepotScalar = 100f;
        }

        public void ResetPassengerToVanilla()
        {
            BusPassengerScalar = 100f;
            TramPassengerScalar = 100f;
            TrainPassengerScalar = 100f;
            SubwayPassengerScalar = 100f;
            ShipPassengerScalar = 100f;
            FerryPassengerScalar = 100f;
            AirplanePassengerScalar = 100f;
        }
    }
}
