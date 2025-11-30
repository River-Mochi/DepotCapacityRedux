// Localization/LocalePL.cs
// Polish (pl-PL) strings for Options UI.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal;

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
            return new Dictionary<string, string>
            {
                // Mod Title / Tabs / Groups
                { m_Setting.GetSettingsLocaleID(), "Dostosuj pojemność transportu [ATC]" },

                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Akcje" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),   "O modzie" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup),
                    "Pojemność zajezdni (maks. pojazdów na zajezdnię)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup),
                    "Pojemność pasażerów (maks. osób na pojazd)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup),  "Informacje" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "Linki wsparcia" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup),      "Debugowanie / logi" },
                { m_Setting.GetOptionGroupLocaleID(Setting.LogGroup),        "Plik logu" },

                // DEPOT labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Zajezdnie autobusowe" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Ile autobusów może utrzymywać / wysyłać każda zajezdnia **Bus Depot**.\n" +
                    "Użyj mnożnika między **1.0×** (vanilla) a **10.0×**.\n" +
                    "Mnoży **bazowy budynek**, nie rozszerzenia." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Zajezdnie taxi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Ile taksówek może utrzymywać każda **zajezdnia taxi**.\n" +
                    "Zwiększenie działa tylko na budynek bazowy." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Zajezdnie tramwajowe" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Ile tramwajów może utrzymywać każda **zajezdnia tramwajowa**.\n" +
                    "Zwiększenie działa tylko na budynek bazowy." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Zajezdnie kolejowe" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Ile pociągów może utrzymywać każda **zajezdnia kolejowa**.\n" +
                    "Zwiększenie działa tylko na budynek bazowy." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Zajezdnie metra" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Ile pojazdów metra może utrzymywać każda **zajezdnia metra**.\n" +
                    "Zwiększenie działa tylko na budynek bazowy." },

                // Depot reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Resetuj wszystkie zajezdnie" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Ustaw wszystkie zajezdnie z powrotem na **1.0×** (domyślna pojemność gry - vanilla)." },

                // PASSENGER labels & descriptions (0.1–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Pasażerowie autobusów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Zmień pojemność pasażerów **autobusów**.\n" +
                    "**0.1×** = 10% domyślnej liczby miejsc (zmniejszenie).\n" +
                    "**1.0×** = domyślna liczba miejsc (vanilla).\n" +
                    "**10.0×** = dziesięć razy więcej miejsc (zwiększenie)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Pasażerowie tramwajów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Zmień maksymalną liczbę pasażerów **tramwajów**.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Pasażerowie pociągów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Zmień liczbę miejsc pasażerskich w **pociągach** – lokomotywy i wagony.\n" +
                    "Wszystkie prefabrykaty typu **Train** są modyfikowane razem." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Pasażerowie metra" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Zmień maksymalną liczbę pasażerów **metra**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Pasażerowie statków" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Zmień pojemność **statków pasażerskich** (nie statków cargo)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Pasażerowie promów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Zmień maksymalną liczbę pasażerów **promów**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Pasażerowie samolotów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Zmień maksymalną liczbę pasażerów **samolotów**." },

                // Passenger convenience + reset buttons
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "Podwój wszystko" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "Ustaw każdy mnożnik pasażerów na **2.0×** (200%).\n" +
                    "Dotyczy autobusów, tramwajów, pociągów, metra, statków, promów i samolotów." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "Resetuj wszystkich pasażerów" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Ustaw wszystkie mnożniki pasażerów z powrotem na **1.0×** (domyślna pojemność gry - vanilla)." },

                // About tab: info
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)),    "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)),     "Wyświetlana nazwa tego modu." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Wersja" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)),  "Aktualna wersja modu." },

                // About tab: links
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Otwórz stronę Paradox Mods z modami autora." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)),
                    "Otwórz społecznościowy Discord w przeglądarce." },

                // About tab: debug
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Włącz szczegółowe logowanie debugowania" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "Włączone = wysyła wiele dodatkowych szczegółowych logów do AdjustTransitCapacity.log.\n" +
                    "Przydatne przy szukaniu błędów, ale szybko zapycha log.\n" +
                    "**Wyłącz** dla normalnej rozgrywki.\n" +
                    "<Jeśli nie wiesz, co to jest, zostaw to na **OFF** i> \n" +
                    "<nie zaznaczaj pola, bo spam logu pogarsza wydajność.>"
                },

                // About tab: log button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Otwórz log" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Otwórz plik logu ATC w domyślnym edytorze tekstu." },
            };
        }

        public void Unload()
        {
        }
    }
}
