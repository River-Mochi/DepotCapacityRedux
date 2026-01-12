// File: Localization/LocalePT_BR.cs
// Portuguese (Brazil) pt-BR strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

    public sealed class LocalePT_BR : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocalePT_BR(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "Indústria" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "Parques-Estradas" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "Sobre" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "Linhas (limites do slider)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "Expandir limites do slider da linha" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "Permite que o slider da linha vá até **1 veículo** na maioria das rotas.\n" +
                    "**O máximo também pode aumentar** (segue a lógica de tempo da rota).\n" +
                    "O jogo usa tempo da rota (dirigir + paradas), então o max varia por linha.\n" +
                    "<Evite conflitos: remova mods que mexem na mesma política de linha>.\n" +
                    "Se você usa outro mod de política, deixe isso OFF.\n" +
                    "Funciona para: ônibus, bonde, trem, metrô, navio, balsa, avião.\n" +
                    "Dica: quer mais veículos? adicione paradas — o jogo pode aumentar o máximo."
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "Capacidade do depósito (max veículos por prédio)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "Depósito de ônibus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "Muda quantos ônibus cada **Depósito de ônibus** mantém/gera.\n" +
                    "**100%** = vanilla.\n" +
                    "**1000%** = 10×.\n" +
                    "Aplica na capacidade base do prédio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "Depósito de táxi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "Quantos táxis cada **Depósito de táxi** mantém.\n" +
                    "Aumenta só o prédio base." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "Depósito de bonde" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "Muda quantos bondes cada **Depósito de bonde** mantém.\n" +
                    "Aplica na capacidade base do prédio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "Depósito de trem" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "Muda quantos trens cada **Depósito de trem** mantém.\n" +
                    "Aplica na capacidade base do prédio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "Depósito de metrô" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "Muda quantos veículos cada **Depósito de metrô** mantém.\n" +
                    "Aplica na capacidade base do prédio." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "Resetar todos os depósitos" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "Volta todos os sliders de depósito para **100%** (vanilla)." },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "Capacidade de passageiros (max por veículo)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "Ônibus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "Muda a capacidade de passageiros do **ônibus**.\n" +
                    "**10%** = 10% dos assentos.\n" +
                    "**100%** = vanilla.\n" +
                    "**1000%** = 10× assentos." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "Bonde" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "Muda a capacidade do **bonde**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "Trem" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "Muda a capacidade do **trem**.\n" +
                    "Aplica em locomotivas e vagões." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "Metrô" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "Muda a capacidade do **metrô**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "Navio" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "Muda a capacidade de **navio de passageiros** (não é cargueiro).\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "Balsa" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "Muda a capacidade da **balsa**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "Avião" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "Muda a capacidade do **avião**.\n" +
                    "**100%** = vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "Dobrar" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "Define todos os sliders de passageiros para **200%**.\n" +
                    "Aplica em ônibus, bonde, trem, metrô, navio, balsa e avião." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "Resetar todos os passageiros" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "Volta todos os sliders de passageiros para **100%** (vanilla)." },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "Veículos de entrega (capacidade de carga)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "Carretas" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "Capacidade das **carretas**.\n" +
                    "Inclui carretas de indústrias especiais (fazenda, floresta, pesca etc.).\n" +
                    "Multiplicador: **1×** = vanilla (**25t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "Vans" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**Vans de entrega**\n" +
                    "Multiplicador: **1×** = vanilla (**4t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "Matéria-prima (óleo/carvão/minério/pedra)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**Caminhões de matéria-prima** (óleo/carvão/minério/pedra)\n" +
                    "Multiplicador: **1×** = vanilla (**20t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "Moto" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**Entrega de moto**\n" +
                    "Multiplicador: **1×** = vanilla, **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "Resetar entregas" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "Volta os multiplicadores de entrega para **1×** (vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "Frota de carga (porto/trem/aeroporto)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "Max frota da estação de carga" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "Multiplicador do máximo de transportadores ativos em **estações de carga**.\n" +
                    "**1×** = vanilla, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "Frota de extratores" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "Multiplicador do **max caminhões** de extratores\n" +
                    "(fazenda, floresta, pesca, minério, óleo + carvão/pedra se existir).\n" +
                    "**1×** = vanilla, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "Resetar frota carga + extratores" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "Volta carga + extratores para **1×** (vanilla)." },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "Manutenção de parques" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "Capacidade do turno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "Multiplicador da **capacidade do turno** (capacidade do veículo).\n" +
                    "Trabalho total antes de voltar ao prédio.\n" +
                    "Mais = fica mais tempo fora." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "Velocidade de trabalho" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "Multiplicador da **velocidade de trabalho**.\n" +
                    "Velocidade = trabalho por tick quando parado." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "Tamanho da frota" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "Multiplicador do **max veículos** do prédio.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "Resetar manutenção de parques" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "Reset para **100%** (vanilla)." },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "Manutenção de estradas" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "Tamanho da frota" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "Multiplicador do **max veículos** por depósito.\n" +
                    "Mais alto = mais caminhões.\n" +
                    "<Nota: pouco ou demais pode piorar o trânsito.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "Capacidade do turno" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "Multiplicador da **capacidade do turno**.\n" +
                    "Trabalho total antes de voltar ao depósito.\n" +
                    "Mais alto = menos retornos." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "Taxa de reparo (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "Taxa = trabalho por tick quando parado.\n" +
                    "No vanilla, reparar pode exigir várias paradas.\n" +
                    "<Alpha: ainda testando.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "Desgaste da estrada (alpha)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Recurso alpha: em teste>\n" +
                    "Quão rápido as estradas acumulam desgaste.\n" +
                    "**100%** = vanilla\n" +
                    "**10%** = 10× mais lento (menos reparos)\n" +
                    "**400%** = 4× mais rápido (mais reparos)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "Resetar manutenção de estradas" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "Volta tudo para **100%** (vanilla)." },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "Info" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "Links" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "Debug / Logs" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "Nome exibido do mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "Versão" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "Versão atual do mod." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "Abrir a página do autor no Paradox Mods." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "Abrir o Discord no navegador." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "Relatório de scan (prefabs)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "Relatório único: prefabs relevantes + valores de desgaste da faixa.\n" +
                    "Arquivo: <ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "Evite clicar várias vezes; espere o status mostrar Done." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Status do scan de prefabs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "Mostra o estado: Parado / Na fila / Rodando / Concluído / Falhou.\n" +
                    "Na fila/Rodando mostra o tempo; Concluído mostra duração + hora de término." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "Logs detalhados" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "ON = mais detalhes no DispatchBoss.log.\n" +
                    "Útil pra investigar problema.\n" +
                    "Pra jogar normal, **deixe OFF**.\n" +
                    "<Se você não sabe o que é isso,>\n" +
                    "**deixe OFF**.\n" +
                    "<Spam no log afeta performance.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "Abrir pasta de logs" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "Abre a pasta de logs.\n" +
                    "Depois abra <DispatchBoss.log> (recomendo Notepad++)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "Abrir pasta de relatórios" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "Abre a pasta de relatórios.\n" +
                    "Depois abra <ScanReport-Prefabs.txt>." },

                // ---- Status Text (format string templates) ----
                { "DB_SCAN_IDLE", "Parado" },
                { "DB_SCAN_QUEUED_FMT", "Na fila ({0})" },
                { "DB_SCAN_RUNNING_FMT", "Rodando ({0})" },
                { "DB_SCAN_DONE_FMT", "Concluído ({0} | {1})" },
                { "DB_SCAN_FAILED", "Falhou" },
                { "DB_SCAN_FAIL_NO_CITY", "CARREGUE UMA CIDADE PRIMEIRO." },
                { "DB_SCAN_UNKNOWN_TIME", "hora desconhecida" },
            };
        }

        public void Unload()
        {
        }
    }
}
