## Internal Systems & Behaviour — Dispatch Boss [DB]

Quick reference for how DB works under the hood.

## Overview Table

| Area / Feature | What it does | Implementation (high level) |
|----------------|--------------|------------------------------|
| **Depot capacity** | Multiplies how many vehicles each transit depot can maintain/spawn. | Reads vanilla from `PrefabBase`, writes scaled values to `TransportDepotData.m_VehicleCapacity`. |
| **Passenger capacity** | Scales seats for public transport vehicles. | Reads vanilla from `PublicTransport` prefab, writes scaled values into runtime `m_PassengerCapacity`. |
| **Industry cargo** | Scales cargo capacity for delivery vehicle prefabs (semi/van/raw/motorbike). | Buckets `DeliveryTruckData` prefabs, reads vanilla from `PrefabBase`, writes scaled `m_CargoCapacity`. |
| **Cargo stations fleet** | Scales max active transporters for cargo stations. | Reads `TransportCompanyData.m_MaxTransports` baseline from prefab, writes scaled runtime value. |
| **Extractor fleet** | Scales max trucks for industrial extractors. | Filters `TransportCompanyData` by prefab name patterns, scales `m_MaxTransports`. |
| **Maintenance vehicles** | Scales maintenance work capacity/rate. | Reads vanilla from `MaintenanceVehicle` prefab, writes scaled `MaintenanceVehicleData` fields. |
| **Maintenance depots** | Scales maximum maintenance vehicles allowed per depot. | Reads vanilla from `MaintenanceDepot` prefab, writes scaled `MaintenanceDepotData.m_VehicleCapacity`. |
| **Road wear speed (alpha)** | Adjusts how fast lanes deteriorate over time. | Scales `LaneDeteriorationData.m_TimeFactor` (prefab-based baseline). |
| **Prefab-based vanilla protection** | Prevents stacking multipliers across runs. | Always reads from `PrefabBase` (original prefab) instead of current runtime data. |
| **One-shot per apply** | Systems run only when explicitly triggered (load/apply/button). | Systems enable, run once, then set `Enabled = false`. |
| **Settings changes reapply** | Slider changes apply immediately to the loaded city. | `Setting.Apply()` re-enables the systems for one more pass. |
| **Transit line slider tuner** | Optional widening of the line vehicle slider limits. | Edits `VehicleCountPolicy` RouteModifier range when enabled. |
| **Prefab scan report** | Debug report of relevant prefabs + current policy ranges. | Button enables `PrefabScanSystem`, writes `ModsData/DispatchBoss/PrefabScanReport.txt`. |
| **Debug logging** | Optional detailed logs to `DispatchBoss.log`. | Controlled by `EnableDebugLogging`, logs via `Mod.s_Log`. |
| **Safe locale loading** | Localization issues can’t break startup. | Locale sources wrapped in try/catch around `LocalizationManager.AddSource`. |
| **Options UI layout** | Tabs: Public-Transit / Industry / Parks-Roads / About. | `Setting` uses CO `SettingsUI*` attributes + Locale-backed labels/descs. |
| **Log/report folder opener** | Opens Logs or ModsData folder. | `ShellOpen.OpenFolderSafe(...)` uses `file:///` + shell fallback. |
| **Minimal runtime work** | No background scanning or polling. | No work unless gameplay mode + system enabled by apply or button. |
