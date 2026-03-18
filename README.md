# Descenders Mod Menu

A feature-rich MelonLoader mod menu for Descenders, built with HarmonyLib and Unity Canvas UI. Provides in-game bike physics tweaks, visual tools, and quality-of-life features accessible through a clean overlay menu.

---

## Features

### Page 1 — Bike Stats
- **Acceleration** — Increase pedal force up to 10 levels
- **Max Speed** — Reduce drag coefficient to raise terminal velocity, with levels 1–10
- **No Speed Cap** — Removes the hard-coded speed limit that kills pedal input above 55 km/h
- **Landing Impact** — Reduce landing impact force to make big drops survivable, with levels 1–10
- **No Bail** — Prevents the game from bailing you off the bike
- **Bike Switcher** — Switch between Enduro, Downhill, Hardtail and BRNZL Enduro on the fly
- **FOV Slider** — Adjust field of view from 45 to 130 across 10 levels

### Page 2 — ESP & Teleport
- **ESP** — Draws player names and tracers on screen for all players in the session
- **ESP Distance** — Toggleable distance readout next to each player label
- **ESP Tracers** — Toggleable lines from screen edge to each player
- **Teleport to Player** — Scan for players in the session and teleport directly to them

### Page 3 — Tools & Hotkeys
- **Scene Dumper** *(debug tool)* — Dumps the full scene hierarchy, vehicle forensics, player data and component index to your desktop as text files. Useful for investigating the game's internal structure.
- **Speed Watcher** *(debug tool)* — Hold F10 while riding to record all changing Vehicle float fields to a text file on your desktop. Useful for identifying speed-related fields and physics caps.

---

## Requirements

- [MelonLoader](https://melonwiki.xyz/) 0.5.7 or newer
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
| F6 | Toggle mod menu open / closed |
| F10 (hold) | Speed watcher — record vehicle field changes while riding |
| F12 | Dump scene to desktop |

---

## Usage Notes

- **No Speed Cap** requires you to lean forward as normal — it simply removes the game's internal limit on how fast pedalling can push you. It does not auto-accelerate.
- **Teleport** works in multiplayer sessions. Press Scan to find players, use the arrows to select, then Teleport.
- **ESP** only shows players currently in your session. Hit Refresh Targets if someone joins mid-session.
- **Save / Load / Reset** buttons at the bottom of Page 1 persist your stat settings between sessions to `UserData/DescendersModMenu/BikeStats.json`.
- The Scene Dumper and Speed Watcher are **debugging utilities** intended for mod development. They are not gameplay features.

---

## Building from Source

- Visual Studio 2022 or Rider
- Target framework: .NET 3.5
- References: `MelonLoader.dll`, `HarmonyLib.dll`, `Assembly-CSharp.dll`, `UnityEngine.*.dll`
- Output DLL goes into `Descenders/Mods/`

---

## Disclaimer

This mod is for single-player and private sessions. Use responsibly. Not affiliated with RageSquid or No More Robots.

---

*Built by Natehyden*
