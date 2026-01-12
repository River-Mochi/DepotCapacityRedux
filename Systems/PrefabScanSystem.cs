// File: Systems/PrefabScanSystem.cs
// Purpose: One-shot prefab scan triggered by a Settings button.
// Output: Writes report to {EnvPath.kUserDataPath}/ModsData/DispatchBoss/PrefabScanReport.txt
// Notes:
// - Runs only when requested (PrefabScanState.RequestScan()).
// - Uses SystemAPI.Query (modern ECS pattern).
// - Deduped + capped to prevent giant outputs and logger issues.
// - Filters out known noise names (Male_/Female_, billboard/sign/poster/etc).
// - Logs ONLY a summary to the mod log (no spam).

namespace DispatchBoss
{
    using Colossal.PSI.Environment; // EnvPath
    using Game;
    using Game.Companies;
    using Game.Prefabs;
    using Game.SceneFlow;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Unity.Entities;

    public sealed partial class PrefabScanSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem = null!;

        // Hard caps: protect users + protect logger/file size.
        private const int kMaxLines = 20000;
        private const int kMaxChars = 1 * 1024 * 1024; // ~1MB
        private const int kMaxKeywordMatches = 700;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            // Only meaningful when prefabs exist.
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
                PrefabScanState.MarkFailed("No city loaded.");
                Enabled = false;
                return;
            }

            PrefabScanState.MarkRunning();

            var sw = Stopwatch.StartNew();

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
                        sb.AppendLine("!! TRUNCATED: Output hit cap (lines or size). Narrow keywords / reduce detail if needed.");
                        lines++;
                        return;
                    }

                    sb.AppendLine(line);
                    lines++;
                }

                string NameOf(Entity e) => GetPrefabNameSafe(e);

                // Header
                Append("DispatchBoss Prefab Scan Report");
                Append($"Timestamp (local): {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                Append($"Mod: {Mod.ModName} {Mod.ModVersion}");
                Append("");

                // ---- Delivery trucks ----
                int semi = 0, van = 0, raw = 0, bike = 0, other = 0;

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
                        EntityManager,
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

                // ---- Lane wear (count + range) ----
                float minTf = float.MaxValue;
                float maxTf = float.MinValue;

                Append("== LaneDeteriorationData Prefabs (count + range) ==");
                foreach ((RefRO<LaneDeteriorationData> laneRef, Entity e) in SystemAPI
                             .Query<RefRO<LaneDeteriorationData>>()
                             .WithAll<PrefabData>()
                             .WithEntityAccess())
                {
                    if (truncated) break;

                    laneTotal++;
                    float tf = laneRef.ValueRO.m_TimeFactor;
                    if (tf < minTf) minTf = tf;
                    if (tf > maxTf) maxTf = tf;
                }

                if (laneTotal > 0)
                    Append($"Lane wear summary: Total={laneTotal} TimeFactor(min={minTf:0.###}, max={maxTf:0.###})");
                else
                    Append("Lane wear summary: Total=0");

                Append("");

                // ---- Keyword scan (deduped + capped) ----
                Append("== Keyword Matches (deduped, capped) ==");

                string[] keywords = new[]
                {
                    // Delivery / industry
                    "oiltruck", "coaltruck", "deliveryvan", "trucktractor", "motorbike",

                    // Maintenance
                    "roadmaintenance", "parkmaintenance",

                    // Extractors
                    "extractor", "coal", "stone", "mine", "quarry",

                    // Fishing discovery
                    "fish", "aquaculture", "industrialaqua", "industrialaquaculturehub"
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

                // Write report (overwrite each run: prevents file explosion)
                string reportPath = GetReportPath();
                string dir = Path.GetDirectoryName(reportPath) ?? string.Empty;
                if (dir.Length > 0)
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(reportPath, sb.ToString(), Encoding.UTF8);

                sw.Stop();

                PrefabScanState.MarkDone(sw.Elapsed, $"Wrote report: {reportPath}");

                // Log ONLY summary (avoid logger spam)
                Mod.s_Log.Info($"{Mod.ModTag} Prefab scan done in {sw.Elapsed.TotalSeconds:0.0}s. Report: {reportPath}");
                Mod.s_Log.Info($"{Mod.ModTag} Scan counts: Delivery={deliveryTotal} MaintVeh={mvTotal} MaintDepot={depotTotal} CargoStations={cargoTotal} IndustrialExtractors={extractorCompanies} Lanes={laneTotal} KeywordMatches={keywordMatches}");
            }
            catch (Exception ex)
            {
                sw.Stop();
                PrefabScanState.MarkFailed($"{ex.GetType().Name}: {ex.Message}");
                Mod.s_Log.Warn($"{Mod.ModTag} Prefab scan failed: {ex.GetType().Name}: {ex.Message}");
            }

            Enabled = false;
        }

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

            // Prefix exclusions
            if (name.StartsWith("Male_", StringComparison.OrdinalIgnoreCase)) return true;
            if (name.StartsWith("Female_", StringComparison.OrdinalIgnoreCase)) return true;
            if (name.IndexOf("_LOD", StringComparison.OrdinalIgnoreCase) >= 0) return true;

            // Contains exclusions
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
            // {EnvPath.kUserDataPath}/ModsData/DispatchBoss/PrefabScanReport.txt
            string root = EnvPath.kUserDataPath;
            return Path.Combine(root, "ModsData", Mod.ModId, "PrefabScanReport.txt");
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
                // ignore (scan should never crash a city)
            }

            return $"PrefabEntity={prefabEntity.Index}:{prefabEntity.Version}";
        }
    }
}
