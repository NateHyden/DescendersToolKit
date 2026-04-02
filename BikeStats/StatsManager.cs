using System;
using System.IO;
using MelonLoader;
using UnityEngine;
using DescendersModMenu.Mods;

namespace DescendersModMenu.BikeStats
{
    public static class StatsManager
    {
        private static readonly string SaveFolder =
            Path.Combine(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData"),
                "DescendersModMenu"
            );

        private static readonly string SaveFile =
            Path.Combine(SaveFolder, "BikeStats.json");

        // ── Save ──────────────────────────────────────────────────────
        public static void SaveStats()
        {
            try
            {
                EnsureSaveFolder();

                var data = new BikeStatsData
                {
                    // Bike / Stats
                    AccelerationLevel = Acceleration.Level,
                    MaxSpeedLevel = MaxSpeedMultiplier.Level,
                    LandingImpactLevel = LandingImpact.Level,
                    NoBailEnabled = NoBail.Enabled,
                    BikeIndex = BikeSwitcher.CurrentBikeIndex,

                    // Movement
                    SpinLevel = Movement.SpinLevel,
                    HopLevel = Movement.HopLevel,
                    WheelieLevel = Movement.WheelieLevel,
                    LeanLevel = Movement.LeanLevel,

                    // Suspension
                    SuspTravelLevel = Suspension.TravelLevel,
                    SuspStiffnessLevel = Suspension.StiffnessLevel,
                    SuspDampingLevel = Suspension.DampingLevel,

                    // Game Modifiers
                    WheelieBalanceLevel = GameModifierMods.WheelieBalanceLevel,
                    InAirCorrLevel = GameModifierMods.InAirCorrLevel,
                    FakieBalanceLevel = GameModifierMods.FakieBalanceLevel,
                    PumpStrengthLevel = GameModifierMods.PumpStrengthLevel,
                    IcePhysicsLevel = GameModifierMods.IcePhysicsLevel,

                    // World / Graphics
                    FovLevel = FOV.Level,
                    GravityLevel = Gravity.Level,
                    TimeOfDayLevel = TimeOfDay.Level,
                    SkyPreset = SkyColours.CurrentPreset,

                    // Tyres
                    WideTyresEnabled = WideTyres.Enabled,
                    WideTyresLevel = WideTyres.Level,
                    StickyTyresEnabled = StickyTyres.Enabled,
                    StickyForce = StickyTyres.SuctionForce,

                    // Toggles
                    SlowMotionEnabled = SlowMotion.Enabled,
                    SlowMotionLevel = SlowMotion.Level,
                    CutBrakesEnabled = CutBrakes.Enabled,
                    NoSpeedCapEnabled = NoSpeedCap.Enabled,
                    ReverseSteerEnabled = ReverseSteering.Enabled,
                    IceModeEnabled = IceMode.Enabled,
                    MirrorModeEnabled = MirrorMode.Enabled,
                    DrunkModeEnabled = DrunkMode.Enabled,
                    FlyModeEnabled = FlyMode.Enabled,
                    EspEnabled = ESP.Enabled,
                    SpeedrunTimerEnabled = SpeedrunTimer.Enabled,
                    SlowMoOnBailEnabled = SlowMoOnBail.Enabled,
                    GhostReplayEnabled = GhostReplay.Enabled,

                    WheelieAngleLimitEnabled = WheelieAngleLimit.Enabled,
                    WheelieAngleLimitLevel = WheelieAngleLimit.Level,
                    AirControlEnabled = AirControl.Enabled,
                    AirControlLevel = AirControl.Level,

                    // General toggles
                    AccelerationEnabled = Acceleration.Enabled,
                    MaxSpeedEnabled = MaxSpeedMultiplier.Enabled,
                    LandingImpactEnabled = LandingImpact.Enabled,
                    FovEnabled = FOV.Enabled,
                    AutoBalanceEnabled = AutoBalance.Enabled,
                    AutoBalanceStrengthLevel = AutoBalance.StrengthLevel,
                    NoSpeedWobblesEnabled = GameModifierMods.NoSpeedWobblesEnabled,

                    // Movement toggles
                    SpinEnabled = Movement.SpinEnabled,
                    HopEnabled = Movement.HopEnabled,
                    WheelieEnabled = Movement.WheelieEnabled,
                    LeanEnabled = Movement.LeanEnabled,

                    // Quick Brake
                    QuickBrakeEnabled = QuickBrake.Enabled,
                    QuickBrakeLevel = QuickBrake.Level,

                    // Floats
                    FlyMoveSpeed = FlyMode.MoveSpeed,
                    FlyClimbSpeed = FlyMode.ClimbSpeed,
                    GhostAlpha = GhostReplay.GhostAlpha,

                    // Menu Customiser
                    MenuPositionPreset = MenuCustomiser.PositionPreset,
                    MenuScaleLevel = MenuCustomiser.ScaleLevel,
                    MenuOpacityLevel = MenuCustomiser.OpacityLevel,

                    // Bike Torch
                    BikeTorchEnabled = BikeTorch.Enabled,
                    BikeTorchIntensityIndex = BikeTorch.IntensityIndex,

                    // Camera Shake
                    CameraShakeEnabled = CameraShake.Enabled,
                    CameraShakeLevel = CameraShake.Level,

                    // Center of Mass
                    CenterOfMassLR = CenterOfMass.OffsetLR,
                    CenterOfMassFB = CenterOfMass.OffsetFB,
                    CenterOfMassUD = CenterOfMass.OffsetUD,

                    // Exploding Props
                    ExplodingPropsEnabled = ExplodingProps.Enabled,

                    // Near Miss Sensitivity
                    NearMissEnabled = NearMissSensitivity.Enabled,
                    NearMissLevel = NearMissSensitivity.Level,

                    // Graphics
                    GraphicsBloomEnabled = GraphicsSettings.BloomEnabled,
                    GraphicsAmbientOccEnabled = GraphicsSettings.AmbientOccEnabled,
                    GraphicsVignetteEnabled = GraphicsSettings.VignetteEnabled,
                    GraphicsDepthOfFieldEnabled = GraphicsSettings.DepthOfFieldEnabled,
                    GraphicsChromaticAbEnabled = GraphicsSettings.ChromaticAbEnabled,

                    // Sky Storm / Rain
                    StormEnabled = SkyColours.StormEnabled,
                    RainIntensityLevel = SkyColours.RainIntensityLevel,
                };

                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SaveFile, json);
                MelonLogger.Msg("[StatsManager] Saved to: " + SaveFile);
            }
            catch (Exception ex)
            {
                MelonLogger.Error("[StatsManager] SaveStats: " + ex.Message);
            }
        }

