# Diatch Boss

**Dispatch Boss**
- Lets players scale how many vehicles each transport depot can maintain or spawn in *Cities: Skylines II*
— plus how many passengers each public transic vehicle can carry.

---

## What it does

- Adds sliders in the mod’s **Options** panel.
- **Depot capacity:** Bus, Taxi, Tram, Train, Subway  
  (scales the maximum vehicles per depot)
- **Passenger capacity:** Bus, Taxi, Tram, Train, Subway, Ship, Ferry, Airplane  
  (scales how many passengers each vehicle can carry)
- Each slider ranges from **100% → 1000%** of vanilla.
- Changing a slider automatically re-applies the new capacities in the loaded city.
- Buttons to Reset sliders to vanilla (100%).

### Notes
- *Depot capacity* = maximum number of vehicles a single depot can maintain or spawn.
- *Passenger capacity* = maximum passengers each vehicle can carry.
- Capacities reset correctly when loading a different city or save.

- Public Transit Line in-game slider:
  - Widens the allowed input range.
  - The game computes line vehicle limits from estimated route time (driving time + number of stops). 
  - Longer routes and routes with more stops usually allow a higher maximum.
  - This mod expands the allowed interval range so the slider can go down to 1 vehicle and increases the maximum where the game allows it.
  - UI slider max might not visually refresh instantly when you flip the toggle, depending on when the line panel/UI re-reads policy + line timing.
  The simulation will keep updating line targets, but the panel can feel sticky unless it's moved a little after a refresh tick.
---

## 11 Languages

- English (en-US), Français French, Deutsch German, Español Spanish, Italiano Italian
- 한국어 Korean, 日本語 Japanese, 简体中文 (Simplified Chinese)
- 繁體中文 Traditional Chinese, Português Brazilian, Polski Polish

---

## Credits

- **River-Mochi** author/maintainer, localization  
- Thanks to **Wayz** for the original *Depot Capacity Changer* (retired from modding)
- yenyang - code review and technical advice
- Necko1996 - testing
- BugsyG - testing
- StarQ for technical support

---

## Links

- [Github repo](https://github.com/River-Mochi/DispatchBoss)
- [Paradox Mods page](https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime)
- [Support Discord](https://discord.gg/HTav7ARPs2)

---

## License

MIT — same as the original.
