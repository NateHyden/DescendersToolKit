using DescendersModMenu.Mods;
using DescendersModMenu.UI;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu
{
    public static class BuildInfo
    {
        public const string Name = "ModMenu";
        public const string Description = "Modkit for Descenders";
        public const string Author = "NateHyden";
        public const string Company = null;
        public const string Version = "3.1.0";
        public const string DownloadLink = null;
    }

    public class DescendersModMenu : MelonMod
    {
        private HarmonyLib.Harmony harmony;

        // Right stick click state for ghost replay binds
        private float _lastRStickClick = -999f;
        private bool _pendingRStickSave = false;
        private float _rStickSaveTime = 0f;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("OnApplicationStart");

            try { CodeStage.AntiCheat.Detectors.InjectionDetector.Dispose(); MelonLogger.Msg("AntiCheat disposed: InjectionDetector"); }
            catch (System.Exception ex) { MelonLogger.Warning("AntiCheat dispose failed (InjectionDetector): " + ex.Message); }
            try { CodeStage.AntiCheat.Detectors.ObscuredCheatingDetector.Dispose(); MelonLogger.Msg("AntiCheat disposed: ObscuredCheatingDetector"); }
            catch (System.Exception ex) { MelonLogger.Warning("AntiCheat dispose failed (ObscuredCheatingDetector): " + ex.Message); }
            try { CodeStage.AntiCheat.Detectors.SpeedHackDetector.Dispose(); MelonLogger.Msg("AntiCheat disposed: SpeedHackDetector"); }
            catch (System.Exception ex) { MelonLogger.Warning("AntiCheat dispose failed (SpeedHackDetector): " + ex.Message); }
            try { CodeStage.AntiCheat.Detectors.TimeCheatingDetector.Dispose(); MelonLogger.Msg("AntiCheat disposed: TimeCheatingDetector"); }
            catch (System.Exception ex) { MelonLogger.Warning("AntiCheat dispose failed (TimeCheatingDetector): " + ex.Message); }
            try { CodeStage.AntiCheat.Detectors.WallHackDetector.Dispose(); MelonLogger.Msg("AntiCheat disposed: WallHackDetector"); }
            catch (System.Exception ex) { MelonLogger.Warning("AntiCheat dispose failed (WallHackDetector): " + ex.Message); }

            harmony = new HarmonyLib.Harmony("DescendersModMenu.Patches");
            try { harmony.PatchAll(); MelonLogger.Msg("Harmony patches applied."); DiagnosticsManager.Report("Harmony", true); }
            catch (System.Exception ex) { MelonLogger.Error("PatchAll failed: " + ex.Message); DiagnosticsManager.Report("Harmony", false, ex.Message); }
            try { NoSpeedCap.ApplyPatch(harmony); DiagnosticsManager.Report("NoSpeedCap", true); }
            catch (System.Exception ex) { MelonLogger.Error("NoSpeedCap.ApplyPatch failed: " + ex.Message); DiagnosticsManager.Report("NoSpeedCap", false, ex.Message); }
            try { NoSpeedCap.ApplyVCPatch(harmony); DiagnosticsManager.Report("NoSpeedCap (VC)", true); }
            catch (System.Exception ex) { MelonLogger.Error("NoSpeedCap.ApplyVCPatch failed: " + ex.Message); DiagnosticsManager.Report("NoSpeedCap (VC)", false, ex.Message); }
            try { CutBrakes.ApplyPatch(harmony); DiagnosticsManager.Report("CutBrakes", true); }
            catch (System.Exception ex) { MelonLogger.Error("CutBrakes.ApplyPatch failed: " + ex.Message); DiagnosticsManager.Report("CutBrakes", false, ex.Message); }
            try { ReverseSteering.ApplyPatch(harmony); DiagnosticsManager.Report("ReverseSteering", true); }
            catch (System.Exception ex) { MelonLogger.Error("ReverseSteering.ApplyPatch failed: " + ex.Message); DiagnosticsManager.Report("ReverseSteering", false, ex.Message); }
            try { AutoBalance.ApplyPatch(harmony); DiagnosticsManager.Report("AutoBalance", true); }
            catch (System.Exception ex) { MelonLogger.Error("AutoBalance.ApplyPatch failed: " + ex.Message); DiagnosticsManager.Report("AutoBalance", false, ex.Message); }
            try { IceMode.ApplyPatch(harmony); DiagnosticsManager.Report("IceMode", true); }
            catch (System.Exception ex) { MelonLogger.Error("IceMode.ApplyPatch failed: " + ex.Message); DiagnosticsManager.Report("IceMode", false, ex.Message); }
            try { SkyColours.ApplyPatch(harmony); }
            catch (System.Exception ex) { MelonLogger.Error("SkyColours.ApplyPatch failed: " + ex.Message); }
            try { DrunkMode.ApplyPatch(harmony); DiagnosticsManager.Report("DrunkMode", true); }
            catch (System.Exception ex) { MelonLogger.Error("DrunkMode.ApplyPatch failed: " + ex.Message); DiagnosticsManager.Report("DrunkMode", false, ex.Message); }
            try { SessionTrackers.ApplyBailPatch(harmony); DiagnosticsManager.Report("BailCounter", true); }
            catch (System.Exception ex) { MelonLogger.Error("BailPatch failed: " + ex.Message); DiagnosticsManager.Report("BailCounter", false, ex.Message); }
            try { GhostReplay.ApplyPatch(harmony); DiagnosticsManager.Report("GhostReplay", true); }
            catch (System.Exception ex) { MelonLogger.Error("GhostReplay.ApplyPatch failed: " + ex.Message); DiagnosticsManager.Report("GhostReplay", false, ex.Message); }
            try { GameModifierMods.ApplyNoSpeedWobblesPatch(harmony); DiagnosticsManager.Report("NoSpeedWobbles", true); }
            catch (System.Exception ex) { MelonLogger.Error("NoSpeedWobbles patch failed: " + ex.Message); DiagnosticsManager.Report("NoSpeedWobbles", false, ex.Message); }
            try { WheelieAngleLimit.ApplyPatch(harmony); DiagnosticsManager.Report("WheelieAngleLimit", true); }
            catch (System.Exception ex) { MelonLogger.Error("WheelieAngleLimit patch failed: " + ex.Message); DiagnosticsManager.Report("WheelieAngleLimit", false, ex.Message); }
            try { MapChanger.ApplyPatch(harmony); DiagnosticsManager.Report("MapChanger", true); }
            catch (System.Exception ex) { MelonLogger.Warning("MapChanger.ApplyPatch failed: " + ex.Message); DiagnosticsManager.Report("MapChanger", false, ex.Message); }
            try { OutfitPresets.Init(); }
            catch (System.Exception ex) { MelonLogger.Error("OutfitPresets.Init failed: " + ex.Message); }
            try { ModChat.Init(); }
            catch (System.Exception ex) { MelonLogger.Error("ModChat.Init failed: " + ex.Message); }
        }

        public override void OnLateInitializeMelon()
        {
            MelonLogger.Msg("OnApplicationLateStart");
            DiagnosticsManager.Report("SlowMotion", true);
            DiagnosticsManager.Report("FOV", true);
            DiagnosticsManager.Report("ESP", true);
            DiagnosticsManager.Report("NoBail", true);
            DiagnosticsManager.Report("Acceleration", true);
            DiagnosticsManager.Report("MaxSpeed", true);
            DiagnosticsManager.Report("LandingImpact", true);
            DiagnosticsManager.Report("BikeSwitcher", true);
            DiagnosticsManager.Report("TeleportToPlayer", true);
            DiagnosticsManager.Report("ScoreManager", true);
            DiagnosticsManager.Report("UnlockAll", true);
            DiagnosticsManager.Report("Movement", true);
            DiagnosticsManager.Report("Gravity", true);
            DiagnosticsManager.Report("TimeOfDay", true);
            DiagnosticsManager.Report("GameModifiers", true);
            DiagnosticsManager.Report("TopSpeed", true);
            DiagnosticsManager.Report("TeleportToCheckpoint", true);
            DiagnosticsManager.Report("Suspension", true);
            DiagnosticsManager.Report("Trees & Foliage", true);
            DiagnosticsManager.Report("Music", true);
            DiagnosticsManager.Report("Jump to Finish", true);
            DiagnosticsManager.Report("Skip Song", true);
            DiagnosticsManager.Report("Bike Size", true);
            DiagnosticsManager.Report("Player Size", true);
            DiagnosticsManager.Report("Invisible Player", true);
            DiagnosticsManager.Report("Turbo Wind", true);
            DiagnosticsManager.Report("No Mistakes", true);
            DiagnosticsManager.Report("Giant Everyone", true);
            DiagnosticsManager.Report("Invisible Bike", true);
            DiagnosticsManager.Report("Moon Mode", true);
            DiagnosticsManager.Report("Wheel Size", true);
            DiagnosticsManager.Report("Fog Remover", true);
            DiagnosticsManager.Report("SessionTrackers", true);
            DiagnosticsManager.Report("WideTyres", true);
            DiagnosticsManager.Report("StickyTyres", true);
            DiagnosticsManager.Report("FlyMode", true);
            DiagnosticsManager.Report("MirrorMode", true);
            DiagnosticsManager.Report("SpeedrunTimer", true);
            DiagnosticsManager.Report("SlowMoOnBail", true);
            DiagnosticsManager.Report("AirControl", true);
            DiagnosticsManager.Report("ModDetection", true);
            TopSpeed.Load();
            TopSpeed.StartTracking();

        }

        public override void OnSceneWasLoaded(int buildindex, string sceneName)
        {
            MelonLogger.Msg("OnSceneWasLoaded: " + buildindex + " | " + sceneName);
            GhostReplay.OnSceneLoaded();
        }

        public override void OnSceneWasInitialized(int buildindex, string sceneName)
        {
            MelonLogger.Msg("OnSceneWasInitialized: " + buildindex + " | " + sceneName);
            GhostReplay.OnSceneInitialized();
            MapChanger.OnSceneInitialized();
            ExplodingProps.OnSceneInitialized(sceneName);
            // Build map list when main menu loads (scene 1) — GameData is ready here
            if (buildindex == 1)
                MapChanger.BuildMapList();
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg("OnSceneWasUnloaded: " + buildIndex + " | " + sceneName);
            SlowMotion.Reset();
            CutBrakes.Reset();
            ReverseSteering.Reset();
            AutoBalance.Reset();
            WideTyres.Reset();
            IceMode.Reset();
            SpeedrunTimer.Reset();
            GameModifierMods.NoSpeedWobblesReset();
            MirrorMode.Reset();
            FlyMode.Reset();
            DrunkMode.Reset();
            OutfitPresets.Reset();
            ModChat.Reset();
            AvalancheMode.Reset();
            GhostReplay.Reset();
            SlowMoOnBail.Reset();
            SkyColours.Reset();
            GraphicsSettings.Reset();
            TopSpeed.ClearCache();
            SessionTrackers.Reset();
            ExplodingProps.Reset();
            StickyTyres.Reset();
            WheelieAngleLimit.Reset();
            AirControl.Reset();
            ScoreManager.ResetMultiplier();
            FOV.ClearCache();
            NoBail.ClearCache();
            Page9UI.ResetWheelSize();
        }

        public override void OnUpdate()
        {
            try
            {
                // ── Ghost Replay keybinds ──────────────────────────────────
                // F3 / RS double-click → toggle on/off
                if (Input.GetKeyDown(KeyCode.F3))
                {
                    GhostReplay.Toggle();
                    Page14UI.RefreshAll();
                }

                // F4 / LS single-click → save current run as ghost
                if (Input.GetKeyDown(KeyCode.F4))
                {
                    GhostReplay.SaveRun();
                    Page14UI.RefreshAll();
                }

                // LS click (JoystickButton8) → set spawn marker
                if (Input.GetKeyDown(KeyCode.JoystickButton8))
                {
                    GhostReplay.SetSpawnMarker();
                    Page14UI.RefreshAll();
                }

                // RS click (JoystickButton9) — single = save run, double = toggle
                if (Input.GetKeyDown(KeyCode.JoystickButton9))
                {
                    float now = Time.realtimeSinceStartup;
                    float gap = now - _lastRStickClick;
                    _lastRStickClick = now;

                    if (gap < 0.4f)
                    {
                        // Double click — toggle on/off
                        GhostReplay.Toggle();
                        Page14UI.RefreshAll();
                        _lastRStickClick = -999f;
                    }
                    else
                    {
                        // Single click — arm pending save
                        _pendingRStickSave = true;
                        _rStickSaveTime = now + 0.4f;
                    }
                }

                // Fire pending RS single-click save
                if (_pendingRStickSave && Time.realtimeSinceStartup >= _rStickSaveTime)
                {
                    _pendingRStickSave = false;
                    if (GhostReplay.IsRecording && GhostReplay.RecordedFrames >= 30)
                    {
                        GhostReplay.SaveRun();
                        Page14UI.RefreshAll();
                    }
                }

                if (Input.GetKeyDown(KeyCode.F6))
                    MenuUI.ToggleMenu();
            }
            catch (System.Exception ex) { MelonLogger.Error("ToggleMenu: " + ex.Message); }
            try { SceneDumper.CheckHotkey(); }
            catch (System.Exception ex) { MelonLogger.Error("SceneDumper: " + ex.Message); }
            try { SpeedWatcher.CheckHotkey(); }
            catch (System.Exception ex) { MelonLogger.Error("SpeedWatcher: " + ex.Message); }
            try { TopSpeed.Tick(); }
            catch (System.Exception ex) { MelonLogger.Error("TopSpeed.Tick: " + ex.Message); }
            try { SessionTrackers.Tick(); }
            catch (System.Exception ex) { MelonLogger.Error("SessionTrackers.Tick: " + ex.Message); }
            try { MenuWindow.TickLive(); }
            catch { }
            try { MirrorMode.Tick(); }
            catch { }
            try { FlyMode.Tick(); }
            catch { }
            try { DrunkMode.Tick(); }
            catch { }
            try { Page11UI.Tick(); }
            catch { }
            try { Page12UI.Tick(); }
            catch { }
            try { Page13UI.Tick(); }
            catch { }
            try { AvalancheMode.Tick(); }
            catch { }
            try { GhostReplay.Tick(); }
            catch { }
            try { MapChanger.Tick(); }
            catch { }
            try { Page14UI.Tick(); }
            catch { }
            try { SlowMoOnBail.Tick(); }
            catch { }
            try { ScoreManager.Tick(); }
            catch { }
            try { ModDetection.TagLocalPlayer(); }
            catch { }

            try { if (Input.GetKeyDown(KeyCode.F2)) SlowMotion.Toggle(); }
            catch (System.Exception ex) { MelonLogger.Error("SlowMotion.Toggle: " + ex.Message); }
        }

        public override void OnFixedUpdate()
        {
            try { AvalancheMode.FixedTick(); }
            catch { }
            try { StickyTyres.FixedTick(); }
            catch { }
            try { AirControl.FixedTick(); }
            catch { }
        }

        public override void OnLateUpdate()
        {
            try { FOV.Apply(); }
            catch (System.Exception ex) { MelonLogger.Error("FOV.Apply: " + ex.Message); }
            try { SkyColours.Tick(); }
            catch (System.Exception ex) { MelonLogger.Error("SkyColours.Tick: " + ex.Message); }
            try { DrunkMode.LateTick(); }
            catch { }
            try { WideTyres.Tick(); }
            catch { }
            try { Page9UI.WheelSizeTick(); }
            catch { }
        }

        public override void OnGUI()
        {
            try { ESP.OnGUI(); }
            catch (System.Exception ex) { MelonLogger.Error("ESP.OnGUI: " + ex.Message); }
            try { GhostHUD.Draw(); }
            catch (System.Exception ex) { MelonLogger.Error("GhostHUD.Draw: " + ex.Message); }
        }

        public override void OnApplicationQuit()
        {
            MenuUI.RestoreCursor();
            SlowMotion.Reset();
            MelonLogger.Msg("OnApplicationQuit");
        }
    }
}