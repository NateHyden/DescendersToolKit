# Descenders Toolkit

**v3.5.0** — A modding toolkit for Descenders built from the ground up with a clean 15-tab sidebar overlay.

> Previously known as Descenders Mod Menu — now rebuilt, rebranded and expanded.

Physics tweaks, world controls, score tools, session trackers, ghost replay, avalanche mode, map changer, outfit presets, mod chat and plenty of chaos — all in one place.

---

## Requirements

- [MelonLoader 0.5.7](https://github.com/LavaGang/MelonLoader/releases/tag/v0.5.7) (https://github.com/LavaGang/MelonLoader) *choose MelonLoader 0.5.7 from the drop down menu*
- Descenders (Steam)

---

## Installation

1. Run the MelonLoader installer, select your Descenders executable and choose version **0.5.7** — it will not work on other versions
2. Launch the game once to let MelonLoader populate, then close it
3. Download `DescendersToolKit.dll` from the [Releases page](https://github.com/NateHyden/DescendersToolKit/releases)
4. Drop it into `Descenders/Mods/`
5. Launch the game and press **F6** to open the menu

> If updating from an older version, delete `DescendersModMenu.dll` from your Mods folder first.

---

## Controls

| Key | Action |
|---|---|
| F6 | Open / Close mod menu |
| F2 | Toggle Slow Motion |
| F3 / RS Dbl Click | Toggle Ghost Replay |
| F4 / RS Click | Save Ghost Run |
| LS Click | Set Ghost Replay spawn marker |
| F10 (hold) | Speed Watcher — records live velocity to file |
| F12 | Dump scene to Desktop |

---

## Tabs & Features

### General — Bike Physics & Session

The main hub. All settings persist to file via Save / Load / Reset buttons that are always visible in the header bar regardless of which tab you are on.

**Bike Physics**
- **Acceleration** — on/off toggle with 10-level slider
- **Max Speed** — on/off toggle with 10-level slider, plus a full No Speed Cap toggle
- **Landing Impact** — raises the impact speed required to trigger a bail. Level 10 makes you nearly impossible to knock off
- **No Bail** — disables bailing entirely
- **Auto Balance** — assists with staying upright, strength adjustable
- **Bike Switcher** — cycle between Enduro, Downhill, Hardtail and BRNZL Enduro
- **FOV** — on/off toggle with level slider; restores the exact default on disable, works across all camera views
- **Slow Motion** — toggle with speed slider (0.1× to 0.9×); also F2 at any time
- **Slow Mo on Bail** — automatically triggers slow motion when you bail
- **No Speed Wobbles** — removes the high-speed steering wobble

**Quick Actions**
- **Quick Brake** — amplifies the game's own braking system while you hold brake. Works on all surfaces including loose terrain. Level 1 is a subtle boost, level 10 is near-instant
- **Super Launch** — fires you forward and upward at high speed in one press

**Session Trackers**
- **Session Timer** — live MM:SS counter from when you load in
- **Top Speed** — records your highest speed this session; saves to `UserData/DescendersModMenu/TopSpeed.txt`
- **Speedrun Timer** — integrates with the in-game timer
- **Bail Counter** — tracks actual crashes, not resets
- **Longest Airtime** — tracks your longest single airtime
- **G-Force** — live G-Force readout updated in real time
- **Peak G-Force** — records the highest G-Force hit this session

---

### ESP — Players & Teleport

- Player name labels with distance readout
- ESP Tracers — draws lines to all nearby players
- **Teleport to Player** — warps to a selected player; score is preserved
- **Teleport to Last Checkpoint** — snaps back to your last triggered checkpoint
- **Mod Users** — scans for and highlights other Descenders Toolkit users in your session

---

### Info — Diagnostics & Reference

Four sub-tabs:
- **System** — Unity version, MelonLoader version, scripting backend, toolkit version and DLL name
- **Mod Status** — every mod listed with OK / FAILED status and error details
- **Hotkeys** — full key binding reference
- **Credits** — GitHub, Nexus links and build info

---

### Unlock

- Unlock all cosmetics — bikes, helmets, jerseys and more
- Unlock all world shortcuts
- Unlock all Steam achievements
- Mark all missions as complete

---

### Score

- Add REP — +100, +500, +1K, +5K, +10K, +50K, +100K
- Remove REP — -100, -500, -1K, -5K, -10K
- **Trick Multiplier** — x1, x2, x5, x10 or x20; locked in until you manually reset to x1

---

### Move — Bike Handling

Every mod has its own independent on/off toggle and level slider.

**Movement**
- **Rotation Speed** — how fast the bike rotates in the air
- **Hop Force** — bunny hop power
- **Wheelie Force** — how aggressively the front wheel pulls up
- **Lean Strength** — left/right lean sensitivity

**Balance & Physics**
- **Wheelie Angle Limit** — caps maximum pitch angle during wheelies (20° to 65°)
- **Air Control** — damps rotation while airborne; higher = more stable landings
- **Pump Strength** — how much speed you generate from pumping

**Misc**
- **Cut Brakes** — removes all braking force; resets automatically on scene change

---

### World — Environment

**Sky**
- Sky Colour presets — Normal, Blood Red, Alien Green, Synthwave, Midnight, Toxic
- Storm — dark sky with wind and rain
- Gravity — level 1 is floaty, level 5 is default (-17.5 m/s²), level 10 is heavy

**Environment**
- Time of Day — Dawn to Night
- Trees & Foliage — toggle
- Music — toggle; restores exact volume on unmute
- Fog — toggle; saves and restores original density

**Level**
- Jump to Finish — instantly triggers the finish line
- Skip Song

---

### Bike — Tuning

**Suspension** — Travel, Stiffness, Damping (all 10-level sliders; level 5 = stock)

**Bike Size** — Giant, Big, Default, Small, Tiny

---

### Silly — Fun & Chaos

**Player** — Size presets (Giant / Big / Default / Small / Tiny), Invisible Player

**Bike** — Invisible Bike, Wheel Size (toggle + Small/Default/Large), Wide Tyres (20-level slider), Sticky Tyres (grip any surface including walls and ceilings)

**Presets** — Moon Mode (low gravity + max suspension; saves and restores your settings on deactivate)

**Multiplayer** — Giant Everyone (scales all other players to Giant / Default / Tiny)

**Controls** — Reverse Steering, Ice Grip (removes all tyre grip), Mirror Mode (flips the world left/right), Fly Mode (detach and fly freely), Drunk Mode (FOV wobble)

**World** — Invisible Player, Turbo Wind, No Mistakes (sideways wall/rock impacts launch you backward)

---

### Graphics — Visuals

**Post Processing** — Toggle Bloom, Ambient Occlusion, Vignette, Depth of Field, Chromatic Aberration individually

**Quality** — Low, Medium, High, Ultra presets

---

### Outfit — Presets

- Three named preset slots — save your full current outfit to any slot
- Load any preset instantly during a session
- Rename slots by clicking the name, typing and pressing Enter
- Delete individual presets

---

### Chat — Mod Users *(Experimental)*

- Send and receive messages with other Descenders Toolkit users in your session
- Scrollable log with timestamps and player names
- Only visible to other mod users

---

### Avalanche

- Boulders (or cubes) spawn above you and roll downhill
- Configurable spawn rate, max hazards, boulder size, spawn distance, height and radius
- Adjustable extra gravity and forward impulse
- Survival timer, Instant Fail on Hit mode, Difficulty Scaling
- Only works in procedural worlds — not bike parks

---

### Ghost Replay

- Records your run in real time and plays it back as a transparent ghost rider
- Save your best run to disk — persists between sessions for racing against yourself
- Clear saved run button
- Ghost HUD overlay showing recording status and run length
- Toggle: F3 or double-click RS — Save: F4 or RS click — Set spawn: LS click
- Max recording length ~5 minutes

---

### Map Changer

- Load any base game world directly — Highlands, Forest, Canyon, Desert, Peaks, Jungle, Ridges, Hell and more
- Browse and load Bike Park and Freeride Workshop maps
- No main menu required

---

## Save System

All settings persist to `UserData/DescendersModMenu/BikeStats.json`. Saved: all levels, all toggle states, tyre settings, suspension, graphics state, sky preset, fly mode speeds, ghost replay alpha and more.

---

## Notes

- Intended for single-player and private sessions
- Suspension sliders default to level 5 — stock game behaviour
- No Speed Cap removes the hard-coded 55 km/h input limit; you still need to lean to accelerate
- Cut Brakes resets automatically on scene change
- Moon Mode saves your gravity and suspension before activating and restores them on deactivate
- No Mistakes only triggers on sideways wall and rock impacts, not landings
- Ghost Replay records every 2 frames; max ~5 minutes. Saves persist until cleared
- Avalanche Mode only works in procedural worlds, not bike parks
- If the mod does not load, check your MelonLoader log in the game folder

---

## Changelog

### v3.5.0
- Rebranded from Descenders Mod Menu to Descenders Toolkit — DLL is now `DescendersToolKit.dll`
- Added Quick Brake — uses the game's own brake input so it works on all surfaces, adjustable strength from subtle to near-instant
- Added Super Launch one-press button
- Save / Load / Reset moved to the header bar — always accessible from every tab
- Info tab redesigned as four sub-tabs: System, Mod Status, Hotkeys, Credits
- Fixed toggle states not saving for FOV, Acceleration, Max Speed, Landing Impact, Auto Balance, No Speed Wobbles and all four Movement mods
- Quick Brake enabled state and level now included in save and load

### v3.2.0
- FOV toggle added — restores default on disable, works across all camera views
- Acceleration and Max Speed now have on/off toggles
- Landing Impact reworked — targets the actual bail speed threshold in the physics engine
- All four Movement mods now have individual on/off toggles
- Wheel Size preset buttons no longer auto-enable the mod
- Mod Chat Experimental badge added

### v3.1.0
- Slow Motion speed slider and Auto Balance strength control
- No Speed Wobbles rewritten via direct Vehicle physics patch
- Wheelie Angle Limit and Air Control replace old Wheelie Balance and In-Air Correction
- Graphics quality presets fixed
- Gravity range extended
- Negative REP buttons, working Trick Multiplier
- Full mod status list with OK/FAILED reporting

### v3.0.0
- Expanded to 15 tabs — Ghost Replay, Avalanche, Map Changer, Outfit Presets, Mod Chat, Graphics
- Storm mode, Sky Colour presets, Wide Tyres, Sticky Tyres, Fly Mode, Drunk Mode, Mirror Mode
- Mod Users detection in ESP tab

### v2.1.0
- New sidebar navigation UI
- Invisible Bike, Moon Mode, Wheel Size, No Mistakes, Session Trackers
- Top Speed matches in-game speedo, persists to file

### v2.0.0
- World, Bike and Silly tabs added
- Cut Brakes, Suspension tuning, Balance modifiers, Teleport to Checkpoint

### v1.0.0
- Initial release

---

## Links

- **Nexus Mods:** [nexusmods.com/descenders/mods/7](https://www.nexusmods.com/descenders/mods/7)
- **Discord:** [discord.gg/rHvCrBdqaR](https://discord.gg/rHvCrBdqaR)

---

## License

Source-available for learning and contribution purposes. You may not redistribute modified versions or claim this project as your own. See the [LICENSE](LICENSE) file for full terms.

---

*Built by NateHyden*
