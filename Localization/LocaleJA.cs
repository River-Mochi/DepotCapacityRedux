// File: Localization/LocaleJA.cs
// Japanese (ja-JP) strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

    public sealed class LocaleJA : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleJA(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.PublicTransitTab), "公共交通" },
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "産業" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "公園-道路" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "情報" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "路線（車両スライダー上限）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "路線スライダーの上限/下限を拡張" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "多くの路線でスライダー最小を **1台** まで許可します。\n" +
                    "**最大値も増えることがあります**（ゲームのルート時間ロジックに従う）。\n" +
                    "ゲームはルート時間（走行時間 + 停留所数）で最大値を決めるため、路線ごとに変わります。\n" +
                    "<競合回避: 同じ路線ポリシーを変更するMODは外してください>。\n" +
                    "別のポリシーMODを使うなら、このチェックはOFF推奨。\n" +
                    "対象: バス、トラム、列車、地下鉄、船、フェリー、飛行機。\n" +
                    "ヒント: もっと車両が欲しいなら停留所を少し増やすと、最大値が上がることがあります。"
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "車庫容量（車庫あたり最大車両）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "バス車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "**バス車庫** が維持/出現できるバス数を変更します。\n" +
                    "**100%** = バニラ（標準）。\n" +
                    "**1000%** = 10×。\n" +
                    "建物の基本容量に適用されます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "タクシー車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "**タクシー車庫** が維持できるタクシー数。\n" +
                    "基本建物にのみ適用されます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "トラム車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "**トラム車庫** が維持できるトラム数。\n" +
                    "建物の基本容量に適用されます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "列車車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "**列車車庫** が維持できる列車数。\n" +
                    "建物の基本容量に適用されます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "地下鉄車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "**地下鉄車庫** が維持できる車両数。\n" +
                    "建物の基本容量に適用されます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "車庫を全てリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "すべての車庫スライダーを **100%**（標準/バニラ）に戻します。" },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "乗客容量（車両あたり最大人数）" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "バス" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "**バス乗客** 容量を変更します。\n" +
                    "**10%** = バニラ座席の10%。\n" +
                    "**100%** = バニラ（標準）。\n" +
                    "**1000%** = 10×座席。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "トラム" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "**トラム乗客** 容量。\n" +
                    "**100%** = バニラ（標準）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "列車" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "**列車乗客** 容量。\n" +
                    "機関車と車両に適用されます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "地下鉄" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "**地下鉄乗客** 容量。\n" +
                    "**100%** = バニラ（標準）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "船" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "**旅客船** 容量（貨物船は対象外）。\n" +
                    "**100%** = バニラ（標準）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "フェリー" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "**フェリー乗客** 容量。\n" +
                    "**100%** = バニラ（標準）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "飛行機" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "**飛行機乗客** 容量。\n" +
                    "**100%** = バニラ（標準）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "2倍" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "すべての乗客スライダーを **200%** に設定します。\n" +
                    "バス/トラム/列車/地下鉄/船/フェリー/飛行機に適用。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "乗客を全てリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "すべての乗客スライダーを **100%**（標準/バニラ）に戻します。" },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "配送車両（貨物容量）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "大型トラック（セミ）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "**セミトラック** 容量。\n" +
                    "特化産業セミ（農業/林業/漁業など）も含みます。\n" +
                    "倍率: **1×** = バニラ（**25t**）、**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "配送バン" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**配送バン**\n" +
                    "倍率: **1×** = バニラ（**4t**）、**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "原材料トラック（石油/石炭/鉱石/石材）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**原材料トラック**（石油/石炭/鉱石/石材）\n" +
                    "倍率: **1×** = バニラ（**20t**）、**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "バイク" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**バイク配送**\n" +
                    "倍率: **1×** = バニラ、**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "配送をリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "配送倍率を **1×**（標準/バニラ）に戻します。" },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "貨物フリート（港/鉄道/空港）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "貨物施設 最大フリート" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "**貨物輸送施設** の最大稼働輸送数の倍率。\n" +
                    "**1×** = バニラ、**5×** = 5×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "採取施設フリート" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "産業 **採取施設の最大トラック** の倍率\n" +
                    "（農業/林業/漁業/鉱石/石油 + 石炭/石材がある場合）。\n" +
                    "**1×** = バニラ、**5×** = 5×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "貨物+採取 フリートをリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "貨物施設 + 採取施設の倍率を **1×**（標準/バニラ）に戻します。" },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "公園メンテ" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "作業量（シフト）容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "**作業量容量**（車両容量）の倍率。\n" +
                    "建物に戻る前にできる総作業量。\n" +
                    "補給多め = 外で長く作業。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "作業速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "**作業速度** の倍率。\n" +
                    "速度 = 停止中のtickあたり作業量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "車庫フリート" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "車庫の **最大車両数** の倍率。\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "公園メンテをリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "すべて **100%**（標準/バニラ）に戻します。" },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "道路メンテ" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "車庫フリート" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "車庫あたり **最大車両数** の倍率。\n" +
                    "高いほど = 車両が増えます。\n" +
                    "<注意: 多すぎ/少なすぎは渋滞の原因になることも。>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "作業量（シフト）容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "**作業量容量** の倍率。\n" +
                    "車庫に戻る前にできる総作業量。\n" +
                    "高いほど = 戻り回数が減ります。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "修理速度（alpha）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "速度 = 停止中のtickあたり作業量。\n" +
                    "バニラでは損傷次第で複数回停止することがあります。\n" +
                    "<Alpha: 実都市で感触テスト中。>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "道路摩耗速度（alpha）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Alpha機能: まだテスト中>\n" +
                    "時間経過で道路が摩耗する速さ。\n" +
                    "**100%** = バニラ\n" +
                    "**10%** = 10×遅い（修理が少ない）\n" +
                    "**400%** = 4×速い（修理が多い）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "道路メンテをリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "すべて **100%**（標準/バニラ）に戻します。" },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "情報" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "リンク" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "デバッグ / ログ" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "MOD" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "MODの表示名。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "バージョン" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "現在のMODバージョン。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "作者のParadox Modsページを開きます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "コミュニティDiscordを開きます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "スキャンレポート（prefab）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "1回だけのレポート: 関連prefab + レーン摩耗値。\n" +
                    "ファイル: <ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "連打しないで。ステータスがDoneになるまで待ってください。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Prefabスキャン状況" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "表示: Idle / Queued / Running / Done / Failed.\n" +
                    "Queued/Running = 経過; Done = 所要 + 完了時刻。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "詳細デバッグログ" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "ON = DispatchBoss.log に詳細を書きます。\n" +
                    "トラブル対応向け。\n" +
                    "普段は **OFF** 推奨。\n" +
                    "<よく分からなければ>\n" +
                    "**OFFのまま**。\n" +
                    "<ログ多すぎは性能に影響。>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "ログフォルダを開く" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "ログフォルダを開きます。\n" +
                    "次に <DispatchBoss.log> を開いてください（Notepad++推奨）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "レポートフォルダを開く" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "レポートフォルダを開きます。\n" +
                    "次に <ScanReport-Prefabs.txt> を開いてください。" },

                // ---- Status Text (format string templates) ----
                { "DB_SCAN_IDLE", "待機" },
                { "DB_SCAN_QUEUED_FMT", "待機中 ({0})" },
                { "DB_SCAN_RUNNING_FMT", "実行中 ({0})" },
                { "DB_SCAN_DONE_FMT", "完了 ({0} | {1})" },
                { "DB_SCAN_FAILED", "失敗" },
                { "DB_SCAN_FAIL_NO_CITY", "先に都市をロード" },
                { "DB_SCAN_UNKNOWN_TIME", "不明" },
            };
        }

        public void Unload()
        {
        }
    }
}
