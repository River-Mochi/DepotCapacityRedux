// File: Settings/Setting.cs
// Purpose: Options UI + saved settings for Dispatch Boss (Public Transit + Industry + Parks/Roads + About).

namespace DispatchBoss
{
    using Colossal.IO.AssetDatabase; // FileLocation
    using Game;
    using Game.Modding;
    using Game.SceneFlow;
    using Game.Settings;
    using Game.UI;
    using System;
    using Unity.Entities;
    using UnityEngine;

    [FileLocation("ModsSettings/DispatchBoss/DispatchBoss")]
    [SettingsUITabOrder(PublicTransitTab, IndustryTab, ParksRoadsTab, AboutTab)]
    [SettingsUIGroupOrder(
        LineVehiclesGroup, DepotGroup, PassengerGroup,
        DeliveryGroup, CargoStationsGroup,
        RoadMaintenanceGroup, ParkMaintenanceGroup,
        AboutInfoGroup, AboutLinksGroup, DebugGroup
    )]
    [SettingsUIShowGroupName(
        LineVehiclesGroup, DepotGroup, PassengerGroup,
        DeliveryGroup, CargoStationsGroup,
        RoadMaintenanceGroup, ParkMaintenanceGroup,
        AboutLinksGroup, DebugGroup
    )]
    public sealed class Setting : ModSetting
    {
        // Tab ids (must match Locale ids).
        public const string PublicTransitTab = "Public-Transit";
        public const string IndustryTab = "Industry";
        public const string ParksRoadsTab = "Parks-Roads";
        public const string AboutTab = "About";

        // Group ids (must match Locale ids).
        public const string LineVehiclesGroup = "LineVehicles";
        public const string DepotGroup = "DepotCapacity";
        public const string PassengerGroup = "PassengerCapacity";

        public const string DeliveryGroup = "DeliveryVehicles";
        public const string CargoStationsGroup = "CargoStations";

        public const string RoadMaintenanceGroup = "RoadMaintenance";
        public const string ParkMaintenanceGroup = "ParkMaintenance";

        public const string AboutInfoGroup = "AboutInfo";
        public const string AboutLinksGroup = "AboutLinks";
        public const string DebugGroup = "Debug";

        // ----------------------------
        // Slider ranges
        // ----------------------------

        // Public-Transit sliders (percent).
        public const float DepotMinPercent = 100f;
        public const float PassengerMinPercent = 10f;
        public const float MaxPercent = 1000f;
        public const float StepPercent = 10f;

        // Industry sliders (scalar 1x..10x).
        public const float ServiceMinScalar = 1f;
        public const float ServiceMaxScalar = 10f;
        public const float ServiceStepScalar = 1f;

        // Cargo station / extractors (scalar 1x..5x).
        public const float CargoStationMinScalar = 1f;
        public const float CargoStationMaxScalar = 5f;
        public const float CargoStationStepScalar = 1f;

        // Parks-Roads: store/display as percent (100%..1000% = 1x..10x).
        public const float MaintenanceMinPercent = 100f;
        public const float MaintenanceMaxPercent = 1000f;
        public const float MaintenanceStepPercent = 10f;

        // Road wear speed: percent (10%..400% = 0.1x..4.0x).
        public const float RoadWearMinPercent = 10f;
        public const float RoadWearMaxPercent = 400f;
        public const float RoadWearStepPercent = 10f;

        private const string UrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        private const string UrlDiscord =
            "https://discord.gg/HTav7ARPs2";

        // Toggle vanilla transit line vehicle count range tuner (global policy).
        [SettingsUISection(PublicTransitTab, LineVehiclesGroup)]
        public bool EnableLineVehicleCountTuner { get; set; } = false;

        public Setting(IMod mod)
            : base(mod)
        {
            // Existing sentinel: older configs can load 0 for percent sliders.
            if (BusDepotScalar == 0f)
            {
                SetDefaults();
            }

            EnsureServiceDefaults();
        }

