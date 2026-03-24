using MelonLoader;
using UnityEngine;
using System.Reflection;

namespace DescendersModMenu.Mods
{
    public static class TimeOfDay
    {
        private static readonly float[] Hours = {
            6f, 8f, 10f, 12f, 14f, 16f, 17.5f, 19f, 20.5f, 22f
        };
        private static readonly string[] Labels = {
            "Dawn", "Morning", "Mid AM", "Noon", "Afternoon",
            "Late PM", "Evening", "Dusk", "Twilight", "Night"
        };

        public static int Level { get; private set; } = 4;
        public static string DisplayValue { get { return Labels[Level - 1]; } }

        public static void Increase() { if (Level < 10) { Level++; Apply(); } }
        public static void Decrease() { if (Level > 1) { Level--; Apply(); } }
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
                // Find TOD_Sky by scanning all MonoBehaviours
                MonoBehaviour[] all = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
                MonoBehaviour sky = null;
                for (int i = 0; i < all.Length; i++)
                {
                    if (all[i].GetType().Name == "TOD_Sky")
                    {
                        sky = all[i];
                        break;
                    }
                }
                if ((object)sky == null) { MelonLogger.Warning("[TimeOfDay] TOD_Sky not found on this map."); return; }

                // Get Cycle object
                FieldInfo cycleField = sky.GetType().GetField("Cycle",
                    BindingFlags.Public | BindingFlags.Instance);
                if ((object)cycleField == null) { MelonLogger.Warning("[TimeOfDay] Cycle field not found."); return; }

                object cycle = cycleField.GetValue(sky);
                if ((object)cycle == null) { MelonLogger.Warning("[TimeOfDay] Cycle is null."); return; }

                // Set Hour
                FieldInfo hourField = cycle.GetType().GetField("Hour",
                    BindingFlags.Public | BindingFlags.Instance);
                if ((object)hourField != null)
                {
                    hourField.SetValue(cycle, Hours[Level - 1]);
                    MelonLogger.Msg("[TimeOfDay] Set to " + Labels[Level - 1] + " (" + Hours[Level - 1] + "h)");
                    return;
                }

                // Some versions use property
                PropertyInfo hourProp = cycle.GetType().GetProperty("Hour",
                    BindingFlags.Public | BindingFlags.Instance);
                if ((object)hourProp != null)
                {
                    hourProp.SetValue(cycle, Hours[Level - 1], null);
                    MelonLogger.Msg("[TimeOfDay] Set to " + Labels[Level - 1] + " (" + Hours[Level - 1] + "h)");
                    return;
                }

                MelonLogger.Warning("[TimeOfDay] Hour field/property not found on Cycle.");
            }
            catch (System.Exception ex) { MelonLogger.Error("[TimeOfDay] Apply: " + ex.Message); }
        }
    }
}