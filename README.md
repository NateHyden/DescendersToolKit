“Descenders ToolKit — Sandbox & Testing Framework for Bike Physics, World Simulation, and Gameplay Experimentation”

---

## Requirements

- [MelonLoader](https://melonwiki.xyz/) 0.5.7
- Descenders (Steam)

## Installation

1. Install MelonLoader 0.5.7 for Descenders
2. Download `DescendersToolkit.dll` from the [Releases](../../releases) page
3. Drop it into `Descenders/Mods/`
4. Launch the game

---

## Tabs & Features

### General — Bike Physics & Session
Acceleration, Max Speed, No Speed Cap, Landing Impact, No Bail, Bike Switcher, FOV, Slow Motion (with speed slider), Auto Balance, Session Timer, Bail Counter, Longest Airtime, Top Speed, Speedrun Timer, No Speed Wobbles

### Move — Movement
Rotation Speed, Hop Force, Wheelie Force, Lean Strength, Wheelie Angle Limit, Air Control, Fakie Balance, Pump Strength, Ice Physics, Cut Brakes

### Bike — Tuning
Suspension Travel, Spring Stiffness, Spring Damping, Bike Size presets

### Graphics — Visuals
Bloom, Ambient Occlusion, Vignette, Depth of Field, Chromatic Aberration, Quality Presets (Low / Medium / High)

### World — Environment
Sky Colour presets, Storm, Gravity, Time of Day, Trees & Foliage, Music, Fog, Jump to Finish, Skip Song

### Silly — Fun
Player Size, Invisible Bike, Wheel Size, Wide Tyres, Sticky Tyres, Moon Mode, Giant Everyone, Invisible Player, Reverse Steering, Ice Grip, Fly Mode, Drunk Mode, Mirror Mode, Turbo Wind, No Mistakes

### Outfit
Save and load up to 3 full outfit presets

### Chat
Send and receive messages with other mod menu users in your session

### Avalanche
Boulders spawn above you and roll downhill. Configurable spawn rate, size, gravity, attraction force, difficulty scaling and instant-fail mode

### Ghost Replay
Records your run and plays it back as a ghost rider. F3 to toggle, F4 to save

### MapChange
Load any base game world or bike park directly without going through the main menu

### ESP
Player name labels, tracers, distance readout, teleport to player, teleport to checkpoint, mod user detection

### Score
Add or subtract REP, set trick multiplier (x1–x20, persists until reset)

### Unlock
Unlock all cosmetics, shortcuts, achievements and missions

### Info
System info, mod status with live OK/FAILED indicators, hotkey reference

---

## Controls

| Key | Action |
|-----|--------|
| F2 | Toggle slow motion |
| F3 | Ghost Replay — toggle |
| F4 | Ghost Replay — save run |
| F6 | Toggle mod menu |
| LS Click | Ghost Replay — set spawn marker |
| RS Click | Ghost Replay — save run |
| RS Dbl Click | Ghost Replay — toggle |

---

## Changelog

### v3.5.0
v3.5.0 — Changelog
Rebrand

Renamed from Descenders Mod Menu to Descenders Toolkit
Output DLL renamed to DescendersToolKit.dll — remove old DescendersModMenu.dll from your Mods folder
Header updated to DESCENDERS TOOLKIT
GitHub link updated to github.com/NateHyden/DescendersToolKit
Info tab credits updated to match

General Tab

Added Quick Brake — toggle that amplifies the game's own brake system when holding brake. Level 1 is a subtle boost, level 10 is near-instant. Works on all surfaces including loose terrain
Added Super Launch button — fires you forward at high speed in one press
Save, Load and Reset buttons moved from the bottom of the General tab into the header bar so they are always visible on every tab

Save System

Fixed toggle states not saving — the following were previously saving level only, not their on/off state: FOV, Acceleration, Max Speed, Landing Impact, Auto Balance, No Speed Wobbles, Rotation Speed, Hop Force, Wheelie Force, Lean Strength
Quick Brake enabled state and level now included in save/load/reset

Info Tab

Redesigned with four sub-tabs: System, Mod Status, Hotkeys, Credits
Credits tab added with official links, attribution and build info

Bug Fixes

FOV now correctly restores default when toggled off
FOV now works across all camera views including after switching camera angle
Acceleration, Max Speed and Landing Impact now have proper on/off toggles instead of applying permanently
Landing Impact completely reworked — now targets the actual bail threshold in Cyclist rather than the health system, so it genuinely reduces falls
All four Movement mods now have individual on/off toggles
Wheel Size preset buttons no longer auto-enable the mod
Quick Brake previously caused the bike to be glued to the floor — fixed by only applying when brake input is detected

### v3.2.0
v3.1.1 — Bug Fixes

FOV now has an on/off toggle and correctly restores default FOV when turned off
FOV now works across all camera views, not just the default
Acceleration now has an on/off toggle instead of applying permanently
Max Speed now has an on/off toggle instead of applying permanently
Landing Impact completely reworked — now actually reduces bails on hard landings
Landing Impact now has an on/off toggle
Rotation Speed, Hop Force, Wheelie Force and Lean Strength all have individual on/off toggles
Wheel Size now has an on/off toggle — preset buttons no longer auto-enable it
Ice Grip now has a description label
Mod Chat header now shows an Experimental badge


### v3.1.0
- **General tab** renamed from Stats
- Top Speed and Speedrun Timer moved into the Session section
- Slow Motion speed slider added (0.1x–0.9x, on same row as toggle)
- Auto Balance strength merged onto same row
- No Speed Wobbles completely rewritten — now works reliably via direct Vehicle physics patch
- **Wheelie Balance** replaced with **Wheelie Angle Limit** — actually caps your pitch angle during a wheelie (20°–65°)
- **In-Air Correction** replaced with **Air Control** — smoothly damps rotation while airborne
- Graphics quality presets fixed to use correct Unity indices, Current label now updates on selection
- Gravity range extended (level 1 = −2.0 up to level 10 = −40.0), default hint shown inline
- Sky colour active preset merged onto the colour row
- Storm kept as a clean on/off toggle
- Wide Tyres width controls merged onto the same row
- No Mistakes sensitivity lowered, inline description added
- Drunk Mode camera roll smoothed
- Score tab: REP rows merged, negative score buttons added, trick multiplier now actually works and stays locked until reset
- Ghost Replay hotkeys added to Info tab
- Hotkey badge width auto-sizes to text content
- All mods now listed in Mod Status with proper FAILED reporting if a patch fails to load
- Build warnings cleared

### v3.0.0
- Expanded to 15 tabs — Ghost Replay, Avalanche, Map Changer, Outfit Presets, Mod Chat, Graphics
- Storm mode, Sky Colour presets, Wide Tyres, Sticky Tyres, Fly Mode, Drunk Mode, Mirror Mode
- Mod Users detection in ESP tab
- Fixed Invisible Bike / Invisible Player restoring wrong renderers on toggle-off

### v2.1.0
- New sidebar navigation UI
- Invisible Bike, Moon Mode, Wheel Size, No Mistakes, Session Trackers added
- Top Speed matches in-game speedo exactly, persists to file
- Info tab tracks 34+ mods

### v2.0.0
- World, Bike and Silly tabs added
- Cut Brakes, Suspension tuning, Balance modifiers, Teleport to Checkpoint
- Menu widened to 700px

### v1.0.0
- Initial release

---

## Notes

- **No Speed Cap** — removes the hard-coded 55 km/h input limit. You still need to lean forward to accelerate.
- **Ghost Replay** — records every 2 frames, max ~5 minutes. Saves persist until you clear or overwrite them.
- **Avalanche Mode** — only works in procedural worlds, not bike parks.
- **Save / Load / Reset** on the General tab persists your settings to `UserData/DescendersModMenu/BikeStats.json`.
- **Top Speed** saves your all-time record to `UserData/DescendersModMenu/TopSpeed.txt`. Only the Reset button clears it.

---
*https://www.nexusmods.com/descenders/mods/7*
*Built by NateHyden — [Discord](https://discord.gg/rHvCrBdqaR)*

## License
This project is source-available for learning and contribution purposes.
You may not redistribute modified versions or claim this project as your own.
See LICENSE file for full terms.
