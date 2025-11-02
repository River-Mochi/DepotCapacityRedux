namespace DepotCapacityRedux
{
    using System.Collections.Generic;
    using Colossal;
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using Unity.Entities;

    [FileLocation(nameof(DepotCapacityRedux))]
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
            if (BusDepotPercent == 0)
            {
                SetDefaults();
            }
        }

        public override void SetDefaults()
        {
            // depot capacity defaults
            BusDepotPercent = 100;
            TaxiDepotPercent = 100;
            TramDepotPercent = 100;
            TrainDepotPercent = 100;
            SubwayDepotPercent = 100;
            ShipDepotPercent = 100;
            FerryDepotPercent = 100;
            AirplaneDepotPercent = 100;

            // passenger capacity defaults
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

            // poke the system
            var world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
            {
                return;
            }

            var system = world.GetExistingSystemManaged<DepotCapacityReduxSystem>();
            if (system != null)
            {
                system.Enabled = true;
            }
        }

        //
        // DEPOT CAPACITY (max vehicles per depot building)
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

        // extra depot types
        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int ShipDepotPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int FerryDepotPercent
        {
            get; set;
        }

        [SettingsUISlider(min = MinPercent, max = MaxPercent, step = StepPercent, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, DepotGroup)]
        public int AirplaneDepotPercent
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

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Depot Capacity Redux" },
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Main" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Depot capacity (max vehicles per depot)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Passenger capacity (max passengers per vehicle)" },

                // depot
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Bus depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotPercent)), "Depot capacity for bus depots. This is how many buses a depot can maintain / spawn." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Taxi depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotPercent)), "Depot capacity for taxi depots." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Tram depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotPercent)), "Depot capacity for tram depots." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Train depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotPercent)), "Depot capacity for train depots." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "Subway depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotPercent)), "Depot capacity for subway depots." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipDepotPercent)), "Ship depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipDepotPercent)), "Depot capacity for ship depots." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryDepotPercent)), "Ferry depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryDepotPercent)), "Depot capacity for ferry depots." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplaneDepotPercent)), "Airplane depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplaneDepotPercent)), "Depot capacity for airplane depots." },

                // passenger
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Bus passenger capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerPercent)), "How many passengers can ride a bus (100% = vanilla)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiPassengerPercent)), "Taxi passenger capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiPassengerPercent)), "How many passengers can ride a taxi." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Tram passenger capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerPercent)), "How many passengers can ride a tram." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Train passenger capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerPercent)), "How many passengers can ride a train." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Subway passenger capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerPercent)), "How many passengers can ride a subway vehicle." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Ship passenger capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerPercent)), "How many passengers can ride a ship." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Ferry passenger capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerPercent)), "How many passengers can ride a ferry." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Airplane passenger capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerPercent)), "How many passengers can ride an airplane." },
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

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            // simple, game-style French
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Depot Capacity Redux" },
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Principal" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacité du dépôt (véhicules max)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacité passagers (par véhicule)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Dépôts de bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotPercent)), "Capacité du dépôt de bus. Nombre de bus que le dépôt peut gérer." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Dépôts de taxi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotPercent)), "Capacité du dépôt de taxis." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Dépôts de tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotPercent)), "Capacité du dépôt de tramways." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Dépôts ferroviaires" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotPercent)), "Capacité du dépôt de trains." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "Dépôts de métro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotPercent)), "Capacité du dépôt de métro." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipDepotPercent)), "Dépôts de navires" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipDepotPercent)), "Capacité du dépôt pour les navires." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryDepotPercent)), "Dépôts de ferries" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryDepotPercent)), "Capacité du dépôt pour les ferries." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplaneDepotPercent)), "Dépôts d’avions" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplaneDepotPercent)), "Capacité du dépôt pour les avions." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Capacité passagers – bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerPercent)), "Nombre de passagers qu’un bus peut transporter." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiPassengerPercent)), "Capacité passagers – taxis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiPassengerPercent)), "Nombre de passagers qu’un taxi peut transporter." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Capacité passagers – tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerPercent)), "Nombre de passagers qu’un tram peut transporter." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Capacité passagers – train" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerPercent)), "Nombre de passagers qu’un train peut transporter." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Capacité passagers – métro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerPercent)), "Nombre de passagers qu’une rame de métro peut transporter." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Capacité passagers – navires" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerPercent)), "Nombre de passagers qu’un navire peut transporter." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Capacité passagers – ferries" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerPercent)), "Nombre de passagers qu’un ferry peut transporter." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Capacité passagers – avions" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerPercent)), "Nombre de passagers qu’un avion peut transporter." },
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

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Depot Capacity Redux" },
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Principal" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacidad del depósito (vehículos máx.)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacidad de pasajeros (por vehículo)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Depósitos de autobuses" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotPercent)), "Capacidad del depósito de autobuses. Cuántos autobuses puede mantener/enviar." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Depósitos de taxis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotPercent)), "Capacidad del depósito de taxis." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Depósitos de tranvías" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotPercent)), "Capacidad del depósito de tranvías." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Depósitos de trenes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotPercent)), "Capacidad del depósito de trenes." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "Depósitos de metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotPercent)), "Capacidad del depósito de metro." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipDepotPercent)), "Depósitos de barcos" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipDepotPercent)), "Capacidad del depósito de barcos." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryDepotPercent)), "Depósitos de ferris" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryDepotPercent)), "Capacidad del depósito de ferris." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplaneDepotPercent)), "Depósitos de aviones" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplaneDepotPercent)), "Capacidad del depósito de aviones." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Capacidad de pasajeros – autobús" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerPercent)), "Número de pasajeros que puede llevar un autobús." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiPassengerPercent)), "Capacidad de pasajeros – taxi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiPassengerPercent)), "Número de pasajeros que puede llevar un taxi." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Capacidad de pasajeros – tranvía" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerPercent)), "Número de pasajeros que puede llevar un tranvía." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Capacidad de pasajeros – tren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerPercent)), "Número de pasajeros que puede llevar un tren." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Capacidad de pasajeros – metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerPercent)), "Número de pasajeros que puede llevar un vagón de metro." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Capacidad de pasajeros – barco" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerPercent)), "Número de pasajeros que puede llevar un barco." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Capacidad de pasajeros – ferry" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerPercent)), "Número de pasajeros que puede llevar un ferry." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Capacidad de pasajeros – avión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerPercent)), "Número de pasajeros que puede llevar un avión." },
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

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Depot Capacity Redux" },
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Hauptmenü" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Depotkapazität (max. Fahrzeuge pro Depot)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Passagierkapazität (pro Fahrzeug)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Busdepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotPercent)), "Depotkapazität für Busdepots. Anzahl der Busse, die das Depot verwalten kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Taxidepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotPercent)), "Depotkapazität für Taxidepots." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Straßenbahndepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotPercent)), "Depotkapazität für Straßenbahnen." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Zugdepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotPercent)), "Depotkapazität für Züge." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "U-Bahn-Depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotPercent)), "Depotkapazität für U-Bahnen." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipDepotPercent)), "Schiffsdepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipDepotPercent)), "Depotkapazität für Schiffe." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryDepotPercent)), "Fährdepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryDepotPercent)), "Depotkapazität für Fähren." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplaneDepotPercent)), "Flugzeugdepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplaneDepotPercent)), "Depotkapazität für Flugzeuge." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Passagierkapazität – Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerPercent)), "Wie viele Passagiere ein Bus transportieren kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiPassengerPercent)), "Passagierkapazität – Taxi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiPassengerPercent)), "Wie viele Passagiere ein Taxi transportieren kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Passagierkapazität – Straßenbahn" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerPercent)), "Wie viele Passagiere eine Straßenbahn transportieren kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Passagierkapazität – Zug" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerPercent)), "Wie viele Passagiere ein Zug transportieren kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Passagierkapazität – U-Bahn" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerPercent)), "Wie viele Passagiere eine U-Bahn transportieren kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Passagierkapazität – Schiff" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerPercent)), "Wie viele Passagiere ein Schiff transportieren kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Passagierkapazität – Fähre" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerPercent)), "Wie viele Passagiere eine Fähre transportieren kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Passagierkapazität – Flugzeug" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerPercent)), "Wie viele Passagiere ein Flugzeug transportieren kann." },
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

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "车辆调度容量重制版" },
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "主要" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "车库容量（每个车库可维护的车辆数）" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "载客量（每辆车可载乘客数）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "公交车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotPercent)), "公交车车库的最大车辆数，100% 为游戏原始值。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "出租车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotPercent)), "出租车车库的最大车辆数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "有轨电车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotPercent)), "有轨电车车库的最大车辆数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "火车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotPercent)), "火车车库的最大车辆数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "地铁车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotPercent)), "地铁车库的最大车辆数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipDepotPercent)), "船只车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipDepotPercent)), "船只/港口的最大车辆数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryDepotPercent)), "渡轮车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryDepotPercent)), "渡轮车库的最大车辆数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplaneDepotPercent)), "飞机机库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplaneDepotPercent)), "飞机机库的最大飞机数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "公交车载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerPercent)), "一辆公交车最多可载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiPassengerPercent)), "出租车载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiPassengerPercent)), "一辆出租车最多可载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "有轨电车载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerPercent)), "一辆有轨电车最多可载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "火车载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerPercent)), "一列火车最多可载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "地铁载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerPercent)), "一列地铁车最多可载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "船只载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerPercent)), "一艘船最多可载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "渡轮载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerPercent)), "一艘渡轮最多可载的乘客数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "飞机载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerPercent)), "一架飞机最多可载的乘客数。" },
            };
        }

        public void Unload()
        {
        }
    }
}
