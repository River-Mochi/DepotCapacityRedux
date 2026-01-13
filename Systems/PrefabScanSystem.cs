// File: Systems/PrefabScanSystem.cs
// Purpose: One-shot prefab scan triggered by OptionsUI button.
// Output: Writes report to {EnvPath.kUserDataPath}/ModsData/DispatchBoss/ScanReport-Prefabs.txt
// Notes:
// - Runs only when requested (PrefabScanState.RequestScan()).
// - Uses SystemAPI.Query + SystemAPI.QueryBuilder.
// - Deduped + capped to prevent giant outputs and logger issues.
// - Logs only a summary line to the mod log (no spam).

namespace DispatchBoss
{
    using Colossal.PSI.Environment; // EnvPath
    using Game;
    using Game.Companies;           // TransportCompanyData
    using Game.Net;                 // LaneCondition (LIVE lanes)
    using Game.Prefabs;             // PrefabSystem, PrefabBase, *Data, CarTrailerType, PrefabRef
    using Game.Routes;              // RouteModifierData, RouteModifierType, TransportType
    using Game.SceneFlow;           // GameManager
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;       // Stopwatch
    using System.IO;
    using System.Text;
    using Unity.Entities;

    public sealed partial class PrefabScanSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem = null!;
        private EntityQuery m_ConfigQuery;

        private const int kMaxLines = 10000;
        private const int kMaxChars = 1 * 1024 * 1024; // ~1MB
        private const int kMaxKeywordMatches = 600;     // keywords are hints only

        private struct TransitDefaultsStats
        {
            public int Count;
            public float MinInterval;
            public float MaxInterval;
            public float MinStop;
            public float MaxStop;

            public void InitFrom(TransportLineData d)
            {
                Count = 1;
                MinInterval = d.m_DefaultVehicleInterval;
                MaxInterval = d.m_DefaultVehicleInterval;
                MinStop = d.m_StopDuration;
                MaxStop = d.m_StopDuration;
            }

            public void Add(TransportLineData d)
            {
                Count++;

                float interval = d.m_DefaultVehicleInterval;
                float stop = d.m_StopDuration;

                if (interval < MinInterval) MinInterval = interval;
                if (interval > MaxInterval) MaxInterval = interval;

                if (stop < MinStop) MinStop = stop;
                if (stop > MaxStop) MaxStop = stop;
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            // Cached query via SystemAPI.QueryBuilder
            m_ConfigQuery = SystemAPI.QueryBuilder()
                .WithAll<UITransportConfigurationData>()
                .Build();

            RequireForUpdate(SystemAPI.QueryBuilder().WithAll<PrefabData>().Build());
            Enabled = false;
        }

