// File: Localization/LocaleDE.cs
// German (de-DE) strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

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
                { m_Setting.GetOptionTabLocaleID(Setting.PublicTransitTab), "ÖPNV" },
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "Industrie" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "Parks-Straßen" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "Info" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "Linien (Fahrzeug-Limits im Spiel)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "Linien-Slider-Limits erweitern" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "Erlaubt beim Linien-Slider **bis runter auf 1 Fahrzeug** (bei den meisten Routen).\n" +
                    "**Max kann auch steigen** (folgt weiterhin der Spiel-Logik).\n" +
                    "Das Spiel nutzt Routenzeit (Fahrzeit + Halte), daher variiert das Maximum pro Linie.\n" +
                    "<Konflikte vermeiden: entferne Mods, die dieselbe Linien-Policy ändern>.\n" +
                    "Wenn du ein anderes Policy-Mod nutzt, lass diese Box AUS.\n" +
                    "Gilt für: Bus, Tram, Zug, U-Bahn, Schiff, Fähre, Flugzeug.\n" +
                    "Tipp: Wenn du noch mehr willst als das Standard-Max, füge einfach ein paar Halte hinzu." +
                    " Das Spiel erhöht dann oft automatisch das Max."
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Depot-Kapazität (max Fahrzeuge pro Depot)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Busdepot" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Ändert, wie viele Busse jedes **Busdepot** betreiben/spawnen kann.\n" +
                    "**100%** = Vanilla (Standard).\n" +
                    "**1000%** = 10× mehr.\n" +
                    "Gilt für die Basis-Kapazität des Gebäudes." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Taxidepot" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Wie viele Taxis jedes **Taxidepot** betreiben kann.\n" +
                    "Erhöhung gilt nur fürs Basis-Gebäude." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Tramdepot" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Ändert, wie viele Trams jedes **Tramdepot** betreiben kann.\n" +
                    "Gilt für die Basis-Kapazität des Gebäudes." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Zugdepot" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Ändert, wie viele Züge jedes **Zugdepot** betreiben kann.\n" +
                    "Gilt für die Basis-Kapazität des Gebäudes." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "U-Bahn-Depot" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Ändert, wie viele U-Bahn-Fahrzeuge jedes **U-Bahn-Depot** betreiben kann.\n" +
                    "Gilt für die Basis-Kapazität des Gebäudes." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Alle Depots zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Setzt alle Depot-Slider auf **100%** (Standard / Vanilla)." },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Passagier-Kapazität (max Personen pro Fahrzeug)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Ändert die **Bus-Passagier**-Kapazität.\n" +
                    "**10%** = 10% der Vanilla-Sitze.\n" +
                    "**100%** = Vanilla (Standard).\n" +
                    "**1000%** = 10× mehr Sitze." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Ändert die **Tram-Passagier**-Kapazität.\n" +
                    "**100%** = Vanilla (Standard)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Zug" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Ändert die **Zug-Passagier**-Kapazität.\n" +
                    "Gilt für Lok und Waggons." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "U-Bahn" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Ändert die **U-Bahn-Passagier**-Kapazität.\n" +
                    "**100%** = Vanilla (Standard)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Schiff" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Ändert die **Passagierschiff**-Kapazität (nicht Frachtschiffe).\n" +
                    "**100%** = Vanilla (Standard)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Fähre" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Ändert die **Fähren-Passagier**-Kapazität.\n" +
                    "**100%** = Vanilla (Standard)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Flugzeug" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Ändert die **Flugzeug-Passagier**-Kapazität.\n" +
                    "**100%** = Vanilla (Standard)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "Verdoppeln" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "Setzt alle Passagier-Slider auf **200%**.\n" +
                    "Gilt für Bus, Tram, Zug, U-Bahn, Schiff, Fähre, Flugzeug." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "Alle Passagiere zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Setzt alle Passagier-Slider auf **100%** (Standard / Vanilla)." },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "Lieferfahrzeuge (Frachtkapazität)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "Sattelschlepper" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "**Sattelschlepper** Kapazität.\n" +
                    "Inkl. Spezial-Semis (Farm, Forst, Fischerei usw.).\n" +
                    "Multiplikator: **1×** = Vanilla (**25t**), **10×** = 10× mehr." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "Lieferwagen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**Lieferwagen**\n" +
                    "Multiplikator: **1×** = Vanilla (**4t**), **10×** = 10× mehr." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "Rohstoff-Laster (Öl, Kohle, Erz, Stein)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**Rohstoff-Laster** (Öl/Kohle/Erz/Stein)\n" +
                    "Multiplikator: **1×** = Vanilla (**20t**), **10×** = 10× mehr." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "Motorrad" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**Motorrad-Lieferung**\n" +
                    "Multiplikator: **1×** = Vanilla, **10×** = 10× mehr." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "Lieferung zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "Setzt Liefer-Multiplikatoren auf **1×** (Standard / Vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "Frachtflotte (Hafen, Zug, Flughafen)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "Frachtstation max. Flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "Multiplikator für **Frachtstationen** max. aktive Transporter.\n" +
                    "**1×** = Vanilla, **5×** = 5× mehr." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "Extraktor-Flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "Multiplikator für **Extraktoren max. Laster**\n" +
                    "(Farm, Forst, Fischerei, Erz, Öl, plus Kohle/Stein wenn verfügbar).\n" +
                    "**1×** = Vanilla, **5×** = 5× mehr." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "Fracht + Extraktoren zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "Setzt Frachtstation + Extraktor-Multiplikator auf **1×** (Standard / Vanilla)." },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "Parkwartung" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "Schicht-Kapazität" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "Multiplikator für **Schicht-Kapazität** (Fahrzeugkapazität).\n" +
                    "Gesamtarbeit, bevor es zurückfährt.\n" +
                    "Mehr Vorräte = länger draußen." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "Fahrzeug-Rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "Multiplikator für **Arbeitsrate**.\n" +
                    "Rate = Arbeit pro Sim-Tick, wenn es steht." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "Depot-Flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "Multiplikator für **max Fahrzeuge** des Depots.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "Parkwartung zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "Setzt alles auf **100%** (Standard / Vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "Straßenwartung" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "Depot-Flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "Multiplikator für **max Fahrzeuge** pro Depot.\n" +
                    "Höher = mehr Laster.\n" +
                    "<Balance: zu wenig oder zu viel kann Verkehr verschlimmern.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "Schicht-Kapazität" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "Multiplikator für **Schicht-Kapazität**.\n" +
                    "Gesamtarbeit, bevor es zurückfährt.\n" +
                    "Höher = weniger Rückfahrten." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "Reparaturrate (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "Rate = Arbeit pro Sim-Tick, wenn es steht.\n" +
                    "In Vanilla brauchen Reparaturen teils mehrere Stopps.\n" +
                    "<Alpha: noch am Testen in echten Städten.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "Straßenverschleiß (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Alpha: noch am Testen>\n" +
                    "Wie schnell Straßen über Zeit verschleißen.\n" +
                    "**100%** = Vanilla\n" +
                    "**10%** = 10× langsamer (weniger Reparaturen)\n" +
                    "**400%** = 4× schneller (mehr Reparaturen)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "Straßenwartung zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "Setzt alles auf **100%** (Standard / Vanilla)." },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "Links" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "Debug / Logs" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Anzeigename des Mods." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Aktuelle Mod-Version." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods-Seite des Autors öffnen." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "Community-Discord im Browser öffnen." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "Scan-Report (Prefabs)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "Einmaliger Report: relevante Prefabs + Lane-Wear-Werte.\n" +
                    "Datei: <ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "Nicht spammen; warte bis Status „Done“ zeigt." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Prefab-Scan-Status" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "Zeigt: Idle / Queued / Running / Done / Failed.\n" +
                    "Queued/Running = Zeit; Done = Dauer + Uhrzeit." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Ausführliche Debug-Logs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "An = extra Details in DispatchBoss.log.\n" +
                    "Gut fürs Troubleshooting.\n" +
                    "**Für normales Spielen AUS**.\n" +
                    "<Wenn du nicht weißt was das ist,>\n" +
                    "**lass es AUS**.\n" +
                    "<Zu viele Logs können Leistung kosten.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Log-Ordner öffnen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Öffnet den Logs-Ordner.\n" +
                    "Dann <DispatchBoss.log> öffnen (Notepad++ empfohlen)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "Report-Ordner öffnen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "Öffnet den Report-Ordner.\n" +
                    "Dann <ScanReport-Prefabs.txt> öffnen." },

                // ---- Status Text (format string templates) ----
                { "DB_SCAN_IDLE", "Leerlauf" },
                { "DB_SCAN_QUEUED_FMT", "Warteschlange ({0})" },
                { "DB_SCAN_RUNNING_FMT", "Läuft ({0})" },
                { "DB_SCAN_DONE_FMT", "Fertig ({0} | {1})" },
                { "DB_SCAN_FAILED", "Fehler" },
                { "DB_SCAN_FAIL_NO_CITY", "STADT LADEN ZUERST" },
                { "DB_SCAN_UNKNOWN_TIME", "unbekannte Zeit" },
            };
        }

        public void Unload()
        {
        }
    }
}
