// Localization/LocaleIT.cs
// Italian (it-IT) strings for Options UI.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal;

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
            return new Dictionary<string, string>
            {
                // Mod Title / Tabs / Groups
                { m_Setting.GetSettingsLocaleID(), "Regola capacità trasporto pubblico [ATC]" },

                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Azioni" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),   "Info" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup),
                    "Capacità dei depositi (veicoli massimi per deposito)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup),
                    "Capacità passeggeri (persone massime per veicolo)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup),
                    "Informazioni" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup),
                    "Link di supporto" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup),
                    "Debug / Log" },
                { m_Setting.GetOptionGroupLocaleID(Setting.LogGroup),
                    "File di log" },

                // DEPOT labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Depositi autobus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Quanti autobus può gestire/generare ogni edificio **Deposito autobus**.\n" +
                    "Usa un moltiplicatore tra **1,0×** (vanilla) e **10,0×**.\n" +
                    "Moltiplica l’**edificio base**, non le estensioni." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Depositi taxi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Quanti taxi può gestire ogni **deposito taxi**.\n" +
                    "Aumenta solo l’edificio base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Depositi tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Quanti tram può gestire ogni **deposito tram**.\n" +
                    "Aumenta solo l’edificio base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Depositi treni" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Quanti treni può gestire ogni **deposito treni**.\n" +
                    "Aumenta solo l’edificio base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Depositi metropolitana" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Quanti **veicoli della metropolitana** può gestire ogni deposito.\n" +
                    "Aumenta solo l’edificio base." },

                // Depot reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Reimposta tutti i depositi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Reimposta tutti i moltiplicatori dei depositi a **1,0×** (capacità predefinita del gioco – vanilla)." },

                // Passenger labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Passeggeri autobus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Modifica i posti **passeggeri degli autobus**.\n" +
                    "**1,0×** = posti vanilla, **10,0×** = dieci volte più posti." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Passeggeri tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Modifica i posti **passeggeri dei tram**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Passeggeri treno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Modifica i posti **passeggeri dei treni**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Passeggeri metropolitana" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Modifica i posti **passeggeri della metropolitana**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Passeggeri nave" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Modifica solo le navi **passeggeri** (non le navi cargo)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Passeggeri traghetto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Modifica i posti **passeggeri dei traghetti**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Passeggeri aereo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Modifica i posti **passeggeri degli aerei**." },

                // Passenger reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Reimposta tutti i passeggeri" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Reimposta tutti i moltiplicatori passeggeri a **1,0×** (capacità predefinita del gioco – vanilla)." },

                // About tab: info
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)),    "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)),     "Nome visualizzato di questa mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Versione" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)),  "Versione attuale della mod." },

                // About tab: links
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Apri la pagina Paradox Mods di questa mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)),
                    "Apri il Discord della community nel browser." },

                // About tab: debug
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Attiva log di debug dettagliato" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "Quando è attivo, molti dettagli di debug aggiuntivi vengono scritti in AdjustTransitCapacity.log.\n" +
                    "Utile per la risoluzione dei problemi, ma riempie il log.\n" +
                    "Consigliato **Disattivare** per il gioco normale.\n" +
                    "<Se non sai a cosa serve, lascialo disattivato e non> \n" +
                    "<selezionare la casella.>"
                },

                // About tab: log button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Apri log" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Apri il file di log ATC nell’editor di testo predefinito." },
            };
        }

        public void Unload()
        {
        }
    }
}
