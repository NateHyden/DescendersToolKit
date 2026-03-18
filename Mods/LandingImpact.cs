using System.Reflection;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    [HarmonyPatch(typeof(PlayerInfoImpact), "OnImpact", new[] { typeof(float) })]
    public static class LandingImpact
    {
        private static float _originalImpactScale = -1f;
        private static FieldInfo _impactScaleField = null;

        public static int Level { get; private set; } = 1;

        public static void Increase()
        {
            if (Level < 10)
            {
                Level++;
                MelonLogger.Msg("[LandingImpact] Level -> " + Level);
                Apply();
            }
        }

        public static void Decrease()
        {
            if (Level > 1)
            {
                Level--;
                MelonLogger.Msg("[LandingImpact] Level -> " + Level);
                Apply();
            }
        }

        public static void SetLevel(int level)
        {
            if (level < 1) level = 1;
            if (level > 10) level = 10;
            Level = level;
            MelonLogger.Msg("[LandingImpact] SetLevel -> " + Level);
            Apply();
        }

        private static FieldInfo FindImpactScaleField(Vehicle vehicle)
        {
            if ((object)_impactScaleField != null)
                return _impactScaleField;

            FieldInfo[] fields = vehicle.GetType().GetFields(
                BindingFlags.Public | BindingFlags.Instance
            );

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo f = fields[i];

                // Use string comparison on type name to avoid Type.op_Inequality
                if (!string.Equals(f.FieldType.Name, "Single", System.StringComparison.Ordinal))
                    continue;

                object val = f.GetValue(vehicle);
                if ((object)val == null) continue;

                float fval = (float)val;

                if (Mathf.Approximately(fval, 500f))
                {
                    MelonLogger.Msg("[LandingImpact] Found impact scale field: " + f.Name);
                    _impactScaleField = f;
                    return f;
                }
            }

            MelonLogger.Warning("[LandingImpact] Could not find impact scale field (public float == 500).");
            return null;
        }

        public static void Apply()
        {
            GameObject player = GameObject.Find("Player_Human");
            if ((object)player == null)
            {
                MelonLogger.Warning("[LandingImpact] Player_Human not found.");
                return;
            }

            Vehicle vehicle = player.GetComponent<Vehicle>();
            if ((object)vehicle == null)
            {
                MelonLogger.Warning("[LandingImpact] Vehicle not found.");
                return;
            }

            FieldInfo field = FindImpactScaleField(vehicle);
            if ((object)field == null)
                return;

            if (_originalImpactScale < 0f)
            {
                _originalImpactScale = (float)field.GetValue(vehicle);
                MelonLogger.Msg("[LandingImpact] Captured default: " + _originalImpactScale);
            }

            float newValue = _originalImpactScale / Level;
            field.SetValue(vehicle, newValue);

            MelonLogger.Msg("[LandingImpact] Impact scale " + _originalImpactScale +
                            " -> " + newValue + " (Level " + Level + ")");
        }

        public static void Prefix(PlayerInfoImpact __instance, ref float __0)
        {
            if ((object)__instance == null || !__instance.IsHumanControlled())
                return;

            if (Level <= 1)
                return;

            __0 = __0 / Level;
        }
    }
}