// File: Localization/LocaleIT.cs
// Italian (it-IT) strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

    public sealed class LocaleIT : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleIT(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.PublicTransitTab), "Trasporto pubblico" },
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "Industria" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "Parchi-Strade" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "Info" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "Linee (limiti del cursore veicoli)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "Espandi limiti cursore linee" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "Permette al cursore delle linee di scendere **fino a 1 veicolo** (su molte rotte).\n" +
                    "**Il massimo può anche aumentare** (segue la logica del gioco).\n" +
                    "Il gioco usa tempo rotta (guida + fermate), quindi il max varia per linea.\n" +
                    "<Evita conflitti: rimuovi mod che cambiano la stessa policy delle linee>.\n" +
                    "Se usi un’altra mod di policy, lascia questa casella OFF.\n" +
                    "Vale per: bus, tram, treno, metro, nave, traghetto, aereo.\n" +
                    "Tip: se vuoi ancora più veicoli, aggiungi qualche fermata: spesso il gioco alza il max da solo."
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacità depositi (max veicoli per deposito)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Deposito bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Cambia quanti bus può gestire/spawnare ogni **Deposito bus**.\n" +
                    "**100%** = vanilla (default).\n" +
                    "**1000%** = 10×.\n" +
                    "Si applica alla capacità base dell’edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Deposito taxi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Quanti taxi può gestire ogni **Deposito taxi**.\n" +
                    "L’aumento vale solo per l’edificio base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Deposito tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Cambia quanti tram può gestire ogni **Deposito tram**.\n" +
                    "Si applica alla capacità base dell’edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Deposito treni" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Cambia quanti treni può gestire ogni **Deposito treni**.\n" +
                    "Si applica alla capacità base dell’edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Deposito metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Cambia quanti veicoli può gestire ogni **Deposito metro**.\n" +
                    "Si applica alla capacità base dell’edificio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Reset tutti i depositi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Riporta tutti i depositi a **100%** (default / vanilla)." },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacità passeggeri (max persone per veicolo)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Cambia la capacità **passeggeri bus**.\n" +
                    "**10%** = 10% dei posti vanilla.\n" +
                    "**100%** = vanilla (default).\n" +
                    "**1000%** = 10× posti." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Cambia la capacità **passeggeri tram**.\n" +
                    "**100%** = vanilla (default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Treno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Cambia la capacità **passeggeri treno**.\n" +
                    "Vale per locomotive e vagoni." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Metro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Cambia la capacità **passeggeri metro**.\n" +
                    "**100%** = vanilla (default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Nave" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Cambia la capacità **nave passeggeri** (non cargo).\n" +
                    "**100%** = vanilla (default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Traghetto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Cambia la capacità **passeggeri traghetto**.\n" +
                    "**100%** = vanilla (default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Aereo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Cambia la capacità **passeggeri aereo**.\n" +
                    "**100%** = vanilla (default)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "Raddoppia" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "Imposta tutti i passeggeri a **200%**.\n" +
                    "Vale per bus, tram, treni, metro, navi, traghetti, aerei." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "Reset tutti i passeggeri" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Riporta tutti i passeggeri a **100%** (default / vanilla)." },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "Veicoli consegna (capacità carico)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "Camion (semi)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "Capacità **camion semi**.\n" +
                    "Include i semi specializzati (fattorie, foreste, pesca, ecc.).\n" +
                    "Moltiplicatore: **1×** = vanilla (**25t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "Furgoni consegna" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**Furgoni consegna**\n" +
                    "Moltiplicatore: **1×** = vanilla (**4t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "Camion materie prime (petrolio, carbone, minerale, pietra)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**Camion materie prime** (petrolio/carbone/minerale/pietra)\n" +
                    "Moltiplicatore: **1×** = vanilla (**20t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "Moto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**Consegna in moto**\n" +
                    "Moltiplicatore: **1×** = vanilla, **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "Reset consegne" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "Riporta le consegne a **1×** (default / vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "Flotta cargo (porto, treno, aeroporto)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "Flotta max stazioni cargo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "Moltiplicatore del max trasportatori attivi per **stazioni cargo**.\n" +
                    "**1×** = vanilla, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "Flotta estrattori" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "Moltiplicatore del max camion per **estrattori**\n" +
                    "(fattorie, foreste, pesca, minerale, petrolio, + carbone/pietra se presenti).\n" +
                    "**1×** = vanilla, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "Reset flotta cargo + estrattori" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "Riporta cargo + estrattori a **1×** (default / vanilla)." },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "Manutenzione parchi" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "Capacità turno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "Moltiplicatore di **capacità turno** (capacità veicolo).\n" +
                    "Lavoro totale prima di tornare all’edificio.\n" +
                    "Più scorte = resta fuori più a lungo." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "Ritmo veicolo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "Moltiplicatore del **ritmo di lavoro**.\n" +
                    "Ritmo = lavoro per tick quando è fermo." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "Flotta deposito" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "Moltiplicatore del **max veicoli** del deposito.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "Reset manutenzione parchi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "Riporta tutto a **100%** (default / vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "Manutenzione strade" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "Flotta deposito" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "Moltiplicatore del **max veicoli** per deposito.\n" +
                    "Più alto = più mezzi.\n" +
                    "<Nota: troppo poco o troppo può peggiorare il traffico.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "Capacità turno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "Moltiplicatore di **capacità turno**.\n" +
                    "Lavoro totale prima di tornare al deposito.\n" +
                    "Più alto = meno rientri." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "Velocità riparazione (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "Velocità = lavoro per tick quando è fermo.\n" +
                    "In vanilla, le riparazioni possono richiedere più soste.\n" +
                    "<Alpha: ancora in test su città reali.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "Usura strade (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Funzione alpha: ancora in test>\n" +
                    "Quanto velocemente le strade si usurano nel tempo.\n" +
                    "**100%** = vanilla\n" +
                    "**10%** = 10× più lenta (meno riparazioni)\n" +
                    "**400%** = 4× più veloce (più riparazioni)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "Reset manutenzione strade" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "Riporta tutto a **100%** (default / vanilla)." },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "Link supporto" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "Debug / Log" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nome visualizzato del mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Versione" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Versione attuale del mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Apri la pagina Paradox Mods dell’autore." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "Apri il Discord della community." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "Report scan (prefab)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "Report una tantum: prefab rilevanti + valori usura.\n" +
                    "File: <ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "Non spammare; aspetta che lo stato dica Done." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Stato scan prefab" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "Mostra: Idle / Queued / Running / Done / Failed.\n" +
                    "Queued/Running = tempo; Done = durata + ora." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Log debug dettagliati" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "ON = più dettagli in DispatchBoss.log.\n" +
                    "Utile per troubleshooting.\n" +
                    "**OFF** per gioco normale.\n" +
                    "<Se non sai cos’è,>\n" +
                    "**lascialo OFF**.\n" +
                    "<Troppi log possono ridurre le prestazioni.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Apri cartella log" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Apri la cartella log.\n" +
                    "Poi apri <DispatchBoss.log> (consigliato Notepad++)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "Apri cartella report" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "Apri la cartella report.\n" +
                    "Poi apri <ScanReport-Prefabs.txt>." },

                // ---- Status Text (format string templates) ----
                { "DB_SCAN_IDLE", "Inattivo" },
                { "DB_SCAN_QUEUED_FMT", "In coda ({0})" },
                { "DB_SCAN_RUNNING_FMT", "In corso ({0})" },
                { "DB_SCAN_DONE_FMT", "Fatto ({0} | {1})" },
                { "DB_SCAN_FAILED", "Errore" },
                { "DB_SCAN_FAIL_NO_CITY", "CARICA PRIMA UNA CITTÀ" },
                { "DB_SCAN_UNKNOWN_TIME", "ora sconosciuta" },
            };
        }

        public void Unload()
        {
        }
    }
}
