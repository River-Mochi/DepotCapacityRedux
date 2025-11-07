// LocaleDE.cs
// German (de-DE) for Options UI.

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
                // ---- MOD TITLE / TAB / GROUPS ----
                { m_Setting.GetSettingsLocaleID(), "Adjust Transit Capacity" },
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Hauptmenü" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Depotkapazität (max. Fahrzeuge pro Depot)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Passagierkapazität (Fahrgäste pro Fahrzeug)" },

                // ---- DEPOT LABELS & DESCRIPTIONS ----
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Busdepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotPercent)), "Wie viele Busse jedes Busdepot warten/spawnen kann. 100 % = Standard, 1000 % = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Taxidepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotPercent)), "Wie viele Taxis jedes Taxidepot warten kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Straßenbahndepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotPercent)), "Wie viele Straßenbahnen jedes Depot warten kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Zugdepots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotPercent)), "Wie viele Züge jedes Depot warten kann." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "U-Bahn-Depots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotPercent)), "Wie viele U-Bahn-Fahrzeuge jedes Depot warten kann." },

                // ---- PASSENGER LABELS & DESCRIPTIONS ----
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Fahrgäste – Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerPercent)), "Multiplikator für Sitzplätze in Bussen. 100 % = Standard, 1000 % = 10× Sitze." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Fahrgäste – Straßenbahn" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerPercent)), "Multiplikator für Sitzplätze in Straßenbahnen." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Fahrgäste – Zug" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerPercent)), "Multiplikator für Sitzplätze in Zügen." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Fahrgäste – U-Bahn" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerPercent)), "Multiplikator für Sitzplätze in U-Bahnen." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Fahrgäste – Schiff" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerPercent)), "Multiplikator für Passagierschiffe (keine Frachtschiffe)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Fahrgäste – Fähre" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerPercent)), "Multiplikator für Fähren." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Fahrgäste – Flugzeug" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerPercent)), "Multiplikator für Passagierflugzeuge." },
            };
        }

        public void Unload()
        {
        }
    }
}
