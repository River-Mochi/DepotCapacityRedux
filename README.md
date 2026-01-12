# Dispatch Boss

**Dispatch Boss** adds Options sliders for *Cities: Skylines II*
to scale:
- Public transit depots (max vehicles)
- Public transit vehicles (passenger capacity)
- Industry delivery vehicle cargo capacity
- Industry + cargo station fleet limits
- Park + road maintenance effectiveness and fleet limits
- Road wear speed (alpha)
- Transit line panel vehicle min/max expansion (alpha)

Everything is applied with **run-once** systems (no per-frame scanning to affect performance).

---

## Features

### Public Transit
- **Depot capacity**: Bus, Taxi, Tram, Train, Subway  
  Scales max vehicles each depot can maintain/spawn.
- **Passenger capacity**: Bus, Tram, Train, Subway, Ship, Ferry, Airplane  
  Scales seats per vehicle.
- **Transit line panel limits (optional) ✅**  
  Expands the vanilla line vehicle-count slider so it can be as low as **1 vehicle** on more routes and allows higher maximums.  
  The game computes limits from an estimated **route cycle time** (travel time + stops), so maximums still vary by route but will be higher.

### Industry
- **Delivery vehicle cargo capacity** multipliers:
  - Semi trucks, Delivery vans, Motorbike delivery, Raw-material trucks (oil/coal/ore/stone)
- **Extractor fleet size** multiplier (industrial extractor transport companies)
- **Cargo station max fleet** multiplier for trucks (harbor/train/airport cargo stations)

### Parks & Roads
- **Park maintenance**: work capacity, work rate, depot fleet size
- **Road maintenance**: work capacity, repair rate (alpha), depot fleet size
- **Road wear speed (alpha)**: scales how fast lanes accumulate deterioration over time

---

## Debug / Tools (About tab)
- **Prefab Scan Report** button writes: `ModsData/DispatchBoss/PrefabScanReport.txt`
- **Verbose debug logs** toggle (extra details in log)
- **Open Log** / **Open Report** buttons open the folders:
  - `Logs/`
  - `ModsData/DispatchBoss/`

---

## Notes / Compatibility
- Sliders re-apply to the loaded city after changes in Options menu.
- Caution: avoid running other mods that edit the same Transit line policy / vehicle-interval modifier (e.g., Transit Policy Adjuster, Smart Transportation).

---

## 11 Languages
English (en-US), Français, Deutsch, Español, Italiano, 한국어, 日本語, 简体中文, 繁體中文, Português (Brasil), Polski

---

## Credits
- **River-Mochi** — author/maintainer, localization
- yenyang — code review and technical advice  
- Thanks to **Wayz** for the original *Depot Capacity Changer* (retired from modding)  
- Necko1996 — testing  
- BugsyG — testing  
- StarQ — technical support  

---

## Links
- [GitHub repo](https://github.com/River-Mochi/DispatchBoss)  
- [Paradox Mods page](https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime)  
- [Support Discord](https://discord.gg/HTav7ARPs2)

---

## License
MIT
