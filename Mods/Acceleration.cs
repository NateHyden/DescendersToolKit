using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class Acceleration
    {
        private static readonly string StartupAccelerationField = "cPkCE^\u0081";

        private static float originalStartupAcceleration = -1f;

        public static int Level { get; private set; } = 1;

        public static void Increase()
        {
            if (Level < 10)
            {
                Level++;
            }

            Apply();
        }

        public static void Decrease()
        {
            if (Level > 1)
            {
                Level--;
            }

            Apply();
        }

        public static void SetLevel(int level)
        {
            if (level < 1)
            {
                level = 1;
            }

            if (level > 10)
            {
                level = 10;
            }

            Level = level;
            Apply();
        }

        public static void Apply()
        {
            GameObject player = GameObject.Find("Player_Human");
            if (player == null)
            {
                MelonLogger.Warning("Acceleration: Player_Human not found.");
                return;
            }

            Vehicle vehicle = player.GetComponent<Vehicle>();
            if (vehicle == null)
            {
                MelonLogger.Warning("Acceleration: Vehicle component not found.");
                return;
            }

            FieldInfo field = vehicle.GetType().GetField(StartupAccelerationField);
            if (object.ReferenceEquals(field, null))
            {
                MelonLogger.Warning("Acceleration: startupAcceleration field not found.");
                return;
            }

            if (originalStartupAcceleration < 0f)
            {
                object currentValue = field.GetValue(vehicle);
                if (currentValue is float currentFloat)
                {
                    originalStartupAcceleration = currentFloat;
                }
                else
                {
                    MelonLogger.Warning("Acceleration: startupAcceleration value was not a float.");
                    return;
                }
            }

            float multiplier = 1f + ((Level - 1) * 0.5f);
            float newValue = originalStartupAcceleration * multiplier;

            field.SetValue(vehicle, newValue);

            MelonLogger.Msg("Startup Acceleration set to level " + Level + " (" + newValue + ")");
        }
    }
}