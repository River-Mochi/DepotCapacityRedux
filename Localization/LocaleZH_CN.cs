// File: Localization/LocaleZH_CN.cs
// Chinese (Simplified) zh-HANS strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

    public sealed class LocaleZH_CN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleZH_CN(Setting setting)
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
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "工业" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "公园-道路" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "关于" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "线路（游戏内车辆滑条限制）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "扩展线路滑条限制" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "让大多数线路的滑条最小值可到 **1 辆车**。\n" +
                    "**最大值也可能提高**（仍遵循游戏逻辑）。\n" +
                    "游戏用线路时间（行驶时间 + 站点数）决定最大值，所以每条线不同。\n" +
                    "<避免冲突：移除修改同一线路政策的MOD>。\n" +
                    "如果你用别的政策MOD，建议这里保持关闭。\n" +
                    "适用：公交、电车、火车、地铁、船、渡轮、飞机。\n" +
                    "小贴士：想要更多车就多加几个站点，游戏常会自动提高最大值。"
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "车库容量（每个车库最大车辆）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "公交车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "调整每个 **公交车库** 可维护/生成的公交数量。\n" +
                    "**100%** = 原版默认。\n" +
                    "**1000%** = 10×。\n" +
                    "作用于建筑的基础容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "出租车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "每个 **出租车车库** 可维护的出租车数量。\n" +
                    "仅影响基础建筑。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "电车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "调整每个 **电车车库** 可维护的电车数量。\n" +
                    "作用于建筑的基础容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "火车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "调整每个 **火车车库** 可维护的火车数量。\n" +
                    "作用于建筑的基础容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "地铁车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "调整每个 **地铁车库** 可维护的车辆数量。\n" +
                    "作用于建筑的基础容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "重置所有车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "把所有车库滑条恢复到 **100%**（原版默认）。" },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "载客量（每辆车最大人数）" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "公交" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "调整 **公交载客量**。\n" +
                    "**10%** = 原版座位的10%。\n" +
                    "**100%** = 原版默认。\n" +
                    "**1000%** = 10×座位。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "电车" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "调整 **电车载客量**。\n" +
                    "**100%** = 原版默认。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "火车" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "调整 **火车载客量**。\n" +
                    "对机车和车厢生效。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "地铁" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "调整 **地铁载客量**。\n" +
                    "**100%** = 原版默认。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "客船" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "调整 **客船载客量**（不含货船）。\n" +
                    "**100%** = 原版默认。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "渡轮" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "调整 **渡轮载客量**。\n" +
                    "**100%** = 原版默认。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "飞机" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "调整 **飞机载客量**。\n" +
                    "**100%** = 原版默认。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "翻倍" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "把所有载客滑条设为 **200%**。\n" +
                    "适用：公交、电车、火车、地铁、客船、渡轮、飞机。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "重置所有载客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "把所有载客滑条恢复到 **100%**（原版默认）。" },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "配送车辆（货物容量）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "半挂卡车" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "**半挂卡车** 容量。\n" +
                    "包含专用产业半挂（农场、林业、渔业等）。\n" +
                    "倍率：**1×** = 原版（**25t**），**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "配送面包车" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**配送面包车**\n" +
                    "倍率：**1×** = 原版（**4t**），**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "原材料卡车（油/煤/矿/石）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**原材料卡车**（油/煤/矿/石）\n" +
                    "倍率：**1×** = 原版（**20t**），**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "摩托车" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**摩托配送**\n" +
                    "倍率：**1×** = 原版，**10×** = 10×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "重置配送" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "把配送倍率恢复到 **1×**（原版默认）。" },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "货运车队（港口/铁路/机场）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "货运设施最大车队" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "**货运站/港口/机场** 最大活跃运输工具倍率。\n" +
                    "**1×** = 原版，**5×** = 5×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "采集设施车队" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "产业 **采集设施最大卡车** 倍率\n" +
                    "（农场、林业、渔业、矿、油 + 有的话煤/石）。\n" +
                    "**1×** = 原版，**5×** = 5×。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "重置货运+采集车队" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "把货运设施 + 采集设施倍率恢复到 **1×**（原版默认）。" },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "公园维护" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "班次容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "**班次容量**（车辆容量）倍率。\n" +
                    "回到建筑前能做的总工作量。\n" +
                    "可理解为：补给更多=外出更久。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "工作速度" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "**工作速度** 倍率。\n" +
                    "速度=车辆停下时每tick完成的工作量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "车库车队规模" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "车库建筑 **最大车辆数** 倍率。\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "重置公园维护" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "把所有数值恢复到 **100%**（原版默认）。" },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "道路维护" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "车库车队规模" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "每个车库 **最大车辆数** 倍率。\n" +
                    "更高=更多卡车。\n" +
                    "<平衡提示：太少或太多都可能让交通变差。>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "班次容量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "**班次容量** 倍率。\n" +
                    "回到车库前能做的总工作量。\n" +
                    "更高=更少返回。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "修理速度（alpha）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "速度=车辆停下时每tick完成的工作量。\n" +
                    "原版里修路可能需要多次停靠。\n" +
                    "<Alpha：还在真实城市里测试手感。>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "道路磨损速度（alpha）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<Alpha功能：还在测试>\n" +
                    "道路随时间累积磨损的速度。\n" +
                    "**100%** = 原版\n" +
                    "**10%** = 10×更慢（更少修路）\n" +
                    "**400%** = 4×更快（更多修路）" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "重置道路维护" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "把所有数值恢复到 **100%**（原版默认）。" },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "信息" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "支持链接" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "调试 / 日志" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "模组" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "显示的模组名称。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "当前模组版本。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "打开作者的 Paradox Mods 页面。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "在浏览器打开社区 Discord。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "扫描报告（prefab）" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "一次性报告：相关 prefab + 车道磨损数值。\n" +
                    "文件：<ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "别连点；等状态显示 Done 再说。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Prefab 扫描状态" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "显示：Idle / Queued / Running / Done / Failed。\n" +
                    "Queued/Running 显示耗时；Done 显示用时+完成时间。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "详细调试日志" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "开启 = 输出更多信息到 DispatchBoss.log。\n" +
                    "用于排查问题。\n" +
                    "正常游玩建议 **关闭**。\n" +
                    "<如果你不知道这是啥，>\n" +
                    "**就保持关闭**。\n" +
                    "<日志太多会影响性能。>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "打开日志文件夹" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "打开日志文件夹。\n" +
                    "然后用文本编辑器打开 <DispatchBoss.log>（推荐 Notepad++）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "打开报告文件夹" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "打开报告文件夹。\n" +
                    "然后打开 <ScanReport-Prefabs.txt>。" },

                // ---- Status Text (format string templates) ----
                { "DB_SCAN_IDLE", "空闲" },
                { "DB_SCAN_QUEUED_FMT", "排队中 ({0})" },
                { "DB_SCAN_RUNNING_FMT", "运行中 ({0})" },
                { "DB_SCAN_DONE_FMT", "完成 ({0} | {1})" },
                { "DB_SCAN_FAILED", "失败" },
                { "DB_SCAN_FAIL_NO_CITY", "先加载城市" },
                { "DB_SCAN_UNKNOWN_TIME", "未知时间" },
            };
        }

        public void Unload()
        {
        }
    }
}
