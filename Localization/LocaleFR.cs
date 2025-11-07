// LocaleFR.cs
// French (fr-FR) for Options UI.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal;

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
                // ---- MOD TITLE / TAB / GROUPS ----
                { m_Setting.GetSettingsLocaleID(), "Adjust Transit Capacity" },
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Principal" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacité du dépôt (véhicules maximum)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacité passagers (voyageurs par véhicule)" },

                // ---- DEPOT LABELS & DESCRIPTIONS ----
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Dépôts de bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotPercent)), "Nombre de bus que chaque dépôt de bus peut entretenir / faire apparaître. 100 % = jeu de base, 1000 % = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Dépôts de taxis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotPercent)), "Nombre de taxis que chaque dépôt de taxis peut entretenir." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Dépôts de tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotPercent)), "Nombre de trams que chaque dépôt de tram peut entretenir." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Dépôts de trains" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotPercent)), "Nombre de trains que chaque dépôt de trains peut entretenir." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "Dépôts de métro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotPercent)), "Nombre de rames de métro que chaque dépôt peut entretenir." },

                // ---- PASSENGER LABELS & DESCRIPTIONS ----
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Passagers – bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerPercent)), "Multiplicateur pour les places assises dans les bus. 100 % = capacité de base, 1000 % = 10× plus de sièges." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Passagers – tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerPercent)), "Multiplicateur pour les places assises dans les trams." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Passagers – train" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerPercent)), "Multiplicateur pour les places assises dans les trains." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Passagers – métro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerPercent)), "Multiplicateur pour les places assises dans le métro." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Passagers – navire" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerPercent)), "Multiplicateur pour les navires de passagers uniquement (pas les cargos)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Passagers – ferry" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerPercent)), "Multiplicateur pour les ferries." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Passagers – avion" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerPercent)), "Multiplicateur pour les avions de passagers." },
            };
        }

        public void Unload()
        {
        }
    }
}
