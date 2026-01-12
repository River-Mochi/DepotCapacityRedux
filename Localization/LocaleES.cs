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
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "Parques y Carreteras" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "Acerca de" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "Líneas (límites del deslizador de vehículos)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "Ampliar límites del deslizador de líneas" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "Permite que el deslizador baje **hasta 1 vehículo** en la mayoría de rutas.\n" +
                    "**El máximo también puede subir** (sigue la lógica del juego).\n" +
                    "El juego usa tiempo de ruta (conducción + paradas), así que el máximo varía por línea.\n" +
                    "<Evita conflictos: quita mods que editen la misma política de líneas>.\n" +
                    "Si usas otro mod de políticas, deja esta casilla APAGADA.\n" +
                    "Funciona para: bus, tranvía, tren, metro, barco, ferry, avión."
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacidad de depósitos (máx vehículos por depósito)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Depósito de buses" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Cambia cuántos buses puede mantener/spawn cada **depósito de buses**.\n" +
                    "**100%** = vanilla (por defecto).\n" +
                    "**1000%** = 10× más.\n" +
                    "Se aplica a la capacidad base del edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Depósito de taxis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Cuántos taxis puede mantener cada **depósito de taxis**.\n" +
                    "El aumento se aplica solo al edificio base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Depósito de tranvías" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Cambia cuántos tranvías puede mantener cada **depósito de tranvías**.\n" +
                    "Se aplica a la capacidad base del edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Depósito de trenes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Cambia cuántos trenes puede mantener cada **depósito de trenes**.\n" +
                    "Se aplica a la capacidad base del edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Depósito de metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Cambia cuántos vehículos puede mantener cada **depósito de metro**.\n" +
                    "Se aplica a la capacidad base del edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Reiniciar todos los depósitos" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Devuelve todos los depósitos a **100%** (por defecto / vanilla)." },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacidad de pasajeros (máx por vehículo)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Cambia la capacidad de **pasajeros del bus**.\n" +
                    "**10%** = 10% de asientos vanilla.\n" +
                    "**100%** = asientos vanilla (por defecto).\n" +
                    "**1000%** = 10× más asientos." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Tranvía" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Cambia la capacidad de **pasajeros del tranvía**.\n" +
                    "**100%** = asientos vanilla (por defecto)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Tren" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Cambia la capacidad de **pasajeros del tren**.\n" +
                    "Se aplica a locomotoras y vagones." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Cambia la capacidad de **pasajeros del metro**.\n" +
                    "**100%** = asientos vanilla (por defecto)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Barco" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Cambia la capacidad de **barco de pasajeros** (no carga).\n" +
                    "**100%** = asientos vanilla (por defecto)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Ferry" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Cambia la capacidad de **pasajeros del ferry**.\n" +
                    "**100%** = asientos vanilla (por defecto)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Avión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Cambia la capacidad de **pasajeros del avión**.\n" +
                    "**100%** = asientos vanilla (por defecto)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "Duplicar" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "Pone todos los deslizadores de pasajeros en **200%**.\n" +
                    "Aplica a buses, tranvías, trenes, metros, barcos, ferries y aviones." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "Reiniciar todos los pasajeros" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Devuelve todos los pasajeros a **100%** (por defecto / vanilla)." },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "Vehículos de reparto (capacidad de carga)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "Camiones tráiler" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "Capacidad de **tráilers**.\n" +
                    "Incluye tráilers especializados (granjas, bosque, pesca, etc.).\n" +
                    "Multiplicador: **1×** = vanilla (**25t**), **10×** = 10× más." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "Furgonetas de reparto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**Furgonetas de reparto**\n" +
                    "Multiplicador: **1×** = vanilla (**4t**), **10×** = 10× más." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "Camiones de materias primas (petróleo, carbón, mineral, piedra)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**Camiones de materias primas** (petróleo/carbón/mineral/piedra)\n" +
                    "Multiplicador: **1×** = vanilla (**20t**), **10×** = 10× más." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "Moto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**Reparto en moto**\n" +
                    "Multiplicador: **1×** = vanilla, **10×** = 10× más." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "Reiniciar reparto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "Devuelve los multiplicadores de reparto a **1×** (por defecto / vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "Flota de carga (puerto, tren, aeropuerto)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "Flota máx de estaciones de carga" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "Multiplicador del máximo de transportistas activos en **estaciones de carga**.\n" +
                    "**1×** = vanilla, **5×** = 5× más." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "Flota de extractores" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "Multiplicador del máximo de camiones de **extractores** industriales\n" +
                    "(granjas, bosque, pesca, mineral, petróleo, + carbón/piedra si existe).\n" +
                    "**1×** = vanilla, **5×** = 5× más." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "Reiniciar flota carga + extractores" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "Devuelve los multiplicadores de carga + extractores a **1×** (por defecto / vanilla)." },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "Mantenimiento de parques" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "Capacidad de turno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "Multiplicador de **capacidad de turno** (capacidad del vehículo).\n" +
                    "Trabajo total antes de volver al edificio.\n" +
                    "Piensa: más suministros = aguanta más fuera." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "Ritmo del vehículo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "Multiplicador del **ritmo de trabajo**.\n" +
                    "Ritmo = trabajo por tick cuando está parado." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "Tamaño de flota del depósito" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "Multiplicador del **máx de vehículos** del depósito.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "Reiniciar mantenimiento parques" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "Devuelve todo a **100%** (por defecto / vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "Mantenimiento de carreteras" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "Tamaño de flota del depósito" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "Multiplicador del **máx de vehículos** por depósito.\n" +
                    "Más alto = más camiones.\n" +
                    "<Nota de balance: muy poco o demasiado puede empeorar el tráfico.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "Capacidad de turno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "Multiplicador de **capacidad de turno**.\n" +
                    "Trabajo total antes de volver al depósito.\n" +
                    "Más alto = menos regresos." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "Ritmo de reparación (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "Ritmo = trabajo por tick cuando está parado.\n" +
                    "En vanilla, una reparación puede requerir varias paradas.\n" +
                    "<Alpha: aún probando cómo se siente en ciudades reales.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "Velocidad de desgaste (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Función alpha: aún en prueba>\n" +
                    "Qué tan rápido se desgastan las carreteras con el tiempo.\n" +
                    "**100%** = vanilla\n" +
                    "**10%** = 10× más lento (menos reparaciones)\n" +
                    "**400%** = 4× más rápido (más reparaciones)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "Reiniciar mantenimiento carreteras" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "Devuelve todo a **100%** (por defecto / vanilla)." },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "Enlaces" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "Debug / Logs" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nombre mostrado del mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Versión" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Versión actual del mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Abrir la página de Paradox Mods del autor." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "Abrir el Discord de la comunidad." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "Informe scan (prefabs)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "Informe único: prefabs útiles + valores de desgaste.\n" +
                    "Archivo: <ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "No lo pulses sin parar; espera a que ponga Done." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Estado del scan de prefabs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "Muestra: Idle / Queued / Running / Done / Failed.\n" +
                    "Queued/Running = tiempo; Done = duración + hora." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Logs debug detallados" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "Activado = más detalles en DispatchBoss.log.\n" +
                    "Útil para depurar.\n" +
                    "**Desactívalo** para jugar normal.\n" +
                    "<Si no sabes qué es,>\n" +
                    "**déjalo en OFF**.\n" +
                    "<El spam de logs puede bajar el rendimiento.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Abrir carpeta de logs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Abre la carpeta de logs.\n" +
                    "Luego abre <DispatchBoss.log> (Notepad++ recomendado)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "Abrir carpeta de informes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "Abre la carpeta de informes.\n" +
                    "Luego abre <ScanReport-Prefabs.txt>." },

                // ---- Prefab scan status text (templates) ----
                { "DB_SCAN_IDLE", "Idle" },
                { "DB_SCAN_QUEUED_FMT", "Queued ({0})" },
                { "DB_SCAN_RUNNING_FMT", "Running ({0})" },
                { "DB_SCAN_DONE_FMT", "Done ({0} | {1})" },
                { "DB_SCAN_FAILED", "Failed" },
                { "DB_SCAN_FAIL_NO_CITY", "CARGA UNA CIUDAD PRIMERO" },
                { "DB_SCAN_UNKNOWN_TIME", "hora desconocida" },
            };
        }

        public void Unload()
        {
        }
    }
}
