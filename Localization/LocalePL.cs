// File: Localization/LocalePL.cs
// Polish (pl-PL) strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

    public sealed class LocalePL : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocalePL(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.PublicTransitTab), "Transport publiczny" },
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "Przemysł" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "Parki-Drogi" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "O modzie" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "Linie (limity suwaka pojazdów)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "Rozszerz limity suwaka na linii" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "Pozwala zejść suwakiem nawet do **1 pojazdu** na wielu liniach.\n" +
                    "**Maksimum też może wzrosnąć** (zgodnie z logiką czasu trasy).\n" +
                    "Gra liczy czas trasy (jazda + liczba przystanków), więc max różni się między liniami.\n" +
                    "<Unikaj konfliktów: usuń mody, które zmieniają tę samą politykę linii>.\n" +
                    "Jeśli używasz innego moda od polityk, zostaw to OFF.\n" +
                    "Działa dla: autobus, tramwaj, pociąg, metro, statek, prom, samolot.\n" +
                    "Tip: chcesz więcej pojazdów? dodaj kilka przystanków — gra często podniesie maksimum."
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Pojemność zajezdni (max pojazdów na budynek)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Zajezdnia autobusowa" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Zmienia ile autobusów może utrzymać/wyjechać z **Zajezdni autobusowej**.\n" +
                    "**100%** = vanilla.\n" +
                    "**1000%** = 10×.\n" +
                    "Dotyczy bazowej pojemności budynku." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Zajezdnia taksówek" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Ile taksówek może utrzymać **Zajezdnia taksówek**.\n" +
                    "Zmiana dotyczy tylko bazowego budynku." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Zajezdnia tramwajowa" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Zmienia ile tramwajów może utrzymać **Zajezdnia tramwajowa**.\n" +
                    "Dotyczy bazowej pojemności budynku." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Zajezdnia kolejowa" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Zmienia ile pociągów może utrzymać **Zajezdnia kolejowa**.\n" +
                    "Dotyczy bazowej pojemności budynku." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Zajezdnia metra" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Zmienia ile składów może utrzymać **Zajezdnia metra**.\n" +
                    "Dotyczy bazowej pojemności budynku." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Resetuj wszystkie zajezdnie" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Ustaw wszystkie suwaki zajezdni na **100%** (vanilla)." },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Pojemność pasażerów (max osób na pojazd)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Autobus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Zmienia pojemność pasażerów **autobusu**.\n" +
                    "**10%** = 10% miejsc.\n" +
                    "**100%** = vanilla.\n" +
                    "**1000%** = 10× miejsc." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Tramwaj" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Zmienia pojemność pasażerów **tramwaju**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Pociąg" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Zmienia pojemność pasażerów **pociągu**.\n" +
                    "Dotyczy lokomotyw i wagonów." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Zmienia pojemność pasażerów **metra**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Statek" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Zmienia pojemność **statku pasażerskiego** (nie cargo).\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Prom" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Zmienia pojemność pasażerów **promu**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Samolot" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Zmienia pojemność pasażerów **samolotu**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "Podwójnie" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "Ustaw wszystkie suwaki pasażerów na **200%**.\n" +
                    "Dotyczy: autobus, tramwaj, pociąg, metro, statek, prom, samolot." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "Resetuj wszystkich pasażerów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Ustaw wszystkie suwaki pasażerów na **100%** (vanilla)." },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "Pojazdy dostawcze (ładowność)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "Ciężarówki (semi)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "Ładowność **semi**.\n" +
                    "Zawiera semi z branż specjalnych (farmy, lasy, rybołówstwo itd.).\n" +
                    "Mnożnik: **1×** = vanilla (**25t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "Vany dostawcze" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**Vany dostawcze**\n" +
                    "Mnożnik: **1×** = vanilla (**4t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "Surowce (ropa/węgiel/ruda/kamień)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**Ciężarówki surowców** (ropa/węgiel/ruda/kamień)\n" +
                    "Mnożnik: **1×** = vanilla (**20t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "Motocykl" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**Dostawy motocyklem**\n" +
                    "Mnożnik: **1×** = vanilla, **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "Reset dostaw" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "Ustaw mnożniki dostaw na **1×** (vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "Flota cargo (port/kolej/lotnisko)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "Max flota stacji cargo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "Mnożnik dla maks. aktywnych transporterów w **stacjach cargo**.\n" +
                    "**1×** = vanilla, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "Flota wydobycia" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "Mnożnik dla **max ciężarówek** w ekstraktorach\n" +
                    "(farmy, lasy, rybołówstwo, ruda, ropa + węgiel/kamień jeśli są).\n" +
                    "**1×** = vanilla, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "Reset floty cargo + wydobycia" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "Ustaw mnożniki cargo + wydobycia na **1×** (vanilla)." },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "Utrzymanie parków" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "Pojemność zmiany" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "Mnożnik **pojemności zmiany** (pojemność pojazdu).\n" +
                    "Ile pracy zrobi zanim wróci do budynku.\n" +
                    "Więcej = dłużej w terenie." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "Tempo pracy" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "Mnożnik **tempa pracy**.\n" +
                    "Tempo = praca na tick, gdy stoi." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "Rozmiar floty" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "Mnożnik **max pojazdów** dla budynku.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "Reset utrzymania parków" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "Reset do **100%** (vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "Utrzymanie dróg" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "Rozmiar floty" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "Mnożnik **max pojazdów** w bazie.\n" +
                    "Więcej = więcej ciężarówek.\n" +
                    "<Uwaga: za mało lub za dużo może pogorszyć ruch.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "Pojemność zmiany" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "Mnożnik **pojemności zmiany**.\n" +
                    "Ile pracy zrobi zanim wróci do bazy.\n" +
                    "Więcej = mniej powrotów." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "Tempo napraw (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "Tempo = praca na tick, gdy stoi.\n" +
                    "W vanilla naprawy mogą wymagać kilku postojów.\n" +
                    "<Alpha: w trakcie testów.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "Szybkość zużycia dróg (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Funkcja alpha: w trakcie testów>\n" +
                    "Jak szybko drogi zużywają się z czasem.\n" +
                    "**100%** = vanilla\n" +
                    "**10%** = 10× wolniej (mniej napraw)\n" +
                    "**400%** = 4× szybciej (więcej napraw)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "Reset utrzymania dróg" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "Reset do **100%** (vanilla)." },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "Linki" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "Debug / Logi" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nazwa moda." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Wersja" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Aktualna wersja moda." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Otwórz stronę autora na Paradox Mods." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "Otwórz Discord w przeglądarce." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "Raport skanu (prefaby)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "Jednorazowy raport: ważne prefaby + wartości zużycia pasa.\n" +
                    "Plik: <ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "Nie klikaj spamem; poczekaj aż status pokaże Done." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Status skanu prefabów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "Pokazuje stan: Bezczynny / W kolejce / W trakcie / Gotowe / Błąd.\n" +
                    "W kolejce/W trakcie pokazuje czas; Gotowe pokazuje czas trwania + godzinę zakończenia." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Szczegółowe logi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "ON = więcej szczegółów do DispatchBoss.log.\n" +
                    "Przydatne do diagnozy.\n" +
                    "Do normalnej gry **wyłącz**.\n" +
                    "<Jeśli nie wiesz co to,>\n" +
                    "**zostaw OFF**.\n" +
                    "<Spam w logach obciąża wydajność.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Otwórz folder logów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Otwórz folder logów.\n" +
                    "Potem otwórz <DispatchBoss.log> (polecam Notepad++)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "Otwórz folder raportów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "Otwórz folder raportów.\n" +
                    "Potem otwórz <ScanReport-Prefabs.txt>." },

                // ---- Status Text (format string templates) ----
                { "DB_SCAN_IDLE", "Bezczynny" },
                { "DB_SCAN_QUEUED_FMT", "W kolejce ({0})" },
                { "DB_SCAN_RUNNING_FMT", "W trakcie ({0})" },
                { "DB_SCAN_DONE_FMT", "Gotowe ({0} | {1})" },
                { "DB_SCAN_FAILED", "Błąd" },
                { "DB_SCAN_FAIL_NO_CITY", "NAJPIERW WCZYTAJ MIASTO." },
                { "DB_SCAN_UNKNOWN_TIME", "nieznany czas" },
            };
        }

        public void Unload()
        {
        }
    }
}
