// File: Settings/Setting.cs
// Purpose: Options UI + saved settings for Adjust Transit Capacity (Public Transit + Services + About).

namespace AdjustTransitCapacity
{
    using Colossal.IO.AssetDatabase;
    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;
    using Game.Settings;
    using Game.UI;
    using System;
    using System.Diagnostics;
    using System.IO;
    using Unity.Entities;
    using UnityEngine;

    [FileLocation("ModsSettings/AdjustTransitCapacity/AdjustTransitCapacity")]
    [SettingsUITabOrder( PublicTransitTab, IndustryTab, ParksRoadsTab, AboutTab )]
    [SettingsUIGroupOrder(
        DepotGroup, PassengerGroup,                     // Public-Transit tab                           
        DeliveryGroup, CargoStationsGroup,              // Industry tab                                     
        RoadMaintenanceGroup, ParkMaintenanceGroup,     // Parks-Roads tab
        AboutInfoGroup, AboutLinksGroup, DebugGroup, LogGroup   // About tab
    )]
    [SettingsUIShowGroupName(
        DepotGroup, PassengerGroup,                     // Public-Transit tab                                               
        DeliveryGroup, CargoStationsGroup,              // Industry tab
        RoadMaintenanceGroup, ParkMaintenanceGroup,     // Parks-Roads tab
        AboutLinksGroup, DebugGroup, LogGroup           // About tab
    )]
    public sealed class Setting : ModSetting
    {
        // Tab ids (must match Locale ids).
        public const string PublicTransitTab    = "Public-Transit";
        public const string IndustryTab         = "Industry";
        public const string ParksRoadsTab       = "Parks-Roads";
        public const string AboutTab            = "About";

        // Group ids (must match Locale ids).
        public const string DepotGroup = "DepotCapacity";
        public const string PassengerGroup = "PassengerCapacity";

        public const string DeliveryGroup = "DeliveryVehicles";
        public const string CargoStationsGroup = "CargoStations";

        public const string RoadMaintenanceGroup = "RoadMaintenance";
        public const string ParkMaintenanceGroup = "ParkMaintenance";

        public const string AboutInfoGroup = "AboutInfo";
        public const string AboutLinksGroup = "AboutLinks";
        public const string DebugGroup = "Debug";
        public const string LogGroup = "Log";

        // Public-Transit sliders (percent).
        public const float DepotMinPercent = 100f;
        public const float PassengerMinPercent = 10f;
        public const float MaxPercent = 1000f;
        public const float StepPercent = 10f;

        // Generic Services sliders (scalar).
        public const float ServiceMinScalar = 1f;
        public const float ServiceMaxScalar = 10f;
        public const float ServiceStepScalar = 1f;

        // Maintenance-specific sliders.
        // Fleet size: allow reducing traffic by lowering allowed vehicles per depot.
        public const float MaintenanceFleetMinScalar = 0.2f; // 20% of vanilla fleet
        public const float MaintenanceFleetMaxScalar = 10f;
        public const float MaintenanceFleetStepScalar = 0.1f;

        // Rate: make stops visibly shorter on heavily worn networks.
        public const float MaintenanceRateMinScalar = 0.5f; // allow slower for experimentation
        public const float MaintenanceRateMaxScalar = 20f;
        public const float MaintenanceRateStepScalar = 0.5f;

        // Road wear speed (LaneDeteriorationData.m_TimeFactor multiplier).
        // 0.2x = 5x slower wear, 2.0x = 2x faster wear.
        public const float RoadWearMinScalar = 0.2f;
        public const float RoadWearMaxScalar = 2.0f;
        public const float RoadWearStepScalar = 0.1f;

        private const string UrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        private const string UrlDiscord =
            "https://discord.gg/HTav7ARPs2";

