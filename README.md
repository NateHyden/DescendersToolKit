# Descenders Mod Menu

A feature-rich MelonLoader mod menu for Descenders, built with HarmonyLib and Unity Canvas UI. Provides in-game bike physics tweaks, score tools, movement modifiers, world controls, suspension tuning, session trackers and silly fun — all accessible through a clean sidebar overlay menu.

Toggle the menu with **F6** at any time.

---

## UI

The mod menu uses a **left sidebar navigation** layout with 15 tabs, an orange Descenders-themed colour palette, and a bottom bar for Save/Load/Reset on the Stats tab.

- **Window:** 800×660px, dark grungy MTB theme with **neon lime** (`#CCFF00`) as the primary accent
- **Sidebar:** 130px wide with tab labels, lime active indicator, a green status dot on the Info tab, and group dividers labelling BIKE, WORLD, TOOLS and SYSTEM sections
- **Toggles:** Lime knob when ON, red/pink when OFF
- **Buttons:** Neon blue for standard actions, orange reserved for destructive actions only
- **Header:** "DESCENDERS MOD MENU" title with version badge and author credit
- **Tab order:** Stats, Move, Bike, Graphics, World, Silly, Outfit, Chat, Avalanche, GhostReplay, MapChange, ESP, Score, Unlock, Info

---

## Features

### Stats — Bike Physics & Session Trackers
- **Acceleration** — Increase pedal force up to 10 levels
- **Max Speed** — Reduce drag coefficient to raise terminal velocity, levels 1–10
- **No Speed Cap** — Removes the hard-coded speed limit that kills pedal input above 55 km/h
- **Landing Impact** — Reduce landing impact force to make big drops survivable, levels 1–10
- **No Bail** — Prevents the game from bailing you off the bike
- **Bike Switcher** — Switch between Enduro, Downhill, Hardtail and BRNZL Enduro on the fly
- **FOV Slider** — Adjust field of view from 45 to 130 across 10 levels
- **Slow Motion** — Halves game speed via TimeScaleManager. Toggle with F2 or in menu
- **Top Speed Recorder** — Tracks your fastest speed using the game's own speedo formula, persists to file across sessions
- **Session Timer** — Live MM:SS counter from when you enter a session
- **Bail Counter** — Counts actual crashes via Harmony patch on Cyclist.Bail(), with reset button
- **Longest Airtime** — Tracks your longest continuous time off the ground, with reset button

### ESP — Visual Tools
- **ESP** — Draws player names and tracers on screen for all players in the session
- **ESP Distance** — Toggleable distance readout next to each player label
- **ESP Tracers** — Toggleable lines from screen edge to each player
- **Teleport to Player** — Scan for players and teleport directly to them without losing your score
- **Teleport to Checkpoint** — Instantly teleport to the last triggered race checkpoint
- **Mod Users** — Highlights other players running the mod menu in your session

### Info — Diagnostics
- **System Info** — Displays Unity version, version match status and MelonLoader version
- **Mod Status** — Scrollable list of all mods with live OK/FAILED circle indicators
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
- **Storm** — Activates storm weather — dark sky, wind and rain
- **Gravity** — 10 levels from floaty to heavy (level 5 = default −17.5)
- **Time of Day** — 10 presets from Dawn through to Night via the TOD_Sky system
- **Sky Colour** — Custom sky colour presets — Blood Red, Alien Green, Synthwave, Midnight, Toxic
- **Trees & Foliage** — Toggle terrain tree and foliage rendering
- **Music** — Mute and restore game music via FMOD VCA, preserving your volume setting
- **Fog** — Toggle fog rendering on and off, saves and restores original density
- **Jump to Finish** — Instantly teleport to the finish line
- **Skip Song** — Skip the currently playing track

### Bike — Tuning
- **Suspension Travel** — Controls how much the fork and shock move (level 5 = default)
- **Spring Stiffness** — Controls spring resistance (level 5 = default)
- **Spring Damping** — Controls how fast the suspension settles (level 5 = default)
- **Bike Size** — Giant / Big / Default / Small / Tiny scale presets for the bike model

