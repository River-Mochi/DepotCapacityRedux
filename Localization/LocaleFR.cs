// Localization/LocaleFR.cs
// French (fr-FR) strings for Options UI.

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
                // Mod Title / Tabs / Groups
                { m_Setting.GetSettingsLocaleID(), "Ajuster la capacité des transports [ATC]" },

                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "Actions" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),   "À propos" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup),
                    "Capacité des dépôts (véhicules max. par dépôt)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup),
                    "Capacité des passagers (personnes max. par véhicule)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup),
                    "Infos" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup),
                    "Liens de support" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup),
                    "Debug / Journalisation" },
                { m_Setting.GetOptionGroupLocaleID(Setting.LogGroup),
                    "Fichier journal" },

                // DEPOT labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Dépôts de bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Nombre de bus que chaque bâtiment de **dépôt de bus** peut entretenir/générer.\n" +
                    "Utilisez un multiplicateur entre **1,0×** (vanilla) et **10,0×**.\n" +
                    "Multiplie le **bâtiment de base**, pas les extensions." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Dépôts de taxis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Nombre de taxis que chaque **dépôt de taxis** peut entretenir.\n" +
                    "Augmente uniquement le bâtiment de base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Dépôts de tramways" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Nombre de tramways que chaque **dépôt de tramways** peut entretenir.\n" +
                    "Augmente uniquement le bâtiment de base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Dépôts de trains" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Nombre de trains que chaque **dépôt de trains** peut entretenir.\n" +
                    "Augmente uniquement le bâtiment de base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Dépôts de métro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Nombre de **véhicules de métro** que chaque dépôt peut entretenir.\n" +
                    "Augmente uniquement le bâtiment de base." },

                // Depot reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Réinitialiser tous les dépôts" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Réinitialise tous les multiplicateurs de dépôts à **1,0×** (capacité par défaut du jeu – vanilla)." },

                // Passenger labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Passagers des bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Modifier le nombre de sièges des **passagers des bus**.\n" +
                    "**1,0×** = sièges vanilla, **10,0×** = dix fois plus de sièges." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Passagers des tramways" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Modifier le nombre de sièges des **passagers des tramways**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Passagers des trains" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Modifier le nombre de sièges des **passagers des trains**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Passagers du métro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Modifier le nombre de sièges des **passagers du métro**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Passagers des navires" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Modifier uniquement les navires **de passagers** (pas les cargos)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Passagers des ferries" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Modifier le nombre de sièges des **passagers des ferries**." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Passagers des avions" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Modifier le nombre de sièges des **passagers des avions**." },

                // Passenger reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Réinitialiser tous les passagers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Réinitialise tous les multiplicateurs de passagers à **1,0×** (capacité par défaut du jeu – vanilla)." },

                // About tab: info
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)),    "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)),     "Nom affiché de ce mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)),  "Version actuelle du mod." },

                // About tab: links
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "Ouvrir la page Paradox Mods de ce mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)),
                    "Ouvrir le Discord de la communauté dans un navigateur." },

                // About tab: debug
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Activer la journalisation de debug détaillée" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "Lorsqu’elle est activée, de nombreux détails de debug supplémentaires sont envoyés dans AdjustTransitCapacity.log.\n" +
                    "Utile pour le dépannage, mais remplit le journal.\n" +
                    "Recommandé : **Désactiver** pour une partie normale.\n" +
                    "<Si vous ne savez pas à quoi cela sert, laissez-le désactivé>\n" +
                    "<et ne cochez pas la case.>"
                },

                // About tab: log button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Ouvrir le journal" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Ouvrir le fichier journal ATC dans l’éditeur de texte par défaut." },
            };
        }

        public void Unload()
        {
        }
    }
}
