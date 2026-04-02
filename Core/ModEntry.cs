using DescendersModMenu.Mods;
using DescendersModMenu.UI;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu
{
    public static class BuildInfo
    {
        public const string Name = "DescendersToolKit";
        public const string Description = "A modding toolkit for Descenders";
        public const string Author = "NateHyden";
        public const string Company = null;
        public const string Version = "3.6.0";
        public const string DownloadLink = null;
    }

    public class DescendersModMenu : MelonMod
    {
        private HarmonyLib.Harmony harmony;

        private float _lastRStickClick = -999f;
        private bool _pendingRStickSave = false;
        private float _rStickSaveTime = 0f;

        // ── Deferred mod reapply after map change ─────────────────────────
        private bool _pendingReapply;
        private bool _reapplyFlyMode;
        private bool _reapplyDrunkMode;
        private bool _reapplyMirrorMode;
        private bool _reapplyWideTyres;
        private bool _reapplyFov;
        private bool _reapplySpeedrunTimer;
        private bool _reapplyAcceleration;
        private bool _reapplyMaxSpeed;
        private bool _reapplyLandingImpact;
        private bool _reapplyMoveSpin;
        private bool _reapplyMoveHop;
        private bool _reapplyMoveWheelie;
        private bool _reapplyMoveLean;
        // Graphics: save "was disabled" — Reset() puts them all back to true,
        // so on the new scene we need to toggle OFF any that the user had turned off.
        private bool _reapplyBloomOff;
        private bool _reapplyAOOff;
        private bool _reapplyVigOff;
        private bool _reapplyDOFOff;
        private bool _reapplyCABOff;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("OnApplicationStart");

            // ── Anti-cheat disposal ───────────────────────────────────
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

            // ── Harmony patches ───────────────────────────────────────
            harmony = new HarmonyLib.Harmony("DescendersModMenu.Patches");
            try { harmony.PatchAll(); MelonLogger.Msg("Harmony patches applied."); DiagnosticsManager.Report("Harmony", true); }
            catch (System.Exception ex) { MelonLogger.Error("PatchAll failed: " + ex.Message); DiagnosticsManager.Report("Harmony", false, ex.Message); }

            try { NoSpeedCap.ApplyPatch(harmony); DiagnosticsManager.Report("NoSpeedCap", true); }
            catch (System.Exception ex) { MelonLogger.Error("NoSpeedCap.ApplyPatch: " + ex.Message); DiagnosticsManager.Report("NoSpeedCap", false, ex.Message); }
            try { NoSpeedCap.ApplyVCPatch(harmony); DiagnosticsManager.Report("NoSpeedCap (VC)", true); }
            catch (System.Exception ex) { MelonLogger.Error("NoSpeedCap.ApplyVCPatch: " + ex.Message); DiagnosticsManager.Report("NoSpeedCap (VC)", false, ex.Message); }
            try { QuickBrake.ApplyPatch(harmony); DiagnosticsManager.Report("QuickBrake", true); }
            catch (System.Exception ex) { MelonLogger.Error("QuickBrake.ApplyPatch: " + ex.Message); DiagnosticsManager.Report("QuickBrake", false, ex.Message); }
            try { CutBrakes.ApplyPatch(harmony); DiagnosticsManager.Report("CutBrakes", true); }
            catch (System.Exception ex) { MelonLogger.Error("CutBrakes.ApplyPatch: " + ex.Message); DiagnosticsManager.Report("CutBrakes", false, ex.Message); }
            try { ReverseSteering.ApplyPatch(harmony); DiagnosticsManager.Report("ReverseSteering", true); }
            catch (System.Exception ex) { MelonLogger.Error("ReverseSteering.ApplyPatch: " + ex.Message); DiagnosticsManager.Report("ReverseSteering", false, ex.Message); }
            try { AutoBalance.ApplyPatch(harmony); DiagnosticsManager.Report("AutoBalance", true); }
            catch (System.Exception ex) { MelonLogger.Error("AutoBalance.ApplyPatch: " + ex.Message); DiagnosticsManager.Report("AutoBalance", false, ex.Message); }
            try { IceMode.ApplyPatch(harmony); DiagnosticsManager.Report("IceMode", true); }
            catch (System.Exception ex) { MelonLogger.Error("IceMode.ApplyPatch: " + ex.Message); DiagnosticsManager.Report("IceMode", false, ex.Message); }
            try { SkyColours.ApplyPatch(harmony); }
            catch (System.Exception ex) { MelonLogger.Error("SkyColours.ApplyPatch: " + ex.Message); }
            try { DrunkMode.ApplyPatch(harmony); DiagnosticsManager.Report("DrunkMode", true); }
            catch (System.Exception ex) { MelonLogger.Error("DrunkMode.ApplyPatch: " + ex.Message); DiagnosticsManager.Report("DrunkMode", false, ex.Message); }
            try { SessionTrackers.ApplyBailPatch(harmony); DiagnosticsManager.Report("BailCounter", true); }
            catch (System.Exception ex) { MelonLogger.Error("BailPatch: " + ex.Message); DiagnosticsManager.Report("BailCounter", false, ex.Message); }
            try { SessionTrackers.ApplyCheckpointPatch(harmony); DiagnosticsManager.Report("CheckpointCounter", true); }
            catch (System.Exception ex) { MelonLogger.Error("CheckpointPatch: " + ex.Message); DiagnosticsManager.Report("CheckpointCounter", false, ex.Message); }
            try { GhostReplay.ApplyPatch(harmony); DiagnosticsManager.Report("GhostReplay", true); }
            catch (System.Exception ex) { MelonLogger.Error("GhostReplay.ApplyPatch: " + ex.Message); DiagnosticsManager.Report("GhostReplay", false, ex.Message); }
            try { GameModifierMods.ApplyNoSpeedWobblesPatch(harmony); DiagnosticsManager.Report("NoSpeedWobbles", true); }
            catch (System.Exception ex) { MelonLogger.Error("NoSpeedWobbles patch: " + ex.Message); DiagnosticsManager.Report("NoSpeedWobbles", false, ex.Message); }
            try { WheelieAngleLimit.ApplyPatch(harmony); DiagnosticsManager.Report("WheelieAngleLimit", true); }
            catch (System.Exception ex) { MelonLogger.Error("WheelieAngleLimit patch: " + ex.Message); DiagnosticsManager.Report("WheelieAngleLimit", false, ex.Message); }
            try { MapChanger.ApplyPatch(harmony); DiagnosticsManager.Report("MapChanger", true); }
            catch (System.Exception ex) { MelonLogger.Warning("MapChanger.ApplyPatch: " + ex.Message); DiagnosticsManager.Report("MapChanger", false, ex.Message); }
            try { NoBail.ApplyPatch(harmony); }
            catch (System.Exception ex) { MelonLogger.Error("NoBail.ApplyPatch: " + ex.Message); }


            try { OutfitPresets.Init(); }
            catch (System.Exception ex) { MelonLogger.Error("OutfitPresets.Init: " + ex.Message); }
            try { ModChat.Init(); }
            catch (System.Exception ex) { MelonLogger.Error("ModChat.Init: " + ex.Message); }
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
            SkyColours.CaptureSceneDefaults();    // must be before any mod changes sky
            GraphicsSettings.CaptureDefaultQuality(); // only records once (first launch)
            GhostReplay.OnSceneInitialized();
            MapChanger.OnSceneInitialized();
            ExplodingProps.OnSceneInitialized(sceneName);
            if (buildindex == 1)
                MapChanger.BuildMapList();
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg("OnSceneWasUnloaded: " + buildIndex + " | " + sceneName);

            // ── Save toggle states before reset ───────────────────────────
            bool wasSlowMotion = SlowMotion.Enabled;
            bool wasFlyMode = FlyMode.Enabled;
            bool wasIceMode = IceMode.Enabled;
            bool wasCutBrakes = CutBrakes.Enabled;
            bool wasStickyTyres = StickyTyres.Enabled;
            bool wasAirControl = AirControl.Enabled;
            bool wasDrunkMode = DrunkMode.Enabled;
            bool wasMirrorMode = MirrorMode.Enabled;
            bool wasReverseSteering = ReverseSteering.Enabled;
            bool wasAutoBalance = AutoBalance.Enabled;
            bool wasLandingImpact = LandingImpact.Enabled;
            bool wasQuickBrake = QuickBrake.Enabled;
            bool wasSlowMoOnBail = SlowMoOnBail.Enabled;
            bool wasWheelieAngle = WheelieAngleLimit.Enabled;
            bool wasMaxSpeed = MaxSpeedMultiplier.Enabled;
            bool wasWideTyres = WideTyres.Enabled;
            bool wasAcceleration = Acceleration.Enabled;
            bool wasMoveSpin = Movement.SpinEnabled;
            bool wasMoveHop = Movement.HopEnabled;
            bool wasMoveWheelie = Movement.WheelieEnabled;
            bool wasMoveLean = Movement.LeanEnabled;
            bool wasNoSpeedWobbles = GameModifierMods.NoSpeedWobblesEnabled;
            bool wasFov = FOV.Enabled;
            bool wasSpeedrunTimer = SpeedrunTimer.Enabled;
            // Graphics: save which effects the user had DISABLED (Reset() restores all to true)
            bool bloomWasOff = !GraphicsSettings.BloomEnabled;
            bool aoWasOff = !GraphicsSettings.AmbientOccEnabled;
            bool vigWasOff = !GraphicsSettings.VignetteEnabled;
            bool dofWasOff = !GraphicsSettings.DepthOfFieldEnabled;
            bool cabWasOff = !GraphicsSettings.ChromaticAbEnabled;

            SlowMotion.Reset();
            QuickBrake.Reset();
            QuickBrake_Patch.ClearCache();
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
            EarthquakeMode.Reset();
            PoliceChaseMode.Reset();
            TrickAttackMode.Reset();
            BoulderDodgeMode.Reset();
            SurvivalMode.Reset();
            TopSpeed.ClearCache();
            SessionTrackers.Reset();
            ExplodingProps.Reset();
            StickyTyres.Reset();
            WheelieAngleLimit.Reset();
            AirControl.Reset();
            CenterOfMass.Reset();
            ScoreManager.ResetMultiplier();
            // ── Fixed mods — now have proper Reset() ─────────────────
            FOV.Reset();                  // replaces FOV.ClearCache()
            Acceleration.Reset();         // restores default accel field
            MaxSpeedMultiplier.Reset();   // restores default drag field
            Movement.Reset();             // restores all 4 movement fields
            LandingImpact.Reset();        // clears Enabled flag
            NoBail.ClearCache();
            Page9UI.ResetWheelSize();

            // ── Restore immediate toggles (no player/camera needed) ───────
            // Harmony patches just need Enabled=true, they re-check each call.
            // Global Unity values (Time.timeScale) persist across scenes.
            if (wasSlowMotion) SlowMotion.Toggle();
            if (wasCutBrakes) CutBrakes.Toggle();
            if (wasReverseSteering) ReverseSteering.Toggle();
            if (wasAutoBalance) AutoBalance.Toggle();
            if (wasQuickBrake) QuickBrake.Toggle();
            if (wasWheelieAngle) WheelieAngleLimit.Toggle();
            if (wasNoSpeedWobbles) GameModifierMods.NoSpeedWobblesToggle();
            if (wasSlowMoOnBail) SlowMoOnBail.Toggle();
            if (wasIceMode) IceMode.Toggle();    // Harmony patch, null-safe
            if (wasStickyTyres) StickyTyres.Toggle(); // pure flag, FixedTick does the work
            if (wasAirControl) AirControl.Toggle();  // pure flag, FixedTick does the work

            // ── Queue deferred toggles (need player/camera to exist) ──────
            _reapplyFlyMode = wasFlyMode;
            _reapplyDrunkMode = wasDrunkMode;
            _reapplyMirrorMode = wasMirrorMode;
            _reapplyWideTyres = wasWideTyres;
            _reapplyFov = wasFov;
            _reapplySpeedrunTimer = wasSpeedrunTimer;
            _reapplyAcceleration = wasAcceleration;
            _reapplyMaxSpeed = wasMaxSpeed;
            _reapplyLandingImpact = wasLandingImpact;
            _reapplyMoveSpin = wasMoveSpin;
            _reapplyMoveHop = wasMoveHop;
            _reapplyMoveWheelie = wasMoveWheelie;
            _reapplyMoveLean = wasMoveLean;
            _reapplyBloomOff = bloomWasOff;
            _reapplyAOOff = aoWasOff;
            _reapplyVigOff = vigWasOff;
            _reapplyDOFOff = dofWasOff;
            _reapplyCABOff = cabWasOff;
            _pendingReapply = wasFlyMode || wasDrunkMode || wasMirrorMode || wasWideTyres ||
                              wasFov || wasSpeedrunTimer || wasAcceleration || wasMaxSpeed ||
                              wasLandingImpact || wasMoveSpin || wasMoveHop || wasMoveWheelie || wasMoveLean ||
                              bloomWasOff || aoWasOff || vigWasOff || dofWasOff || cabWasOff;
        }

        public override void OnUpdate()
        {
            // ── Deferred mod reapply once player exists after map change ──
            if (_pendingReapply)
            {
                try
                {
                    if ((object)GameObject.Find("Player_Human") != null)
                    {
                        _pendingReapply = false;
                        if (_reapplyFlyMode) { _reapplyFlyMode = false; FlyMode.Toggle(); }
                        if (_reapplyDrunkMode) { _reapplyDrunkMode = false; DrunkMode.Toggle(); }
                        if (_reapplyMirrorMode) { _reapplyMirrorMode = false; MirrorMode.Toggle(); }
                        if (_reapplyWideTyres) { _reapplyWideTyres = false; WideTyres.Toggle(); }
                        if (_reapplyFov) { _reapplyFov = false; FOV.Toggle(); }
                        if (_reapplySpeedrunTimer) { _reapplySpeedrunTimer = false; SpeedrunTimer.Toggle(); }
                        if (_reapplyAcceleration) { _reapplyAcceleration = false; Acceleration.Toggle(); }
                        if (_reapplyMaxSpeed) { _reapplyMaxSpeed = false; MaxSpeedMultiplier.Toggle(); }
                        if (_reapplyLandingImpact) { _reapplyLandingImpact = false; LandingImpact.Toggle(); }
                        if (_reapplyMoveSpin) { _reapplyMoveSpin = false; Movement.ToggleSpin(); }
                        if (_reapplyMoveHop) { _reapplyMoveHop = false; Movement.ToggleHop(); }
                        if (_reapplyMoveWheelie) { _reapplyMoveWheelie = false; Movement.ToggleWheelie(); }
                        if (_reapplyMoveLean) { _reapplyMoveLean = false; Movement.ToggleLean(); }
                        // Graphics: Reset() restores all to true, so toggle OFF any the user had disabled
                        if (_reapplyBloomOff) { _reapplyBloomOff = false; GraphicsSettings.ToggleBloom(); }
                        if (_reapplyAOOff) { _reapplyAOOff = false; GraphicsSettings.ToggleAO(); }
                        if (_reapplyVigOff) { _reapplyVigOff = false; GraphicsSettings.ToggleVignette(); }
                        if (_reapplyDOFOff) { _reapplyDOFOff = false; GraphicsSettings.ToggleDOF(); }
                        if (_reapplyCABOff) { _reapplyCABOff = false; GraphicsSettings.ToggleChromatic(); }
                    }
                }
                catch (System.Exception ex) { MelonLogger.Error("PendingReapply: " + ex.Message); _pendingReapply = false; }
            }
            try
            {
                if (Input.GetKeyDown(KeyCode.F3))
                { GhostReplay.Toggle(); Page14UI.RefreshAll(); }

                if (Input.GetKeyDown(KeyCode.F4))
                { GhostReplay.SaveRun(); Page14UI.RefreshAll(); }

                if (Input.GetKeyDown(KeyCode.JoystickButton8))
                {
                    if (SurvivalMode.Enabled && SurvivalMode.IsGameOver)
                        SurvivalMode.ResetRun();
                    else
                    { GhostReplay.SetSpawnMarker(); Page14UI.RefreshAll(); }
                }

                if (Input.GetKeyDown(KeyCode.JoystickButton9))
                {
                    float now = Time.realtimeSinceStartup;
                    float gap = now - _lastRStickClick;
                    _lastRStickClick = now;
                    if (gap < 0.4f)
                    { GhostReplay.Toggle(); Page14UI.RefreshAll(); _lastRStickClick = -999f; }
                    else
                    { _pendingRStickSave = true; _rStickSaveTime = now + 0.4f; }
                }
                if (_pendingRStickSave && Time.realtimeSinceStartup >= _rStickSaveTime)
                {
                    _pendingRStickSave = false;
                    if (GhostReplay.IsRecording && GhostReplay.RecordedFrames >= 30)
                    { GhostReplay.SaveRun(); Page14UI.RefreshAll(); }
                }

                if (Input.GetKeyDown(KeyCode.F6)) MenuUI.ToggleMenu();
            }
            catch (System.Exception ex) { MelonLogger.Error("ToggleMenu: " + ex.Message); }

            try { SceneDumper.CheckHotkey(); } catch (System.Exception ex) { MelonLogger.Error("SceneDumper: " + ex.Message); }
            try { SpeedWatcher.CheckHotkey(); } catch (System.Exception ex) { MelonLogger.Error("SpeedWatcher: " + ex.Message); }
            try { TopSpeed.Tick(); } catch (System.Exception ex) { MelonLogger.Error("TopSpeed.Tick: " + ex.Message); }
            try { SessionTrackers.Tick(); } catch (System.Exception ex) { MelonLogger.Error("SessionTrackers.Tick: " + ex.Message); }
            try { MenuWindow.TickLive(); } catch { }
            try { MirrorMode.Tick(); } catch { }
            try { FlyMode.Tick(); } catch { }
            try { DrunkMode.Tick(); } catch { }
            try { Page11UI.Tick(); } catch { }
            try { Page12UI.Tick(); } catch { }
            try { Page3UI.Tick(); } catch { }
            if (!Page11UI.IsRenaming && !Page12UI.IsChatFocused
                && !Page15UI.IsSeedFocused && !PageModesUI.IsTAInputFocused)
                try { Page9UI.IdentityTick(); } catch { }
            try { SessionTrackers.CheckpointTick(); } catch { }
            try { PageModesUI.Tick(); } catch { }
            try { AvalancheMode.Tick(); } catch { }
            try { PoliceChaseMode.Tick(); } catch { }
            try { TrickAttackMode.Tick(); } catch { }
            try { BoulderDodgeMode.Tick(); } catch { }
            try { SurvivalMode.Tick(); } catch { }
            try { GhostReplay.Tick(); } catch { }
            try { MapChanger.Tick(); } catch { }
            try { Page15UI.SeedTick(); } catch { }
            try { Page14UI.Tick(); } catch { }
            try { SlowMoOnBail.Tick(); } catch { }
            try { ScoreManager.Tick(); } catch { }
            try { ModDetection.TagLocalPlayer(); } catch { }

            try { if (Input.GetKeyDown(KeyCode.F2)) SlowMotion.Toggle(); }
            catch (System.Exception ex) { MelonLogger.Error("SlowMotion.Toggle: " + ex.Message); }
        }

        public override void OnFixedUpdate()
        {
            try { AvalancheMode.FixedTick(); } catch { }
            try { EarthquakeMode.FixedTick(); } catch { }
            try { PoliceChaseMode.FixedTick(); } catch { }
            try { StickyTyres.FixedTick(); } catch { }
            try { AirControl.FixedTick(); } catch { }
            try { CenterOfMass.FixedTick(); } catch { }
            try { BoulderDodgeMode.FixedTick(); } catch { }
        }

        public override void OnLateUpdate()
        {
            try { FOV.Apply(); } catch (System.Exception ex) { MelonLogger.Error("FOV.Apply: " + ex.Message); }
            try { SkyColours.Tick(); } catch (System.Exception ex) { MelonLogger.Error("SkyColours.Tick: " + ex.Message); }
            try { DrunkMode.LateTick(); } catch { }
            try { WideTyres.Tick(); } catch { }
            try { Page9UI.WheelSizeTick(); } catch { }
        }

        public override void OnGUI()
        {
            try { ESP.OnGUI(); } catch (System.Exception ex) { MelonLogger.Error("ESP.OnGUI: " + ex.Message); }
            try { GhostHUD.Draw(); } catch (System.Exception ex) { MelonLogger.Error("GhostHUD.Draw: " + ex.Message); }
            try { PoliceHUD.Draw(); } catch { }
            try { TrickAttackHUD.Draw(); } catch { }
            try { SurvivalHUD.Draw(); } catch { }
        }

        public override void OnApplicationQuit()
        {
            MenuUI.RestoreCursor();
            SlowMotion.Reset();
            QuickBrake.Reset();
            QuickBrake_Patch.ClearCache();
            MelonLogger.Msg("OnApplicationQuit");
        }
    }
}