        public override void SetDefaults()
        {
            // Public-Transit defaults (percent).
            EnableLineVehicleCountTuner = false;
            ResetDepotToVanilla();
            ResetPassengerToVanilla();

            // Industry defaults (scalar).
            SemiTruckCargoScalar = 1f;
            DeliveryVanCargoScalar = 1f;
            OilTruckCargoScalar = 1f;
            MotorbikeDeliveryCargoScalar = 1f;

            CargoStationMaxTrucksScalar = 1f;
            ExtractorMaxTrucksScalar = 1f;

            // Parks-Roads defaults (percent).
            RoadWearScalar = 100f;

            RoadMaintenanceVehicleCapacityScalar = 100f;
            RoadMaintenanceVehicleRateScalar = 100f;
            RoadMaintenanceDepotScalar = 100f;

            ParkMaintenanceVehicleCapacityScalar = 100f;
            ParkMaintenanceVehicleRateScalar = 100f;
            ParkMaintenanceDepotScalar = 100f;

            // Debug.
            EnableDebugLogging = false;
        }

        public override void Apply()
        {
            base.Apply();

            GameManager gm = GameManager.instance;
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
            TryEnableOnce<TransitSystem>(world, "TransitSystem");
            TryEnableOnce<MaintenanceSystem>(world, "MaintenanceSystem");
            TryEnableOnce<IndustrySystem>(world, "IndustrySystem");
            TryEnableOnce<LaneWearSystem>(world, "LaneWearSystem");
            TryEnableOnce<VehicleCountPolicyTunerSystem>(world, "VehicleCountPolicyTunerSystem");
        }

