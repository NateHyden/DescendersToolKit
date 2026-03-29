using System;

namespace DescendersModMenu.BikeStats
{
    [Serializable]
    public class BikeStatsData
    {
        // ── Bike / Stats (original) ────────────────────────────────────
        public int AccelerationLevel = 1;
        public int MaxSpeedLevel = 1;
        public int LandingImpactLevel = 1;
        public bool NoBailEnabled = false;
        public int BikeIndex = 0;

        // ── Movement ───────────────────────────────────────────────────
        public int SpinLevel = 1;
        public int HopLevel = 1;
        public int WheelieLevel = 1;
        public int LeanLevel = 1;

        // ── Suspension ─────────────────────────────────────────────────
        public int SuspTravelLevel = 5;
        public int SuspStiffnessLevel = 5;
        public int SuspDampingLevel = 5;

        // ── Game Modifiers ─────────────────────────────────────────────
        public int WheelieBalanceLevel = 1;
        public int InAirCorrLevel = 1;
        public int FakieBalanceLevel = 1;
        public int PumpStrengthLevel = 1;
        public int IcePhysicsLevel = 1;

        // ── World / Graphics ───────────────────────────────────────────
        public int FovLevel = 5;
        public int GravityLevel = 5;
        public int TimeOfDayLevel = 4;
        public int SkyPreset = 0;

        // ── Tyres ──────────────────────────────────────────────────────
        public bool WideTyresEnabled = false;
        public int WideTyresLevel = 5;
        public bool StickyTyresEnabled = false;
        public float StickyForce = 150f;

        // ── Toggles ────────────────────────────────────────────────────
        public bool SlowMotionEnabled = false;
        public int SlowMotionLevel = 5;
        public bool CutBrakesEnabled = false;
        public bool NoSpeedCapEnabled = false;
        public bool ReverseSteerEnabled = false;
        public bool IceModeEnabled = false;
        public bool MirrorModeEnabled = false;
        public bool DrunkModeEnabled = false;
        public bool FlyModeEnabled = false;
        public bool EspEnabled = false;
        public bool SpeedrunTimerEnabled = false;
        public bool SlowMoOnBailEnabled = false;
        public bool GhostReplayEnabled = false;

        // ── New mods ───────────────────────────────────────────────────
        public bool WheelieAngleLimitEnabled = false;
        public int WheelieAngleLimitLevel = 5;
        public bool AirControlEnabled = false;
        public int AirControlLevel = 5;

        // ── Floats ─────────────────────────────────────────────────────
        public float FlyMoveSpeed = 30f;
        public float FlyClimbSpeed = 20f;
        public float GhostAlpha = 0.45f;
    }
}