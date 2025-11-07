// LocaleZH.cs
// Simplified Chinese (zh-HANS) for Options UI.

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
                // ---- MOD TITLE / TAB / GROUPS ----
                { m_Setting.GetSettingsLocaleID(), "公共交通容量调整" },
                { m_Setting.GetOptionTabLocaleID(Setting.MainTab), "主要" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "车库容量（每个车库的最大车辆数）" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "乘客容量（每辆车的乘客数）" },

                // ---- DEPOT LABELS & DESCRIPTIONS ----
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotPercent)), "公交车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotPercent)), "每个公交车车库可维护/生成的公交车数量。100% = 原版，1000% = 10 倍。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotPercent)), "出租车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotPercent)), "每个出租车车库可维护的出租车数量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotPercent)), "有轨电车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotPercent)), "每个有轨电车车库可维护的电车数量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotPercent)), "火车车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotPercent)), "每个火车车库可维护的列车数量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotPercent)), "地铁车库" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotPercent)), "每个地铁车库可维护的地铁车辆数量。" },

                // ---- PASSENGER LABELS & DESCRIPTIONS ----
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerPercent)), "公交车乘客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerPercent)), "公交车座位数量倍率。100% = 原版座位，1000% = 10 倍座位。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerPercent)), "有轨电车乘客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerPercent)), "有轨电车座位数量倍率。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerPercent)), "火车乘客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerPercent)), "火车座位数量倍率。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerPercent)), "地铁乘客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerPercent)), "地铁座位数量倍率。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerPercent)), "客运船乘客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerPercent)), "仅对客运船生效的倍率（不影响货船）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerPercent)), "渡轮乘客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerPercent)), "渡轮乘客容量倍率。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerPercent)), "飞机乘客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerPercent)), "客机乘客容量倍率。" },
            };
        }

        public void Unload()
        {
        }
    }
}
