// Settings/Setting.cs
// Options UI + saved settings

namespace AdjustTransitCapacity
{

        using System;                               // Exception
        using System.Diagnostics;                   // Process, ProcessStartInfo
        using System.IO;                            // Path, File, Directory
        using Colossal.IO.AssetDatabase;            // FileLocation, LoadSettings
        using Colossal.Logging;                     // UnityLogger
        using Game;                                 // GameManager, GameMode
        using Game.Modding;                         // ModSetting
        using Game.SceneFlow;                       // GameMode
        using Game.Settings;                        // SettingsUI attributes, ModSetting APIs
        using Game.UI;                              // Unit enum (kPercentage)
        using Unity.Entities;                       // World, ECS system lookup
        using UnityEngine;                          // Application.OpenURL, persistentDataPath


        /// <summary>
        /// ATC options: depot/passenger percent sliders, about info, links, and debug toggle.
        /// </summary>
        [FileLocation("ModsSettings/AdjustTransitCapacity/AdjustTransitCapacity")]  // Settings file location.
    [SettingsUITabOrder(
        ActionsTab,
        AboutTab
    )]
    [SettingsUIGroupOrder(
        DepotGroup, PassengerGroup,
        AboutInfoGroup, AboutLinksGroup,
        DebugGroup, LogGroup
    )]
    [SettingsUIShowGroupName(
        DepotGroup, PassengerGroup,
        AboutLinksGroup, DebugGroup, LogGroup
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

        // Slider ranges in percent:
        // Depots    : 100–1000 (100% = vanilla 1.0x, 1000% = 10.0x)
        // Passengers: 10–1000  (10% = 0.1x,    1000% = 10.0x)
        public const float DepotMinPercent = 100f;
        public const float PassengerMinPercent = 10f;
        public const float MaxPercent = 1000f;
        public const float StepPercent = 10f;

        // External links
        private const string UrlParadox =
            "https://mods.paradoxplaza.com/authors/kimosabe1/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        private const string UrlDiscord =
            "https://discord.gg/HTav7ARPs2";

        public Setting(IMod mod)
            : base(mod)
        {
            // For brand-new settings file -> populate defaults.
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
            // when actual gameplay is running (city is loaded).
            GameManager? gm = GameManager.instance;
            if (gm == null || !gm.gameMode.IsGame())
            {
                // Main menu - settings are saved.
                // First use when city finishes loading.
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
                // Run one more tick in the current city to reapply new slider values.
                system.Enabled = true;
            }
        }

        // ------------------------
        // Actions tab: depot (max)
        // ------------------------

        // Stored as percent: 100–1000. Runtime scalar = value / 100f.

        [SettingsUISlider(min = DepotMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public float BusDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = DepotMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public float TaxiDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = DepotMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public float TramDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = DepotMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public float TrainDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = DepotMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, DepotGroup)]
        public float SubwayDepotScalar
        {
            get; set;
        }

        // Convenience button to reset all depots.
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

        // Stored as percent: 10–1000. Runtime scalar = value / 100f.
        // Taxi passengers is not changed (CS2 keeps 4 seats, more complex dispatch system).

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float BusPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float TramPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float TrainPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float SubwayPassengerScalar
        {
            get; set;
        }

        // Passenger-only types (not cargo).

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float ShipPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float FerryPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent,
            scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public float AirplanePassengerScalar
        {
            get; set;
        }

        // Convenience button: set all passenger sliders to 200% (2.0x).
        [SettingsUIButtonGroup(PassengerGroup)]
        [SettingsUIButton]
        [SettingsUISection(ActionsTab, PassengerGroup)]
        public bool DoublePassengersButton
        {
            set
            {
                if (!value)
                {
                    return;
                }

                BusPassengerScalar = 200f;
                TramPassengerScalar = 200f;
                TrainPassengerScalar = 200f;
                SubwayPassengerScalar = 200f;
                ShipPassengerScalar = 200f;
                FerryPassengerScalar = 200f;
                AirplanePassengerScalar = 200f;

                Apply();
            }
        }

        // Convenience button: set all passenger to vanilla defaults (100%).
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

        // ABOUT TAB: Open Log Button
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
                    // 1. Prefer logPath from the logger if available.
                    string? logPath = null;

                    if (Mod.s_Log is UnityLogger unityLogger &&
                        !string.IsNullOrEmpty(unityLogger.logPath))
                    {
                        logPath = unityLogger.logPath;
                    }
                    else
                    {
                        // Fallback orig method path
                        var logsDir = Path.Combine(Application.persistentDataPath, "Logs");
                        logPath = Path.Combine(logsDir, "AdjustTransitCapacity.log");
                    }

                    // 2. Try to open the log file with Unity (works cross-platform)
                    if (!string.IsNullOrEmpty(logPath) && File.Exists(logPath))
                    {
                        OpenWithUnityFileUrl(logPath);
                        return;
                    }

                    // 3. If no file, try the folder.
                    var folder = Path.GetDirectoryName(logPath ?? string.Empty);

                    if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
                    {
                        OpenWithUnityFileUrl(folder, isDirectory: true);
                        return;
                    }

                    Mod.s_Log.Info($"{Mod.ModTag} OpenLogButton: no log file yet, and log folder not found.");
                }
                catch (Exception ex)
                {
                    // Unity fails for any reason, then use Windows shell (always works).
                    try
                    {
                        var logsDir = Path.Combine(Application.persistentDataPath, "Logs");
                        var logPath = Path.Combine(logsDir, "AdjustTransitCapacity.log");

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
                        // Do not crash options UI, just log problem.
                    }

                    Mod.s_Log.Warn($"{Mod.ModTag} OpenLogButton failed: {ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        // ----------------
        // HELPERS: logging
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
