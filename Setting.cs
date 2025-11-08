// Setting.cs
// Purpose: options UI + saved settings for Adjust Transit Capacity.

namespace AdjustTransitCapacity
{
    using System;
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using Unity.Entities;
    using UnityEngine;

    // Keep custom location folder under ModsSettings
    [FileLocation("ModsSettings/AdjustTransitCapacity/AdjustTransitCapacity")]
    [SettingsUIGroupOrder(
        DepotGroup,
        PassengerGroup,
        AboutInfoGroup,
        AboutLinksGroup,
        DebugGroup
    )]
    [SettingsUIShowGroupName(
        DepotGroup,
        PassengerGroup,
        AboutLinksGroup,
        DebugGroup
    )]
    public sealed class Setting : ModSetting
    {
        // ---- TABS ----
        public const string MainTab = "Main";
        public const string AboutTab = "About";

        // ---- GROUPS (Main tab) ----
        public const string DepotGroup = "DepotCapacity";
        public const string PassengerGroup = "PassengerCapacity";

        // ---- GROUPS (About tab) ----
        public const string AboutInfoGroup = "AboutInfo";
        public const string AboutLinksGroup = "AboutLinks";
        public const string DebugGroup = "Debug";

        // ---- SHARED SLIDER RANGE (1x–10x) ----
        // All sliders represent a direct multiplier: 1.0 = vanilla, 10.0 = 10x.
        public const float MinScalar = 1f;
        public const float MaxScalar = 10f;
        public const float StepScalar = 0.1f;

        // ---- External links ----
        private const string UrlParadox =
            "https://mods.paradoxplaza.com/uploaded?orderBy=desc&sortBy=best&time=alltime";

        private const string UrlDiscord =
            "https://discord.gg/HTav7ARPs2";

        // ---- CTOR ----
        public Setting(IMod mod)
            : base(mod)
        {
            // Brand-new settings file → populate defaults
            if (BusDepotScalar == 0f)
            {
                SetDefaults();
            }
        }

        // ---- DEFAULT VALUES ----
        public override void SetDefaults()
        {
            // Depots (5 original types) — 1x vanilla
            ResetDepotToVanilla();

            // Passengers (taxis stay vanilla 4 seats in game)
            ResetPassengerToVanilla();

            // Debug off by default
            EnableDebugLogging = false;
        }

        // ---- Helper: reset depot sliders to vanilla (1.0x) ----
        public void ResetDepotToVanilla()
        {
            BusDepotScalar = 1f;
            TaxiDepotScalar = 1f;
            TramDepotScalar = 1f;
            TrainDepotScalar = 1f;
            SubwayDepotScalar = 1f;
        }

        // ---- Helper: reset passenger sliders to vanilla (1.0x) ----
        public void ResetPassengerToVanilla()
        {
            BusPassengerScalar = 1f;
            TramPassengerScalar = 1f;
            TrainPassengerScalar = 1f;
            SubwayPassengerScalar = 1f;
            ShipPassengerScalar = 1f;
            FerryPassengerScalar = 1f;
            AirplanePassengerScalar = 1f;
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

            var system = world.GetExistingSystemManaged<AdjustTransitCapacitySystem>();
            if (system != null)
            {
                system.Enabled = true;
            }
        }

        // --------------------------------------------------------------------
        // MAIN TAB: DEPOT CAPACITY (max vehicles per depot building)
        // 1.0 = vanilla, 10.0 = 10x
        // --------------------------------------------------------------------

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, DepotGroup)]
        public float BusDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, DepotGroup)]
        public float TaxiDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, DepotGroup)]
        public float TramDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, DepotGroup)]
        public float TrainDepotScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, DepotGroup)]
        public float SubwayDepotScalar
        {
            get; set;
        }

        // ---- Depot: Reset to vanilla button ----
        [SettingsUIButtonGroup(DepotGroup)]
        [SettingsUIButton]
        [SettingsUISection(MainTab, DepotGroup)]
        public bool ResetDepotToVanillaButton
        {
            set
            {
                if (!value)
                    return;

                ResetDepotToVanilla();
                Apply();
            }
        }

        // --------------------------------------------------------------------
        // MAIN TAB: PASSENGER CAPACITY (max passengers per vehicle)
        // Taxi passenger capacity is not changed (CS2 keeps 4 seats).
        // 1.0 = vanilla, 10.0 = 10x
        // --------------------------------------------------------------------

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public float BusPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public float TramPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public float TrainPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public float SubwayPassengerScalar
        {
            get; set;
        }

        // Passenger-only types (not depots)
        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public float ShipPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public float FerryPassengerScalar
        {
            get; set;
        }

        [SettingsUISlider(min = MinScalar, max = MaxScalar, step = StepScalar,
            scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public float AirplanePassengerScalar
        {
            get; set;
        }

        // ---- Passenger: Reset to vanilla button ----
        [SettingsUIButtonGroup(PassengerGroup)]
        [SettingsUIButton]
        [SettingsUISection(MainTab, PassengerGroup)]
        public bool ResetPassengerToVanillaButton
        {
            set
            {
                if (!value)
                    return;

                ResetPassengerToVanilla();
                Apply();
            }
        }

        // --------------------------------------------------------------------
        // ABOUT TAB: INFO
        // --------------------------------------------------------------------

        [SettingsUISection(AboutTab, AboutInfoGroup)]
        public string ModNameDisplay => $"{Mod.ModName} {Mod.ModTag}";

        [SettingsUISection(AboutTab, AboutInfoGroup)]
        public string ModVersionDisplay => Mod.ModVersion;

        // --------------------------------------------------------------------
        // ABOUT TAB: LINKS
        // --------------------------------------------------------------------

        [SettingsUIButtonGroup(AboutLinksGroup)]
        [SettingsUIButton]
        [SettingsUISection(AboutTab, AboutLinksGroup)]
        public bool OpenParadoxMods
        {
            set
            {
                if (!value)
                    return;

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
                    return;

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

        // --------------------------------------------------------------------
        // ABOUT TAB: DEBUG
        // --------------------------------------------------------------------

        [SettingsUISection(AboutTab, DebugGroup)]
        public bool EnableDebugLogging
        {
            get; set;
        }
    }
}