### Silly — Fun Stuff
- **Player Size** — Giant / Big / Default / Small / Tiny scale presets for your rider
- **Invisible Bike** — Hide the bike model while keeping physics intact
- **Wheel Size** — Small / Default / Large wheel scaling via bone transforms and physics radius
- **Wide Tyres** — Widen the tyres across 20 levels
- **Sticky Tyres** — Tyres grip any surface including walls and ceilings
- **Moon Mode** — One-button preset: low gravity + max suspension travel + min damping. Saves your current settings and restores them on deactivation
- **Giant Everyone** — Scale all other players' riders in multiplayer
- **Invisible Player** — Disable all renderers on your rider
- **Reverse Steering** — Flip left/right steering input
- **Ice Grip** — Reduce tyre friction for slippery handling
- **Fly Mode** — Detach from the bike and fly freely through the level
- **Drunk Mode** — Applies a persistent FOV wobble effect
- **Mirror Mode** — Flips the game world left/right
- **Turbo Wind** — Crank the WindZone to maximum turbulence
- **No Mistakes** — Any sideways collision with a wall, rock or obstacle launches you backwards. Landings and gentle bumps are ignored

### Graphics — Visuals
- **Bloom** — Toggle bloom post processing effect
- **Ambient Occlusion** — Toggle ambient occlusion
- **Vignette** — Toggle vignette effect
- **Depth of Field** — Toggle depth of field blur
- **Chromatic Aberration** — Toggle chromatic aberration
- **Quality Preset** — Switch between Low, Medium, High and Ultra render quality on the fly

### Outfit — Customisation
- **Outfit Presets** — Save and load up to 3 full outfit loadouts (helmet, jersey, bike) for quick switching
- **Quick Actions** — Shortcuts to apply saved outfits instantly in session

### Chat — Multiplayer
- **Mod Chat** — Send and receive messages with other players running the mod menu in your session

### Avalanche — Chaos Mode
- **Avalanche Mode** — Spawns boulders above you that roll downhill. Survive as long as possible
- **Survival Timer** — Tracks how long you've lasted since the avalanche started
- **Active Hazards** — Live count of boulders currently in play
- **Spawn Settings** — Configure spawn interval, max hazards, boulder size and spawn radius
- **Physics Settings** — Control boulder shape, extra gravity and attraction force
- **Instant Fail on Hit** — End the run immediately on first boulder contact
- **Difficulty Scaling** — Gradually increases spawn rate the longer you survive
- **Show Timer** — Toggle the on-screen survival timer overlay

### Ghost Replay — Run Recording
- **Ghost Replay** — Records your run and plays it back as a semi-transparent ghost rider alongside you
- **Recording Controls** — F3 / RS double-click to toggle, F4 / LS click to save current run
- **Ghost Alpha** — Adjust ghost transparency
- **Saved Ghost Run** — Displays length and time of your saved ghost run
- **Clear** — Wipe the saved ghost recording

