// File: Localization/LocaleFR.cs
// French (fr-FR) strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

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
                { m_Setting.GetOptionTabLocaleID(Setting.PublicTransitTab), "Transports publics" },
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "Industrie" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "Parcs-Routes" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "À propos" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "Lignes (limites du curseur véhicules)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "Étendre les limites du curseur de ligne" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "Permet de descendre jusqu’à **1 véhicule** sur beaucoup de lignes.\n" +
                    "**Le max peut aussi augmenter** (selon la logique du temps de trajet).\n" +
                    "Le jeu utilise le temps de ligne (conduite + nb d’arrêts), donc le max varie.\n" +
                    "<Éviter les conflits : retirez les mods qui modifient la même politique de ligne>.\n" +
                    "Si vous utilisez un autre mod de politique, laissez ça OFF.\n" +
                    "Marche pour : bus, tram, train, métro, bateau, ferry, avion.\n" +
                    "Astuce : ajoutez quelques arrêts — le jeu peut augmenter le max."
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacité des dépôts (max véhicules par dépôt)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Dépôt de bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Change combien de bus un **Dépôt de bus** peut maintenir/générer.\n" +
                    "**100%** = vanilla.\n" +
                    "**1000%** = 10×.\n" +
                    "S’applique à la capacité de base du bâtiment." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Dépôt de taxi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Combien de taxis un **Dépôt de taxi** peut maintenir.\n" +
                    "Ne touche que le bâtiment de base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Dépôt de tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Change combien de trams un **Dépôt de tram** peut maintenir.\n" +
                    "S’applique à la capacité de base du bâtiment." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Dépôt de train" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Change combien de trains un **Dépôt de train** peut maintenir.\n" +
                    "S’applique à la capacité de base du bâtiment." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Dépôt de métro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Change combien de rames un **Dépôt de métro** peut maintenir.\n" +
                    "S’applique à la capacité de base du bâtiment." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Réinitialiser tous les dépôts" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Remet tous les dépôts à **100%** (vanilla)." },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacité passagers (max par véhicule)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Change la capacité passagers du **bus**.\n" +
                    "**10%** = 10% des places.\n" +
                    "**100%** = vanilla.\n" +
                    "**1000%** = 10× places." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Change la capacité du **tram**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Train" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Change la capacité du **train**.\n" +
                    "S’applique aux locomotives et wagons." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Métro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Change la capacité du **métro**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Bateau" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Change la capacité du **bateau passagers** (pas cargo).\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Ferry" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Change la capacité du **ferry**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Avion" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Change la capacité de l’**avion**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "Doubler" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "Met tous les curseurs passagers à **200%**.\n" +
                    "Bus, tram, train, métro, bateau, ferry, avion." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "Réinitialiser tous les passagers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Remet tous les passagers à **100%** (vanilla)." },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "Véhicules de livraison (capacité cargo)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "Semi-remorques" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "Capacité des **semi**.\n" +
                    "Inclut les semi spécialisés (ferme, forêt, pêche, etc.).\n" +
                    "Multiplicateur : **1×** = vanilla (**25t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "Fourgonnettes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**Fourgonnettes de livraison**\n" +
                    "Multiplicateur : **1×** = vanilla (**4t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "Matières (pétrole/charbon/minerai/pierre)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**Camions matières** (pétrole/charbon/minerai/pierre)\n" +
                    "Multiplicateur : **1×** = vanilla (**20t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "Moto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**Livraison moto**\n" +
                    "Multiplicateur : **1×** = vanilla, **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "Réinitialiser livraisons" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "Remet les multiplicateurs à **1×** (vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "Flotte cargo (port/train/aéroport)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "Max flotte stations cargo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "Multiplicateur du max de transporteurs actifs des **stations cargo**.\n" +
                    "**1×** = vanilla, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "Flotte extracteurs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "Multiplicateur du **max camions** des extracteurs\n" +
                    "(fermes, forêt, pêche, minerai, pétrole + charbon/pierre si dispo).\n" +
                    "**1×** = vanilla, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "Réinitialiser cargo + extracteurs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "Remet cargo + extracteurs à **1×** (vanilla)." },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "Entretien des parcs" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "Capacité de tournée" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "Multiplicateur de la **capacité de tournée** (capacité véhicule).\n" +
                    "Travail total avant retour au bâtiment.\n" +
                    "Plus = reste dehors plus longtemps." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "Vitesse de travail" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "Multiplicateur de la **vitesse de travail**.\n" +
                    "Vitesse = travail par tick quand arrêté." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "Taille de flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "Multiplicateur du **max véhicules** du bâtiment.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "Réinitialiser parcs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "Remet tout à **100%** (vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "Entretien des routes" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "Taille de flotte" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "Multiplicateur du **max véhicules** par dépôt.\n" +
                    "Plus haut = plus de camions.\n" +
                    "<Note : trop peu ou trop peut nuire au trafic.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "Capacité de tournée" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "Multiplicateur de la **capacité de tournée**.\n" +
                    "Travail total avant retour au dépôt.\n" +
                    "Plus haut = moins de retours." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "Vitesse de réparation (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "Vitesse = travail par tick quand arrêté.\n" +
                    "En vanilla, les réparations peuvent prendre plusieurs arrêts.\n" +
                    "<Alpha : en test.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "Usure des routes (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Fonction alpha : en test>\n" +
                    "Vitesse d’usure des routes.\n" +
                    "**100%** = vanilla\n" +
                    "**10%** = 10× plus lent (moins de réparations)\n" +
                    "**400%** = 4× plus rapide (plus de réparations)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "Réinitialiser routes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "Remet tout à **100%** (vanilla)." },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "Liens" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "Debug / Logs" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nom affiché du mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Version actuelle du mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Ouvrir la page Paradox Mods de l’auteur." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "Ouvrir le Discord dans le navigateur." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "Rapport scan (prefabs)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "Rapport unique : prefabs + valeurs d’usure de voie.\n" +
                    "Fichier : <ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "Évitez de cliquer en boucle ; attendez le status Done." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Status du scan prefabs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "Affiche l’état : Inactif / En file / En cours / Terminé / Échec.\n" +
                    "En file/En cours montre le temps ; Terminé montre la durée + l’heure de fin." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Logs détaillés" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "ON = plus de détails dans DispatchBoss.log.\n" +
                    "Utile pour dépanner.\n" +
                    "Pour jouer normal, **désactivez**.\n" +
                    "<Si vous ne savez pas ce que c’est,>\n" +
                    "**laissez OFF**.\n" +
                    "<Le spam de logs impacte les perfs.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Ouvrir dossier logs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Ouvre le dossier des logs.\n" +
                    "Ensuite ouvrez <DispatchBoss.log> (Notepad++ conseillé)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "Ouvrir dossier rapports" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "Ouvre le dossier des rapports.\n" +
                    "Ensuite ouvrez <ScanReport-Prefabs.txt>." },

                // ---- Status Text (format string templates) ----
                { "DB_SCAN_IDLE", "Inactif" },
                { "DB_SCAN_QUEUED_FMT", "En file ({0})" },
                { "DB_SCAN_RUNNING_FMT", "En cours ({0})" },
                { "DB_SCAN_DONE_FMT", "Terminé ({0} | {1})" },
                { "DB_SCAN_FAILED", "Échec" },
                { "DB_SCAN_FAIL_NO_CITY", "CHARGEZ UNE VILLE D’ABORD." },
                { "DB_SCAN_UNKNOWN_TIME", "heure inconnue" },
            };
        }

        public void Unload()
        {
        }
    }
}
