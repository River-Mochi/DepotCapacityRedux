// File: Localization/LocaleKO.cs
// Korean (ko-KR) strings for Options UI.

namespace DispatchBoss
{
    using Colossal;
    using System.Collections.Generic;

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
                { m_Setting.GetOptionTabLocaleID(Setting.PublicTransitTab), "대중교통" },
                { m_Setting.GetOptionTabLocaleID(Setting.IndustryTab),      "산업" },
                { m_Setting.GetOptionTabLocaleID(Setting.ParksRoadsTab),    "공원-도로" },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab),         "정보" },

                // --------------------
                // Public-Transit tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.LineVehiclesGroup), "노선 (게임 내 차량 슬라이더 제한)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableLineVehicleCountTuner)), "노선 차량 슬라이더 제한 확장" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableLineVehicleCountTuner)),
                    "대부분 노선에서 슬라이더 최소값을 **1대**까지 허용합니다.\n" +
                    "**최대값도 증가할 수 있음** (게임의 노선 시간 로직 유지).\n" +
                    "게임은 노선 시간(주행 + 정류장 수)을 사용하므로 최대값은 노선마다 달라요.\n" +
                    "<충돌 방지: 같은 노선 정책을 건드리는 모드는 제거하세요>.\n" +
                    "다른 정책 모드를 쓸 거면 이 체크는 OFF.\n" +
                    "대상: 버스, 트램, 기차, 지하철, 선박, 페리, 비행기.\n" +
                    "팁: 기본 최대값보다 더 원하면 정류장을 몇 개 추가해 보세요." +
                    " 게임이 최대 슬라이더를 올려줄 때가 많아요."
                },

                // Depot Capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.DepotGroup), "차고지 용량 (차고지당 최대 차량)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusDepotScalar)), "버스 차고지" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusDepotScalar)),
                    "**버스 차고지**가 유지/스폰 가능한 버스 수를 변경합니다.\n" +
                    "**100%** = 기본(바닐라).\n" +
                    "**1000%** = 10×.\n" +
                    "건물 기본 용량에 적용됩니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TaxiDepotScalar)), "택시 차고지" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TaxiDepotScalar)),
                    "**택시 차고지**가 유지 가능한 택시 수.\n" +
                    "기본 건물에만 적용됩니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramDepotScalar)), "트램 차고지" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramDepotScalar)),
                    "**트램 차고지**가 유지 가능한 트램 수.\n" +
                    "건물 기본 용량에 적용됩니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainDepotScalar)), "기차 차고지" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainDepotScalar)),
                    "**기차 차고지**가 유지 가능한 기차 수.\n" +
                    "건물 기본 용량에 적용됩니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayDepotScalar)), "지하철 차고지" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayDepotScalar)),
                    "**지하철 차고지**가 유지 가능한 차량 수.\n" +
                    "건물 기본 용량에 적용됩니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDepotToVanillaButton)), "모든 차고지 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDepotToVanillaButton)),
                    "모든 차고지 슬라이더를 **100%**(기본/바닐라)로 되돌립니다." },

                // Passenger capacity sliders
                { m_Setting.GetOptionGroupLocaleID(Setting.PassengerGroup), "승객 수용량 (차량당 최대 인원)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BusPassengerScalar)), "버스" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BusPassengerScalar)),
                    "**버스 승객** 수용량을 변경합니다.\n" +
                    "**10%** = 기본 좌석의 10%.\n" +
                    "**100%** = 기본(바닐라).\n" +
                    "**1000%** = 10× 좌석." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TramPassengerScalar)), "트램" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TramPassengerScalar)),
                    "**트램 승객** 수용량.\n" +
                    "**100%** = 기본(바닐라)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainPassengerScalar)), "기차" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainPassengerScalar)),
                    "**기차 승객** 수용량.\n" +
                    "기관차 + 객차에 적용됩니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SubwayPassengerScalar)), "지하철" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SubwayPassengerScalar)),
                    "**지하철 승객** 수용량.\n" +
                    "**100%** = 기본(바닐라)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipPassengerScalar)), "선박" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipPassengerScalar)),
                    "**여객선** 수용량(화물선 제외).\n" +
                    "**100%** = 기본(바닐라)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FerryPassengerScalar)), "페리" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FerryPassengerScalar)),
                    "**페리 승객** 수용량.\n" +
                    "**100%** = 기본(바닐라)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirplanePassengerScalar)), "비행기" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirplanePassengerScalar)),
                    "**비행기 승객** 수용량.\n" +
                    "**100%** = 기본(바닐라)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DoublePassengersButton)), "두 배" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DoublePassengersButton)),
                    "모든 승객 슬라이더를 **200%**로 설정합니다.\n" +
                    "버스/트램/기차/지하철/선박/페리/비행기에 적용." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetPassengerToVanillaButton)), "모든 승객 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetPassengerToVanillaButton)),
                    "모든 승객 슬라이더를 **100%**(기본/바닐라)로 되돌립니다." },

                // ----------------
                // INDUSTRY tab
                // ----------------

                { m_Setting.GetOptionGroupLocaleID(Setting.DeliveryGroup), "배송 차량 (화물 용량)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SemiTruckCargoScalar)), "대형 트럭(세미)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SemiTruckCargoScalar)),
                    "**세미 트럭** 용량.\n" +
                    "특화 산업 세미(농장/임업/어업 등) 포함.\n" +
                    "배수: **1×** = 기본(**25t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryVanCargoScalar)), "배송 밴" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryVanCargoScalar)),
                    "**배송 밴**\n" +
                    "배수: **1×** = 기본(**4t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OilTruckCargoScalar)), "원자재 트럭(석유, 석탄, 광석, 돌)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OilTruckCargoScalar)),
                    "**원자재 트럭**(석유/석탄/광석/돌)\n" +
                    "배수: **1×** = 기본(**20t**), **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)), "오토바이" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MotorbikeDeliveryCargoScalar)),
                    "**오토바이 배송**\n" +
                    "배수: **1×** = 기본, **10×** = 10×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)), "배송 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetDeliveryToVanillaButton)),
                    "배송 배수를 **1×**(기본/바닐라)로 되돌립니다." },

                { m_Setting.GetOptionGroupLocaleID(Setting.CargoStationsGroup), "화물 플릿(항만, 철도, 공항)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)), "화물 시설 최대 플릿" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CargoStationMaxTrucksScalar)),
                    "**화물 운송 시설**의 최대 활성 운송수단 배수.\n" +
                    "**1×** = 기본, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)), "추출 시설 플릿" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ExtractorMaxTrucksScalar)),
                    "산업 **추출 시설 최대 트럭** 배수\n" +
                    "(농장/임업/어업/광석/석유 + 석탄/돌 가능 시).\n" +
                    "**1×** = 기본, **5×** = 5×." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)), "화물 + 추출 플릿 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetCargoStationsToVanillaButton)),
                    "화물 시설 + 추출 시설 배수를 **1×**(기본/바닐라)로 되돌립니다." },

                // -------------------
                // Parks-Roads
                // -------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.ParkMaintenanceGroup), "공원 유지보수" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)), "작업량(턴) 용량" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleCapacityScalar)),
                    "**작업량 용량**(차량 용량) 배수.\n" +
                    "돌아가기 전까지 처리 가능한 총 작업.\n" +
                    "물자 더 많음 = 밖에서 더 오래." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)), "작업 속도" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicleRateScalar)),
                    "**작업 속도** 배수.\n" +
                    "속도 = 정지 중 tick당 처리량." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)), "차고지 플릿" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceDepotScalar)),
                    "차고지 **최대 차량 수** 배수.\n" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)), "공원 유지보수 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetParkMaintenanceToVanillaButton)),
                    "모두 **100%**(기본/바닐라)로 되돌립니다." },

                { m_Setting.GetOptionGroupLocaleID(Setting.RoadMaintenanceGroup), "도로 유지보수" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)), "차고지 플릿" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceDepotScalar)),
                    "차고지당 **최대 차량 수** 배수.\n" +
                    "높을수록 = 더 많은 차량.\n" +
                    "<밸런스: 너무 적거나 너무 많으면 교통에 악영향.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)), "작업량(턴) 용량" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleCapacityScalar)),
                    "**작업량 용량** 배수.\n" +
                    "돌아가기 전까지 처리 가능한 총 작업.\n" +
                    "높을수록 = 복귀 횟수 감소." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)), "수리 속도(알파)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicleRateScalar)),
                    "속도 = 정지 중 tick당 처리량.\n" +
                    "바닐라에선 손상 정도에 따라 여러 번 멈출 수 있어요.\n" +
                    "<알파: 실제 도시에서 체감 테스트 중.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWearScalar)), "도로 마모 속도(알파)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWearScalar)),
                    "<알파 기능: 아직 테스트 중>\n" +
                    "시간에 따라 도로가 마모되는 속도.\n" +
                    "**100%** = 기본\n" +
                    "**10%** = 10× 느림(수리 덜 필요)\n" +
                    "**400%** = 4× 빠름(수리 더 필요)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)), "도로 유지보수 초기화" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetRoadMaintenanceToVanillaButton)),
                    "모두 **100%**(기본/바닐라)로 되돌립니다." },

                // --------------------
                // About tab
                // --------------------

                { m_Setting.GetOptionGroupLocaleID(Setting.AboutInfoGroup), "정보" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutLinksGroup), "링크" },
                { m_Setting.GetOptionGroupLocaleID(Setting.DebugGroup), "디버그 / 로그" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModNameDisplay)), "모드" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModNameDisplay)), "모드 표시 이름." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModVersionDisplay)), "버전" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModVersionDisplay)), "현재 모드 버전." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenParadoxMods)), "Paradox" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenParadoxMods)), "작성자의 Paradox Mods 페이지 열기." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenDiscord)), "Discord" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenDiscord)), "커뮤니티 Discord 열기." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RunPrefabScanButton)), "스캔 리포트(prefab)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RunPrefabScanButton)),
                    "1회 리포트: 관련 prefab + 레인 마모 값.\n" +
                    "파일: <ModsData/DispatchBoss/ScanReport-Prefabs.txt>\n" +
                    "연타 금지; 상태가 Done 될 때까지 기다리세요." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrefabScanStatus)), "Prefab 스캔 상태" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrefabScanStatus)),
                    "스캔 상태 표시: 대기 / 대기열 / 실행 중 / 완료 / 실패.\n" +
                    "대기열/실행 중은 경과 시간, 완료는 소요 시간+완료 시간을 표시합니다." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableDebugLogging)), "상세 디버그 로그" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableDebugLogging)),
                    "ON = DispatchBoss.log에 더 많은 정보 출력.\n" +
                    "문제 해결에 유용.\n" +
                    "일반 플레이는 **OFF** 추천.\n" +
                    "<잘 모르겠으면>\n" +
                    "**OFF 유지**.\n" +
                    "<로그가 많으면 성능에 영향.>" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenLogButton)), "로그 폴더 열기" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenLogButton)),
                    "로그 폴더를 엽니다.\n" +
                    "그다음 <DispatchBoss.log>를 텍스트 편집기로 여세요(Notepad++ 추천)." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OpenReportButton)), "리포트 폴더 열기" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OpenReportButton)),
                    "리포트 폴더를 엽니다.\n" +
                    "그다음 <ScanReport-Prefabs.txt>를 여세요." },

                // ---- Status Text (format string templates) ----
                { "DB_SCAN_IDLE", "대기" },
                { "DB_SCAN_QUEUED_FMT", "대기열 ({0})" },
                { "DB_SCAN_RUNNING_FMT", "실행 중 ({0})" },
                { "DB_SCAN_DONE_FMT", "완료 ({0} | {1})" },
                { "DB_SCAN_FAILED", "실패" },
                { "DB_SCAN_FAIL_NO_CITY", "도시를 먼저 로드" },
                { "DB_SCAN_UNKNOWN_TIME", "시간 알 수 없음" },
            };
        }

        public void Unload()
        {
        }
    }
}
