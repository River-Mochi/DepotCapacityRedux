// LocaleES.cs
// Spanish (es-ES) for Options UI.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal;

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
                // ---- MOD TITLE / TAB / GROUPS ----
                { m_Setting.GetSettingsLocaleID(), "Adjust Transit Capacity" },
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "Principal" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacidad del depósito (vehículos máximos)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacidad de pasajeros (viajeros por vehículo)" },

                // ---- DEPOT LABELS & DESCRIPTIONS ----
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "Depósitos de autobuses" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotPercent)), "Cuántos autobuses puede mantener/generar cada depósito de autobuses. 100 % = juego base, 1000 % = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "Depósitos de taxis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotPercent)), "Cuántos taxis puede mantener cada depósito de taxis." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "Depósitos de tranvías" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotPercent)), "Cuántos tranvías puede mantener cada depósito de tranvías." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "Depósitos de trenes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotPercent)), "Cuántos trenes puede mantener cada depósito de trenes." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "Depósitos de metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotPercent)), "Cuántos vehículos de metro puede mantener cada depósito." },

                // ---- PASSENGER LABELS & DESCRIPTIONS ----
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "Pasajeros – autobús" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerPercent)), "Multiplicador para los asientos de los autobuses. 100 % = capacidad base, 1000 % = 10× asientos." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "Pasajeros – tranvía" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerPercent)), "Multiplicador para los asientos de los tranvías." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "Pasajeros – tren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerPercent)), "Multiplicador para los asientos de los trenes." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "Pasajeros – metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerPercent)), "Multiplicador para los asientos del metro." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "Pasajeros – barco" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerPercent)), "Multiplicador para barcos de pasajeros (no de carga)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "Pasajeros – ferry" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerPercent)), "Multiplicador para ferris." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "Pasajeros – avión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerPercent)), "Multiplicador para aviones de pasajeros." },
            };
        }

        public void Unload()
        {
        }
    }
}
