using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class FOV
    {
        public static int Level { get; private set; } = 5;

        // Level 1=45, Level 5=85 (matches game default), Level 10=130
        private static float GetFOV() { return 45f + (Level - 1) * 9.4f; }

        public static string DisplayValue { get { return ((int)GetFOV()).ToString(); } }

        public static void Increase() { if (Level < 10) Level++; }
        public static void Decrease() { if (Level > 1) Level--; }
        public static void SetLevel(int level)
        {
            if (level < 1) level = 1;
            if (level > 10) level = 10;
            Level = level;
        }

        // Set targetFOV on CameraAngle so BikeCamera's own lerp drives toward our value
        // This works for all camera modes since every BikeCamera has a CameraAngle reference
        public static void Apply()
        {

            try
            {
            BikeCamera[] cameras = GameObject.FindObjectsOfType<BikeCamera>();
            if ((object)cameras == null || cameras.Length == 0) return;

            float target = GetFOV();
            for (int i = 0; i < cameras.Length; i++)
            {
                BikeCamera bc = cameras[i];
                if ((object)bc == null) continue;

                // CameraAngle is the public field P\x82lio[ on BikeCamera
                // Access it via reflection since the field name has unprintable chars
                System.Reflection.FieldInfo[] fields = bc.GetType().GetFields(
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance
                );
                for (int j = 0; j < fields.Length; j++)
                {
                    if (!string.Equals(fields[j].FieldType.Name, "CameraAngle",
                        System.StringComparison.Ordinal)) continue;

                    CameraAngle ca = fields[j].GetValue(bc) as CameraAngle;
                    if ((object)ca == null) break;

                    ca.targetFOV = target;
                    break;
                }
            }
            }
            catch (System.Exception ex) { MelonLogger.Error("FOV.Apply: " + ex.Message); }
        
        }
    }
}