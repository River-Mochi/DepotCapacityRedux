// Localization/LocaleZH.cs
// Simplified Chinese (zh-HANS) strings for Options UI.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal;

    public sealed class LocaleZH : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleZH(Setting setting)
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
                { m_Setting.GetSettingsLocaleID(), "调整公共交通运力 [ATC]" },

                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "操作" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),   "关于" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup),
                    "车库容量（每个车库的最大车辆数）" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup),
                    "载客量（每辆车的最大乘客数）" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup),
                    "信息" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup),
                    "支持链接" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup),
                    "调试 / 日志" },
                { m_Setting.GetOptionGroupLocaleID(Setting.LogGroup),
                    "日志文件" },

                // DEPOT labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "公交车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "每座 **公交车车库** 可以维护/生成多少辆公交车。\n" +
                    "使用 **1.0×**（原版）到 **10.0×** 之间的倍率。\n" +
                    "只对**基础建筑**生效，不影响扩展建筑。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "出租车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "每座 **出租车车库** 可以维护多少辆出租车。\n" +
                    "只增加基础建筑。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "有轨电车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "每座 **有轨电车车库** 可以维护多少辆电车。\n" +
                    "只增加基础建筑。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "火车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "每座 **火车车库** 可以维护多少列火车。\n" +
                    "只增加基础建筑。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "地铁车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "每座车库可以维护多少 **地铁车辆**。\n" +
                    "只增加基础建筑。" },

                // Depot reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "重置所有车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "将所有车库倍率重置为 **1.0×**（游戏默认运力——原版）。" },

                // Passenger labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "公交车载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "调整 **公交车乘客座位** 数量。\n" +
                    "**1.0×** = 原版座位数，**10.0×** = 原来的十倍座位数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "有轨电车载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "调整 **有轨电车乘客座位** 数量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "火车载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "调整 **火车乘客座位** 数量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "地铁载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "调整 **地铁乘客座位** 数量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "客船载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "只调整 **客运船只**（不影响货船）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "渡轮载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "调整 **渡轮乘客座位** 数量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "客机载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "调整 **客机乘客座位** 数量。" },

                // Passenger reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "重置所有载客量" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "将所有载客倍率重置为 **1.0×**（游戏默认运力——原版）。" },

                // About tab: info
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)),    "模组" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)),     "此模组的显示名称。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "版本" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)),  "当前模组版本。" },

                // About tab: links
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "在浏览器中打开此模组的 Paradox Mods 页面。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)),
                    "在浏览器中打开社区 Discord。" },

                // About tab: debug
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "启用详细调试日志" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "启用后，许多额外的调试信息会写入 AdjustTransitCapacity.log。\n" +
                    "有助于排查问题，但会产生大量日志。\n" +
                    "建议在正常游戏时 **关闭**。\n" +
                    "<如果你不知道这是做什么用的，请保持关闭，>\n" +
                    "<不要勾选此复选框。>"
                },

                // About tab: log button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "打开日志" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "使用默认文本编辑器打开 ATC 日志文件。" },
            };
        }

        public void Unload()
        {
        }
    }
}
