// Settings/Setting.cs
// Options UI + saved settings for Adjust Transit Capacity.

namespace AdjustTransitCapacity
{
    using System;
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
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

        // Defaults

        public override void SetDefaults()
        {
            ResetDepotToVanilla();
            ResetPassengerToVanilla();

            // Verbose logging off by default.
            EnableDebugLogging = false;
        }

        // Apply callback

        public override void Apply()
        {
            base.Apply();

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

        // Actions tab: depot capacity (max vehicles per depot)
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

        // Actions tab: passenger capacity (max passengers per vehicle)
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

        // About tab: info

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

        // About tab: debug

        [SettingsUISection(AboutTab, DebugGroup)]
        public bool EnableDebugLogging
        {
            get; set;
        }

        // Helper methods

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
