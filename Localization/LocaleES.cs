// File: Localization/LocaleES.cs
// Spanish (es-ES) strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

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
                { m_Setting.GetOptionTabLocaleID(Setting.PublicTransitTab), "Transporte público" },
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "Industria" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "Parques-Carreteras" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "Acerca de" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "Líneas (límites del deslizador)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "Ampliar límites del deslizador de línea" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "Permite bajar el deslizador hasta **1 vehículo** en muchas rutas.\n" +
                    "**El máximo también puede subir** (según la lógica de tiempo de ruta).\n" +
                    "El juego usa el tiempo de ruta (conducción + paradas), así que el max varía por línea.\n" +
                    "<Evita conflictos: quita mods que toquen la misma política de línea>.\n" +
                    "Si usas otro mod de políticas, deja esto OFF.\n" +
                    "Funciona para: bus, tranvía, tren, metro, barco, ferry, avión.\n" +
                    "Tip: si quieres más vehículos, añade paradas — el juego suele subir el máximo."
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacidad de depósitos (max vehículos por edificio)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Depósito de buses" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Cambia cuántos buses puede mantener/generar cada **Depósito de buses**.\n" +
                    "**100%** = vanilla.\n" +
                    "**1000%** = 10×.\n" +
                    "Aplica a la capacidad base del edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Depósito de taxis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Cuántos taxis puede mantener cada **Depósito de taxis**.\n" +
                    "Solo afecta al edificio base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Depósito de tranvías" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Cambia cuántos tranvías puede mantener cada **Depósito de tranvías**.\n" +
                    "Aplica a la capacidad base del edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Depósito de trenes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Cambia cuántos trenes puede mantener cada **Depósito de trenes**.\n" +
                    "Aplica a la capacidad base del edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Depósito de metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Cambia cuántos vehículos puede mantener cada **Depósito de metro**.\n" +
                    "Aplica a la capacidad base del edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Restablecer todos los depósitos" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Vuelve todos los deslizadores de depósito a **100%** (vanilla)." },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacidad de pasajeros (max por vehículo)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Cambia la capacidad de pasajeros del **bus**.\n" +
                    "**10%** = 10% de asientos.\n" +
                    "**100%** = vanilla.\n" +
                    "**1000%** = 10× asientos." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Tranvía" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Cambia la capacidad del **tranvía**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Tren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Cambia la capacidad del **tren**.\n" +
                    "Aplica a locomotoras y vagones." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Cambia la capacidad del **metro**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Barco" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Cambia la capacidad del **barco de pasajeros** (no carga).\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Ferry" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Cambia la capacidad del **ferry**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Avión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Cambia la capacidad del **avión**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "Duplicar" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "Pone todos los deslizadores de pasajeros en **200%**.\n" +
                    "Aplica a bus, tranvía, tren, metro, barco, ferry y avión." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "Restablecer todos los pasajeros" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Vuelve todos los deslizadores de pasajeros a **100%** (vanilla)." },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "Vehículos de reparto (capacidad de carga)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "Camiones (tráiler)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "Capacidad de **tráilers**.\n" +
                    "Incluye tráilers de industrias especiales (granjas, forestal, pesca, etc.).\n" +
                    "Multiplicador: **1×** = vanilla (**25t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "Furgonetas" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**Furgonetas de reparto**\n" +
                    "Multiplicador: **1×** = vanilla (**4t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "Materias (petróleo/carbón/mena/piedra)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**Camiones de materias** (petróleo/carbón/mena/piedra)\n" +
                    "Multiplicador: **1×** = vanilla (**20t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "Moto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**Reparto en moto**\n" +
                    "Multiplicador: **1×** = vanilla, **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "Restablecer reparto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "Vuelve los multiplicadores de reparto a **1×** (vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "Flota de carga (puerto/tren/aeropuerto)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "Max flota de estación de carga" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "Multiplicador del máximo de transportistas activos en **estaciones de carga**.\n" +
                    "**1×** = vanilla, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "Flota de extractores" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "Multiplicador del **max camiones** de extractores\n" +
                    "(granjas, forestal, pesca, mena, petróleo + carbón/piedra si existe).\n" +
                    "**1×** = vanilla, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "Restablecer flota carga + extractores" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "Vuelve carga + extractores a **1×** (vanilla)." },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "Mantenimiento de parques" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "Capacidad de turno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "Multiplicador de **capacidad de turno** (capacidad del vehículo).\n" +
                    "Trabajo total antes de volver al edificio.\n" +
                    "Más = más tiempo fuera." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "Velocidad de trabajo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "Multiplicador de **velocidad de trabajo**.\n" +
                    "Velocidad = trabajo por tick cuando está parado." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "Tamaño de flota" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "Multiplicador del **max vehículos** del edificio.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "Restablecer parques" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "Restablece a **100%** (vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "Mantenimiento de carreteras" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "Tamaño de flota" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "Multiplicador de **max vehículos** por depósito.\n" +
                    "Más alto = más camiones.\n" +
                    "<Nota: muy pocos o demasiados puede empeorar el tráfico.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "Capacidad de turno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "Multiplicador de **capacidad de turno**.\n" +
                    "Trabajo total antes de volver al depósito.\n" +
                    "Más alto = menos regresos." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "Tasa de reparación (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "Tasa = trabajo por tick cuando está parado.\n" +
                    "En vanilla, reparar puede requerir varias paradas.\n" +
                    "<Alpha: todavía en pruebas.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "Desgaste de carreteras (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Función alpha: en pruebas>\n" +
                    "Qué tan rápido se acumula el desgaste con el tiempo.\n" +
                    "**100%** = vanilla\n" +
                    "**10%** = 10× más lento (menos reparaciones)\n" +
                    "**400%** = 4× más rápido (más reparaciones)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "Restablecer carreteras" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "Restablece todo a **100%** (vanilla)." },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "Enlaces" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "Depurar / Logs" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nombre mostrado del mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Versión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Versión actual del mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Abrir la página del autor en Paradox Mods." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "Abrir el Discord en el navegador." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "Informe de scan (prefabs)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "Informe único: prefabs relevantes + valores de desgaste del carril.\n" +
                    "Archivo: <ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "No hagas spam; espera a que el estado muestre Done." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Estado del scan de prefabs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "Muestra el estado: Inactivo / En cola / En ejecución / Listo / Error.\n" +
                    "En cola/En ejecución muestra el tiempo; Listo muestra duración + hora de fin." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Logs detallados" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "ON = envía más detalles a DispatchBoss.log.\n" +
                    "Útil para solucionar problemas.\n" +
                    "Para jugar normal, **déjalo OFF**.\n" +
                    "<Si no sabes qué es esto,>\n" +
                    "**déjalo OFF**.\n" +
                    "<El spam de logs afecta el rendimiento.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Abrir carpeta de logs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Abre la carpeta de logs.\n" +
                    "Luego abre <DispatchBoss.log> (recomendado Notepad++)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "Abrir carpeta de informes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "Abre la carpeta de informes.\n" +
                    "Luego abre <ScanReport-Prefabs.txt>." },

                // ---- Status Text (format string templates) ----
                { "DB_SCAN_IDLE", "Inactivo" },
                { "DB_SCAN_QUEUED_FMT", "En cola ({0})" },
                { "DB_SCAN_RUNNING_FMT", "En ejecución ({0})" },
                { "DB_SCAN_DONE_FMT", "Listo ({0} | {1})" },
                { "DB_SCAN_FAILED", "Error" },
                { "DB_SCAN_FAIL_NO_CITY", "CARGA UNA CIUDAD PRIMERO." },
                { "DB_SCAN_UNKNOWN_TIME", "hora desconocida" },
            };
        }

        public void Unload()
        {
        }
    }
}
