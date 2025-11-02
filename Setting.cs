// Setting.cs
// Settings, groups, sliders, and all 5 locales for Depot Capacity Redux.

namespace DepotCapacityRedux
{
    using System.Collections.Generic;
    using Colossal;
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using Unity.Entities;

    // store settings here:
    [FileLocation("ModsSettings/DepotCapacityRedux/DepotCapacityRedux")]
    [SettingsUIGroupOrder(DepotGroup, PassengerGroup)]
    [SettingsUIShowGroupName(DepotGroup, PassengerGroup)]
    public sealed class Setting : ModSetting
    {
        // tabs + groups
        public const string MainTab = "Main";
        public const string DepotGroup = "DepotCapacity";
        public const string PassengerGroup = "PassengerCapacity";

        // shared ranges
        public const int MinPercent = 100;   // 1x
        public const int MaxPercent = 1000;  // 10x
        public const int StepPercent = 25;

        public Setting(IMod mod)
            : base(mod)
        {
            // brand new settings file -> populate with defaults
            if (BusDepotPercent == 0)
            {
                SetDefaults();
            }
        }

        public override void SetDefaults()
        {
            // DEPOTS 5 types handled
            BusDepotPercent = 100;
            TaxiDepotPercent = 100;
            TramDepotPercent = 100;
            TrainDepotPercent = 100;
            SubwayDepotPercent = 100;

            // PASSENGERS
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

            // tell ECS system to re-apply once
            World world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
            {
                return;
            }

            DepotCapacityReduxSystem system = world.GetExistingSystemManaged<DepotCapacityReduxSystem>();
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

        //
        // PASSENGER CAPACITY (max passengers per vehicle)
        //

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

        // passenger vehicles only
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
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Passenger capacity (max passengers per vehicle)" },

                // depot labels + desc
                { m_Setting.GetOptionLabelLocaleID("BusDepotPercent"), "Bus depots" },
                { m_Setting.GetOptionDescLocaleID("BusDepotPercent"), "Max vehicles each bus depot can maintain/spawn. 100% = vanilla, 1000% = 10×." },

                { m_Setting.GetOptionLabelLocaleID("TaxiDepotPercent"), "Taxi depots" },
                { m_Setting.GetOptionDescLocaleID("TaxiDepotPercent"), "Max vehicles each taxi depot can maintain. 100% = vanilla." },

                { m_Setting.GetOptionLabelLocaleID("TramDepotPercent"), "Tram depots" },
                { m_Setting.GetOptionDescLocaleID("TramDepotPercent"), "Max vehicles each tram depot can maintain." },

                { m_Setting.GetOptionLabelLocaleID("TrainDepotPercent"), "Train depots" },
                { m_Setting.GetOptionDescLocaleID("TrainDepotPercent"), "Max vehicles each train depot can maintain." },

                { m_Setting.GetOptionLabelLocaleID("SubwayDepotPercent"), "Subway depots" },
                { m_Setting.GetOptionDescLocaleID("SubwayDepotPercent"), "Max vehicles each subway depot can maintain." },

                // passenger labels + desc
                { m_Setting.GetOptionLabelLocaleID("BusPassengerPercent"), "Bus passengers" },
                { m_Setting.GetOptionDescLocaleID("BusPassengerPercent"), "Passenger seats per bus. 100% = vanilla, 1000% = 10×." },

                { m_Setting.GetOptionLabelLocaleID("TaxiPassengerPercent"), "Taxi passengers" },
                { m_Setting.GetOptionDescLocaleID("TaxiPassengerPercent"), "Passenger seats per taxi." },

                { m_Setting.GetOptionLabelLocaleID("TramPassengerPercent"), "Tram passengers" },
                { m_Setting.GetOptionDescLocaleID("TramPassengerPercent"), "Passenger seats per tram." },

                { m_Setting.GetOptionLabelLocaleID("TrainPassengerPercent"), "Train passengers" },
                { m_Setting.GetOptionDescLocaleID("TrainPassengerPercent"), "Passenger seats per train." },

                { m_Setting.GetOptionLabelLocaleID("SubwayPassengerPercent"), "Subway passengers" },
                { m_Setting.GetOptionDescLocaleID("SubwayPassengerPercent"), "Passenger seats per subway vehicle." },

                { m_Setting.GetOptionLabelLocaleID("ShipPassengerPercent"), "Ship passengers" },
                { m_Setting.GetOptionDescLocaleID("ShipPassengerPercent"), "Passenger seats per passenger ship." },

                { m_Setting.GetOptionLabelLocaleID("FerryPassengerPercent"), "Ferry passengers" },
                { m_Setting.GetOptionDescLocaleID("FerryPassengerPercent"), "Passenger seats per ferry." },

                { m_Setting.GetOptionLabelLocaleID("AirplanePassengerPercent"), "Airplane passengers" },
                { m_Setting.GetOptionDescLocaleID("AirplanePassengerPercent"), "Passenger seats per passenger airplane." },
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

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacité du dépôt (véhicules max)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacité passagers (par véhicule)" },

                { m_Setting.GetOptionLabelLocaleID("BusDepotPercent"), "Dépôts de bus" },
                { m_Setting.GetOptionDescLocaleID("BusDepotPercent"), "Nombre maximal de bus qu’un dépôt de bus peut gérer. 100 % = valeur de base." },

                { m_Setting.GetOptionLabelLocaleID("TaxiDepotPercent"), "Dépôts de taxi" },
                { m_Setting.GetOptionDescLocaleID("TaxiDepotPercent"), "Nombre maximal de taxis qu’un dépôt de taxi peut gérer." },

                { m_Setting.GetOptionLabelLocaleID("TramDepotPercent"), "Dépôts de tram" },
                { m_Setting.GetOptionDescLocaleID("TramDepotPercent"), "Nombre maximal de tramways qu’un dépôt peut gérer." },

                { m_Setting.GetOptionLabelLocaleID("TrainDepotPercent"), "Dépôts ferroviaires" },
                { m_Setting.GetOptionDescLocaleID("TrainDepotPercent"), "Nombre maximal de trains qu’un dépôt peut gérer." },

                { m_Setting.GetOptionLabelLocaleID("SubwayDepotPercent"), "Dépôts de métro" },
                { m_Setting.GetOptionDescLocaleID("SubwayDepotPercent"), "Nombre maximal de rames de métro qu’un dépôt peut gérer." },

                { m_Setting.GetOptionLabelLocaleID("BusPassengerPercent"), "Passagers – bus" },
                { m_Setting.GetOptionDescLocaleID("BusPassengerPercent"), "Places disponibles dans un bus. 100 % = valeur de base." },

                { m_Setting.GetOptionLabelLocaleID("TaxiPassengerPercent"), "Passagers – taxi" },
                { m_Setting.GetOptionDescLocaleID("TaxiPassengerPercent"), "Places disponibles dans un taxi." },

                { m_Setting.GetOptionLabelLocaleID("TramPassengerPercent"), "Passagers – tram" },
                { m_Setting.GetOptionDescLocaleID("TramPassengerPercent"), "Places disponibles dans un tramway." },

                { m_Setting.GetOptionLabelLocaleID("TrainPassengerPercent"), "Passagers – train" },
                { m_Setting.GetOptionDescLocaleID("TrainPassengerPercent"), "Places disponibles dans un train." },

                { m_Setting.GetOptionLabelLocaleID("SubwayPassengerPercent"), "Passagers – métro" },
                { m_Setting.GetOptionDescLocaleID("SubwayPassengerPercent"), "Places disponibles dans une rame de métro." },

                { m_Setting.GetOptionLabelLocaleID("ShipPassengerPercent"), "Passagers – navire" },
                { m_Setting.GetOptionDescLocaleID("ShipPassengerPercent"), "Places disponibles dans un navire de passagers." },

                { m_Setting.GetOptionLabelLocaleID("FerryPassengerPercent"), "Passagers – ferry" },
                { m_Setting.GetOptionDescLocaleID("FerryPassengerPercent"), "Places disponibles dans un ferry." },

                { m_Setting.GetOptionLabelLocaleID("AirplanePassengerPercent"), "Passagers – avion" },
                { m_Setting.GetOptionDescLocaleID("AirplanePassengerPercent"), "Places disponibles dans un avion de passagers." },
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

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacidad del depósito (vehículos máx.)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacidad de pasajeros (por vehículo)" },

                { m_Setting.GetOptionLabelLocaleID("BusDepotPercent"), "Depósitos de autobuses" },
                { m_Setting.GetOptionDescLocaleID("BusDepotPercent"), "Cantidad máxima de autobuses que puede mantener un depósito de autobuses. 100 % = juego base." },

                { m_Setting.GetOptionLabelLocaleID("TaxiDepotPercent"), "Depósitos de taxis" },
                { m_Setting.GetOptionDescLocaleID("TaxiDepotPercent"), "Cantidad máxima de taxis que puede mantener un depósito de taxis." },

                { m_Setting.GetOptionLabelLocaleID("TramDepotPercent"), "Depósitos de tranvías" },
                { m_Setting.GetOptionDescLocaleID("TramDepotPercent"), "Cantidad máxima de tranvías que puede mantener un depósito." },

                { m_Setting.GetOptionLabelLocaleID("TrainDepotPercent"), "Depósitos de trenes" },
                { m_Setting.GetOptionDescLocaleID("TrainDepotPercent"), "Cantidad máxima de trenes que puede mantener un depósito." },

                { m_Setting.GetOptionLabelLocaleID("SubwayDepotPercent"), "Depósitos de metro" },
                { m_Setting.GetOptionDescLocaleID("SubwayDepotPercent"), "Cantidad máxima de vehículos de metro que puede mantener un depósito." },

                { m_Setting.GetOptionLabelLocaleID("BusPassengerPercent"), "Pasajeros – autobús" },
                { m_Setting.GetOptionDescLocaleID("BusPassengerPercent"), "Asientos por autobús. 100 % = base, 1000 % = 10×." },

                { m_Setting.GetOptionLabelLocaleID("TaxiPassengerPercent"), "Pasajeros – taxi" },
                { m_Setting.GetOptionDescLocaleID("TaxiPassengerPercent"), "Asientos por taxi." },

                { m_Setting.GetOptionLabelLocaleID("TramPassengerPercent"), "Pasajeros – tranvía" },
                { m_Setting.GetOptionDescLocaleID("TramPassengerPercent"), "Asientos por tranvía." },

                { m_Setting.GetOptionLabelLocaleID("TrainPassengerPercent"), "Pasajeros – tren" },
                { m_Setting.GetOptionDescLocaleID("TrainPassengerPercent"), "Asientos por tren." },

                { m_Setting.GetOptionLabelLocaleID("SubwayPassengerPercent"), "Pasajeros – metro" },
                { m_Setting.GetOptionDescLocaleID("SubwayPassengerPercent"), "Asientos por vehículo de metro." },

                { m_Setting.GetOptionLabelLocaleID("ShipPassengerPercent"), "Pasajeros – barco" },
                { m_Setting.GetOptionDescLocaleID("ShipPassengerPercent"), "Asientos por barco de pasajeros." },

                { m_Setting.GetOptionLabelLocaleID("FerryPassengerPercent"), "Pasajeros – ferry" },
                { m_Setting.GetOptionDescLocaleID("FerryPassengerPercent"), "Asientos por ferry." },

                { m_Setting.GetOptionLabelLocaleID("AirplanePassengerPercent"), "Pasajeros – avión" },
                { m_Setting.GetOptionDescLocaleID("AirplanePassengerPercent"), "Asientos por avión de pasajeros." },
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

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Depotkapazität (max. Fahrzeuge pro Depot)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Passagierkapazität (pro Fahrzeug)" },

                { m_Setting.GetOptionLabelLocaleID("BusDepotPercent"), "Busdepots" },
                { m_Setting.GetOptionDescLocaleID("BusDepotPercent"), "Maximale Anzahl an Bussen, die ein Busdepot verwalten kann. 100 % = Standard." },

                { m_Setting.GetOptionLabelLocaleID("TaxiDepotPercent"), "Taxidepots" },
                { m_Setting.GetOptionDescLocaleID("TaxiDepotPercent"), "Maximale Anzahl an Taxis, die ein Taxidepot verwalten kann." },

                { m_Setting.GetOptionLabelLocaleID("TramDepotPercent"), "Straßenbahndepots" },
                { m_Setting.GetOptionDescLocaleID("TramDepotPercent"), "Maximale Anzahl an Straßenbahnen pro Depot." },

                { m_Setting.GetOptionLabelLocaleID("TrainDepotPercent"), "Zugdepots" },
                { m_Setting.GetOptionDescLocaleID("TrainDepotPercent"), "Maximale Anzahl an Zügen pro Depot." },

                { m_Setting.GetOptionLabelLocaleID("SubwayDepotPercent"), "U-Bahn-Depots" },
                { m_Setting.GetOptionDescLocaleID("SubwayDepotPercent"), "Maximale Anzahl an U-Bahn-Fahrzeugen pro Depot." },

                { m_Setting.GetOptionLabelLocaleID("BusPassengerPercent"), "Fahrgäste – Bus" },
                { m_Setting.GetOptionDescLocaleID("BusPassengerPercent"), "Sitzplätze pro Bus. 100 % = Standard, 1000 % = 10×." },

                { m_Setting.GetOptionLabelLocaleID("TaxiPassengerPercent"), "Fahrgäste – Taxi" },
                { m_Setting.GetOptionDescLocaleID("TaxiPassengerPercent"), "Sitzplätze pro Taxi." },

                { m_Setting.GetOptionLabelLocaleID("TramPassengerPercent"), "Fahrgäste – Straßenbahn" },
                { m_Setting.GetOptionDescLocaleID("TramPassengerPercent"), "Sitzplätze pro Straßenbahn." },

                { m_Setting.GetOptionLabelLocaleID("TrainPassengerPercent"), "Fahrgäste – Zug" },
                { m_Setting.GetOptionDescLocaleID("TrainPassengerPercent"), "Sitzplätze pro Zug." },

                { m_Setting.GetOptionLabelLocaleID("SubwayPassengerPercent"), "Fahrgäste – U-Bahn" },
                { m_Setting.GetOptionDescLocaleID("SubwayPassengerPercent"), "Sitzplätze pro U-Bahn." },

                { m_Setting.GetOptionLabelLocaleID("ShipPassengerPercent"), "Fahrgäste – Schiff" },
                { m_Setting.GetOptionDescLocaleID("ShipPassengerPercent"), "Sitzplätze pro Passagierschiff." },

                { m_Setting.GetOptionLabelLocaleID("FerryPassengerPercent"), "Fahrgäste – Fähre" },
                { m_Setting.GetOptionDescLocaleID("FerryPassengerPercent"), "Sitzplätze pro Fähre." },

                { m_Setting.GetOptionLabelLocaleID("AirplanePassengerPercent"), "Fahrgäste – Flugzeug" },
                { m_Setting.GetOptionDescLocaleID("AirplanePassengerPercent"), "Sitzplätze pro Flugzeug." },
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

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "车库容量（每个车库最大车辆数）" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "载客量（每辆车可载乘客数）" },

