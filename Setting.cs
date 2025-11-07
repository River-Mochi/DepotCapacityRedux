// Setting.cs
// Purpose: options UI + saved settings for Adjust Transit Capacity.

namespace AdjustTransitCapacity
{
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using Unity.Entities;

    // Keep custom location folder under ModsSettings
    [FileLocation("ModsSettings/AdjustTransitCapacity/AdjustTransitCapacity")]
    [SettingsUIGroupOrder(DepotGroup, PassengerGroup)]
    [SettingsUIShowGroupName(DepotGroup, PassengerGroup)]
    public sealed class Setting : ModSetting
    {
        // ---- TABS & GROUPS ----
        public const string MainTab = "Main";
        public const string DepotGroup = "DepotCapacity";
        public const string PassengerGroup = "PassengerCapacity";

        // ---- SHARED SLIDER RANGE ----
        // 100% = 1x vanilla, 1000% = 10x
        public const int MinPercent = 100;
        public const int MaxPercent = 1000;
        public const int StepPercent = 25;

        // ---- CTOR ----
        public Setting(IMod mod)
            : base(mod)
        {
            // Brand-new settings file → populate defaults
            if (BusDepotPercent == 0)
            {
                SetDefaults();
            }
        }

        // ---- DEFAULT VALUES ----
        public override void SetDefaults()
        {
            // Depots (5 original types)
            BusDepotPercent = 100;
            TaxiDepotPercent = 100;
            TramDepotPercent = 100;
            TrainDepotPercent = 100;
            SubwayDepotPercent = 100;

            // Passengers (taxis stay vanilla 4 seats in game)
            BusPassengerPercent = 100;
            TramPassengerPercent = 100;
            TrainPassengerPercent = 100;
            SubwayPassengerPercent = 100;
            ShipPassengerPercent = 100;
            FerryPassengerPercent = 100;
            AirplanePassengerPercent = 100;
        }

        // ---- APPLY CALLBACK ----
        public override void Apply()
        {
            base.Apply();

            // Tell the system to reapply multipliers once
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
            {
                return;
            }

            AdjustTransitCapacitySystem system =
                world.GetExistingSystemManaged<AdjustTransitCapacitySystem>();
            if (system != null)
            {
                system.Enabled = true;
            }
        }

        // ---- DEPOT CAPACITY (max vehicles per depot building) ----
        // 100% = vanilla, 1000% = 10x

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int BusDepotPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int TaxiDepotPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int TramDepotPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int TrainDepotPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int SubwayDepotPercent
        {
            get; set;
        }

        // ---- PASSENGER CAPACITY (max passengers per vehicle) ----
        // Taxi passenger capacity is not changed (CS2 keeps 4 seats).

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int BusPassengerPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int TramPassengerPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int TrainPassengerPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int SubwayPassengerPercent
        {
            get; set;
        }

        // Passenger-only types (not depots)
        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int ShipPassengerPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int FerryPassengerPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int AirplanePassengerPercent
        {
            get; set;
        }
    }
}