        public Setting(IMod mod)
            : base(mod)
        {
            // Existing sentinel: older configs can load 0 for percent sliders.
            if (BusDepotScalar == 0f)
            {
                SetDefaults();
            }

            // Services scalars should never be 0 (older configs won’t have them).
            EnsureServiceDefaults();
        }

        public override void SetDefaults()
        {
            // Public-Transit defaults (percent).
            ResetDepotToVanilla();
            ResetPassengerToVanilla();

            // Services defaults (scalar).
            SemiTruckCargoScalar = 1f;
            DeliveryVanCargoScalar = 1f;
            OilTruckCargoScalar = 1f;                // Label/desc: Raw Materials Trucks (oil, coal, ore, stone)
            MotorbikeDeliveryCargoScalar = 1f;
            CargoStationMaxTrucksScalar = 1f;

            // Road maintenance
            RoadWearScalar = 1f; // vanilla wear speed
            RoadMaintenanceVehicleCapacityScalar = 1f;
            RoadMaintenanceVehicleRateScalar = 1f;
            RoadMaintenanceDepotScalar = 1f;

            // Park maintenance
            ParkMaintenanceVehicleCapacityScalar = 1f;
            ParkMaintenanceVehicleRateScalar = 1f;
            ParkMaintenanceDepotScalar = 1f;

            // Debug.
            EnableDebugLogging = false;
        }