                { m_Setting.GetOptionLabelLocaleID("BusDepotPercent"), "公交车车库" },
                { m_Setting.GetOptionDescLocaleID("BusDepotPercent"), "每个公交车车库可以维护 / 派出的最大公交数。100% 为原版，1000% 为 10 倍。" },

                { m_Setting.GetOptionLabelLocaleID("TaxiDepotPercent"), "出租车车库" },
                { m_Setting.GetOptionDescLocaleID("TaxiDepotPercent"), "每个出租车车库可以维护的最大出租车数量。" },

                { m_Setting.GetOptionLabelLocaleID("TramDepotPercent"), "有轨电车车库" },
                { m_Setting.GetOptionDescLocaleID("TramDepotPercent"), "每个有轨电车车库可以维护的最大车辆数。" },

                { m_Setting.GetOptionLabelLocaleID("TrainDepotPercent"), "火车车库" },
                { m_Setting.GetOptionDescLocaleID("TrainDepotPercent"), "每个火车车库可以维护的最大列车数。" },

                { m_Setting.GetOptionLabelLocaleID("SubwayDepotPercent"), "地铁车库" },
                { m_Setting.GetOptionDescLocaleID("SubwayDepotPercent"), "每个地铁车库可以维护的最大地铁车辆数。" },

