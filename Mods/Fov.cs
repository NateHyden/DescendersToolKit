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

        // Cached references — rebuilt on scene load or when null
        private static BikeCamera[] _cachedCameras = null;
        private static CameraAngle[] _cachedAngles = null;

        public static void Increase() { if (Level < 10) Level++; }
        public static void Decrease() { if (Level > 1) Level--; }
        public static void SetLevel(int level)
        {
            if (level < 1) level = 1;
            if (level > 10) level = 10;
            Level = level;
        }

        public static void ClearCache()
        {
            _cachedCameras = null;
            _cachedAngles = null;
        }

        // Called from OnLateUpdate every frame — uses cache, no FindObjectsOfType after first call
        public static void Apply()
        {
            try
            {
                // Build cache if missing
                if ((object)_cachedCameras == null || _cachedCameras.Length == 0)
                {
                    _cachedCameras = GameObject.FindObjectsOfType<BikeCamera>();
                    if ((object)_cachedCameras == null || _cachedCameras.Length == 0) return;

                    _cachedAngles = new CameraAngle[_cachedCameras.Length];
                    for (int i = 0; i < _cachedCameras.Length; i++)
                    {
                        System.Reflection.FieldInfo[] fields = _cachedCameras[i].GetType().GetFields(
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        for (int j = 0; j < fields.Length; j++)
                        {
                            if (!string.Equals(fields[j].FieldType.Name, "CameraAngle",
                                System.StringComparison.Ordinal)) continue;
                            _cachedAngles[i] = fields[j].GetValue(_cachedCameras[i]) as CameraAngle;
                            break;
                        }
                    }
                }

                float target = GetFOV();
                for (int i = 0; i < _cachedAngles.Length; i++)
                {
                    if ((object)_cachedAngles[i] == null) continue;
                    _cachedAngles[i].targetFOV = target;
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("FOV.Apply: " + ex.Message); }
        }
    }
}