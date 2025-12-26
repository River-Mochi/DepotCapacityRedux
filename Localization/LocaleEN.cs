// File: Localization/LocaleEN.cs
// English (en-US) strings for Options UI.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal;

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
            string title = Mod.ShortName;

            if (!string.IsNullOrEmpty(Mod.ModVersion))
            {
                title = title + " (" + Mod.ModVersion + ")";
            }

            return new Dictionary<string, string>
            {
                // --------------------------
                // Mod title / tabs / groups
                // --------------------------

                { m_Setting.GetSettingsLocaleID(), title },

                // Tabs (match Setting.cs tab ids)
                { m_Setting.GetOptionTabLocaleID(Setting.PublicTransitTab), "Public-Transit" },
                { m_Setting.GetOptionTabLocaleID(Setting.ServicesTab),      "Services" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "About" },

                // Groups (match Setting.cs group ids)
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup),          "Depot capacity (max vehicles per depot)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup),      "Passenger capacity (max people per vehicle)" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup),       "Delivery vehicles (cargo capacity)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup),  "Cargo stations (max transports)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup),"Road maintenance" },
                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup),"Park maintenance" },

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup),      "Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup),     "Support links" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup),          "Debug / Logging" },
                { m_Setting.GetOptionGroupLocaleID(Setting.LogGroup),            "Log file" },

                // --------------------
                // Public-Transit tab
                // --------------------

                // Depot capacity (percent sliders)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Bus depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Change how many buses each **Bus Depot** can maintain/spawn.\n" +
                    "**100%** = vanilla (game default).\n" +
                    "**1000%** = ten times more.\n" +
                    "Applies to the base building capacity." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Taxi depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Change how many taxis each **Taxi Depot** can maintain.\n" +
                    "Applies to the base building capacity." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Tram depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Change how many trams each **Tram Depot** can maintain.\n" +
                    "Applies to the base building capacity." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Train depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Change how many trains each **Train Depot** can maintain.\n" +
                    "Applies to the base building capacity." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Subway depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Change how many subway vehicles each **Subway Depot** can maintain.\n" +
                    "Applies to the base building capacity." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Reset all depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Set all depot sliders back to **100%** (game default / vanilla)." },

                // Passenger capacity (percent sliders)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Bus passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Change **bus passenger** capacity.\n" +
                    "**10%** = 10% of vanilla seats.\n" +
                    "**100%** = vanilla seats (game default).\n" +
                    "**1000%** = ten times more seats." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Tram passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Change **tram passenger** capacity.\n" +
                    "**100%** = vanilla seats (game default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Train passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Change **train passenger** capacity.\n" +
                    "Applies to train engines and cars." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Subway passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Change **subway passenger** capacity.\n" +
                    "**100%** = vanilla seats (game default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Ship passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Change **passenger ship** capacity (not cargo ships).\n" +
                    "**100%** = vanilla seats (game default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Ferry passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Change **ferry passenger** capacity.\n" +
                    "**100%** = vanilla seats (game default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Airplane passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Change **airplane passenger** capacity.\n" +
                    "**100%** = vanilla seats (game default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "Double up" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "Set every passenger slider to **200%**.\n" +
                    "Applies to buses, trams, trains, subways, ships, ferries, and airplanes." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "Reset all passengers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Set all passenger sliders back to **100%** (game default / vanilla)." },

                // -------------------
                // Services tab
                // -------------------

                // Delivery vehicles (scalar sliders)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "Semi trucks cargo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "**Semi trucks** (large) \n" +
                     "Multiplier: 1× = vanilla **25t**, 10x = **250t**\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "Delivery vans cargo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**Delivery vans**\n" +
                    "**1×** = vanilla **4t**, 10× = **40t**" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "Raw materials trucks (oil, coal, ore, stone)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**Raw materials** trucks (oil/coal/ore/stone).\n" +
                    "Multiplier: 1× = vanilla **20t**, 10x = **200t**" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "Motorbike cargo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**Motorbike delivery**\n" +
                    "**1× = vanilla 0.1t to 10× = 1.0t** \n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "Reset delivery" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "Set multipliers back to **1×** (game default / vanilla)." },

                // Cargo stations (scalar sliders)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "Cargo station max transports" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "**Cargo Transport Stations** (max Semi trucks for Harbor, Cargo Station, etc..).\n" +
                    "**1×** = vanilla, **10×** = ten times more than base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "Reset cargo stations" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "Reset back to **1×** (game default / vanilla)." },

                // Road maintenance (scalar sliders)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "Road repair truck capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "**Road maintenance** truck capacity.\n" +
                    "Capacity = how much total work a truck can do before it must return to a facility." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "Road repair truck rate" },
                {  m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "**Road maintenance** truck work rate.\n" +
                    "How much work it does per tick while stopped (maxMaintenanceAmount).\n" +
                    "This directly controls “how long it sits and blocks a lane.\n" +
                    "There also seems to be animation time." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "Road Depot fleet size" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "**Road Maintenance Depot** maximum allowed vehicles.\n" +
                    "Changes the number of vehicles each Road building is assigned." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "Reset road maintenance" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "Set Road Maintenance multipliers back to **1×** (game default / vanilla)." },

                // Park maintenance (scalar sliders)

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "Park maintenance vehicle capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "**Park maintenance** vehicle capacity.\n" +
                     "Capacity = how much total work a truck can do before it must return to a facility." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "Park maintenance vehicle rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "**Park maintenance** truck work rate.\n" +
                    "How much work it does per tick while stopped.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "Park maintenance depot fleet size" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "**Park Maintenance Depot** max vehicles allowed.\n" +
                    "Fleet size: how many trucks a building is allowed to have." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "Reset park maintenance" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "Set Park Maintenance multipliers back to **1×** (game default / vanilla)." },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Display name of this mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Current mod version." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Open Paradox Mods website for the author's mods." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)),
                    "Open the community Discord in a browser." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Enable verbose debug logging" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "Enabled = send many extra debug details to AdjustTransitCapacity.log.\n" +
                    "Useful for troubleshooting, but will spam the log.\n" +
                    "**Disable** for normal gameplay.\n" +
                    "<If you do not know what this is,>\n" +
                    "**leave it OFF**, and\n " +
                    "<do not check the box; Log spam affects performance.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Open Log" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Open the ATC log file in the default text editor." },
            };
        }

        public void Unload()
        {
        }
    }
}
