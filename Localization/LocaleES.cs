// Localization/LocaleES.cs
// Spanish (es-ES) strings for Options UI.

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
                // Mod Title / Tabs / Groups
                { m_Setting.GetSettingsLocaleID(), "Ajustar capacidad de transporte [ATC]" },

                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Acciones" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),   "Acerca de" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup),
                    "Capacidad de depósito (vehículos máximos por depósito)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup),
                    "Capacidad de pasajeros (personas máximas por vehículo)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup),
                    "Información" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup),
                    "Enlaces de soporte" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup),
                    "Depuración / Registro" },
                { m_Setting.GetOptionGroupLocaleID(Setting.LogGroup),
                    "Archivo de registro" },

                // DEPOT labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Depósitos de autobuses" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Cuántos autobuses puede mantener/generar cada edificio de **depósito de autobuses**.\n" +
                    "Usa un multiplicador entre **1,0×** (vanilla) y **10,0×**.\n" +
                    "Multiplica el **edificio base**, no las ampliaciones." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Depósitos de taxis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Cuántos taxis puede mantener cada **depósito de taxis**.\n" +
                    "Aumenta solo el edificio base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Depósitos de tranvías" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Cuántos tranvías puede mantener cada **depósito de tranvías**.\n" +
                    "Aumenta solo el edificio base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Depósitos de trenes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Cuántos trenes puede mantener cada **depósito de trenes**.\n" +
                    "Aumenta solo el edificio base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Depósitos de metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Cuántos **vehículos de metro** puede mantener cada depósito.\n" +
                    "Aumenta solo el edificio base." },

                // Depot reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Restablecer todos los depósitos" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Restablece todos los multiplicadores de depósitos a **1,0×** (capacidad predeterminada del juego – vanilla)." },

                // Passenger labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Pasajeros de autobús" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Cambiar los asientos de **pasajeros de autobús**.\n" +
                    "**1,0×** = asientos vanilla, **10,0×** = diez veces más asientos." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Pasajeros de tranvía" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Cambiar los asientos de **pasajeros de tranvía**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Pasajeros de tren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Cambiar los asientos de **pasajeros de tren**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Pasajeros de metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Cambiar los asientos de **pasajeros de metro**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Pasajeros de barco" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Cambiar solo los barcos **de pasajeros** (no los barcos de carga)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Pasajeros de ferry" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Cambiar los asientos de **pasajeros de ferry**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Pasajeros de avión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Cambiar los asientos de **pasajeros de avión**." },

                // Passenger reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Restablecer todos los pasajeros" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Restablece todos los multiplicadores de pasajeros a **1,0×** (capacidad predeterminada del juego – vanilla)." },

                // About tab: info
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)),    "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)),     "Nombre mostrado de este mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Versión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)),  "Versión actual del mod." },

                // About tab: links
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Abrir el sitio de Paradox Mods para este mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)),
                    "Abrir el Discord de la comunidad en el navegador." },

                // About tab: debug
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Activar registro de depuración detallado" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "Cuando está activado, se envían muchos detalles extra de depuración a AdjustTransitCapacity.log.\n" +
                    "Útil para solucionar problemas, pero llenará el registro.\n" +
                    "Se recomienda **Desactivar** para el juego normal.\n" +
                    "<Si no sabes para qué sirve, déjalo desactivado y no > \n" +
                    "marques la casilla.>"
                },

                // About tab: log button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Abrir registro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Abrir el archivo de registro de ATC en el editor de texto predeterminado." },
            };
        }

        public void Unload()
        {
        }
    }
}
