// Setting.cs
// Purpose: options UI + saved settings for Depot Capacity Redux.

namespace DepotCapacityRedux
{
    using System.Collections.Generic;
    using Colossal;
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using Unity.Entities;

    // keep custom location folder
    [FileLocation("ModsSettings/DepotCapacityRedux/DepotCapacityRedux")]
    [SettingsUIGroupOrder(DepotGroup, PassengerGroup)]
    [SettingsUIShowGroupName(DepotGroup, PassengerGroup)]
    public sealed class Setting : ModSetting
    {
        // tabs + groups
        public const string MainTab = "Main";
        public const string DepotGroup = "DepotCapacity";
        public const string PassengerGroup = "PassengerCapacity";

        // shared slider ranges
        public const int MinPercent = 100;   // 1x
        public const int MaxPercent = 1000;  // 10x
        public const int StepPercent = 25;

        public Setting(IMod mod)
            : base(mod)
        {
            // brand-new settings file → populate
            if (BusDepotPercent == 0)
            {
                SetDefaults();
            }
        }

        public override void SetDefaults()
        {
            // 5 DEPOTS 
            BusDepotPercent = 100;
            TaxiDepotPercent = 100;
            TramDepotPercent = 100;
            TrainDepotPercent = 100;
            SubwayDepotPercent = 100;

            // PASSENGERS (taxis stay vanilla 4 seats in game)
            BusPassengerPercent = 100;
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

            // tell the system to reapply multipliers once
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

        //
        // DEPOT CAPACITY (max vehicles per depot building)
        // 100% = vanilla, 1000% = 10x
        //

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int BusDepotPercent
        {
            get;
            set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int TaxiDepotPercent
        {
            get;
            set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int TramDepotPercent
        {
            get;
            set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int TrainDepotPercent
        {
            get;
            set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int SubwayDepotPercent
        {
            get;
            set;
        }

        //
        // PASSENGER CAPACITY (max passengers per vehicle)
        // taxi passenger removed (CS2 keeps 4 seats)
        //

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int BusPassengerPercent
        {
            get;
            set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int TramPassengerPercent
        {
            get;
            set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int TrainPassengerPercent
        {
            get;
            set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int SubwayPassengerPercent
        {
            get;
            set;
        }

        // passenger-only types (not depots)
        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int ShipPassengerPercent
        {
            get;
            set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int FerryPassengerPercent
        {
            get;
            set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, PassengerGroup)]
        public int AirplanePassengerPercent
        {
            get;
            set;
        }
    }

    //
    // ENGLISH
    //
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
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Main" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Depot capacity (max vehicles per depot)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Passenger capacity (max riders per vehicle)" },

                // Depot (5 only)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Bus depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotPercent)), "How many buses each bus depot can maintain/spawn. 100% = vanilla, 1000% = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Taxi depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotPercent)), "How many taxis each taxi depot can maintain." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Tram depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotPercent)), "How many trams each tram depot can maintain." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Train depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotPercent)), "How many trains each train depot can maintain." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "Subway depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotPercent)), "How many subway vehicles each depot can maintain." },

                // Passenger (no taxi)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Bus passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerPercent)), "Multiplier for bus passenger seats. 100% = vanilla seats, 1000% = 10× seats." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Tram passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerPercent)), "Multiplier for tram passenger seats." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Train passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerPercent)), "Multiplier for train passenger seats." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Subway passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerPercent)), "Multiplier for subway passenger seats." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Ship passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerPercent)), "Multiplier for passenger ships only (not cargo ships)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Ferry passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerPercent)), "Multiplier for ferries." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Airplane passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerPercent)), "Multiplier for passenger airplanes." },
            };
        }

        public void Unload()
        {
        }
    }

    //
    // FRENCH (fr-FR)
    //
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
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Principal" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacité du dépôt" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacité passagers" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Dépôts de bus" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Dépôts de taxis" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Dépôts de tram" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Dépôts de trains" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "Dépôts de métro" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Passagers – bus" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Passagers – tram" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Passagers – train" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Passagers – métro" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Passagers – navire" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Passagers – ferry" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Passagers – avion" },
            };
        }

        public void Unload()
        {
        }
    }

    //
    // SPANISH (es-ES)
    //
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
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Principal" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacidad del depósito" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacidad de pasajeros" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Depósitos de autobuses" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Depósitos de taxis" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Depósitos de tranvías" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Depósitos de trenes" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "Depósitos de metro" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Pasajeros – autobús" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Pasajeros – tranvía" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Pasajeros – tren" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Pasajeros – metro" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Pasajeros – barco" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Pasajeros – ferry" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Pasajeros – avión" },
            };
        }

        public void Unload()
        {
        }
    }

    //
    // GERMAN (de-DE)
    //
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
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Hauptmenü" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Depotkapazität" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Passagierkapazität" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Busdepots" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Taxidepots" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Straßenbahndepots" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Zugdepots" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "U-Bahn-Depots" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Fahrgäste – Bus" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Fahrgäste – Straßenbahn" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Fahrgäste – Zug" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Fahrgäste – U-Bahn" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Fahrgäste – Schiff" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Fahrgäste – Fähre" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Fahrgäste – Flugzeug" },
            };
        }

        public void Unload()
        {
        }
    }

    //
    // SIMPLIFIED CHINESE (zh-HANS)
    //
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
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "主要" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "车库容量" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "乘客数量" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "公交车车库" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "出租车车库" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "有轨电车车库" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "火车车库" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "地铁车库" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "公交车乘客" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "有轨电车乘客" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "火车乘客" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "地铁乘客" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "客运船乘客" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "渡轮乘客" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "飞机乘客" },
            };
        }

        public void Unload()
        {
        }
    }
}