        public override void Apply()
        {
            base.Apply();

            GameManager? gm = GameManager.instance;
            if (gm == null || !gm.gameMode.IsGame())
            {
                return;
            }

            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
            {
                return;
            }

            // Settings changes should re-run the systems once.
            try
            {
                AdjustTransitCapacitySystem transitSystem =
                    world.GetExistingSystemManaged<AdjustTransitCapacitySystem>();
                if (transitSystem != null)
                {
                    transitSystem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Mod.s_Log.Warn($"{Mod.ModTag} Apply: failed enabling AdjustTransitCapacitySystem: {ex.GetType().Name}: {ex.Message}");
            }

            try
            {
                ServiceVehiclesSystem serviceSystem =
                    world.GetExistingSystemManaged<ServiceVehiclesSystem>();
                if (serviceSystem != null)
                {
                    serviceSystem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Mod.s_Log.Warn($"{Mod.ModTag} Apply: failed enabling ServiceVehiclesSystem: {ex.GetType().Name}: {ex.Message}");
            }
        }

        // ----------------------------
        // Public-Transit tab (Tab 1)
        // ----------------------------

        // Depot capacity (percent).

        [SettingsUISlider(min = DepotMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, DepotGroup)]
        public float BusDepotScalar { get; set; }

        [SettingsUISlider(min = DepotMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, DepotGroup)]
        public float TaxiDepotScalar { get; set; }

        [SettingsUISlider(min = DepotMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, DepotGroup)]
        public float TramDepotScalar { get; set; }

        [SettingsUISlider(min = DepotMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, DepotGroup)]
        public float TrainDepotScalar { get; set; }

        [SettingsUISlider(min = DepotMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, DepotGroup)]
        public float SubwayDepotScalar { get; set; }

        [SettingsUIButtonGroup(DepotGroup)]
        [SettingsUIButton]
        [SettingsUISection(PublicTransitTab, DepotGroup)]
        public bool ResetDepotToVanillaButton
        {
            set
            {
                if (!value) return;

                ResetDepotToVanilla();
                ApplyAndSave();
            }
        }

        // Passenger capacity (percent).

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, PassengerGroup)]
        public float BusPassengerScalar { get; set; }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, PassengerGroup)]
        public float TramPassengerScalar { get; set; }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, PassengerGroup)]
        public float TrainPassengerScalar { get; set; }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, PassengerGroup)]
        public float SubwayPassengerScalar { get; set; }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, PassengerGroup)]
        public float ShipPassengerScalar { get; set; }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, PassengerGroup)]
        public float FerryPassengerScalar { get; set; }

        [SettingsUISlider(min = PassengerMinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(PublicTransitTab, PassengerGroup)]
        public float AirplanePassengerScalar { get; set; }

        [SettingsUIButtonGroup(PassengerGroup)]
        [SettingsUIButton]
        [SettingsUISection(PublicTransitTab, PassengerGroup)]
        public bool DoublePassengersButton
        {
            set
            {
                if (!value) return;

                BusPassengerScalar = 200f;
                TramPassengerScalar = 200f;
                TrainPassengerScalar = 200f;
                SubwayPassengerScalar = 200f;
                ShipPassengerScalar = 200f;
                FerryPassengerScalar = 200f;
                AirplanePassengerScalar = 200f;

                ApplyAndSave();
            }
        }

        [SettingsUIButtonGroup(PassengerGroup)]
        [SettingsUIButton]
        [SettingsUISection(PublicTransitTab, PassengerGroup)]
        public bool ResetPassengerToVanillaButton
        {
            set
            {
                if (!value) return;

                ResetPassengerToVanilla();
                ApplyAndSave();
            }
        }

        // ----------------------------
        // Industry (Tab 2)
        // ----------------------------

        // Delivery / cargo (scalar).

        [SettingsUISlider(min = ServiceMinScalar, max = ServiceMaxScalar, step = ServiceStepScalar)]
        [SettingsUISection(IndustryTab, DeliveryGroup)]
        public float SemiTruckCargoScalar { get; set; } = 1f;

        [SettingsUISlider(min = ServiceMinScalar, max = ServiceMaxScalar, step = ServiceStepScalar)]
        [SettingsUISection(IndustryTab, DeliveryGroup)]
        public float DeliveryVanCargoScalar { get; set; } = 1f;

        // Property name retained for backward compatibility.
        [SettingsUISlider(min = ServiceMinScalar, max = ServiceMaxScalar, step = ServiceStepScalar)]
        [SettingsUISection(IndustryTab, DeliveryGroup)]
        public float OilTruckCargoScalar { get; set; } = 1f;

        [SettingsUISlider(min = ServiceMinScalar, max = ServiceMaxScalar, step = ServiceStepScalar)]
        [SettingsUISection(IndustryTab, DeliveryGroup)]
        public float MotorbikeDeliveryCargoScalar { get; set; } = 1f;

        [SettingsUIButtonGroup(DeliveryGroup)]
        [SettingsUIButton]
        [SettingsUISection(IndustryTab, DeliveryGroup)]
        public bool ResetDeliveryToVanillaButton
        {
            set
            {
                if (!value) return;

                SemiTruckCargoScalar = 1f;
                DeliveryVanCargoScalar = 1f;
                OilTruckCargoScalar = 1f;
                MotorbikeDeliveryCargoScalar = 1f;

                ApplyAndSave();
            }
        }

        // Cargo stations (scalar).

        [SettingsUISlider(min = ServiceMinScalar, max = ServiceMaxScalar, step = ServiceStepScalar)]
        [SettingsUISection(IndustryTab, CargoStationsGroup)]
        public float CargoStationMaxTrucksScalar { get; set; } = 1f;

        [SettingsUIButtonGroup(CargoStationsGroup)]
        [SettingsUIButton]
        [SettingsUISection(IndustryTab, CargoStationsGroup)]
        public bool ResetCargoStationsToVanillaButton
        {
            set
            {
                if (!value) return;

                CargoStationMaxTrucksScalar = 1f;
                ApplyAndSave();
            }
        }

        // ----------------------------
        // Parks-Roads tab (Tab 3)
        // ----------------------------

        // Road maintenance (scalars).

        [SettingsUISlider(min = RoadWearMinScalar, max = RoadWearMaxScalar, step = RoadWearStepScalar)]
        [SettingsUISection(ParksRoadsTab, RoadMaintenanceGroup)]
        public float RoadWearScalar { get; set; } = 1f;

        [SettingsUISlider(min = ServiceMinScalar, max = ServiceMaxScalar, step = ServiceStepScalar)]
        [SettingsUISection(ParksRoadsTab, RoadMaintenanceGroup)]
        public float RoadMaintenanceVehicleCapacityScalar { get; set; } = 1f;

        [SettingsUISlider(min = MaintenanceRateMinScalar, max = MaintenanceRateMaxScalar, step = MaintenanceRateStepScalar)]
        [SettingsUISection(ParksRoadsTab, RoadMaintenanceGroup)]
        public float RoadMaintenanceVehicleRateScalar { get; set; } = 1f;

        [SettingsUISlider(min = MaintenanceFleetMinScalar, max = MaintenanceFleetMaxScalar, step = MaintenanceFleetStepScalar)]
        [SettingsUISection(ParksRoadsTab, RoadMaintenanceGroup)]
        public float RoadMaintenanceDepotScalar { get; set; } = 1f;

        [SettingsUIButtonGroup(RoadMaintenanceGroup)]
        [SettingsUIButton]
        [SettingsUISection(ParksRoadsTab, RoadMaintenanceGroup)]
        public bool ResetRoadMaintenanceToVanillaButton
        {
            set
            {
                if (!value) return;

                RoadWearScalar = 1f;
                RoadMaintenanceVehicleCapacityScalar = 1f;
                RoadMaintenanceVehicleRateScalar = 1f;
                RoadMaintenanceDepotScalar = 1f;

                ApplyAndSave();
            }
        }

        // Park maintenance (scalars).

        [SettingsUISlider(min = ServiceMinScalar, max = ServiceMaxScalar, step = ServiceStepScalar)]
        [SettingsUISection(ParksRoadsTab, ParkMaintenanceGroup)]
        public float ParkMaintenanceVehicleCapacityScalar { get; set; } = 1f;

        [SettingsUISlider(min = MaintenanceRateMinScalar, max = MaintenanceRateMaxScalar, step = MaintenanceRateStepScalar)]
        [SettingsUISection(ParksRoadsTab, ParkMaintenanceGroup)]
        public float ParkMaintenanceVehicleRateScalar { get; set; } = 1f;

        [SettingsUISlider(min = MaintenanceFleetMinScalar, max = MaintenanceFleetMaxScalar, step = MaintenanceFleetStepScalar)]
        [SettingsUISection(ParksRoadsTab, ParkMaintenanceGroup)]
        public float ParkMaintenanceDepotScalar { get; set; } = 1f;

        [SettingsUIButtonGroup(ParkMaintenanceGroup)]
        [SettingsUIButton]
        [SettingsUISection(ParksRoadsTab, ParkMaintenanceGroup)]
        public bool ResetParkMaintenanceToVanillaButton
        {
            set
            {
                if (!value) return;

                ParkMaintenanceVehicleCapacityScalar = 1f;
                ParkMaintenanceVehicleRateScalar = 1f;
                ParkMaintenanceDepotScalar = 1f;

                ApplyAndSave();
            }
        }


        // -------------------------
        // About tab (tab 4)
        // -------------------------

        [SettingsUISection(AboutTab, AboutInfoGroup)]
        public string ModNameDisplay => $"{Mod.ModName} {Mod.ModTag}";

        [SettingsUISection(AboutTab, AboutInfoGroup)]
        public string ModVersionDisplay => Mod.ModVersion;

        [SettingsUIButtonGroup(AboutLinksGroup)]
        [SettingsUIButton]
        [SettingsUISection(AboutTab, AboutLinksGroup)]
        public bool OpenParadoxMods
        {
            set
            {
                if (!value) return;

                try
                {
                    Application.OpenURL(UrlParadox);
                }
                catch (Exception ex)
                {
                    Mod.s_Log.Warn($"{Mod.ModTag} OpenParadoxMods failed: {ex.GetType().Name}: {ex.Message}");
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
                if (!value) return;

                try
                {
                    Application.OpenURL(UrlDiscord);
                }
                catch (Exception ex)
                {
                    Mod.s_Log.Warn($"{Mod.ModTag} OpenDiscord failed: {ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        [SettingsUISection(AboutTab, DebugGroup)]
        public bool EnableDebugLogging { get; set; }

        [SettingsUIButtonGroup(LogGroup)]
        [SettingsUIButton]
        [SettingsUISection(AboutTab, LogGroup)]
        public bool OpenLogButton
        {
            set
            {
                if (!value) return;

                try
                {
                    string? logPath = null;

                    if (Mod.s_Log is UnityLogger unityLogger &&
                        !string.IsNullOrEmpty(unityLogger.logPath))
                    {
                        logPath = unityLogger.logPath;
                    }
                    else
                    {
                        string logsDir = Path.Combine(Application.persistentDataPath, "Logs");
                        logPath = Path.Combine(logsDir, "AdjustTransitCapacity.log");
                    }

                    if (!string.IsNullOrEmpty(logPath) && File.Exists(logPath))
                    {
                        OpenWithUnityFileUrl(logPath);
                        return;
                    }

                    string? folder = Path.GetDirectoryName(logPath ?? string.Empty);
                    if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
                    {
                        OpenWithUnityFileUrl(folder, isDirectory: true);
                        return;
                    }

                    Mod.s_Log.Info($"{Mod.ModTag} OpenLogButton: no log file yet, and log folder not found.");
                }
                catch (Exception ex)
                {
                    try
                    {
                        string logsDir = Path.Combine(Application.persistentDataPath, "Logs");
                        string logPath2 = Path.Combine(logsDir, "AdjustTransitCapacity.log");

                        if (File.Exists(logPath2))
                        {
                            var psi = new ProcessStartInfo(logPath2)
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
                    }

                    Mod.s_Log.Warn($"{Mod.ModTag} OpenLogButton failed: {ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        // ------------------------------
        // Helpers
        // ------------------------------

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

        private static void OpenWithUnityFileUrl(string path, bool isDirectory = false)
        {
            string normalized = path.Replace('\\', '/');

            if (isDirectory && !normalized.EndsWith("/", StringComparison.Ordinal))
            {
                normalized += "/";
            }

            string uri = "file:///" + normalized;
            Application.OpenURL(uri);
        }

        private void EnsureServiceDefaults()
        {
            // Older config files can load missing fields as 0; clamp to safe defaults.

            if (SemiTruckCargoScalar <= 0f) SemiTruckCargoScalar = 1f;
            if (DeliveryVanCargoScalar <= 0f) DeliveryVanCargoScalar = 1f;
            if (OilTruckCargoScalar <= 0f) OilTruckCargoScalar = 1f;
            if (MotorbikeDeliveryCargoScalar <= 0f) MotorbikeDeliveryCargoScalar = 1f;
            if (CargoStationMaxTrucksScalar <= 0f) CargoStationMaxTrucksScalar = 1f;

            if (RoadWearScalar <= 0f) RoadWearScalar = 1f;

            if (RoadMaintenanceVehicleCapacityScalar <= 0f) RoadMaintenanceVehicleCapacityScalar = 1f;
            if (RoadMaintenanceVehicleRateScalar <= 0f) RoadMaintenanceVehicleRateScalar = 1f;
            if (RoadMaintenanceDepotScalar <= 0f) RoadMaintenanceDepotScalar = 1f;

            if (ParkMaintenanceVehicleCapacityScalar <= 0f) ParkMaintenanceVehicleCapacityScalar = 1f;
            if (ParkMaintenanceVehicleRateScalar <= 0f) ParkMaintenanceVehicleRateScalar = 1f;
            if (ParkMaintenanceDepotScalar <= 0f) ParkMaintenanceDepotScalar = 1f;
        }
    }
}
