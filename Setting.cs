// Setting.cs
// Options UI for Depot Capacity Redux. Two groups: depot and passenger.
// Sliders show 100%–1000% in steps of 25.

namespace DepotCapacityRedux
{
    using System.Collections.Generic;
    using Colossal;
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using Unity.Entities;

    [FileLocation(Mod.SettingsPath)]
    [SettingsUIGroupOrder(DepotGroup, PassengerGroup)]
    [SettingsUIShowGroupName(DepotGroup, PassengerGroup)]
    public sealed class Setting : ModSetting
    {
        public const string MainTab = "Main";
        public const string DepotGroup = "DepotCapacity";
        public const string PassengerGroup = "PassengerCapacity";

        public const int MinPercent = 100;
        public const int MaxPercent = 1000;
        public const int StepPercent = 25;

        public Setting(IMod mod)
            : base(mod)
        {
            if (BusDepotPercent == 0)
            {
                SetDefaults();
            }
        }

        public override void SetDefaults()
        {
            // depots (5 original types)
            BusDepotPercent = 100;
            TaxiDepotPercent = 100;
            TramDepotPercent = 100;
            TrainDepotPercent = 100;
            SubwayDepotPercent = 100;

            // passengers (all enabled types)
            BusPassengerPercent = 100;
            TaxiPassengerPercent = 100;
            TramPassengerPercent = 100;
            TrainPassengerPercent = 100;
            SubwayPassengerPercent = 100;
            ShipPassengerPercent = 100;
            FerryPassengerPercent = 100;
            AirplanePassengerPercent = 100;
        }

        public override void Apply()
        {
            base.Apply();

            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
            {
                return;
            }

            DepotCapacityReduxSystem system =
                world.GetExistingSystemManaged<DepotCapacityReduxSystem>();
            if (system != null)
            {
                system.Enabled = true;
            }
        }

        // ---------------------------------------------------------------------
        // DEPOT CAPACITY (max vehicles per depot)
        // ---------------------------------------------------------------------

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

        // ---------------------------------------------------------------------
        // PASSENGER CAPACITY (max passengers per vehicle)
        // ---------------------------------------------------------------------

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int BusPassengerPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int TaxiPassengerPercent
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

        // passenger-only types
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

        // ---------------------------------------------------------------------
        // LOCALE: EN
        // ---------------------------------------------------------------------
        public sealed class LocaleEN : IDictionarySource
        {
            private readonly Setting m_Setting;

            public LocaleEN(Setting setting)
            {
                m_Setting = setting;
            }

            public IEnumerable<KeyValuePair<string, string>> ReadEntries(
                IList<IDictionaryEntryError> errors,
                Dictionary<string, int> indexCounts)
            {
                return new Dictionary<string, string>
                {
                    { m_Setting.GetSettingsLocaleID(), "Depot Capacity Redux" },
                    { m_Setting.GetOptionTabLocaleID(MainTab), "Main" },

                    { m_Setting.GetOptionGroupLocaleID(DepotGroup), "Depot capacity (max vehicles per depot)" },
                    { m_Setting.GetOptionGroupLocaleID(PassengerGroup), "Passenger capacity" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BusDepotPercent)), "Bus depots" },
                    { m_Setting.GetOptionDescLocaleID(nameof(BusDepotPercent)), "Number of buses a bus depot can maintain. 100% = vanilla, 1000% = 10×." },

