## Internal Systems & Behaviour — Dispatch Boss [DB]

Quick reference for how DB works under the hood.

## Overview Table

| Area / Feature | What it does | Implementation (high level) |
|----------------|--------------|------------------------------|
| **Transit depot capacity** | Multiplies max vehicles per depot building (bus/taxi/tram/train/subway). | Reads vanilla values from `PrefabBase`, writes scaled values into depot runtime data (per relevant prefab/data component). |
| **Transit passenger capacity** | Scales passenger seats per vehicle (bus/tram/train/subway/ship/ferry/airplane). | Reads vanilla seat counts from transit vehicle prefabs, writes scaled values into runtime capacity fields. |
| **Transit line slider** | Policy tuner expands vanilla line vehicle-count slider limits (can reach 1 vehicle on more routes; higher max when allowed). | Edits `VehicleCountPolicy` `RouteModifierData` entry for `RouteModifierType.VehicleInterval` when mode is `InverseRelative`. Captures original values once per session; restores when toggle is off. |
| **Industry delivery** | Scales cargo capacity by vehicle bucket (semi/van/raw materials/motorbike). | Uses `DeliveryTruckData` plus trailer info (`CarTractorData` / `CarTrailerData`) and name/cap heuristics to classify; writes scaled cargo capacity to runtime component. |
| **Extractor fleet size** | Multiplies max active transports for industrial extractor transport companies. | Targets `TransportCompanyData` entries matching industrial extractor naming rules; scales `m_MaxTransports` with clamp. |
| **Cargo station fleet size** | Multiplies cargo station max active transports (harbor/train/airport cargo). | Targets `CargoTransportStationData` + `TransportCompanyData`; scales `m_MaxTransports` with clamp. |
| **Park maintenance tuning** | Scales maintenance vehicle capacity/rate and depot fleet size for parks maintenance. | Targets `MaintenanceVehicleData` + `MaintenanceDepotData` by maintenance type; writes scaled `m_MaintenanceCapacity`, `m_MaintenanceRate`, and `m_VehicleCapacity`. |
| **Road maintenance tuning** | Scales maintenance vehicle capacity/rate and depot fleet size for road maintenance. | Same pattern as park maintenance, filtered by road maintenance type. |
| **Road wear speed (alpha)** | Changes how quickly lanes accumulate deterioration over time. | `LaneWearSystem` scales `LaneDeteriorationData.m_TimeFactor` on prefab entities; caches original per entity to prevent stacking; run-once. |
| **Run-once systems** | Prevents per-frame overhead; applies changes only when needed. | Each system enables on city load or via `Setting.Apply()`, runs once, then sets `Enabled = false`. |
| **Settings changes reapply** | Options slider changes update current city without reload. | `Setting.Apply()` enables relevant systems once (`TryEnableOnce<T>()`). |
| **Verbose debug logs** | Extra detail in `DispatchBoss.log` for troubleshooting. | Controlled by `EnableDebugLogging`; systems log base→new summaries and important decisions only. |
| **Prefab Scan Report** | Button makes a capped, deduped report of relevant prefabs and current values. | `PrefabScanSystem` runs only when requested via UI; writes `ModsData/DispatchBoss/PrefabScanReport.txt`; `PrefabScanState` tracks status for UI display. |
| **Open Log / Open Report** | Opens folder locations from Options UI buttons. | `ShellOpen` opens folders using Unity `Application.OpenURL(file://...)` first; includes safe process fallback; no crash on failure. |
| **Standardized paths** | Keeps logs/settings/data in consistent user-data locations. | Uses `EnvPath.kUserDataPath` and community-standard folders (`Logs/`, `ModsSettings/`, `ModsData/`). |
| **Localization** | 11 languages supported. | Locale files implement `IDictionarySource`. |
| **Minimal runtime work** | No scanning loops running every tick. | Full exit paths when not in gameplay; all heavy scans are button-triggered only. |