        private static void TryEnableOnce<T>(World world, string label) where T : GameSystemBase
        {
            try
            {
                T sys = world.GetExistingSystemManaged<T>();
                if (sys != null)
                {
                    sys.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Mod.s_Log.Warn($"{Mod.ModTag} Apply: failed enabling {label}: {ex.GetType().Name}: {ex.Message}");
            }
        }

        // ----------------------------
        // Public-Transit tab
        // ----------------------------

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
        // Industry
        // ----------------------------

        [SettingsUISlider(min = ServiceMinScalar, max = ServiceMaxScalar, step = ServiceStepScalar)]
        [SettingsUISection(IndustryTab, DeliveryGroup)]
        public float SemiTruckCargoScalar { get; set; } = 1f;

        [SettingsUISlider(min = ServiceMinScalar, max = ServiceMaxScalar, step = ServiceStepScalar)]
        [SettingsUISection(IndustryTab, DeliveryGroup)]
        public float DeliveryVanCargoScalar { get; set; } = 1f;

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

        [SettingsUISlider(min = CargoStationMinScalar, max = CargoStationMaxScalar, step = CargoStationStepScalar)]
        [SettingsUISection(IndustryTab, CargoStationsGroup)]
        public float CargoStationMaxTrucksScalar { get; set; } = 1f;

        [SettingsUISlider(min = CargoStationMinScalar, max = CargoStationMaxScalar, step = CargoStationStepScalar)]
        [SettingsUISection(IndustryTab, CargoStationsGroup)]
        public float ExtractorMaxTrucksScalar { get; set; } = 1f;

        [SettingsUIButtonGroup(CargoStationsGroup)]
        [SettingsUIButton]
        [SettingsUISection(IndustryTab, CargoStationsGroup)]
        public bool ResetCargoStationsToVanillaButton
        {
            set
            {
                if (!value) return;

                CargoStationMaxTrucksScalar = 1f;
                ExtractorMaxTrucksScalar = 1f;

                ApplyAndSave();
            }
        }

        // ----------------------------------
        // Parks-Roads (percent)
        // ---------------------------------

        [SettingsUISlider(min = MaintenanceMinPercent, max = MaintenanceMaxPercent, step = MaintenanceStepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ParksRoadsTab, ParkMaintenanceGroup)]
        public float ParkMaintenanceDepotScalar { get; set; } = 100f;

        [SettingsUISlider(min = MaintenanceMinPercent, max = MaintenanceMaxPercent, step = MaintenanceStepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ParksRoadsTab, ParkMaintenanceGroup)]
        public float ParkMaintenanceVehicleCapacityScalar { get; set; } = 100f;

        [SettingsUISlider(min = MaintenanceMinPercent, max = MaintenanceMaxPercent, step = MaintenanceStepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ParksRoadsTab, ParkMaintenanceGroup)]
        public float ParkMaintenanceVehicleRateScalar { get; set; } = 100f;

        [SettingsUIButtonGroup(ParkMaintenanceGroup)]
        [SettingsUIButton]
        [SettingsUISection(ParksRoadsTab, ParkMaintenanceGroup)]
        public bool ResetParkMaintenanceToVanillaButton
        {
            set
            {
                if (!value) return;

                ParkMaintenanceDepotScalar = 100f;
                ParkMaintenanceVehicleCapacityScalar = 100f;
                ParkMaintenanceVehicleRateScalar = 100f;

                ApplyAndSave();
            }
        }

        [SettingsUISlider(min = MaintenanceMinPercent, max = MaintenanceMaxPercent, step = MaintenanceStepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ParksRoadsTab, RoadMaintenanceGroup)]
        public float RoadMaintenanceDepotScalar { get; set; } = 100f;

        [SettingsUISlider(min = MaintenanceMinPercent, max = MaintenanceMaxPercent, step = MaintenanceStepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ParksRoadsTab, RoadMaintenanceGroup)]
        public float RoadMaintenanceVehicleCapacityScalar { get; set; } = 100f;

        [SettingsUISlider(min = MaintenanceMinPercent, max = MaintenanceMaxPercent, step = MaintenanceStepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ParksRoadsTab, RoadMaintenanceGroup)]
        public float RoadMaintenanceVehicleRateScalar { get; set; } = 100f;

        [SettingsUISlider(min = RoadWearMinPercent, max = RoadWearMaxPercent, step = RoadWearStepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(ParksRoadsTab, RoadMaintenanceGroup)]
        public float RoadWearScalar { get; set; } = 100f;

        [SettingsUIButtonGroup(RoadMaintenanceGroup)]
        [SettingsUIButton]
        [SettingsUISection(ParksRoadsTab, RoadMaintenanceGroup)]
        public bool ResetRoadMaintenanceToVanillaButton
        {
            set
            {
                if (!value) return;

                RoadMaintenanceDepotScalar = 100f;
                RoadMaintenanceVehicleCapacityScalar = 100f;
                RoadMaintenanceVehicleRateScalar = 100f;
                RoadWearScalar = 100f;

                ApplyAndSave();
            }
        }

        // -------------------------
        // About
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
                    Mod.s_Log.Info($"{Mod.ModTag} OpenParadoxMods failed: {ex.GetType().Name}: {ex.Message}");
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
                    Mod.s_Log.Info($"{Mod.ModTag} OpenDiscord failed: {ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        // DEBUG/LOGGING

        [SettingsUIButtonGroup(DebugGroup)]
        [SettingsUIButton]
        [SettingsUISection(AboutTab, DebugGroup)]
        public bool RunPrefabScanButton
        {
            set
            {
                if (!value) return;

                GameManager gm = GameManager.instance;
                if (gm == null || !gm.gameMode.IsGame())
                {
                    PrefabScanState.MarkFailed(PrefabScanState.FailCode.NoCityLoaded, null);
                    return;
                }

                if (!PrefabScanState.RequestScan())
                {
                    Mod.s_Log.Info($"{Mod.ModTag} Prefab scan already queued/running.");
                    return;
                }

                try
                {
                    World world = World.DefaultGameObjectInjectionWorld;
                    if (world != null)
                    {
                        PrefabScanSystem scan = world.GetOrCreateSystemManaged<PrefabScanSystem>();
                        scan.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    PrefabScanState.MarkFailed(PrefabScanState.FailCode.Exception, $"{ex.GetType().Name}: {ex.Message}");
                    Mod.s_Log.Warn($"{Mod.ModTag} RunPrefabScanButton failed: {ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        [SettingsUIButtonGroup(DebugGroup)]
        [SettingsUISection(AboutTab, DebugGroup)]
        public string PrefabScanStatus => PrefabScanStatusText.Format(PrefabScanState.GetSnapshot());

        [SettingsUIButtonGroup(DebugGroup)]
        [SettingsUIButton]
        [SettingsUISection(AboutTab, DebugGroup)]
        public bool OpenReportButton
        {
            set => ShellOpen.OpenFolderSafe(ShellOpen.GetModsDataFolder(), "OpenReport");
        }

        [SettingsUISection(AboutTab, DebugGroup)]
        public bool EnableDebugLogging { get; set; }

        [SettingsUIButton]
        [SettingsUISection(AboutTab, DebugGroup)]
        public bool OpenLogButton
        {
            set => ShellOpen.OpenFolderSafe(ShellOpen.GetLogsFolder(), "OpenLog");
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

        private void EnsureServiceDefaults()
        {
            // Industry scalars: older config files can load missing fields as 0; clamp to safe defaults.
            if (SemiTruckCargoScalar <= 0f) SemiTruckCargoScalar = 1f;
            if (DeliveryVanCargoScalar <= 0f) DeliveryVanCargoScalar = 1f;
            if (OilTruckCargoScalar <= 0f) OilTruckCargoScalar = 1f;
            if (MotorbikeDeliveryCargoScalar <= 0f) MotorbikeDeliveryCargoScalar = 1f;

            if (CargoStationMaxTrucksScalar <= 0f) CargoStationMaxTrucksScalar = 1f;
            if (ExtractorMaxTrucksScalar <= 0f) ExtractorMaxTrucksScalar = 1f;

            // Old builds used scalars (1..10) and wear (0.2..2.0). New builds store percent.
            float NormalizeMaintenancePercent(float v)
            {
                if (v <= 0f) return 100f;
                if (v > 0f && v <= 10.0001f) v *= 100f; // old scalar -> percent
                if (v < MaintenanceMinPercent) v = MaintenanceMinPercent;
                if (v > MaintenanceMaxPercent) v = MaintenanceMaxPercent;
                return v;
            }

            float NormalizeWearPercent(float v)
            {
                if (v <= 0f) return 100f;
                if (v > 0f && v <= 2.0001f) v *= 100f; // old wear scalar -> percent
                if (v < RoadWearMinPercent) v = RoadWearMinPercent;
                if (v > RoadWearMaxPercent) v = RoadWearMaxPercent;
                return v;
            }

            RoadMaintenanceDepotScalar = NormalizeMaintenancePercent(RoadMaintenanceDepotScalar);
            RoadMaintenanceVehicleCapacityScalar = NormalizeMaintenancePercent(RoadMaintenanceVehicleCapacityScalar);
            RoadMaintenanceVehicleRateScalar = NormalizeMaintenancePercent(RoadMaintenanceVehicleRateScalar);

            ParkMaintenanceDepotScalar = NormalizeMaintenancePercent(ParkMaintenanceDepotScalar);
            ParkMaintenanceVehicleCapacityScalar = NormalizeMaintenancePercent(ParkMaintenanceVehicleCapacityScalar);
            ParkMaintenanceVehicleRateScalar = NormalizeMaintenancePercent(ParkMaintenanceVehicleRateScalar);

            RoadWearScalar = NormalizeWearPercent(RoadWearScalar);
        }
    }
}