        protected override void OnUpdate()
        {
            if (PrefabScanState.CurrentPhase != PrefabScanState.Phase.Requested)
            {
                Enabled = false;
                return;
            }

            GameManager gm = GameManager.instance;
            if (gm == null || !gm.gameMode.IsGame())
            {
                PrefabScanState.MarkFailed(PrefabScanState.FailCode.NoCityLoaded, null);
                Enabled = false;
                return;
            }

            PrefabScanState.MarkRunning();

            var sw = Stopwatch.StartNew();

            int transitLinePrefabTotal = 0;
            int deliveryTotal = 0;
            int mvTotal = 0;
            int depotTotal = 0;
            int cargoTotal = 0;
            int laneTotal = 0;
            int extractorCompanies = 0;
            int keywordMatches = 0;

            try
            {
                var sb = new StringBuilder(256 * 1024);
                int lines = 0;
                bool truncated = false;

                void Append(string line)
                {
                    if (truncated)
                        return;

                    if (lines >= kMaxLines || sb.Length >= kMaxChars)
                    {
                        truncated = true;
                        sb.AppendLine("!! TRUNCATED: Output hit cap (lines or size). Reduce scope (keywords/detail).");
                        lines++;
                        return;
                    }

                    sb.AppendLine(line);
                    lines++;
                }

                string NameOf(Entity e) => GetPrefabNameSafe(e);

                // NEW: “prove coverage” diagnostic — counts LIVE lanes per lane-prefab.
                void AppendLiveLaneUsage()
                {
                    if (truncated)
                        return;

                    Append("");
                    Append("== Live lane usage (LaneCondition + PrefabRef) ==");
                    Append("Counts live lane entities grouped by PrefabRef.m_Prefab (lane prefab).");
                    Append("This is the proof that a small set of lane prefabs can power many road types.");
                    Append("");

                    // Set of lane prefabs that actually have LaneDeteriorationData (the ones we tweak).
                    var wearPrefabs = new HashSet<Entity>();

                    foreach ((RefRO<LaneDeteriorationData> detRO, Entity prefabEntity) in SystemAPI
                                 .Query<RefRO<LaneDeteriorationData>>()
                                 .WithAll<PrefabData>()
                                 .WithEntityAccess())
                    {
                        wearPrefabs.Add(prefabEntity);
                    }

                    var counts = new Dictionary<Entity, int>(64);
                    long liveLaneTotal = 0;

                    // LIVE lanes: LaneCondition + PrefabRef, but NOT PrefabData.
                    foreach (RefRO<PrefabRef> prefabRefRO in SystemAPI
                                 .Query<RefRO<PrefabRef>>()
                                 .WithAll<LaneCondition>()
                                 .WithNone<PrefabData>())
                    {
                        Entity prefab = prefabRefRO.ValueRO.m_Prefab;
                        liveLaneTotal++;

                        if (counts.TryGetValue(prefab, out int c))
                            counts[prefab] = c + 1;
                        else
                            counts[prefab] = 1;
                    }

                    if (liveLaneTotal == 0 || counts.Count == 0)
                    {
                        Append("No live lanes found (unexpected).");
                        return;
                    }

                    long covered = 0;
                    foreach (var kvp in counts)
                    {
                        if (wearPrefabs.Contains(kvp.Key))
                            covered += kvp.Value;
                    }

                    float pct = (float)covered * 100f / (float)liveLaneTotal;

                    Append($"Live lanes summary: LiveLanes={liveLaneTotal:n0} UniqueLanePrefabs={counts.Count:n0}");
                    Append($"Coverage by LaneDeteriorationData prefabs: {covered:n0}/{liveLaneTotal:n0} ({pct:0.0}%)");
                    Append("");

                    // Print top N most-used lane prefabs (by live lane count)
                    const int kTop = 30;

                    var top = new List<KeyValuePair<Entity, int>>(counts);
                    top.Sort((a, b) => b.Value.CompareTo(a.Value));

                    int printed = 0;
                    for (int i = 0; i < top.Count && printed < kTop; i++)
                    {
                        var kvp = top[i];
                        string name = NameOf(kvp.Key);
                        bool isWear = wearPrefabs.Contains(kvp.Key);

                        Append($"- {name} ({kvp.Key.Index}:{kvp.Key.Version}) UsedByLanes={kvp.Value:n0} WearPrefab={isWear}");
                        printed++;
                    }
                }

                Append("Dispatch Boss: Prefab Scan Report");
                Append($"Timestamp (local): {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                Append($"Mod: {Mod.ModName} {Mod.ModVersion}");
                Append("");

                // ---- Lane wear ----
                const float kUpdatesPerDay = 16f;
                const int kMaxLaneDetails = 250; // cap printed per-prefab lines

                Append("== Lane wear (LaneDeteriorationData prefabs) ==");
                Append("Wear sources:");
                Append("- Time wear: LaneCondition.m_Wear += (1/16) * TimeFactor per deterioration tick.");
                Append("- Traffic wear: Car/Train Navigation adds SideEffects.x * TrafficFactor when vehicles traverse lanes.");
                Append("");

                int laneListed = 0;

                float minTf = float.MaxValue, maxTf = float.MinValue;
                float minTraf = float.MaxValue, maxTraf = float.MinValue;

                foreach ((RefRO<LaneDeteriorationData> laneRef, Entity e) in SystemAPI
                             .Query<RefRO<LaneDeteriorationData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    if (truncated) break;

                    laneTotal++;

                    LaneDeteriorationData cur = laneRef.ValueRO;
                    float tf = cur.m_TimeFactor;
                    float traf = cur.m_TrafficFactor;

                    if (tf < minTf) minTf = tf;
                    if (tf > maxTf) maxTf = tf;

                    if (traf < minTraf) minTraf = traf;
                    if (traf > maxTraf) maxTraf = traf;

                    if (laneListed < kMaxLaneDetails)
                    {
                        float vanTf = float.NaN;
                        float vanTraf = float.NaN;

                        if (m_PrefabSystem.TryGetPrefab(e, out PrefabBase pb) &&
                            pb.TryGet(out Game.Prefabs.LaneDeterioration author))
                        {
                            vanTf = author.m_TimeDeterioration;
                            vanTraf = author.m_TrafficDeterioration;
                        }

                        float xTime = (!float.IsNaN(vanTf) && vanTf > 0f) ? (tf / vanTf) : float.NaN;
                        float xTraf = (!float.IsNaN(vanTraf) && vanTraf > 0f) ? (traf / vanTraf) : float.NaN;

                        float expPerTick = tf / kUpdatesPerDay;

                        Append(
                            $"- {NameOf(e)} ({e.Index}:{e.Version}) " +
                            $"Vanilla(Time={Fmt(vanTf)}, Traffic={Fmt(vanTraf)}) " +
                            $"Current(Time={tf:0.###}, Traffic={traf:0.###}) " +
                            $"xTime={Fmt(xTime)} xTraffic={Fmt(xTraf)} ExpΔ(Time)/Tick={expPerTick:0.###}");

                        laneListed++;
                    }
                }

                if (laneTotal > laneListed)
                {
                    Append($"(details capped) Printed={laneListed} of Total={laneTotal} (cap={kMaxLaneDetails}).");
                }

                Append("");
                Append(laneTotal > 0
                    ? $"Lane wear summary: Total={laneTotal} TimeFactor(min={minTf:0.###}, max={maxTf:0.###}) TrafficFactor(min={minTraf:0.###}, max={maxTraf:0.###})"
                    : "Lane wear summary: Total=0");
                Append("");

                // NEW: Prove Coverage section
                AppendLiveLaneUsage();

                // ---- Transit lines ----
                Append("== Transit lines (vanilla timing inputs) ==");
                Append("Vehicle targets are based on route time estimate (segment durations + stop count).");
                Append("");

                var perType = new Dictionary<TransportType, TransitDefaultsStats>();

                foreach ((RefRO<TransportLineData> lineRef, Entity e) in SystemAPI
                             .Query<RefRO<TransportLineData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    if (truncated) break;

                    transitLinePrefabTotal++;

                    TransportLineData d = lineRef.ValueRO;
                    TransportType type = d.m_TransportType;

                    if (!perType.TryGetValue(type, out TransitDefaultsStats stats))
                    {
                        stats = default;
                        stats.InitFrom(d);
                        perType[type] = stats;
                    }
                    else
                    {
                        stats.Add(d);
                        perType[type] = stats;
                    }
                }

                Append("-- TransportLineData prefab defaults (by TransportType) --");
                if (perType.Count == 0)
                {
                    Append("No TransportLineData prefabs found.");
                }
                else
                {
                    foreach (var kvp in perType)
                    {
                        TransportType type = kvp.Key;
                        TransitDefaultsStats s2 = kvp.Value;

                        Append(
                            $"- {type}: Count={s2.Count} " +
                            $"DefaultVehicleInterval(min={s2.MinInterval:0.###}, max={s2.MaxInterval:0.###}) " +
                            $"StopDuration(min={s2.MinStop:0.###}, max={s2.MaxStop:0.###})");
                    }
                }

                Append("");

                // ---- VehicleCountPolicy ----
                Append("-- VehicleCountPolicy (RouteModifierType.VehicleInterval) --");
                if (m_ConfigQuery.IsEmptyIgnoreFilter)
                {
                    Append("UITransportConfigurationData not found; policy entity could not be resolved.");
                }
                else
                {
                    UITransportConfigurationPrefab config =
                        m_PrefabSystem.GetSingletonPrefab<UITransportConfigurationPrefab>(m_ConfigQuery);

                    Entity policyEntity = m_PrefabSystem.GetEntity(config.m_VehicleCountPolicy);

                    if (policyEntity == Entity.Null || !SystemAPI.Exists(policyEntity))
                    {
                        Append("VehicleCountPolicy entity could not be resolved.");
                    }
                    else if (!SystemAPI.HasBuffer<RouteModifierData>(policyEntity))
                    {
                        Append("VehicleCountPolicy has no RouteModifierData buffer.");
                    }
                    else
                    {
                        DynamicBuffer<RouteModifierData> buf = SystemAPI.GetBuffer<RouteModifierData>(policyEntity);
                        bool foundVehicleInterval = false;

                        for (int i = 0; i < buf.Length; i++)
                        {
                            RouteModifierData item = buf[i];
                            if (item.m_Type != RouteModifierType.VehicleInterval)
                                continue;

                            foundVehicleInterval = true;
                            Append($"Mode={item.m_Mode} Range(input)={item.m_Range.min:0.###}..{item.m_Range.max:0.###}");

                            if (item.m_Mode == ModifierValueMode.InverseRelative)
                            {
                                float appliedMin = InverseRelativeAppliedFromInput(item.m_Range.min);
                                float appliedMax = InverseRelativeAppliedFromInput(item.m_Range.max);
                                Append($"Range(appliedΔ @ endpoints) inputMin→{appliedMin:0.###}, inputMax→{appliedMax:0.###}");
                            }

                            break;
                        }

                        if (!foundVehicleInterval)
                        {
                            Append("No VehicleInterval modifier found in VehicleCountPolicy.");
                        }
                    }
                }

                Append("");

                // ---- Delivery trucks ----
                int semi = 0, van = 0, raw = 0, bike = 0, other = 0;

                var tractorLookup = SystemAPI.GetComponentLookup<CarTractorData>(isReadOnly: true);
                var trailerLookup = SystemAPI.GetComponentLookup<CarTrailerData>(isReadOnly: true);

                Append("== DeliveryTruckData Prefabs ==");
                foreach ((RefRO<DeliveryTruckData> truckRef, Entity e) in SystemAPI
                             .Query<RefRO<DeliveryTruckData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    if (truncated) break;

                    deliveryTotal++;
                    DeliveryTruckData dt = truckRef.ValueRO;

                    int vanillaCap = dt.m_CargoCapacity;
                    if (m_PrefabSystem.TryGetPrefab(e, out PrefabBase pb) &&
                        pb.TryGet(out Game.Prefabs.DeliveryTruck baseTruck))
                    {
                        vanillaCap = baseTruck.m_CargoCapacity;
                    }

                    VehicleHelpers.GetTrailerTypeInfo(
                        tractorLookup,
                        trailerLookup,
                        e,
                        out bool hasTractor,
                        out CarTrailerType tractorType,
                        out bool hasTrailer,
                        out CarTrailerType trailerType);

                    VehicleHelpers.DeliveryBucket bucket = VehicleHelpers.ClassifyDeliveryTruckPrefab(
                        NameOf(e),
                        vanillaCap,
                        dt.m_TransportedResources,
                        hasTractor,
                        tractorType,
                        hasTrailer,
                        trailerType);

                    switch (bucket)
                    {
                        case VehicleHelpers.DeliveryBucket.Semi: semi++; break;
                        case VehicleHelpers.DeliveryBucket.Van: van++; break;
                        case VehicleHelpers.DeliveryBucket.RawMaterials: raw++; break;
                        case VehicleHelpers.DeliveryBucket.Motorbike: bike++; break;
                        default: other++; break;
                    }

                    Append($"- {NameOf(e)} ({e.Index}:{e.Version}) Bucket={bucket} VanillaCap={vanillaCap} CurCap={dt.m_CargoCapacity} Resources={dt.m_TransportedResources} Tractor={hasTractor}:{tractorType} Trailer={hasTrailer}:{trailerType}");
                }

                Append($"Delivery summary: Total={deliveryTotal} Semi={semi} Van={van} Raw={raw} Motorbike={bike} Other={other}");
                Append("");

                // ---- Maintenance vehicles ----
                Append("== MaintenanceVehicleData Prefabs ==");
                foreach ((RefRO<MaintenanceVehicleData> mvRef, Entity e) in SystemAPI
                             .Query<RefRO<MaintenanceVehicleData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    if (truncated) break;

                    mvTotal++;
                    MaintenanceVehicleData mv = mvRef.ValueRO;

                    int vanillaCap = mv.m_MaintenanceCapacity;
                    int vanillaRate = mv.m_MaintenanceRate;

                    if (m_PrefabSystem.TryGetPrefab(e, out PrefabBase pb) &&
                        pb.TryGet(out Game.Prefabs.MaintenanceVehicle baseMv))
                    {
                        vanillaCap = baseMv.m_MaintenanceCapacity;
                        vanillaRate = baseMv.m_MaintenanceRate;
                    }

                    Append($"- {NameOf(e)} ({e.Index}:{e.Version}) Type={mv.m_MaintenanceType} VanillaCap={vanillaCap} CurCap={mv.m_MaintenanceCapacity} VanillaRate={vanillaRate} CurRate={mv.m_MaintenanceRate}");
                }
                Append($"MaintenanceVehicle summary: Total={mvTotal}");
                Append("");

                // ---- Maintenance depots ----
                Append("== MaintenanceDepotData Prefabs ==");
                foreach ((RefRO<MaintenanceDepotData> depotRef, Entity e) in SystemAPI
                             .Query<RefRO<MaintenanceDepotData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    if (truncated) break;

                    depotTotal++;
                    MaintenanceDepotData md = depotRef.ValueRO;

                    int vanillaVehicles = md.m_VehicleCapacity;
                    if (m_PrefabSystem.TryGetPrefab(e, out PrefabBase pb) &&
                        pb.TryGet(out Game.Prefabs.MaintenanceDepot baseDepot))
                    {
                        vanillaVehicles = baseDepot.m_VehicleCapacity;
                    }

                    Append($"- {NameOf(e)} ({e.Index}:{e.Version}) Type={md.m_MaintenanceType} VanillaVehicles={vanillaVehicles} CurVehicles={md.m_VehicleCapacity}");
                }
                Append($"MaintenanceDepot summary: Total={depotTotal}");
                Append("");

                // ---- Cargo stations ----
                Append("== Cargo Transport Stations (CargoTransportStationData + TransportCompanyData) ==");
                foreach ((RefRO<TransportCompanyData> tcRef, Entity e) in SystemAPI
                             .Query<RefRO<TransportCompanyData>>()
                             .WithAll<CargoTransportStationData, PrefabData>()
                             .WithEntityAccess())
                {
                    if (truncated) break;

                    cargoTotal++;
                    TransportCompanyData tc = tcRef.ValueRO;

                    int vanillaMax = tc.m_MaxTransports;
                    if (m_PrefabSystem.TryGetPrefab(e, out PrefabBase pb) &&
                        pb.TryGet(out CargoTransportStation station))
                    {
                        vanillaMax = station.transports;
                    }

                    Append($"- {NameOf(e)} ({e.Index}:{e.Version}) VanillaMaxTransports={vanillaMax} CurMaxTransports={tc.m_MaxTransports}");
                }
                Append($"Cargo station summary: Total={cargoTotal}");
                Append("");

                // ---- Industrial extractor transport companies ----
                Append("== Industrial Extractor TransportCompanies (for Extractor trucks slider) ==");
                Append("Filter: name starts with Industrial_ AND contains Extractor/Coal/Stone/Mine/Quarry. Skips CurMaxTransports=0. Deduped by name.");

                var seenExtractors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach ((RefRO<TransportCompanyData> tcRef, Entity e) in SystemAPI
                             .Query<RefRO<TransportCompanyData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    if (truncated) break;

                    string name = NameOf(e);
                    if (IsExcludedName(name))
                        continue;

                    if (!IsTargetIndustrialExtractorCompany(name))
                        continue;

                    TransportCompanyData tc = tcRef.ValueRO;

                    if (tc.m_MaxTransports == 0)
                        continue;

                    if (!seenExtractors.Add(name))
                        continue;

                    extractorCompanies++;
                    Append($"- {name} ({e.Index}:{e.Version}) CurMaxTransports={tc.m_MaxTransports}");
                }

                Append($"Industrial extractor summary: Unique={extractorCompanies}");
                Append("");

                // ---- Keyword scan (deduped + capped) ----
                Append("== Keyword Matches (deduped, capped) ==");
                Append("These are hints for discovering relevant prefabs. Keep keywords narrow.");

                string[] keywords = new[]
                {
                    "deliveryvan",
                    "trucktractor",
                    "motorbike",
                    "roadmaintenance",
                    "parkmaintenance",
                    "industrialaquaculturehub",
                    "aquaculture",
                };

                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach ((RefRO<PrefabData> _, Entity e) in SystemAPI
                             .Query<RefRO<PrefabData>>()
                             .WithEntityAccess())
                {
                    if (truncated) break;
                    if (keywordMatches >= kMaxKeywordMatches) break;

                    string n = NameOf(e);
                    if (string.IsNullOrEmpty(n)) continue;

                    if (IsExcludedName(n))
                        continue;

                    string lower = n.ToLowerInvariant();

                    int hitIndex = -1;
                    for (int i = 0; i < keywords.Length; i++)
                    {
                        if (lower.Contains(keywords[i]))
                        {
                            hitIndex = i;
                            break;
                        }
                    }

                    if (hitIndex < 0) continue;
                    if (!seen.Add(n)) continue;

                    keywordMatches++;
                    Append($"- {n} ({e.Index}:{e.Version}) hit='{keywords[hitIndex]}'");
                }

                Append($"Keyword match summary: UniqueMatches={keywordMatches} Cap={kMaxKeywordMatches}");
                Append("");

                // Write report (overwrite each run)
                string reportPath = GetReportPath();
                string dir = Path.GetDirectoryName(reportPath) ?? string.Empty;
                if (dir.Length > 0)
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(reportPath, sb.ToString(), Encoding.UTF8);

                sw.Stop();

                PrefabScanState.MarkDone(sw.Elapsed, reportPath);

                Mod.s_Log.Info($"{Mod.ModTag} Prefab scan done in {sw.Elapsed.TotalSeconds:0.0}s. Report: {reportPath}");
                Mod.s_Log.Info(
                    $"{Mod.ModTag} PrefabScan counts (prefab entities): " +
                    $"TransitLines={transitLinePrefabTotal}, DeliveryTrucks={deliveryTotal}, " +
                    $"MaintVehicles={mvTotal}, MaintDepots={depotTotal}, CargoStations={cargoTotal}, " +
                    $"ExtractorCompanies={extractorCompanies}, LaneWearPrefabs={laneTotal}, KeywordHits={keywordMatches}");
            }
            catch (Exception ex)
            {
                sw.Stop();
                PrefabScanState.MarkFailed(PrefabScanState.FailCode.Exception, $"{ex.GetType().Name}: {ex.Message}");
                Mod.s_Log.Warn($"{Mod.ModTag} Prefab scan failed: {ex.GetType().Name}: {ex.Message}");
            }

            Enabled = false;
        }

        private static float InverseRelativeAppliedFromInput(float input) => (-input) / (1f + input);

        private static string Fmt(float v) => float.IsNaN(v) ? "n/a" : v.ToString("0.###");

        private static bool IsTargetIndustrialExtractorCompany(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            if (!name.StartsWith("Industrial_", StringComparison.OrdinalIgnoreCase))
                return false;

            if (name.IndexOf("Extractor", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("Coal", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("Stone", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("Mine", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("Quarry", StringComparison.OrdinalIgnoreCase) >= 0) return true;

            return false;
        }

        private static bool IsExcludedName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            if (name.StartsWith("Male_", StringComparison.OrdinalIgnoreCase)) return true;
            if (name.StartsWith("Female_", StringComparison.OrdinalIgnoreCase)) return true;
            if (name.IndexOf("_LOD", StringComparison.OrdinalIgnoreCase) >= 0) return true;

            string[] tokens =
            {
                "Tomestone", "StandingStone", "Crapfish", "PileStone", "Pilecoal", "Billboard", "Sign", "Poster", "NetBasket", "NetBox",
                "GasStation", "FarmCage", "FarmPontoon", "FishTub", "FlyFish", "FarmFilterSystem"
            };

            for (int i = 0; i < tokens.Length; i++)
            {
                if (name.IndexOf(tokens[i], StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }

        private static string GetReportPath()
        {
            string root = EnvPath.kUserDataPath;
            return Path.Combine(root, "ModsData", Mod.ModId, "ScanReport-Prefabs.txt");
        }

        private string GetPrefabNameSafe(Entity prefabEntity)
        {
            try
            {
                if (m_PrefabSystem != null &&
                    m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase pb))
                {
                    return pb.name ?? "(unnamed)";
                }
            }
            catch
            {
            }

            return $"PrefabEntity={prefabEntity.Index}:{prefabEntity.Version}";
        }
    }
}
