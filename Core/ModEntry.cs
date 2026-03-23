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
        public const string Version = "1.0.0";
        public const string DownloadLink = null;
    }

    public class DescendersModMenu : MelonMod
    {


        private HarmonyLib.Harmony harmony;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("OnApplicationStart");

            // Dispose CodeStage AntiCheat detectors.
            // Without this, modifying ObscuredFloat/Int values triggers the
            // "Uh oh... Unauthorized modifications detected. Closing game." popup.
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
            try { harmony.PatchAll(); MelonLogger.Msg("Harmony patches applied."); }
            catch (System.Exception ex) { MelonLogger.Error("PatchAll failed: " + ex.Message); }
            try { NoSpeedCap.ApplyPatch(harmony); }
            catch (System.Exception ex) { MelonLogger.Error("NoSpeedCap.ApplyPatch failed: " + ex.Message); }
            try { NoSpeedCap.ApplyVCPatch(harmony); }
            catch (System.Exception ex) { MelonLogger.Error("NoSpeedCap.ApplyVCPatch failed: " + ex.Message); }
        }

        public override void OnLateInitializeMelon()
        {
            MelonLogger.Msg("OnApplicationLateStart");
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