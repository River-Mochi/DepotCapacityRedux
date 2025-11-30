## Internal Systems & Behaviour — Adjust Transit Capacity [ATC]

Quick reference for how ATC works under the hood.

## Overview Table

| Area / Feature | What it does | Implementation (high level) |
|----------------|--------------|------------------------------|
| **Depot capacity scaling** | Multiplies how many vehicles each depot can maintain/spawn. | Reads vanilla depot values from `PrefabBase`, writes scaled values into `TransportDepotData.m_VehicleCapacity`. |
| **Passenger capacity scaling** | Scales passenger seats for buses, trams, trains, subways, ships, ferries, airplanes. | Reads vanilla seat counts from `PublicTransport` prefab, writes scaled values into `m_PassengerCapacity`. |
| **Tram special handling** | Trams have 3 sections; UI shows combined total. | Debug logs show per-section base → new and a 3× total line. |
| **Prefab-based vanilla protection** | Prevents stacking or multiplying already-modified values. | Always reads from `PrefabBase` (original prefab) instead of current runtime data. |
| **One-shot per city** | Applies all changes once when a city loads. | System enables once, runs, then sets `Enabled = false`. |
| **Settings changes reapply** | Changing sliders in Options UI updates the current city instantly. | `Setting.Apply()` enables the system for one more pass. |
| **Debug logging** | Optional detailed logs showing base→new values and a city summary. | Controlled by `EnableDebugLogging`; prints via `Mod.s_Log`. |
| **PrisonVan guard** | Prevents scaling Prison Vans (they’re flagged as Bus type). | Checks prefab name for `"PrisonVan"` and skips. |
| **City type summary (debug)** | Shows which transport types exist in this save. | Tracks via `m_SeenDepotTypes` / `m_SeenPassengerTypes`. |
| **Safe locale loading** | Localization issues can’t break mod startup. | `AddLocaleSource()` wraps `LocalizationManager.AddSource` in try/catch. |
| **Options UI – Actions tab** | All depot sliders, passenger sliders, “Double Up”, and reset buttons. | Defined in `Setting` via `SettingsUISlider`, `SettingsUIButton`, etc. |
| **Options UI – About tab** | Shows version, mod name, Paradox button, Discord, debug toggle, open log. | Locale-backed display fields in `Setting`. |
| **Log file opener** | Opens the ATC log file or Logs folder. | Uses `file:///` URI + Windows shell fallback. |
| **Defaults & slider ranges** | Depots: 100–1000%. Passengers: 10–1000%. Steps: 10%. | Constants in `Setting` (`DepotMinPercent`, etc.). |
| **Settings persistence** | Saves values across sessions. | `AssetDatabase.global.LoadSettings(ModId, setting, new Setting(this));`. |
| **System scheduling** | Ensures all prefabs exist before scaling. | `updateSystem.UpdateAfter(..., PrefabUpdate)`. |
| **Localization** | Full localizations: EN, FR, ES, DE, IT, JA, KO, PT-BR, ZH-HANS, ZH-HANT, PL. | Each locale file implements `IDictionarySource`. |
| **Minimal runtime work** | System does zero work unless explicitly triggered. | Full exit if not gameplay mode
