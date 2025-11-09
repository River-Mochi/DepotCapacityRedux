// Localization/LocaleDE.cs
// German (de-DE) strings for Options UI.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal;

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
                // Mod Title / Tabs / Groups
                { m_Setting.GetSettingsLocaleID(), "ÖPNV-Kapazität anpassen [ATC]" },

                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Aktionen" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),   "Info"     },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup),
                    "Depotkapazität (max. Fahrzeuge pro Depot)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup),
                    "Passagierkapazität (max. Personen pro Fahrzeug)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup),
                    "Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup),
                    "Support-Links" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup),
                    "Debug / Protokollierung" },
                { m_Setting.GetOptionGroupLocaleID(Setting.LogGroup),
                    "Protokolldatei" },

                // DEPOT labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Busdepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Wie viele Busse jedes **Busdepot** warten/erzeugen kann.\n" +
                    "Verwende einen Multiplikator zwischen **1,0×** (Vanilla) und **10,0×**.\n" +
                    "Multipliziert das **Basisgebäude**, nicht Erweiterungen." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Taxidepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Wie viele Taxis jedes **Taxidepot** warten kann.\n" +
                    "Erhöht nur das Basisgebäude." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Straßenbahndepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Wie viele Straßenbahnen jedes **Straßenbahndepot** warten kann.\n" +
                    "Erhöht nur das Basisgebäude." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Zugdepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Wie viele Züge jedes **Zugdepot** warten kann.\n" +
                    "Erhöht nur das Basisgebäude." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "U-Bahn-Depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Wie viele **U-Bahn-Fahrzeuge** jedes Depot warten kann.\n" +
                    "Erhöht nur das Basisgebäude." },

                // Depot reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Alle Depots zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Setzt alle Depot-Multiplikatoren auf **1,0×** (Standardkapazität des Spiels – Vanilla) zurück." },

                // Passenger labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Buspassagiere" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "**Bus-Passagierplätze** ändern.\n" +
                    "**1,0×** = Vanilla-Sitzplätze, **10,0×** = zehnmal so viele Sitzplätze." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Straßenbahnpassagiere" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "**Straßenbahn-Passagierplätze** ändern." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Zugpassagiere" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "**Zug-Passagierplätze** ändern." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "U-Bahn-Passagiere" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "**U-Bahn-Passagierplätze** ändern." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Schiffspassagiere" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Nur **Passagierschiffe** ändern (keine Frachtschiffe)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Fährpassagiere" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "**Fähr-Passagierplätze** ändern." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Flugzeugpassagiere" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "**Flugzeug-Passagierplätze** ändern." },

                // Passenger reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Alle Passagiere zurücksetzen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Setzt alle Passagier-Multiplikatoren auf **1,0×** (Standardkapazität des Spiels – Vanilla) zurück." },

                // About tab: info
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)),    "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)),     "Anzeigename dieses Mods." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)),  "Aktuelle Mod-Version." },

                // About tab: links
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Paradox-Mods-Website für diesen Mod öffnen." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)),
                    "Die Community-Discord-Seite im Browser öffnen." },

                // About tab: debug
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Ausführliche Debug-Protokollierung aktivieren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "Wenn aktiviert, werden viele zusätzliche Debug-Details in AdjustTransitCapacity.log geschrieben.\n" +
                    "Nützlich für Fehleranalyse, erzeugt aber viele Logeinträge.\n" +
                    "Empfehlung: **Deaktivieren** für normales Spielen.\n" +
                    "<Wenn du nicht weißt, wofür das ist, lass es deaktiviert und aktiviere> \n" +
                    "<das Kontrollkästchen nicht.>"
                },

                // About tab: log button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Protokoll öffnen" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Die ATC-Protokolldatei im Standard-Texteditor öffnen." },
            };
        }

        public void Unload()
        {
        }
    }
}
