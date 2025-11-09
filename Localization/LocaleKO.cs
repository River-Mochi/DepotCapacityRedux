// Localization/LocaleKO.cs
// Korean (ko-KR) strings for Options UI.

namespace AdjustTransitCapacity
{
    using System.Collections.Generic;
    using Colossal;

    public sealed class LocaleKO : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleKO(Setting setting)
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
                { m_Setting.GetSettingsLocaleID(), "대중교통 수용량 조정 [ATC]" },

                { m_Setting.GetOptionTabLocaleID(Setting.ActionsTab), "작업" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),   "정보" },

                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup),
                    "차고 용량 (차고당 최대 차량 수)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup),
                    "승객 수용량 (차량당 최대 인원 수)" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup),
                    "정보" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup),
                    "지원 링크" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup),
                    "디버그 / 로그" },
                { m_Setting.GetOptionGroupLocaleID(Setting.LogGroup),
                    "로그 파일" },

                // DEPOT labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "버스 차고" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "각 **버스 차고** 건물이 유지/생성할 수 있는 버스 수.\n" +
                    "**1.0×**(바닐라)에서 **10.0×** 사이의 배수를 사용하세요.\n" +
                    "**기본 건물**에만 적용되며, 확장 건물에는 적용되지 않습니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "택시 차고" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "각 **택시 차고**가 유지할 수 있는 택시 수.\n" +
                    "기본 건물에만 증가가 적용됩니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "트램 차고" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "각 **트램 차고**가 유지할 수 있는 트램 수.\n" +
                    "기본 건물에만 증가가 적용됩니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "기차 차고" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "각 **기차 차고**가 유지할 수 있는 기차 수.\n" +
                    "기본 건물에만 증가가 적용됩니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "지하철 차고" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "각 차고가 유지할 수 있는 **지하철 차량** 수.\n" +
                    "기본 건물에만 증가가 적용됩니다." },

                // Depot reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "모든 차고 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "모든 차고 배수를 **1.0×**(게임 기본 수용량 – 바닐라)로 되돌립니다." },

                // Passenger labels & descriptions (1.0–10.0x)
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "버스 승객" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "**버스 승객** 좌석 수를 변경합니다.\n" +
                    "**1.0×** = 바닐라 좌석 수, **10.0×** = 10배 좌석 수." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "트램 승객" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "**트램 승객** 좌석 수를 변경합니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "기차 승객" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "**기차 승객** 좌석 수를 변경합니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "지하철 승객" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "**지하철 승객** 좌석 수를 변경합니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "여객선 승객" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "**여객선**만 변경합니다 (화물선 제외)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "페리 승객" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "**페리 승객** 좌석 수를 변경합니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "항공기 승객" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "**항공기 승객** 좌석 수를 변경합니다." },

                // Passenger reset button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "모든 승객 설정 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "모든 승객 배수를 **1.0×**(게임 기본 수용량 – 바닐라)로 되돌립니다." },

                // About tab: info
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)),    "모드" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)),     "이 모드의 표시 이름입니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "버전" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)),  "현재 모드 버전입니다." },

                // About tab: links
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)),
                    "이 모드의 Paradox Mods 페이지를 엽니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)),
                    "브라우저에서 커뮤니티 Discord를 엽니다." },

                // About tab: debug
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "자세한 디버그 로그 활성화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "활성화하면 많은 추가 디버그 정보가 AdjustTransitCapacity.log에 기록됩니다.\n" +
                    "문제 해결에 유용하지만 로그가 매우 많아집니다.\n" +
                    "일반적인 플레이에서는 **비활성화**를 권장합니다.\n" +
                    "<이 옵션이 무엇을 하는지 모른다면 비활성화 상태로> \n " +
                    "<두고 체크하지 마세요.>"
                },

                // About tab: log button
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "로그 열기" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "기본 텍스트 편집기로 ATC 로그 파일을 엽니다." },
            };
        }

        public void Unload()
        {
        }
    }
}