        // ── Load ──────────────────────────────────────────────────────
        public static void LoadStats()
        {
            try
            {
                if (!File.Exists(SaveFile))
                {
                    MelonLogger.Warning("[StatsManager] No save file found: " + SaveFile);
                    return;
                }

                string json = File.ReadAllText(SaveFile);
                BikeStatsData data = JsonUtility.FromJson<BikeStatsData>(json);

                if (data == null)
                {
                    MelonLogger.Warning("[StatsManager] JSON returned null.");
                    return;
                }

                // Bike / Stats
                Acceleration.SetLevel(data.AccelerationLevel);
                MaxSpeedMultiplier.SetLevel(data.MaxSpeedLevel);
                LandingImpact.SetLevel(data.LandingImpactLevel);
                NoBail.SetEnabled(data.NoBailEnabled);
                BikeSwitcher.SetBike(data.BikeIndex);

                // Floats first (some toggles read them)
                FlyMode.MoveSpeed = data.FlyMoveSpeed;
                FlyMode.ClimbSpeed = data.FlyClimbSpeed;
                GhostReplay.GhostAlpha = data.GhostAlpha;
                StickyTyres.SuctionForce = data.StickyForce;
                SlowMotion.SetLevel(data.SlowMotionLevel);

                // Levels
                Movement.SetSpinLevel(data.SpinLevel);
                Movement.SetHopLevel(data.HopLevel);
                Movement.SetWheelieLevel(data.WheelieLevel);
                Movement.SetLeanLevel(data.LeanLevel);
                Suspension.SetTravelLevel(data.SuspTravelLevel);
                Suspension.SetStiffnessLevel(data.SuspStiffnessLevel);
                Suspension.SetDampingLevel(data.SuspDampingLevel);
                GameModifierMods.SetWheelieBalanceLevel(data.WheelieBalanceLevel);
                GameModifierMods.SetInAirCorrLevel(data.InAirCorrLevel);
                GameModifierMods.SetFakieBalanceLevel(data.FakieBalanceLevel);
                GameModifierMods.SetPumpStrengthLevel(data.PumpStrengthLevel);
                GameModifierMods.SetIcePhysicsLevel(data.IcePhysicsLevel);
                FOV.SetLevel(data.FovLevel);
                Gravity.SetLevel(data.GravityLevel);
                TimeOfDay.SetLevelSilent(data.TimeOfDayLevel);
                if (data.SkyPreset != 0) SkyColours.ApplyPreset(data.SkyPreset);
                WideTyres.SetLevel(data.WideTyresLevel);

                // Toggles — only enable if saved true, don't double-toggle
                if (data.WideTyresEnabled && !WideTyres.Enabled) WideTyres.Toggle();
                if (data.StickyTyresEnabled && !StickyTyres.Enabled) StickyTyres.Toggle();
                if (data.SlowMotionEnabled && !SlowMotion.Enabled) SlowMotion.Toggle();
                if (data.CutBrakesEnabled && !CutBrakes.Enabled) CutBrakes.Toggle();
                if (data.NoSpeedCapEnabled && !NoSpeedCap.Enabled) NoSpeedCap.Toggle();
                if (data.ReverseSteerEnabled && !ReverseSteering.Enabled) ReverseSteering.Toggle();
                if (data.IceModeEnabled && !IceMode.Enabled) IceMode.Toggle();
                if (data.MirrorModeEnabled && !MirrorMode.Enabled) MirrorMode.Toggle();
                if (data.DrunkModeEnabled && !DrunkMode.Enabled) DrunkMode.Toggle();
                if (data.FlyModeEnabled && !FlyMode.Enabled) FlyMode.Toggle();
                if (data.EspEnabled && !ESP.Enabled) ESP.Toggle();
                if (data.SpeedrunTimerEnabled && !SpeedrunTimer.Enabled) SpeedrunTimer.Toggle();
                if (data.SlowMoOnBailEnabled && !SlowMoOnBail.Enabled) SlowMoOnBail.Toggle();
                if (data.GhostReplayEnabled && !GhostReplay.Enabled) GhostReplay.Toggle();

                WheelieAngleLimit.SetLevel(data.WheelieAngleLimitLevel);
                AirControl.SetLevel(data.AirControlLevel);
                if (data.WheelieAngleLimitEnabled && !WheelieAngleLimit.Enabled) WheelieAngleLimit.Toggle();
                if (data.AirControlEnabled && !AirControl.Enabled) AirControl.Toggle();

                // General toggles
                if (data.AccelerationEnabled && !Acceleration.Enabled) Acceleration.Toggle();
                if (data.MaxSpeedEnabled && !MaxSpeedMultiplier.Enabled) MaxSpeedMultiplier.Toggle();
                if (data.LandingImpactEnabled && !LandingImpact.Enabled) LandingImpact.Toggle();
                if (data.FovEnabled && !FOV.Enabled) FOV.Toggle();
                AutoBalance.SetStrengthLevel(data.AutoBalanceStrengthLevel);
                if (data.AutoBalanceEnabled && !AutoBalance.Enabled) AutoBalance.Toggle();
                if (data.NoSpeedWobblesEnabled && !GameModifierMods.NoSpeedWobblesEnabled) GameModifierMods.NoSpeedWobblesToggle();

                // Movement toggles
                if (data.SpinEnabled && !Movement.SpinEnabled) Movement.ToggleSpin();
                if (data.HopEnabled && !Movement.HopEnabled) Movement.ToggleHop();
                if (data.WheelieEnabled && !Movement.WheelieEnabled) Movement.ToggleWheelie();
                if (data.LeanEnabled && !Movement.LeanEnabled) Movement.ToggleLean();

                // Quick Brake
                QuickBrake.SetLevel(data.QuickBrakeLevel);
                if (data.QuickBrakeEnabled && !QuickBrake.Enabled) QuickBrake.Toggle();

                // Menu Customiser
                MenuCustomiser.PositionPreset = data.MenuPositionPreset;
                MenuCustomiser.ScaleLevel = data.MenuScaleLevel;
                MenuCustomiser.OpacityLevel = data.MenuOpacityLevel;
                MenuCustomiser.Apply();

                // Bike Torch
                BikeTorch.IntensityIndex = data.BikeTorchIntensityIndex;
                if (data.BikeTorchEnabled && !BikeTorch.Enabled) BikeTorch.Toggle();

                // Camera Shake
                CameraShake.SetLevel(data.CameraShakeLevel);
                if (data.CameraShakeEnabled && !CameraShake.Enabled) CameraShake.Toggle();

                // Center of Mass
                CenterOfMass.SetLR(data.CenterOfMassLR);
                CenterOfMass.SetFB(data.CenterOfMassFB);
                CenterOfMass.SetUD(data.CenterOfMassUD);

                // Exploding Props
                if (data.ExplodingPropsEnabled && !ExplodingProps.Enabled) ExplodingProps.Toggle();

                // Near Miss Sensitivity
                NearMissSensitivity.SetLevel(data.NearMissLevel);
                if (data.NearMissEnabled && !NearMissSensitivity.Enabled) NearMissSensitivity.Toggle();

                // Graphics
                if (data.GraphicsBloomEnabled != GraphicsSettings.BloomEnabled) GraphicsSettings.ToggleBloom();
                if (data.GraphicsAmbientOccEnabled != GraphicsSettings.AmbientOccEnabled) GraphicsSettings.ToggleAO();
                if (data.GraphicsVignetteEnabled != GraphicsSettings.VignetteEnabled) GraphicsSettings.ToggleVignette();
                if (data.GraphicsDepthOfFieldEnabled != GraphicsSettings.DepthOfFieldEnabled) GraphicsSettings.ToggleDOF();
                if (data.GraphicsChromaticAbEnabled != GraphicsSettings.ChromaticAbEnabled) GraphicsSettings.ToggleChromatic();

                // Sky Storm / Rain
                SkyColours.SetRainIntensityLevel(data.RainIntensityLevel);
                if (data.StormEnabled && !SkyColours.StormEnabled) SkyColours.ToggleStorm();
                else if (!data.StormEnabled && SkyColours.StormEnabled) SkyColours.ToggleStorm();

                MelonLogger.Msg("[StatsManager] Loaded from: " + SaveFile);
            }
            catch (Exception ex)
            {
                MelonLogger.Error("[StatsManager] LoadStats: " + ex.Message);
            }
        }

