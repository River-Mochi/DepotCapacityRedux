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
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "Parcs & Routes" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "À propos" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "Lignes (limites du curseur véhicules)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "Élargir les limites du curseur des lignes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "Permet au curseur des lignes de descendre **jusqu’à 1 véhicule** sur la plupart des trajets.\n" +
                    "**Le maximum peut aussi augmenter** (selon la logique du jeu).\n" +
                    "Le jeu utilise le temps de trajet (conduite + nombre d’arrêts), donc le max varie par ligne.\n" +
                    "<Éviter les conflits : retirez les mods qui modifient la même politique de ligne>.\n" +
                    "Si vous utilisez un autre mod de politique, laissez cette case DÉCOCHÉE.\n" +
                    "Fonctionne pour : bus, tram, train, métro, bateau, ferry, avion."
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacité des dépôts (max véhicules par dépôt)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Dépôt de bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Modifie combien de bus chaque **dépôt de bus** peut gérer/spawn.\n" +
                    "**100%** = vanilla (défaut du jeu).\n" +
                    "**1000%** = 10× plus.\n" +
                    "S’applique à la capacité de base du bâtiment." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Dépôt de taxis" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Combien de taxis chaque **dépôt de taxis** peut gérer.\n" +
                    "L’augmentation s’applique au bâtiment de base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Dépôt de trams" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Modifie combien de trams chaque **dépôt de trams** peut gérer.\n" +
                    "S’applique à la capacité de base du bâtiment." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Dépôt de trains" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Modifie combien de trains chaque **dépôt de trains** peut gérer.\n" +
                    "S’applique à la capacité de base du bâtiment." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Dépôt de métro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Modifie combien de rames chaque **dépôt de métro** peut gérer.\n" +
                    "S’applique à la capacité de base du bâtiment." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Réinitialiser tous les dépôts" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Remet tous les curseurs de dépôts à **100%** (défaut du jeu / vanilla)." },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacité passagers (max par véhicule)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Modifie la capacité **passagers des bus**.\n" +
                    "**10%** = 10% des sièges vanilla.\n" +
                    "**100%** = sièges vanilla (défaut du jeu).\n" +
                    "**1000%** = 10× plus de sièges." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Modifie la capacité **passagers des trams**.\n" +
                    "**100%** = sièges vanilla (défaut du jeu)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Train" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Modifie la capacité **passagers des trains**.\n" +
                    "S’applique aux locomotives et wagons." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Métro" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Modifie la capacité **passagers du métro**.\n" +
                    "**100%** = sièges vanilla (défaut du jeu)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Bateau" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Modifie la capacité **bateau passagers** (pas les cargos).\n" +
                    "**100%** = sièges vanilla (défaut du jeu)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Ferry" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Modifie la capacité **passagers des ferries**.\n" +
                    "**100%** = sièges vanilla (défaut du jeu)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Avion" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Modifie la capacité **passagers des avions**.\n" +
                    "**100%** = sièges vanilla (défaut du jeu)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "Doubler" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "Met tous les curseurs passagers à **200%**.\n" +
                    "S’applique aux bus, trams, trains, métros, bateaux, ferries et avions." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "Réinitialiser tous les passagers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Remet tous les curseurs passagers à **100%** (défaut du jeu / vanilla)." },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "Véhicules de livraison (capacité cargo)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "Poids lourds (semi)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "Capacité des **semi-remorques**.\n" +
                    "Inclut les semi spécialisés (fermes, forêt, pêche, etc.).\n" +
                    "Multiplicateur : **1×** = vanilla (**25t**), **10×** = 10× plus." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "Vans de livraison" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**Vans de livraison**\n" +
                    "Multiplicateur : **1×** = vanilla (**4t**), **10×** = 10× plus." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "Camions matières premières (pétrole, charbon, minerai, pierre)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**Camions matières premières** (pétrole/charbon/minerai/pierre)\n" +
                    "Multiplicateur : **1×** = vanilla (**20t**), **10×** = 10× plus." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "Moto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**Livraison à moto**\n" +
                    "Multiplicateur : **1×** = vanilla, **10×** = 10× plus." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "Réinitialiser livraisons" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "Remet les multiplicateurs livraison à **1×** (défaut du jeu / vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "Flotte cargo (port, train, aéroport)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "Flotte max des stations cargo" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "Multiplicateur du max de transporteurs actifs pour les **stations cargo**.\n" +
                    "**1×** = vanilla, **5×** = 5× plus." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "Flotte extracteurs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "Multiplicateur du max de camions des **extracteurs** industriels\n" +
                    "(fermes, forêt, pêche, minerai, pétrole, + charbon/pierre si dispo).\n" +
                    "**1×** = vanilla, **5×** = 5× plus." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "Réinitialiser flotte cargo + extracteurs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "Remet les multiplicateurs cargo + extracteurs à **1×** (défaut du jeu / vanilla)." },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "Entretien des parcs" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "Capacité de tournée" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "Multiplicateur de **capacité de tournée** (capacité véhicule).\n" +
                    "Travail total avant retour au bâtiment.\n" +
                    "En gros : plus de matériel = reste dehors plus longtemps." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "Vitesse de travail" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "Multiplicateur du **rythme de travail**.\n" +
                    "Rythme = travail par tick de simulation quand le véhicule est à l’arrêt." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "Taille de flotte du dépôt" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "Multiplicateur du **max véhicules** du dépôt.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "Réinitialiser entretien parc" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "Remet tout à **100%** (défaut du jeu / vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "Entretien des routes" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "Taille de flotte du dépôt" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "Multiplicateur du **max véhicules** par dépôt.\n" +
                    "Plus haut = plus de camions.\n" +
                    "<Note équilibre : trop peu ou trop peut aggraver le trafic.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "Capacité de tournée" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "Multiplicateur de **capacité de tournée**.\n" +
                    "Travail total avant retour au dépôt.\n" +
                    "Plus haut = moins de retours." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "Taux de réparation (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "Taux = travail par tick de simulation quand le véhicule est à l’arrêt.\n" +
                    "En vanilla, les réparations peuvent demander plusieurs arrêts.\n" +
                    "<Alpha : encore en test sur de vraies villes.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "Vitesse d’usure des routes (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Fonction alpha : encore en test>\n" +
                    "Vitesse à laquelle les routes s’usent avec le temps.\n" +
                    "**100%** = vanilla\n" +
                    "**10%** = usure 10× plus lente (moins de réparations)\n" +
                    "**400%** = usure 4× plus rapide (plus de réparations)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "Réinitialiser entretien routes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "Remet tout à **100%** (défaut du jeu / vanilla)." },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "Infos" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "Liens" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "Debug / Logs" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nom affiché du mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Version actuelle du mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Ouvrir la page Paradox Mods de l’auteur." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "Ouvrir le Discord de la communauté." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "Rapport scan (prefabs)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "Rapport unique : prefabs utiles + valeurs d’usure.\n" +
                    "Fichier : <ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "Évitez de spammer ; attendez le statut « Done »." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Statut du scan prefabs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "Affiche : Idle / Queued / Running / Done / Failed.\n" +
                    "Queued/Running = temps écoulé ; Done = durée + heure." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Logs debug verbeux" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "Activé = plus de détails dans DispatchBoss.log.\n" +
                    "Utile pour dépanner.\n" +
                    "**Désactivez** en jeu normal.\n" +
                    "<Si vous ne savez pas,>\n" +
                    "**laissez OFF**.\n" +
                    "<Le spam de logs peut réduire les perfs.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Ouvrir dossier logs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Ouvre le dossier des logs.\n" +
                    "Ensuite : ouvrez <DispatchBoss.log> (Notepad++ conseillé)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "Ouvrir dossier rapports" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "Ouvre le dossier des rapports.\n" +
                    "Ensuite : ouvrez <ScanReport-Prefabs.txt>." },

                // ---- Prefab scan status text (templates) ----
                { "DB_SCAN_IDLE", "Idle" },
                { "DB_SCAN_QUEUED_FMT", "Queued ({0})" },
                { "DB_SCAN_RUNNING_FMT", "Running ({0})" },
                { "DB_SCAN_DONE_FMT", "Done ({0} | {1})" },
                { "DB_SCAN_FAILED", "Failed" },
                { "DB_SCAN_FAIL_NO_CITY", "CHARGEZ UNE VILLE D’ABORD" },
                { "DB_SCAN_UNKNOWN_TIME", "heure inconnue" },
            };
        }

        public void Unload()
        {
        }
    }
}
