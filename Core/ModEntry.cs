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
        public const string Version = "2.0.0";
        public const string DownloadLink = null;
    }

    public class DescendersModMenu : MelonMod
    {
        private HarmonyLib.Harmony harmony;

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
            try { CutBrakes.ApplyPatch(harmony); }
            catch (System.Exception ex) { MelonLogger.Error("CutBrakes.ApplyPatch failed: " + ex.Message); }
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
            DiagnosticsManager.Report("CutBrakes", true);
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
            DiagnosticsManager.Report("Exploding Props", true);
            DiagnosticsManager.Report("Giant Everyone", true);
            TopSpeed.StartTracking();
        }

        public override void OnSceneWasLoaded(int buildindex, string sceneName)
        {
            MelonLogger.Msg("OnSceneWasLoaded: " + buildindex + " | " + sceneName);
        }

        public override void OnSceneWasInitialized(int buildindex, string sceneName)
        {
            MelonLogger.Msg("OnSceneWasInitialized: " + buildindex + " | " + sceneName);
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg("OnSceneWasUnloaded: " + buildIndex + " | " + sceneName);
            SlowMotion.Reset();
            CutBrakes.Reset();
            TopSpeed.Reset();
        }

        public override void OnUpdate()
        {
            try { if (NoBail.Enabled) NoBail.Apply(); }
            catch (System.Exception ex) { MelonLogger.Error("NoBail.Apply: " + ex.Message); }
            try { if (Input.GetKeyDown(KeyCode.F6)) MenuUI.ToggleMenu(); }
            catch (System.Exception ex) { MelonLogger.Error("ToggleMenu: " + ex.Message); }
            try { SceneDumper.CheckHotkey(); }
            catch (System.Exception ex) { MelonLogger.Error("SceneDumper: " + ex.Message); }
            try { SpeedWatcher.CheckHotkey(); }
            catch (System.Exception ex) { MelonLogger.Error("SpeedWatcher: " + ex.Message); }
            try { TopSpeed.Tick(); }
            catch (System.Exception ex) { MelonLogger.Error("TopSpeed.Tick: " + ex.Message); }

            try { if (Input.GetKeyDown(KeyCode.F2)) SlowMotion.Toggle(); }
            catch (System.Exception ex) { MelonLogger.Error("SlowMotion.Toggle: " + ex.Message); }
        }

        public override void OnLateUpdate()
        {
            try { FOV.Apply(); }
            catch (System.Exception ex) { MelonLogger.Error("FOV.Apply: " + ex.Message); }
        }

        public override void OnGUI()
        {
            try { ESP.OnGUI(); }
            catch (System.Exception ex) { MelonLogger.Error("ESP.OnGUI: " + ex.Message); }
        }

        public override void OnApplicationQuit()
        {
            MenuUI.RestoreCursor();
            SlowMotion.Reset();
            MelonLogger.Msg("OnApplicationQuit");
        }
    }
}