        // ── Reset ─────────────────────────────────────────────────────
        public static void ResetStats()
        {
            try
            {
                // Bike / Stats
                Acceleration.SetLevel(1);
                MaxSpeedMultiplier.SetLevel(1);
                LandingImpact.SetLevel(1);
                NoBail.SetEnabled(false);
                BikeSwitcher.SetBike(0);

                // Movement
                Movement.SetSpinLevel(1);
                Movement.SetHopLevel(1);
                Movement.SetWheelieLevel(1);
                Movement.SetLeanLevel(1);

                // Suspension
                Suspension.SetTravelLevel(5);
                Suspension.SetStiffnessLevel(5);
                Suspension.SetDampingLevel(5);

                // Game Modifiers
                GameModifierMods.SetWheelieBalanceLevel(1);
                GameModifierMods.SetInAirCorrLevel(1);
                GameModifierMods.SetFakieBalanceLevel(1);
                GameModifierMods.SetPumpStrengthLevel(1);
                GameModifierMods.SetIcePhysicsLevel(1);

                // World / Graphics
                FOV.SetLevel(5);
                Gravity.SetLevel(5);
                TimeOfDay.SetLevelSilent(4);
                SkyColours.SetPresetSilent(0);
                WideTyres.SetLevel(5);

                // Floats
                FlyMode.MoveSpeed = 30f;
                FlyMode.ClimbSpeed = 20f;
                GhostReplay.GhostAlpha = 0.45f;
                StickyTyres.SuctionForce = 150f;
                SlowMotion.SetLevel(5);

                // Toggles — turn off anything that's on
                if (WideTyres.Enabled) WideTyres.Toggle();
                if (StickyTyres.Enabled) StickyTyres.Toggle();
                if (SlowMotion.Enabled) SlowMotion.Toggle();
                if (CutBrakes.Enabled) CutBrakes.Toggle();
                if (NoSpeedCap.Enabled) NoSpeedCap.Toggle();
                if (ReverseSteering.Enabled) ReverseSteering.Toggle();
                if (IceMode.Enabled) IceMode.Toggle();
                if (MirrorMode.Enabled) MirrorMode.Toggle();
                if (DrunkMode.Enabled) DrunkMode.Toggle();
                if (FlyMode.Enabled) FlyMode.Toggle();
                if (ESP.Enabled) ESP.Toggle();
                if (SpeedrunTimer.Enabled) SpeedrunTimer.Toggle();
                if (SlowMoOnBail.Enabled) SlowMoOnBail.Toggle();
                if (GhostReplay.Enabled) GhostReplay.Toggle();

                // General toggles
                if (Acceleration.Enabled) Acceleration.Toggle();
                if (MaxSpeedMultiplier.Enabled) MaxSpeedMultiplier.Toggle();
                if (LandingImpact.Enabled) LandingImpact.Toggle();
                if (FOV.Enabled) FOV.Toggle();
                if (AutoBalance.Enabled) AutoBalance.Toggle();
                AutoBalance.SetStrengthLevel(5);
                if (GameModifierMods.NoSpeedWobblesEnabled) GameModifierMods.NoSpeedWobblesToggle();

                // Movement toggles
                if (Movement.SpinEnabled) Movement.ToggleSpin();
                if (Movement.HopEnabled) Movement.ToggleHop();
                if (Movement.WheelieEnabled) Movement.ToggleWheelie();
                if (Movement.LeanEnabled) Movement.ToggleLean();

                // Quick Brake
                if (QuickBrake.Enabled) QuickBrake.Toggle();
                QuickBrake.SetLevel(5);

                // Bike Torch
                if (BikeTorch.Enabled) BikeTorch.Toggle();
                BikeTorch.IntensityIndex = 2;

                // Camera Shake
                if (CameraShake.Enabled) CameraShake.Toggle();
                CameraShake.SetLevel(5);

                // Center of Mass
                CenterOfMass.SetLR(0f);
                CenterOfMass.SetFB(0f);
                CenterOfMass.SetUD(0f);

                // Exploding Props
                if (ExplodingProps.Enabled) ExplodingProps.Toggle();

                // Near Miss Sensitivity
                if (NearMissSensitivity.Enabled) NearMissSensitivity.Toggle();
                NearMissSensitivity.SetLevel(5);

                // Graphics (reset to all enabled)
                if (!GraphicsSettings.BloomEnabled) GraphicsSettings.ToggleBloom();
                if (!GraphicsSettings.AmbientOccEnabled) GraphicsSettings.ToggleAO();
                if (!GraphicsSettings.VignetteEnabled) GraphicsSettings.ToggleVignette();
                if (!GraphicsSettings.DepthOfFieldEnabled) GraphicsSettings.ToggleDOF();
                if (!GraphicsSettings.ChromaticAbEnabled) GraphicsSettings.ToggleChromatic();

                // Sky Storm / Rain
                if (SkyColours.StormEnabled) SkyColours.ToggleStorm();
                SkyColours.SetRainIntensityLevel(5);

                MelonLogger.Msg("[StatsManager] Reset to defaults.");
            }
            catch (Exception ex)
            {
                MelonLogger.Error("[StatsManager] ResetStats: " + ex.Message);
            }
        }

        private static void EnsureSaveFolder()
        {
            if (!Directory.Exists(SaveFolder))
                Directory.CreateDirectory(SaveFolder);
        }
    }
}