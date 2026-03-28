using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class SlowMoOnBail
    {
        public static bool Enabled { get; private set; } = false;

        private const float SlowScale = 0.25f; // how slow — 0.25 = 25% speed
        private const float Duration = 3.0f;  // real-time seconds before restoring

        private static bool _active = false;  // currently in slow-mo from a bail
        private static float _endTime = -1f;    // real-time when we should restore

        public static void Toggle()
        {
            Enabled = !Enabled;
            if (!Enabled) CancelIfActive();
            MelonLogger.Msg("[SlowMoOnBail] -> " + (Enabled ? "ON" : "OFF"));
        }

        // Called by SessionTrackers.OnBailDetected() every time a bail happens
        public static void OnBail()
        {
            if (!Enabled) return;

            // Restart the timer even if already in slow-mo (bailed again mid-replay)
            _active = true;
            _endTime = Time.realtimeSinceStartup + Duration;

            SetScale(SlowScale);
            MelonLogger.Msg("[SlowMoOnBail] Bail detected — slow-mo for " + Duration + "s");
        }

        // Called every frame from OnUpdate
        public static void Tick()
        {
            if (!_active) return;

            if (Time.realtimeSinceStartup >= _endTime)
            {
                _active = false;
                SetScale(1f);
                MelonLogger.Msg("[SlowMoOnBail] Restored normal speed.");
            }
        }

        private static void CancelIfActive()
        {
            if (!_active) return;
            _active = false;
            _endTime = -1f;
            SetScale(1f);
        }

        private static void SetScale(float scale)
        {
            try
            {
                TimeScaleManager mgr = Object.FindObjectOfType<TimeScaleManager>();
                if ((object)mgr != null)
                    mgr.SetTimeScale(scale, true);
                else
                {
                    Time.timeScale = scale;
                    Time.fixedDeltaTime = 0.02f * scale;
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("[SlowMoOnBail] SetScale: " + ex.Message); }
        }

        public static void Reset()
        {
            Enabled = false;
            CancelIfActive();
        }
    }
}