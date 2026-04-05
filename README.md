# Descenders Mod Menu / Descenders Toolkit

**v3.7.0** — The most complete mod menu for Descenders. 60+ mods, 6 game modes, ghost replay, map changer, outfit presets, mod chat and full scene persistence — all in one clean sidebar overlay.

> Previously known as Descenders Mod Menu — now rebuilt, rebranded and expanded as Descenders Toolkit.

Physics tweaks, bike tuning, world controls, score tools, session trackers, ghost replay, game modes, map changer, outfit presets, mod chat and plenty of chaos — all in one place. Works online and offline.

**Download:** [Nexus Mods](https://www.nexusmods.com/descenders/mods/7) · [GitHub Releases](https://github.com/NateHyden/DescendersModMenu/releases) · [Discord](https://discord.gg/rHvCrBdqaR)

---

## Requirements

- [MelonLoader 0.5.7](https://github.com/LavaGang/MelonLoader/releases/tag/v0.5.7) *choose MelonLoader 0.5.7 from the drop down menu*
- Descenders (Steam or Microsoft Store / Xbox PC)

---

## Installation — Steam

1. Run the MelonLoader installer, select your Descenders executable and choose version **0.5.7** — it will not work on other versions
2. Launch the game once to let MelonLoader populate, then close it
3. Download `DescendersToolKit.dll` from the [Releases page](https://github.com/NateHyden/DescendersModMenu/releases)
4. Drop it into `Descenders/Mods/`
5. Launch the game and press **F6** to open the menu

> If updating from an older version, delete any previous `DescendersModMenu.dll` or `DescendersToolKit.dll` from your Mods folder first.

---

## Installation — Microsoft Store / Xbox PC (Game Pass)

MelonLoader and the toolkit also work on the Microsoft Store and Xbox PC (Game Pass) version of Descenders. The installer won't auto-detect the Xbox version, so you need to point it there manually.

> **Note:** The Microsoft Store version is largely untested. MelonLoader installs correctly and the mod menu loads and functions, but not every mod has been fully tested on this version. If you encounter issues specific to the Microsoft Store build, please report them on [Discord](https://discord.gg/rHvCrBdqaR) or [GitHub](https://github.com/NateHyden/DescendersModMenu/issues).

### Recommended: Install Descenders to a custom folder

The default Microsoft Store install location (`C:\Program Files\WindowsApps`) is hidden and heavily permission-locked, making it difficult to access game files. It is **strongly recommended** to install Descenders to a custom folder on any drive before proceeding.

To change where Xbox games install:
1. Open the **Xbox app** → click your profile picture → **Settings** → **Installation options**
2. Change the default install location to a folder you have full access to (e.g. `D:\XboxGames`)
3. If Descenders is already installed in the default location, uninstall it and reinstall — it will now go to your chosen folder

This avoids all permission issues and makes the Mods folder easy to access.

### Finding your game files

If you've installed to a custom folder, simply navigate there in File Explorer. Otherwise:

1. Open the **Xbox app** on your PC
2. Go to **Library**, find **Descenders**, click the **three dots (⋯)**
3. Select **Manage** then click **Files** — this will open the game's install folder in File Explorer

> If using the default WindowsApps location, you may need to grant yourself folder permissions. Right-click the game folder → Properties → Security → Edit → Add your user account → grant Full Control.

### Installing MelonLoader

1. Run the [MelonLoader 0.5.7](https://github.com/LavaGang/MelonLoader/releases/tag/v0.5.7) installer
2. MelonLoader will **not** auto-detect the Xbox version — click the **Select Game** or **Add Game** button in the installer
3. Navigate to your Descenders install folder and select **Descenders.exe**
4. Make sure version **0.5.7** is selected and install
5. Launch the game once to let MelonLoader populate its folders, then close it
6. Download `DescendersToolKit.dll` from the [Releases page](https://github.com/NateHyden/DescendersModMenu/releases) and drop it into the `Mods` folder inside your Descenders directory
7. Launch the game and press **F6** to open the menu

> This has been tested and confirmed working. Despite previous reports that mods are not possible on the Microsoft Store version, MelonLoader installs and runs correctly when pointed to the game manually.

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

### General — Bike Physics

The main hub. Save / Load / Reset buttons are always visible in the header bar regardless of which tab you are on.

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

---

### Session — Stats & HUD

- **Session Timer** — live MM:SS counter from when you load in
- **Top Speed** — records your highest speed this session; saves to `UserData/DescendersModMenu/TopSpeed.txt`
- **Speedrun Timer** — integrates with the in-game timer
- **Bail Counter** — tracks actual crashes, not resets
- **Checkpoint Counter** — counts triggered checkpoints this session
- **Longest Airtime** — tracks your longest single airtime
- **G-Force / Peak G-Force** — live readout and highest value this session
- **On-Screen HUD** — toggle a top-right overlay showing all live stats while riding (resolution-scaled)
- All session stats and Top Speed reset automatically when a new map loads

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
- **Near Miss Sensitivity** — controls how close objects need to pass to trigger a near miss
- **Center of Mass** — shift rider weight Left/Right, Forward/Back, Up/Down

---

### Bike — Tuning & Parts

Scrollable tab. All mods have active row highlighting when enabled.

**Suspension** — Travel, Stiffness, Damping (all 10-level sliders; level 5 = stock)

**Bike Size** — 10 presets across two rows: Colossal, Giant, Big, Default, Small, Tiny, Extra Tiny and more. Default restores the scale captured at map load

**Bike Parts**
- **Invisible Bike** — hides the entire bike model
- **Wheel Size** — toggle + 5 presets (Tiny / Small / Default / Large / Huge). Works alongside Wide Tyres
- **Wide Tyres** — 20-level slider. Scales relative to the current wheel size so both mods combine properly
- **Sticky Tyres** — grip any surface including slopes, walls and ceilings

**Controls**
- **Reverse Steering** — flips left/right steering input
- **Ice Grip** — removes all tyre grip
- **Cut Brakes** — disables braking entirely

**Torch**
- **Bike Headlight** — toggle with five brightness levels (Dim to Max)

---

### Graphics — Post Processing & Quality

- Toggle **Bloom**, **Ambient Occlusion**, **Vignette**, **Chromatic Aberration** individually (all default ON)
- Toggle **Depth of Field** (defaults OFF)
- Quality presets — Low, Medium, High, Ultra, Default
- Graphics always reset to the map's native defaults on scene change — they are not persisted

---

### World — Environment

**Sky**
- Sky Colour presets — Normal, Blood Red, Alien Green, Synthwave, Midnight, Toxic
- **Storm** — dark sky with heavy rain. Built on a custom particle system (up to 20,000 particles) attached to the camera, intensity-adjustable via slider. Persists correctly across the entire session
- Gravity — level 1 is floaty, level 5 is default (-17.5 m/s²), level 10 is heavy

**Environment**
- **Time of Day** — Dawn to Night. Each map loads with its own native time of day. Resets to the loaded map's native time when reset is pressed
- **Trees & Foliage** — toggle
- **Music** — toggle; restores exact volume on unmute
- **Fog** — toggle; saves and restores original density
- **Turbo Wind** — cranks wind zones to maximum
- **No Mistakes / Exploding Props** — props launch into the air on impact

**Level**
- Jump to Finish — instantly triggers the finish line
- Skip Song

---

### Fun — Chaos & Player

**Player Size** — Giant, Big, Default, Small, Tiny. Default restores the scale captured at map load

**Presets** — Moon Mode (low gravity + max suspension; saves and restores your settings on deactivate)

**Multiplayer** — Giant Everyone — 7 size presets scaling all other players simultaneously

**Effects** — Mirror Mode (flips the world left/right), Fly Mode (detach and fly freely), Drunk Mode (FOV wobble)

**Player** — Invisible Player

**Camera** — Camera Shake — adjustable intensity

---

### Outfit — Presets

- Five named preset slots — save your full current outfit to any slot
- Load any preset instantly during a session
- Rename slots by clicking the name, typing and pressing Enter
- Delete individual presets

---

### Chat — Mod Users *(Experimental)*

- Send and receive messages with other Descenders Toolkit users in your session
- Scrollable log with timestamps and player names
- Only visible to other mod users

---

### Modes *(Experimental)*

All modes work in both procedural worlds and bike parks. Global Reset turns off any active mode. Modes always reset on scene change and are not saved.

- **Avalanche** — Boulders spawn above you and roll downhill. Configurable spawn rate, size, gravity and difficulty scaling
- **Earthquake** — The ground shakes beneath you
- **Police Chase** — A pursuer is on your tail. Don't get caught
- **Trick Attack** — Score as many points as possible before the timer runs out
- **Boulder Dodge** — Boulders drop from directly above your predicted path. Dodge them
- **Survival** — You have 100HP. Bail and lose health, get airtime to heal. Game over when you hit zero. Configurable bleed rate, bail penalty and heal threshold

---

### Ghost Replay

- Records your run in real time and plays it back as a transparent ghost rider
- Save your best run to disk — persists between sessions for racing against yourself
- Clear saved run button
- Ghost HUD overlay showing recording status and run length
- Toggle: F3 or double-click RS — Save: F4 or RS click — Set spawn: LS click
- Max recording length ~5 minutes
- Ghost Replay resets on scene change and is not saved globally

---

### ESP — Players & Teleport

- Player name labels with distance readout
- ESP Tracers — draws lines to all nearby players
- **Teleport to Last Checkpoint** — snaps back to your last triggered checkpoint
- **Mod Users** — scans for and highlights other Descenders Toolkit users in your session
- ESP resets on scene change and is not saved globally

---

### Score

- Add REP — +100, +500, +1K, +5K, +10K, +50K, +100K
- Remove REP — -100, -500, -1K, -5K, -10K
- **Trick Multiplier** — x1, x2, x5, x10 or x20; locked in until you manually reset to x1

---

### Map Changer

- Load any base game world directly — Highlands, Forest, Canyon, Desert, Peaks, Jungle, Ridges, Hell and more
- Browse and load Bike Park and Freeride Workshop maps
- Load any level by seed number — share seeds with friends to ride the same world
- No main menu required

---

### Info/Customise — Diagnostics, Reference & Settings

Six sub-tabs:
- **System** — Unity version, MelonLoader version, scripting backend, toolkit version and live Steam player count
- **Mod Status** — every mod listed with OK / FAILED status and error details
- **Hotkeys** — full key binding reference
- **Credits** — GitHub, Nexus links, build info and game credits
- **Customise** — move, resize and adjust the opacity of the mod menu. Settings save automatically
- **Dev Tools** — Assembly Scanner: checks every reflected field and method the mod uses against the live game assembly. Saves a full report to `UserData/DescendersModMenu/AssemblyReport.txt`

---

## Menu Customiser

The mod menu can be repositioned, resized and made more or less transparent to suit your setup. Settings are saved automatically to `UserData/DescendersModMenu/MenuLayout.json` and restored every session.

---

## Save System

All settings persist to `UserData/DescendersModMenu/BikeStats.json`. Your last saved settings **auto-load when you first spawn in** — no need to press Load manually.

**Saved:** all levels, all toggle states, tyre settings, suspension, game modifiers, bike/player/wheel size, invisible states, torch state and intensity, camera shake, center of mass, fly mode speeds, menu layout and more.

**Not saved (always reset per-map):** Graphics tab, Sky section (sky colour preset, storm, rain), Time of Day, Game Modes, Ghost Replay, ESP.

**Scene persistence:** Active mods are snapshotted when you leave a map and automatically reapplied when you spawn into the next one. Each map loads with its own native time of day and graphics.

---

## Update Notifications

The toolkit checks for new versions on GitHub when the game launches. If a newer release is available, a green notification appears in the menu header. The check runs on a background thread and never affects game performance.

---

## Notes

- Intended for single-player and private sessions
- Suspension sliders default to level 5 — stock game behaviour
- No Speed Cap uses a velocity-zeroing technique to let the game's native acceleration run without the hard-coded speed limit. Works alongside the Acceleration mod
- Wide Tyres and Wheel Size combine properly — wide tyres scale relative to the current wheel size
- Cut Brakes resets automatically on scene change
- Moon Mode saves your gravity and suspension before activating and restores them on deactivate
- Ghost Replay records every 2 frames; max ~5 minutes. Saves persist until cleared
- If the mod does not load, check your MelonLoader log in the game folder

---

## Changelog

### v3.7.0
- **Update notifications** — the toolkit now checks GitHub for new releases on startup. If a newer version is available, a green notification appears in the menu header. Runs on a background thread with no impact on game performance

### v3.6.2
- **Scene transition system** — active mods now persist across map changes. Mods are snapshotted before scene unload and reapplied when you spawn into the next map. Intermediate scenes (EmptyScene) are skipped correctly. Each mod wrapped in individual error handling so one failure cannot affect the rest
- **Auto-load on startup** — your last saved settings load automatically when you first spawn in, with no warnings or log noise
- **Load button now resets first** — pressing Load clears all current state before applying saved values, ensuring stale mods get properly turned off
- **NoSpeedCap + Acceleration compatibility** — completely reworked. NoSpeedCap now uses a velocity-zeroing technique to let the game's native acceleration run uncapped instead of replacing it with simplified physics. Both mods work together naturally
- **Wide Tyres + Wheel Size compatibility** — fixed execution order and scale combination. Wide tyres now scale relative to the current wheel size instead of overwriting it
- **Wheel Size reset fixed** — now properly restores wheel radius physics value before clearing caches. No more tiny-wheel RPM or sinking into the floor after global reset
- **Scene-native time of day** — each map loads with its own intended time. Dark maps stay dark, afternoon maps stay bright. Time of Day removed from scene persistence and global save
- **Scene defaults re-captured live** — Reset and Load buttons now re-read the current scene's actual time of day and sky values instead of using potentially stale captures
- **GameModifiers apply on Load/Reset** — SetLevel functions now call ApplyMod so Pump Strength, Wheelie Balance and other game modifiers actually take effect after Load or Reset (was previously UI-only)
- **Newly persisted across scenes:** Bike Scale, Player Scale, Invisible Bike, Invisible Player, Wheel Size, Wide Tyres level, NoSpeedCap, Bike Torch, Camera Shake, Near Miss Sensitivity, Center of Mass, Suspension, Exploding Props
- **Graphics tab false dot fixed** — Depth of Field default corrected so the Graphics tab no longer shows a green modified dot on startup
- **Not persisted (always reset per-map):** Graphics tab, Sky section, Time of Day, Game Modes, Ghost Replay, ESP
- **UI polish** — sidebar tab text brightened for readability, "Created by NateHyden" text brightened
- **Log cleanup** — eliminated Type::op_Equality and FieldInfo::op_Equality log spam, removed unnecessary warnings during scene transitions and before player spawn

### v3.6.1
- **Storm fully rebuilt** — heavy rain now renders via a custom particle system attached to the camera (up to 20,000 particles, intensity-scaled). Storm persists correctly throughout the session. Old TickStorm moved from destroyed TOD_Sky to OnUpdate. Environment flags enforced every frame via new Harmony patches
- **Session tab** — session stats and HUD moved out of General into their own dedicated tab. On-screen HUD toggle added. All session data and Top Speed reset on map load
- **Sidebar scroll and active mod dots** — sidebar scrolls when tabs overflow; a neon dot appears next to any tab with active mods
- **Reset Tab buttons** — added to Bike, Move and World tabs
- **Active row highlighting** — toggle rows tint green when the mod is on
- **Tab reorganisation** — Bike tab gained Invisible Bike, Wheel Size, Wide Tyres, Sticky Tyres, Reverse Steering, Ice Grip, Cut Brakes and Torch. Fun trimmed to effects only. World gained Turbo Wind and No Mistakes. Move lost Cut Brakes
- **Size presets expanded** — Bike Size 10 presets, Wheel Size 5 presets, Giant Everyone 7 presets
- **Default scale capture** — Player and Bike Default buttons restore the actual scale from map load, not a hardcoded value
- **Global Reset overhauled** — now covers all 6 modes, Wheelie Angle Limit, Air Control, Session HUD, player/bike scale, moon mode, invisible states, turbo wind, trees, music, fog, wheel size. Fixed Moon Mode deactivation ordering. Depth of Field defaults OFF. GameModifiers reset to 5. Time of Day resets to map's native time

### v3.6.0
- Added Bike Torch, Boulder Dodge Mode, Survival Mode, Trick Attack Mode, Police Chase Mode, Earthquake Mode
- Added Camera Shake, Center of Mass, Near Miss Sensitivity, Graphics toggles per-effect, Assembly Scanner
- Added Menu Customiser — reposition, resize and adjust opacity; auto-saves
- All game modes moved into a dedicated Modes tab with tabbed navigation
- Sticky Tyres completely rewritten — now uses raycast-based surface detection, works reliably on all surfaces
- Fly Mode fixed — no longer throws rider off the bike on activation
- Input crossbleed fixed — typing in one box no longer types into another simultaneously
- QuickBrake NullReferenceException on scene unload fixed
- "Silly" tab renamed to "Fun", "Info" renamed to "Info/Customise"
- SESSION header now has a Reset All button for session stats
- Save/Load/Reset header buttons flash green on click
- Save system expanded — torch, camera shake, center of mass, graphics, sky storm and more now persist

### v3.5.0
- Rebranded from Descenders Mod Menu to Descenders Toolkit — DLL is now `DescendersToolKit.dll`
- Added Quick Brake
- Save / Load / Reset moved to the header bar
- Info tab redesigned as four sub-tabs: System, Mod Status, Hotkeys, Credits
- Fixed toggle states not saving for several mods

### v3.2.0
- FOV toggle, Acceleration and Max Speed toggles added
- Landing Impact reworked
- All four Movement mods now have individual on/off toggles

### v3.1.0
- Slow Motion speed slider and Auto Balance strength control
- No Speed Wobbles rewritten
- Wheelie Angle Limit and Air Control added
- Graphics quality presets fixed

### v3.0.0
- Expanded to 15 tabs — Ghost Replay, Avalanche, Map Changer, Outfit Presets, Mod Chat, Graphics
- Storm mode, Sky Colour presets, Wide Tyres, Sticky Tyres, Fly Mode, Drunk Mode, Mirror Mode

### v2.1.0
- New sidebar navigation UI
- Invisible Bike, Moon Mode, Wheel Size, No Mistakes, Session Trackers

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
