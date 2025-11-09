// Localization/LocaleJA.cs
// Japanese (ja-JP) strings for Options UI.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal;

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
            return new Dictionary<string, string>
            {
                // Mod Title / Tabs / Groups
                { m_Setting.GetSettingsLocaleID(), "公共交通の収容力を調整 [ATC]" },

                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "操作" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),   "概要" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup),
                    "車庫の容量（車庫ごとの最大車両数）" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup),
                    "乗客容量（車両ごとの最大人数）" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup),
                    "情報" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup),
                    "サポートリンク" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup),
                    "デバッグ / ログ" },
                { m_Setting.GetOptionGroupLocaleID(Setting.LogGroup),
                    "ログファイル" },

                // DEPOT labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "バス車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "各 **バス車庫** 建物が整備／生成できるバスの台数。\n" +
                    "**1.0×**（バニラ）から **10.0×** の倍率を使用します。\n" +
                    "拡張ではなく、**基礎建物**のみを倍増します。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "タクシー車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "各 **タクシー車庫** が整備できるタクシーの台数。\n" +
                    "基礎建物のみ増加します。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "トラム車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "各 **トラム車庫** が整備できるトラムの台数。\n" +
                    "基礎建物のみ増加します。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "鉄道車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "各 **鉄道車庫** が整備できる列車の本数。\n" +
                    "基礎建物のみ増加します。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "地下鉄車庫" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "各車庫が整備できる **地下鉄車両** の数。\n" +
                    "基礎建物のみ増加します。" },

                // Depot reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "すべての車庫をリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "すべての車庫倍率を **1.0×**（ゲームのデフォルト容量＝バニラ）に戻します。" },

                // Passenger labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "バス乗客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "**バス乗客** の座席数を変更します。\n" +
                    "**1.0×** = バニラの座席数、**10.0×** = 10倍の座席数。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "トラム乗客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "**トラム乗客** の座席数を変更します。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "列車乗客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "**列車乗客** の座席数を変更します。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "地下鉄乗客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "**地下鉄乗客** の座席数を変更します。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "旅客船乗客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "**旅客船** のみ変更します（貨物船は対象外）。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "フェリー乗客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "**フェリー乗客** の座席数を変更します。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "旅客機乗客" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "**旅客機乗客** の座席数を変更します。" },

                // Passenger reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "すべての乗客設定をリセット" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "すべての乗客倍率を **1.0×**（ゲームのデフォルト容量＝バニラ）に戻します。" },

                // About tab: info
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)),    "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)),     "この Mod の表示名。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "バージョン" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)),  "現在の Mod バージョン。" },

                // About tab: links
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "この Mod の Paradox Mods ページを開きます。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)),
                    "ブラウザでコミュニティ Discord を開きます。" },

                // About tab: debug
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "詳細なデバッグログを有効化" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "有効にすると、多くの追加デバッグ情報が AdjustTransitCapacity.log に出力されます。\n" +
                    "トラブルシューティングに便利ですが、ログが大量になります。\n" +
                    "通常のプレイでは **無効** にすることを推奨します。\n" +
                    "<これが何のためのオプションか分からない場合は> \n" +
                    "<有効にせずチェックを入れないでください。>"
                },

                // About tab: log button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "ログを開く" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "既定のテキストエディタで ATC ログファイルを開きます。" },
            };
        }

        public void Unload()
        {
        }
    }
}
