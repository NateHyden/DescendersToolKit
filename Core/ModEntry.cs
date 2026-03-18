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

            harmony = new HarmonyLib.Harmony("DescendersModMenu.Patches");
            harmony.PatchAll();

            MelonLogger.Msg("Harmony patches applied.");
            NoSpeedCap.ApplyPatch(harmony);
            NoSpeedCap.ApplyVCPatch(harmony);
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
        }

        public override void OnUpdate()
        {
            if (NoBail.Enabled)
            {
                NoBail.Apply();
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                MenuUI.ToggleMenu();
            }

            SceneDumper.CheckHotkey();
            SpeedWatcher.CheckHotkey();
        }
        public override void OnLateUpdate()
        {
            FOV.Apply();
        }

        public override void OnGUI()
        {
            ESP.OnGUI();
        }
        public override void OnApplicationQuit()
        {
            MenuUI.RestoreCursor();
            MelonLogger.Msg("OnApplicationQuit");
        }
    }
}