                { m_Setting.GetOptionLabelLocaleID("BusPassengerPercent"), "公交车乘客" },
                { m_Setting.GetOptionDescLocaleID("BusPassengerPercent"), "每辆公交车可以载的乘客数。100% 为原版，1000% 为 10 倍。" },

                { m_Setting.GetOptionLabelLocaleID("TaxiPassengerPercent"), "出租车乘客" },
                { m_Setting.GetOptionDescLocaleID("TaxiPassengerPercent"), "每辆出租车可以载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID("TramPassengerPercent"), "有轨电车乘客" },
                { m_Setting.GetOptionDescLocaleID("TramPassengerPercent"), "每辆有轨电车可以载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID("TrainPassengerPercent"), "火车乘客" },
                { m_Setting.GetOptionDescLocaleID("TrainPassengerPercent"), "每列火车可以载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID("SubwayPassengerPercent"), "地铁乘客" },
                { m_Setting.GetOptionDescLocaleID("SubwayPassengerPercent"), "每辆地铁车可以载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID("ShipPassengerPercent"), "客运船乘客" },
                { m_Setting.GetOptionDescLocaleID("ShipPassengerPercent"), "每艘客运船可以载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID("FerryPassengerPercent"), "渡轮乘客" },
                { m_Setting.GetOptionDescLocaleID("FerryPassengerPercent"), "每艘渡轮可以载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID("AirplanePassengerPercent"), "飞机乘客" },
                { m_Setting.GetOptionDescLocaleID("AirplanePassengerPercent"), "每架客机可以载的乘客数。" },
            };
        }

        public void Unload()
        {
        }
    }
}