                    { m_Setting.GetOptionLabelLocaleID(nameof(TaxiDepotPercent)), "Taxi depots" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TramDepotPercent)), "Tram depots" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TrainDepotPercent)), "Train depots" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(SubwayDepotPercent)), "Subway depots" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BusPassengerPercent)), "Bus passengers" },
                    { m_Setting.GetOptionDescLocaleID(nameof(BusPassengerPercent)), "Passenger seats for buses. 100% = vanilla, 1000% = 10×." },

                    { m_Setting.GetOptionLabelLocaleID(nameof(TaxiPassengerPercent)), "Taxi passengers" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TramPassengerPercent)), "Tram passengers" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TrainPassengerPercent)), "Train passengers" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(SubwayPassengerPercent)), "Subway passengers" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(ShipPassengerPercent)), "Ship passengers" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(FerryPassengerPercent)), "Ferry passengers" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirplanePassengerPercent)), "Airplane passengers" },
                };
            }

            public void Unload()
            {
            }
        }

        // ---------------------------------------------------------------------
        // LOCALE: FR
        // ---------------------------------------------------------------------
        public sealed class LocaleFR : IDictionarySource
        {
            private readonly Setting m_Setting;

            public LocaleFR(Setting setting)
            {
                m_Setting = setting;
            }

            public IEnumerable<KeyValuePair<string, string>> ReadEntries(
                IList<IDictionaryEntryError> errors,
                Dictionary<string, int> indexCounts)
            {
                return new Dictionary<string, string>
                {
                    { m_Setting.GetSettingsLocaleID(), "Depot Capacity Redux" },
                    { m_Setting.GetOptionTabLocaleID(MainTab), "Principal" },

                    { m_Setting.GetOptionGroupLocaleID(DepotGroup), "Capacité du dépôt" },
                    { m_Setting.GetOptionGroupLocaleID(PassengerGroup), "Capacité passagers" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BusDepotPercent)), "Dépôts de bus" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TaxiDepotPercent)), "Dépôts de taxi" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TramDepotPercent)), "Dépôts de tram" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TrainDepotPercent)), "Dépôts de trains" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(SubwayDepotPercent)), "Dépôts de métro" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BusPassengerPercent)), "Passagers – bus" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TaxiPassengerPercent)), "Passagers – taxi" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TramPassengerPercent)), "Passagers – tram" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TrainPassengerPercent)), "Passagers – train" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(SubwayPassengerPercent)), "Passagers – métro" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(ShipPassengerPercent)), "Passagers – navire" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(FerryPassengerPercent)), "Passagers – ferry" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirplanePassengerPercent)), "Passagers – avion" },
                };
            }

            public void Unload()
            {
            }
        }

        // ---------------------------------------------------------------------
        // LOCALE: ES
        // ---------------------------------------------------------------------
        public sealed class LocaleES : IDictionarySource
        {
            private readonly Setting m_Setting;

            public LocaleES(Setting setting)
            {
                m_Setting = setting;
            }

            public IEnumerable<KeyValuePair<string, string>> ReadEntries(
                IList<IDictionaryEntryError> errors,
                Dictionary<string, int> indexCounts)
            {
                return new Dictionary<string, string>
                {
                    { m_Setting.GetSettingsLocaleID(), "Depot Capacity Redux" },
                    { m_Setting.GetOptionTabLocaleID(MainTab), "Principal" },

                    { m_Setting.GetOptionGroupLocaleID(DepotGroup), "Capacidad del depósito" },
                    { m_Setting.GetOptionGroupLocaleID(PassengerGroup), "Capacidad de pasajeros" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BusDepotPercent)), "Depósitos de autobuses" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TaxiDepotPercent)), "Depósitos de taxis" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TramDepotPercent)), "Depósitos de tranvías" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TrainDepotPercent)), "Depósitos de trenes" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(SubwayDepotPercent)), "Depósitos de metro" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BusPassengerPercent)), "Pasajeros – autobús" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TaxiPassengerPercent)), "Pasajeros – taxi" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TramPassengerPercent)), "Pasajeros – tranvía" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TrainPassengerPercent)), "Pasajeros – tren" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(SubwayPassengerPercent)), "Pasajeros – metro" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(ShipPassengerPercent)), "Pasajeros – barco" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(FerryPassengerPercent)), "Pasajeros – ferry" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirplanePassengerPercent)), "Pasajeros – avión" },
                };
            }

            public void Unload()
            {
            }
        }

        // ---------------------------------------------------------------------
        // LOCALE: DE
        // ---------------------------------------------------------------------
        public sealed class LocaleDE : IDictionarySource
        {
            private readonly Setting m_Setting;

            public LocaleDE(Setting setting)
            {
                m_Setting = setting;
            }

            public IEnumerable<KeyValuePair<string, string>> ReadEntries(
                IList<IDictionaryEntryError> errors,
                Dictionary<string, int> indexCounts)
            {
                return new Dictionary<string, string>
                {
                    { m_Setting.GetSettingsLocaleID(), "Depot Capacity Redux" },
                    { m_Setting.GetOptionTabLocaleID(MainTab), "Hauptmenü" },

                    { m_Setting.GetOptionGroupLocaleID(DepotGroup), "Depotkapazität" },
                    { m_Setting.GetOptionGroupLocaleID(PassengerGroup), "Passagierkapazität" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BusDepotPercent)), "Busdepots" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TaxiDepotPercent)), "Taxidepots" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TramDepotPercent)), "Straßenbahndepots" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TrainDepotPercent)), "Zugdepots" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(SubwayDepotPercent)), "U-Bahn-Depots" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BusPassengerPercent)), "Fahrgäste – Bus" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TaxiPassengerPercent)), "Fahrgäste – Taxi" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TramPassengerPercent)), "Fahrgäste – Straßenbahn" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TrainPassengerPercent)), "Fahrgäste – Zug" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(SubwayPassengerPercent)), "Fahrgäste – U-Bahn" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(ShipPassengerPercent)), "Fahrgäste – Schiff" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(FerryPassengerPercent)), "Fahrgäste – Fähre" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirplanePassengerPercent)), "Fahrgäste – Flugzeug" },
                };
            }

            public void Unload()
            {
            }
        }

        // ---------------------------------------------------------------------
        // LOCALE: ZH-HANS
        // ---------------------------------------------------------------------
        public sealed class LocaleZH : IDictionarySource
        {
            private readonly Setting m_Setting;

            public LocaleZH(Setting setting)
            {
                m_Setting = setting;
            }

            public IEnumerable<KeyValuePair<string, string>> ReadEntries(
                IList<IDictionaryEntryError> errors,
                Dictionary<string, int> indexCounts)
            {
                return new Dictionary<string, string>
                {
                    { m_Setting.GetSettingsLocaleID(), "车库容量重制版" },
                    { m_Setting.GetOptionTabLocaleID(MainTab), "主要" },

                    { m_Setting.GetOptionGroupLocaleID(DepotGroup), "车库容量" },
                    { m_Setting.GetOptionGroupLocaleID(PassengerGroup), "乘客数量" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BusDepotPercent)), "公交车车库" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TaxiDepotPercent)), "出租车车库" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TramDepotPercent)), "有轨电车车库" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TrainDepotPercent)), "火车车库" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(SubwayDepotPercent)), "地铁车库" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BusPassengerPercent)), "公交车乘客" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TaxiPassengerPercent)), "出租车乘客" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TramPassengerPercent)), "有轨电车乘客" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(TrainPassengerPercent)), "火车乘客" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(SubwayPassengerPercent)), "地铁乘客" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(ShipPassengerPercent)), "客运船乘客" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(FerryPassengerPercent)), "渡轮乘客" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirplanePassengerPercent)), "飞机乘客" },
                };
            }

            public void Unload()
            {
            }
        }
    }
}
