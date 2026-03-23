using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class SlowMotion
    {
        public static bool Enabled { get; private set; } = false;

        private const float SlowScale = 0.5f;

        public static void Toggle()
        {
            Enabled = !Enabled;
            Apply();
            MelonLogger.Msg("SlowMotion -> " + (Enabled ? "ON" : "OFF"));
        }

        public static void Apply()
        {
            SetScale(Enabled ? SlowScale : 1f);
        }

        public static void Reset()
        {
            Enabled = false;
            SetScale(1f);
        }

        private static void SetScale(float scale)
        {

            try
            {
            // The game has a TimeScaleManager that lerps Time.timeScale to its
            // own target every frame — setting Time.timeScale directly gets
            // overwritten instantly. We must set it via SetTimeScale instead.
            var mgr = Object.FindObjectOfType<TimeScaleManager>();
            if ((object)mgr != null)
            {
                mgr.SetTimeScale(scale, true);
            }
            else
            {
                // Fallback if manager not found yet
                Time.timeScale = scale;
                Time.fixedDeltaTime = 0.02f * scale;
            }
            }
            catch (System.Exception ex) { MelonLogger.Error("SlowMotion.SetScale: " + ex.Message); }
        
        }
    }
}