using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class MaxSpeedMultiplier
    {
        // ei[frnu = 0.06 - drag coefficient
        // Scan for a small public float between 0.04 and 0.1
        // so NoSpeedCap temporarily setting it to 0.001 doesn't break us
        private static float _originalValue = -1f;
        private static FieldInfo _field = null;

        public static int Level { get; private set; } = 1;

        public static void Increase()
        {
            if (Level < 10) Level++;
            Apply();
        }

        public static void Decrease()
        {
            if (Level > 1) Level--;
            Apply();
        }

        public static void SetLevel(int level)
        {
            if (level < 1) level = 1;
            if (level > 10) level = 10;
            Level = level;
            Apply();
        }

        private static FieldInfo FindField(Vehicle vehicle)
        {
            if ((object)_field != null) return _field;

            FieldInfo[] fields = vehicle.GetType().GetFields(
                BindingFlags.Public | BindingFlags.Instance
            );

            // Scan for the drag field - it's a small public float between 0.04 and 0.1
            // We use a range so a temporary modification by NoSpeedCap doesn't break us
            // We also check the saved original if we already know it
            for (int i = 0; i < fields.Length; i++)
            {
                if (!string.Equals(fields[i].FieldType.Name, "Single",
                    System.StringComparison.Ordinal)) continue;

                object val = fields[i].GetValue(vehicle);
                if ((object)val == null) continue;
                float f = (float)val;

                // Original value is 0.06 - look for something in that ballpark
                // Use range 0.001 to 0.1 to catch it even if temporarily modified
                if (f >= 0.001f && f <= 0.1f)
                {
                    _field = fields[i];
                    MelonLogger.Msg("MaxSpeed: found drag field " + fields[i].Name + " = " + f);
                    // Capture original as 0.06 since we know it regardless of current value
                    _originalValue = 0.06f;
                    return _field;
                }
            }

            MelonLogger.Warning("MaxSpeed: drag field not found.");
            return null;
        }

        public static void Apply()
        {
            GameObject player = GameObject.Find("Player_Human");
            if ((object)player == null)
            {
                MelonLogger.Warning("MaxSpeed: Player_Human not found.");
                return;
            }

            Vehicle vehicle = player.GetComponent<Vehicle>();
            if ((object)vehicle == null)
            {
                MelonLogger.Warning("MaxSpeed: Vehicle not found.");
                return;
            }

            FieldInfo field = FindField(vehicle);
            if ((object)field == null) return;

            float multiplier = 1f + ((Level - 1) * 0.5f);
            float newValue = _originalValue / multiplier;
            field.SetValue(vehicle, newValue);
            MelonLogger.Msg("MaxSpeed: Level " + Level + " drag -> " + newValue);
        }
    }
}