# Descenders Mod Menu

A feature-rich MelonLoader mod menu for Descenders, built with HarmonyLib and Unity Canvas UI. Provides in-game bike physics tweaks, score tools, movement modifiers, world controls, suspension tuning, and silly fun — all accessible through a clean overlay menu.

Toggle the menu with **F6** at any time.

---

## Features

### Stats — Bike Physics
- **Acceleration** — Increase pedal force up to 10 levels
- **Max Speed** — Reduce drag coefficient to raise terminal velocity, levels 1–10
- **No Speed Cap** — Removes the hard-coded speed limit that kills pedal input above 55 km/h
- **Landing Impact** — Reduce landing impact force to make big drops survivable, levels 1–10
- **No Bail** — Prevents the game from bailing you off the bike
- **Bike Switcher** — Switch between Enduro, Downhill, Hardtail and BRNZL Enduro on the fly
- **FOV Slider** — Adjust field of view from 45 to 130 across 10 levels
- **Slow Motion** — Halves game speed via TimeScaleManager. Toggle with F2 or in menu
- **Top Speed Recorder** — Tracks and displays your fastest speed this session with a reset button

### ESP — Visual Tools
- **ESP** — Draws player names and tracers on screen for all players in the session
- **ESP Distance** — Toggleable distance readout next to each player label
- **ESP Tracers** — Toggleable lines from screen edge to each player
- **Teleport to Player** — Scan for players and teleport directly to them without losing your score
- **Teleport to Checkpoint** — Instantly teleport to the last triggered race checkpoint

### Info — Diagnostics
- **System Info** — Displays Unity version, version match status and MelonLoader version
- **Mod Status** — Scrollable list of all 30 mods with live OK/FAILED indicators
- **Hotkeys** — Quick reference for all keyboard shortcuts

### Unlock — Progression
- **Cosmetics** — Unlocks all cosmetic items including bikes, helmets and jerseys
- **Shortcuts** — Unlocks all world shortcuts across every biome
- **Achievements** — Unlocks all Steam achievements
- **Missions** — Marks all missions as complete

### Score — REP Tools
- **Add Score** — Add REP points to your current session in preset amounts (+100 up to +1M)
- **Trick Multiplier** — Set your trick score multiplier (x1, x2, x5, x10, x20)

### Move — Movement Modifiers
- **Rotation Speed** — Controls how fast you spin and flip in the air
- **Hop Force** — Boosts bunny hop launch velocity
- **Wheelie Force** — Increases the upward force when pulling wheelies
- **Lean Strength** — Controls how aggressively the bike leans
- **Wheelie Balance** — Game modifier for infinite wheelie balance
- **In-Air Correction** — Game modifier for mid-air bike correction strength
- **Fakie Balance** — Game modifier for fakie stability
- **Pump Strength** — Game modifier for pump track force
- **Ice Physics** — Reduces wheel grip for slippery surfaces (level 1 = max ice, level 5 = default, level 10 = max grip)
- **Cut Brakes** — Disables braking entirely via Harmony patch

### World — Environment
- **Gravity** — 10 levels from floaty to heavy (level 5 = default −17.5)
- **Time of Day** — 10 presets from Dawn through to Night via the TOD_Sky system
- **Trees & Foliage** — Toggle terrain tree and foliage rendering
- **Music** — Mute and restore game music via FMOD VCA, preserving your volume setting
- **Jump to Finish** — Instantly teleport to the finish line
- **Skip Song** — Skip the currently playing track

### Bike — Tuning & Fun
- **Suspension Travel** — Controls how much the fork and shock move (level 5 = default)
- **Spring Stiffness** — Controls spring resistance (level 5 = default)
- **Spring Damping** — Controls how fast the suspension settles (level 5 = default)
- **Bike Size** — Giant / Big / Default / Small / Tiny scale presets for the bike model

### Silly — Fun Stuff
- **Player Size** — Giant / Big / Default / Small / Tiny scale presets for your rider
- **Giant Everyone** — Scale all other players' riders in multiplayer
- **Invisible Player** — Disable all renderers on your rider
- **Turbo Wind** — Crank the WindZone to maximum turbulence
- **Exploding Props** — Massively increase BounceVolume force so everything flies on contact

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
- **No Speed Cap** requires you to lean forward as normal — it removes the game's internal limit. It does not auto-accelerate.
- **Teleport to Player** works in multiplayer sessions. Press Scan to find players, use arrows to select, then Teleport. Score is preserved.
- **ESP** only shows players currently in your session. Hit Refresh Targets if someone joins mid-session.
- **Slow Motion** resets automatically when you change scene or quit.
- **Cut Brakes** uses a Harmony postfix on VehicleController.FixedUpdate and resets on scene change.
- **Suspension** modifiers apply to both wheels simultaneously. Level 5 is always default on all three sliders.
- **Music** mute saves your current volume before muting and restores it exactly on unmute.
- **Save / Load / Reset** buttons at the bottom of the Stats tab persist your settings between sessions to `UserData/DescendersModMenu/BikeStats.json`.

---

## Building from Source
- Visual Studio 2022 or Rider
- Target framework: .NET 4.7.2
- References: `MelonLoader.dll`, `HarmonyLib.dll`, `Assembly-CSharp.dll`, `UnityEngine.*.dll`
- Output DLL goes into `Descenders/Mods/`

---

## Changelog

### v2.0.0
- Added World tab — Gravity, Time of Day, Trees & Foliage, Music, Jump to Finish, Skip Song
- Added Bike tab — Suspension Travel, Stiffness, Damping, Bike Size presets
- Added Silly tab — Player Size, Giant Everyone, Invisible Player, Turbo Wind, Exploding Props
- Added Cut Brakes (Move tab) via Harmony postfix on VehicleController.FixedUpdate
- Added Top Speed Recorder (Stats tab)
- Added Teleport to Checkpoint (ESP tab)
- Added Balance & Physics modifiers — Wheelie Balance, In-Air Correction, Fakie Balance, Pump Strength, Ice Physics
- Menu widened to 700px, 9 tabs
- Diagnostics page now tracks all 30 mods
- Version bumped to 2.0.0

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