### MapChange — Level Select
- **Map Changer** — Browse and load any base game world or bike park directly from the menu without going through the main menu
- **Base Game Maps** — All procedural biomes: Highlands, Forest, Canyon, Peaks, Desert, Jungle, Favela, Glaciers, Ridges, Hell
- **Bike Parks & Freeride** — All handcrafted bike park maps

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
| F3 | Toggle Ghost Replay on / off |
| F4 | Save current run as ghost |
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
- **Top Speed** uses the game's own speedo formula (`velocity × 3.6 / gravity × 9.81`) and saves your all-time record to `UserData/DescendersModMenu/TopSpeed.txt`. Only the Reset button clears it.
- **Bail Counter** uses a Harmony postfix on `Cyclist.Bail()` — counts actual crashes, not manual resets or checkpoint teleports.
- **Moon Mode** saves your current Gravity, Suspension Travel and Damping levels before activating, and restores them when you deactivate.
- **No Mistakes** only triggers on sideways impacts (contact normal y ≤ 0.5) above 5 m/s — landings and gentle bumps are ignored.
- **Ghost Replay** records position and rotation every 2 frames. Max recording length is 18,000 frames (~5 minutes). Save your best run with F4 and it will persist until you clear it or record a new one.
- **Avalanche Mode** only works in procedural worlds — bike parks have no terrain for boulders to spawn on.
- **Map Changer** loads maps the same way the game does internally, preserving career progress and session state.
- **Outfit Presets** save your current helmet, jersey and bike selection. Presets persist between sessions.
- **Save / Load / Reset** buttons at the bottom of the Stats tab persist your settings between sessions to `UserData/DescendersModMenu/BikeStats.json`.

---

## Building from Source

- Visual Studio 2022 or Rider
- Target framework: .NET 4.7.2
- References: `MelonLoader.dll`, `HarmonyLib.dll`, `Assembly-CSharp.dll`, `UnityEngine.*.dll`
- Output DLL goes into `Descenders/Mods/`

---

## Changelog

### v3.0.0
- Expanded from 9 to 15 tabs — added Graphics, Outfit, Chat, Avalanche, GhostReplay, MapChange
- **Ghost Replay** — records your run and plays it back as a ghost rider. Save your best run and compare on future attempts. F3 to toggle, F4 to save
- **Avalanche Mode** — boulders spawn above you and roll downhill. Configurable spawn rate, boulder size, gravity, attraction force, difficulty scaling and instant-fail mode. Survival timer included
- **Map Changer** — load any base game world or bike park directly from the menu. No main menu required
- **Outfit Presets** — save and load up to 3 full outfit loadouts for quick in-session switching
- **Mod Chat** — send and receive messages with other mod menu users in your session
- **Graphics tab** — toggle Bloom, Ambient Occlusion, Vignette, Depth of Field and Chromatic Aberration individually, plus Low/Medium/High/Ultra quality presets
- **Storm mode** — dark sky, wind and rain via direct TOD_Sky skydome swap and EffectList refresh (World tab)
- **Sky Colour presets** — Blood Red, Alien Green, Synthwave, Midnight, Toxic (World tab)
- **Wide Tyres** — 20-level tyre width slider (Silly tab)
- **Sticky Tyres** — grip any surface including walls and ceilings (Silly tab)
- **Fly Mode** — detach from the bike and fly freely (Silly tab)
- **Drunk Mode** — persistent FOV wobble effect (Silly tab)
- **Mirror Mode** — flips the world left/right (Silly tab)
- **Reverse Steering** — moved to Silly tab controls section
- **Mod Users** indicator in ESP tab — highlights other players running the mod menu
- Ghost Replay keybinds: F3 toggle, F4 save run, RS double-click toggle, LS click save
- Fixed Invisible Bike and Invisible Player restoring placeholder/disabled renderers on toggle-off
- UI updated to v3.0.0 version badge in header

### v2.1.0
- New sidebar navigation UI replacing the old horizontal tab bar
- Added Invisible Bike (Silly tab)
- Added Moon Mode preset — low gravity + bouncy suspension with save/restore (Silly tab)
- Added Wheel Size — Small / Default / Large via bone transforms and physics radius (Silly tab)
- Added No Mistakes — wall/rock collisions launch you backwards (Silly tab)
- Added Session Timer, Bail Counter, Longest Airtime (Stats tab)
- Added Fog Remover toggle (World tab)
- Top Speed now matches the in-game speedometer exactly and persists to file
- Bail Counter uses Cyclist.Bail() Harmony patch — counts real crashes, not resets
- Action buttons use a softer orange
- Mod status dots are proper circles
- Info tab now tracks 34+ mods
- Menu height scales to fit all content
- Version display enlarged in header

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
