using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class Gravity
    {
        // Default Descenders gravity is -17.5
        private static readonly float[] Levels = {
            -5f, -8f, -11f, -14f, -17.5f, -21f, -24f, -27f, -30f, -35f
        };

        public static int Level { get; private set; } = 5; // 5 = default

        public static string DisplayValue { get { return Levels[Level - 1].ToString("F1"); } }

        public static void Increase() { if (Level < 10) { Level++; Apply(); } }
        public static void Decrease() { if (Level > 1)  { Level--; Apply(); } }
        public static void SetLevel(int level)
        {
            if (level < 1) level = 1;
            if (level > 10) level = 10;
            Level = level;
            Apply();
        }

        public static void Apply()
        {
            try
            {
                Physics.gravity = new Vector3(0f, Levels[Level - 1], 0f);
                MelonLogger.Msg("[Gravity] Set to " + Levels[Level - 1]);
            }
            catch (System.Exception ex) { MelonLogger.Error("[Gravity] Apply: " + ex.Message); }
        }

        public static void Reset()
        {
            Level = 5;
            Apply();
        }
    }
}
