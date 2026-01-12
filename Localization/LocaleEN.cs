// File: Localization/LocaleEN.cs
// English (en-US) strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

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
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "Industry" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "Parks-Roads" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "About" },

                // --------------------
                // Public-Transit tab
                // --------------------

               // Line vehicle count (vanilla line panel limits)
                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "Transit Lines (in-game slider vehicle limits)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "Expand transit line slider limits" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "1. Allows Transit line slider go as **low as 1 vehicle** on most routes (even long ones).\n" +
                    "2. **Maximums are higher than vanilla but still allows the game's logic**\n" +
                    "Game calculates limits on routes (driving time + stop count), so max varies per line (but will be a little higher with this ON [x]).\n" +
                    "<Avoid conflicts: remove any mod that edits the same policy> (e.g.: TransportPolicyAdjuster or SmartTransporation).\n" +
                    "If you want to use that mod with this mod, then keep this checkbox [ ] off.> Better to not have both.\n" +
                    "Works for: bus, tram, train, subway, ship, ferry, airplane.\n" 
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Depot capacity (max vehicles per depot)" },  // group title
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Change how many buses each **Bus Depot** can maintain/spawn.\n" +
                    "**100%** = vanilla (game default).\n" +
                    "**1000%** = 10x more.\n" +
                    "Applies to the base building capacity." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Taxi depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "How many taxis each **taxi depot** can maintain.\n" +
                    "Increase applies to the base depot building only." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Change how many trams each **Tram Depot** can maintain.\n" +
                    "Applies to the base building capacity." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Train" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Change how many trains each **Train Depot** can maintain.\n" +
                    "Applies to the base building capacity." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Subway" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Change how many subway vehicles each **Subway Depot** can maintain.\n" +
                    "Applies to the base building capacity." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Reset all depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Set all depot sliders back to **100%** (game default / vanilla)." },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup),       "Passenger capacity (max people per vehicle)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Change **bus passenger** capacity.\n" +
                    "**10%** = 10% of vanilla seats.\n" +
                    "**100%** = vanilla seats (game default).\n" +
                    "**1000%** = ten times more seats." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Change **tram passenger** capacity.\n" +
                    "**100%** = vanilla seats (game default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Train" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Change **train passenger** capacity.\n" +
                    "Applies to train engines and cars." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Subway" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Change **subway passenger** capacity.\n" +
                    "**100%** = vanilla seats (game default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Ship" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Change **passenger ship** capacity (not cargo ships).\n" +
                    "**100%** = vanilla seats (game default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Ferry" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Change **ferry passenger** capacity.\n" +
                    "**100%** = vanilla seats (game default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Airplane" },
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

                // -----------------
                // INDUSTRY tab
                // ----------------

                // Delivery vehicles

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup),        "Delivery vehicles (cargo capacity)" },   // group title

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "Semi trucks" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "**Semi trucks** (large)\n" +
                    "Includes Specilized Industry (Farms,Fish, Forests)\n" +
                    "Multiplier: **1×** = vanilla (**25t**), **10×** = ten times more." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "Delivery vans" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**Delivery vans**\n" +
                    "Multiplier: **1×** = vanilla (**4t**), **10×** = ten times more." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "Raw materials trucks (oil, coal, ore, stone)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**Raw materials** trucks (oil/coal/ore/stone)\n" +
                    "Multiplier: **1×** = vanilla (**20t**), **10×** = ten times more." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "Motorbike" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**Motorbike delivery**\n" +
                    "Multiplier: **1×** = vanilla, **10×** = ten times more." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "Reset delivery" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "Set Delivery multipliers back to **1×** (game default / vanilla)." },

                // Extractor Company Fleet
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "Extractor Fleet" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "Multiplier for Industrial **Extractors max trucks** (farms, forestry, fish, ore, oil, plus coal/stone when available).\n" +
                    "**1×** = vanilla, **5×** = five times more." },

                // Cargo stations
                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup),   "Cargo Fleet (harbor, train, airport)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "Cargo station max fleet" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "Multiplier for **Cargo Transport Stations** maximum active transporters.\n" +
                    "**1×** = vanilla, **5×** = five times more." },

                // Rename reset button (same property name, just better wording)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "Reset Cargo + Extractors Fleet" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "Set Cargo Station + Extractor multipliers back to **1×** (game default / vanilla)." },

                // -------------------
                // Parks-Roads
                // -------------------

                // Park maintenance
                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "Park maintenance" }, // group title
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "Work Shift Capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "Multiplier for Work Shift = vehicle capacity.\n" +
                    "Total work a truck can do before it must return to the building." +
                    "Imagine the truck is getting extra supplies so it can stay out longer." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "Vehicle rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "Multiplier for vehicle work rate.\n" +
                    "Rate = how much work it does per simulation tick while stopped."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "Depot fleet size" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "Multiplier for the **maximum vehicles** the depot building can handle.\n"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "Reset park maintenance" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "Reset all values back to **100%** (game default / vanilla)." },

                  // Road maintenance
                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "Road maintenance" }, // group title
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "Depot Fleet size" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "Multiplier for **Depot maximum vehicles allowed** per building.\n" +
                    "Higher = more trucks (monitor for balance as too few or too many can hurt traffic."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "Work Shift capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "Multiplier for **Work Shift**.\n" +
                    "Work Shift = capacity in game code.\n" +
                    "Total work a truck can do before it must return to the depot. \n" +
                    "Higher = fewer returns"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "Repair Rate (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "Rate = how much work it does per simulation tick while stopped (related to speed of work).\n" +
                    "Roads have different amounts of damage; in vanilla, sometimes not all damage is repaired in one stop.\n" +
                    "Alpha version: when rate is increased, trucks still stop and go briefly. Not known yet if stop animation can be eliminated"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "Road wear speed (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "Road Wear is Alpha feature still testing\n" +
                    "How fast roads accumulate wear over time.\n" +
                    "**100%** = vanilla\n" +
                    "**10%** = 10× slower wear (fewer repair needed).\n" +
                    "**400%** = 4× faster wear (more repairs needed)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "Reset road maintenance" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "Set all values back to **100%** (game default / vanilla)." },

                // --------------------
                // About tab
                // --------------------
                
                // Group titles
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup),       "Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup),      "Support links" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup),           "Debug / Logging" },

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

                // DEBUG
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "Prefab SCAN REPORT" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "One-time scan report of relevant prefabs and lane wear settings.\n" +
                    "Report: <ModsData/DispatchBoss/PrefabScanReport.txt>\n" +
                    "Avoid clicking repeatedly; wait for status to show Done." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Prefab scan status" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "Shows scan state: Idle / Running... / Done / Failed.\n" +
                    "When Running, shows elapsed time; when Done, shows finish time and duration." },


                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Verbose debug logs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "Enabled = sends many extra details to DispatchBoss.log.\n" +
                    "Useful for troubleshooting.\n" +
                    "**Disable** for normal gameplay.\n" +
                    "<If you do not know what this is,>\n" +
                    "**leave it OFF**, and do not check the box.\n " +
                    "<Log spam affects performance.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Open Log folder" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Open DispatchBoss.log file with your text editor (Notepad++ recommended." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "Open Scan Report folder " },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "Open PrefabScanReport.txt file with your text editor (Notepad++ recommended." },

            };
        }

        public void Unload()
        {
        }
    }
}
