// File: Localization/LocaleZH_HANT.cs
// Chinese (Traditional) zh-HANT strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

    public sealed class LocaleZH_HANT : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleZH_HANT(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.PublicTransitTab), "大眾運輸" },
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "工業" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "公園-道路" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "關於" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "路線（遊戲內車輛滑桿限制）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "擴展路線滑桿限制" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "讓多數路線滑桿最小值可到 **1 輛車**。\n" +
                    "**最大值也可能提高**（仍遵循遊戲邏輯）。\n" +
                    "遊戲使用路線時間（行駛時間 + 站點數）決定最大值，所以每條線不同。\n" +
                    "<避免衝突：移除修改同一路線政策的MOD>。\n" +
                    "如果你用別的政策MOD，建議這裡保持關閉。\n" +
                    "適用：公車、電車、火車、地鐵、船、渡輪、飛機。\n" +
                    "小提示：想要更多車就多加幾個站點，遊戲常會自動提高最大值。"
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "車庫容量（每個車庫最大車輛）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "公車車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "調整每個 **公車車庫** 可維護/生成的公車數量。\n" +
                    "**100%** = 原版預設。\n" +
                    "**1000%** = 10×。\n" +
                    "作用於建築的基礎容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "計程車車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "每個 **計程車車庫** 可維護的計程車數量。\n" +
                    "只影響基礎建築。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "電車車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "調整每個 **電車車庫** 可維護的電車數量。\n" +
                    "作用於建築的基礎容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "火車車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "調整每個 **火車車庫** 可維護的火車數量。\n" +
                    "作用於建築的基礎容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "地鐵車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "調整每個 **地鐵車庫** 可維護的車輛數量。\n" +
                    "作用於建築的基礎容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "重置所有車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "把所有車庫滑桿恢復到 **100%**（原版預設）。" },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "載客量（每輛車最大人數）" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "公車" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "調整 **公車載客量**。\n" +
                    "**10%** = 原版座位的10%。\n" +
                    "**100%** = 原版預設。\n" +
                    "**1000%** = 10×座位。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "電車" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "調整 **電車載客量**。\n" +
                    "**100%** = 原版預設。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "火車" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "調整 **火車載客量**。\n" +
                    "對車頭與車廂生效。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "地鐵" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "調整 **地鐵載客量**。\n" +
                    "**100%** = 原版預設。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "客船" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "調整 **客船載客量**（不含貨船）。\n" +
                    "**100%** = 原版預設。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "渡輪" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "調整 **渡輪載客量**。\n" +
                    "**100%** = 原版預設。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "飛機" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "調整 **飛機載客量**。\n" +
                    "**100%** = 原版預設。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "加倍" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "把所有載客滑桿設為 **200%**。\n" +
                    "適用：公車、電車、火車、地鐵、客船、渡輪、飛機。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "重置所有載客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "把所有載客滑桿恢復到 **100%**（原版預設）。" },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "配送車輛（貨物容量）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "半掛卡車" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "**半掛卡車** 容量。\n" +
                    "包含專用產業半掛（農場、林業、漁業等）。\n" +
                    "倍率：**1×** = 原版（**25t**），**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "配送廂型車" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**配送廂型車**\n" +
                    "倍率：**1×** = 原版（**4t**），**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "原物料卡車（油/煤/礦/石）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**原物料卡車**（油/煤/礦/石）\n" +
                    "倍率：**1×** = 原版（**20t**），**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "機車" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**機車配送**\n" +
                    "倍率：**1×** = 原版，**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "重置配送" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "把配送倍率恢復到 **1×**（原版預設）。" },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "貨運車隊（港口/鐵路/機場）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "貨運設施最大車隊" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "**貨運運輸設施** 最大活躍運輸工具倍率。\n" +
                    "**1×** = 原版，**5×** = 5×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "採集設施車隊" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "產業 **採集設施最大卡車** 倍率\n" +
                    "（農場、林業、漁業、礦、油 + 有的話煤/石）。\n" +
                    "**1×** = 原版，**5×** = 5×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "重置貨運+採集車隊" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "把貨運設施 + 採集設施倍率恢復到 **1×**（原版預設）。" },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "公園維護" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "班次容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "**班次容量**（車輛容量）倍率。\n" +
                    "回到建築前能做的總工作量。\n" +
                    "可理解：補給多=外出更久。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "工作速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "**工作速度** 倍率。\n" +
                    "速度=車輛停下時每tick完成的工作量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "車庫車隊規模" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "車庫建築 **最大車輛數** 倍率。\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "重置公園維護" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "把所有數值恢復到 **100%**（原版預設）。" },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "道路維護" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "車庫車隊規模" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "每個車庫 **最大車輛數** 倍率。\n" +
                    "更高=更多卡車。\n" +
                    "<平衡提示：太少或太多都可能讓交通變差。>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "班次容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "**班次容量** 倍率。\n" +
                    "回到車庫前能做的總工作量。\n" +
                    "更高=更少返回。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "修理速度（alpha）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "速度=車輛停下時每tick完成的工作量。\n" +
                    "原版修路可能需要多次停靠。\n" +
                    "<Alpha：還在真實城市裡測手感。>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "道路磨損速度（alpha）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Alpha功能：還在測試>\n" +
                    "道路隨時間累積磨損的速度。\n" +
                    "**100%** = 原版\n" +
                    "**10%** = 10×更慢（更少修路）\n" +
                    "**400%** = 4×更快（更多修路）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "重置道路維護" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "把所有數值恢復到 **100%**（原版預設）。" },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "資訊" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "支援連結" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "除錯 / 日誌" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "模組" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "顯示的模組名稱。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "目前模組版本。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "開啟作者的 Paradox Mods 頁面。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "用瀏覽器開啟社群 Discord。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "掃描報告（prefab）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "一次性報告：相關 prefab + 車道磨損數值。\n" +
                    "檔案：<ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "不要連點；等狀態顯示 Done 再說。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Prefab 掃描狀態" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "顯示：Idle / Queued / Running / Done / Failed。\n" +
                    "Queued/Running 顯示耗時；Done 顯示用時+完成時間。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "詳細除錯日誌" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "開啟 = 輸出更多資訊到 DispatchBoss.log。\n" +
                    "用於排查問題。\n" +
                    "一般遊玩建議 **關閉**。\n" +
                    "<如果你不知道這是什麼，>\n" +
                    "**就保持關閉**。\n" +
                    "<日誌太多會影響效能。>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "開啟日誌資料夾" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "開啟日誌資料夾。\n" +
                    "然後用文字編輯器打開 <DispatchBoss.log>（推薦 Notepad++）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "開啟報告資料夾" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "開啟報告資料夾。\n" +
                    "然後打開 <ScanReport-Prefabs.txt>。" },

                // ---- Status Text (format string templates) ----
                { "DB_SCAN_IDLE", "閒置" },
                { "DB_SCAN_QUEUED_FMT", "排隊中 ({0})" },
                { "DB_SCAN_RUNNING_FMT", "執行中 ({0})" },
                { "DB_SCAN_DONE_FMT", "完成 ({0} | {1})" },
                { "DB_SCAN_FAILED", "失敗" },
                { "DB_SCAN_FAIL_NO_CITY", "先載入城市" },
                { "DB_SCAN_UNKNOWN_TIME", "未知時間" },
            };
        }

        public void Unload()
        {
        }
    }
}
