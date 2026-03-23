# Descenders Mod Menu
A feature-rich MelonLoader mod menu for Descenders, built with HarmonyLib and Unity Canvas UI. Provides in-game bike physics tweaks, score tools, movement modifiers, unlocks, and quality-of-life features accessible through a clean overlay menu.

---

## Features

### Stats — Bike Physics
- **Acceleration** — Increase pedal force up to 10 levels
- **Max Speed** — Reduce drag coefficient to raise terminal velocity, with levels 1–10
- **No Speed Cap** — Removes the hard-coded speed limit that kills pedal input above 55 km/h
- **Landing Impact** — Reduce landing impact force to make big drops survivable, with levels 1–10
- **No Bail** — Prevents the game from bailing you off the bike
- **Bike Switcher** — Switch between Enduro, Downhill, Hardtail and BRNZL Enduro on the fly
- **FOV Slider** — Adjust field of view from 45 to 130 across 10 levels
- **Slow Motion** — Halves game speed via TimeScaleManager. Toggle with F2 or in menu

### ESP — Visual Tools
- **ESP** — Draws player names and tracers on screen for all players in the session
- **ESP Distance** — Toggleable distance readout next to each player label
- **ESP Tracers** — Toggleable lines from screen edge to each player
- **Teleport to Player** — Scan for players in the session and teleport directly to them without losing your score

### Tools — Debug Utilities
- **Scene Dumper** — Dumps the full scene hierarchy, vehicle forensics, player data and component index to your desktop as text files
- **Speed Watcher** — Hold F10 while riding to record all changing Vehicle float fields to a text file. Useful for identifying speed-related fields and physics caps

### Unlock — Progression
- **Cosmetics** — Unlocks all cosmetic items including bikes, helmets and jerseys
- **Shortcuts** — Unlocks all world shortcuts across every biome
- **Achievements** — Unlocks all Steam achievements
- **Missions** — Marks all missions as complete

### Score — REP Tools
- **Add Score** — Add REP points to your current session in preset amounts (+100 up to +1M)
- **Trick Multiplier** — Set your trick score multiplier (x1, x2, x5, x10, x20)

### Move — Movement Modifiers
- **Rotation Speed** — Controls how fast you spin and flip in the air (level 1 = default)
- **Hop Force** — Boosts bunny hop launch velocity (level 1 = default)
- **Wheelie Force** — Increases the upward force when pulling wheelies (level 1 = default)
- **Lean Strength** — Controls how aggressively the bike leans (level 1 = default)

---

## Requirements
- [MelonLoader](https://melonwiki.xyz/) 0.5.7
- Descenders (Steam) — tested on Unity 2017.4.40f1

---

## Installation
1. Install MelonLoader for Descenders if you haven't already
2. Download `DescendersModMenu.dll` from the [Releases](../../releases) page
3. Drop it into your `Descenders/Mods/` folder
4. Launch the game — you should see ModMenu listed in the MelonLoader console on startup

---

## Controls
| Key | Action |
|-----|--------|
| F2 | Toggle slow motion |
| F6 | Toggle mod menu open / closed |
| F10 (hold) | Speed watcher — record vehicle field changes while riding |
| F12 | Dump scene to desktop |

---

## Usage Notes
- **No Speed Cap** requires you to lean forward as normal — it simply removes the game's internal limit on how fast pedalling can push you. It does not auto-accelerate.
- **Teleport** works in multiplayer sessions. Press Scan to find players, use the arrows to select, then Teleport. Score is preserved on teleport.
- **ESP** only shows players currently in your session. Hit Refresh Targets if someone joins mid-session.
- **Slow Motion** resets automatically when you change scene or quit the game.
- **Movement modifiers** all default to level 1 (stock game behaviour). Increases are applied live when in a session.
- **Save / Load / Reset** buttons at the bottom of Stats persist your settings between sessions to `UserData/DescendersModMenu/BikeStats.json`.
- The Scene Dumper and Speed Watcher are **debugging utilities** intended for mod development. They are not gameplay features.

---

## Building from Source
- Visual Studio 2022 or Rider
- Target framework: .NET 4.7.2
- References: `MelonLoader.dll`, `HarmonyLib.dll`, `Assembly-CSharp.dll`, `UnityEngine.*.dll`
- Output DLL goes into `Descenders/Mods/`

---

## Changelog

### v1.2.0
- Added Slow Motion (F2 toggle)
- Added Unlock tab — Cosmetics, Shortcuts, Achievements, Missions
- Added Score tab — Add REP, Trick Multiplier
- Added Move tab — Rotation Speed, Hop Force, Wheelie Force, Lean Strength
- Fixed score resetting on teleport
- Added defensive error handling across all mods and UI pages

### v1.0.0
- Initial release — Stats, ESP, Teleport, Tools

---

## Disclaimer
This mod is for single-player and private sessions. Use responsibly. Not affiliated with RageSquid or No More Robots.

---

Contact me on Discord for support or feature requests  
https://discord.gg/rHvCrBdqaR

*Built by Natehyden*
