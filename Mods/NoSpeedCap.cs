using System.Reflection;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class NoSpeedCap
    {
        public static bool Enabled { get; private set; } = false;

        public static void Toggle()
        {
            Enabled = !Enabled;
            MelonLogger.Msg("NoSpeedCap -> " + (Enabled ? "ON" : "OFF"));
        }

        public static void SetEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        // Also patch VehicleController.FixedUpdate which zeros j[fCiJt when speed > 55
        public static void ApplyVCPatch(HarmonyLib.Harmony harmony)
        {
            try
            {
                System.Type vcType = typeof(VehicleController);
                MethodInfo fixedUpdate = vcType.GetMethod(
                    "FixedUpdate",
                    BindingFlags.NonPublic | BindingFlags.Instance
                );

                if ((object)fixedUpdate == null)
                {
                    MelonLogger.Warning("[NoSpeedCap] Could not find VehicleController.FixedUpdate.");
                    return;
                }

                MethodInfo postfix = typeof(NoSpeedCap_VCPatch).GetMethod(
                    "Postfix", BindingFlags.Public | BindingFlags.Static
                );

                harmony.Patch(fixedUpdate, postfix: new HarmonyMethod(postfix));
                MelonLogger.Msg("[NoSpeedCap] Patched VehicleController.FixedUpdate.");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("[NoSpeedCap] VC Patch failed: " + ex.Message);
            }
        }

        // Manual patch since E{Kza has unprintable chars in its name
        public static void ApplyPatch(HarmonyLib.Harmony harmony)
        {
            try
            {
                // Find E{Kza by scanning all private methods on Vehicle
                // It's the one that takes no params and contains the speed cap logic
                System.Type vehicleType = typeof(Vehicle);
                MethodInfo[] methods = vehicleType.GetMethods(
                    BindingFlags.NonPublic | BindingFlags.Instance
                );

                MethodInfo target = null;
                for (int i = 0; i < methods.Length; i++)
                {
                    MethodInfo m = methods[i];
                    // E{Kza starts with 'E' and has no parameters
                    if (m.GetParameters().Length != 0) continue;
                    if (!m.ReturnType.Equals(typeof(void))) continue;
                    if (!m.Name.StartsWith("E")) continue;
                    // It's not a Unity event method
                    if (m.Name == "enabled") continue;
                    // Check it's not a property getter
                    if (m.IsSpecialName) continue;


                    // The real E{Kza has 3 chars before 'Kza': E + 2 non-ascii
                    if (m.Name.Length == 7 && m.Name.EndsWith("Kza"))
                    {
                        target = m;
                        break;
                    }
                }

                if ((object)target == null)
                {
                    MelonLogger.Warning("[NoSpeedCap] Could not find E{Kza method.");
                    return;
                }

                MethodInfo prefix = typeof(NoSpeedCap_EKzaPatch).GetMethod(
                    "Prefix",
                    BindingFlags.Public | BindingFlags.Static
                );

                harmony.Patch(target, prefix: new HarmonyMethod(prefix));
                MelonLogger.Msg("[NoSpeedCap] Patched successfully.");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("[NoSpeedCap] Patch failed: " + ex.Message);
            }
        }
    }

    // Postfix on VehicleController.FixedUpdate
    // Restores j[fCiJt (inputAcceleration) after the game zeros it at speed > 55
    public static class NoSpeedCap_VCPatch
    {
        private static FieldInfo _vehicleField = null; // CDVkgio - the Vehicle ref
        private static PropertyInfo _tiltProp = null; // dyEyUZ - raw tilt input
        private static PropertyInfo _inputAccProp = null; // j[fCiJt - inputAcceleration
        private static bool _vcFieldsCached = false;

        public static void Postfix(VehicleController __instance)
        {
            if (!NoSpeedCap.Enabled) return;
            if ((object)__instance == null) return;

            // Cache fields
            if ((object)_vehicleField == null)
            {
                FieldInfo[] fields = __instance.GetType().GetFields(
                    BindingFlags.NonPublic | BindingFlags.Instance
                );
                for (int i = 0; i < fields.Length; i++)
                {
                    if (string.Equals(fields[i].FieldType.Name, "Vehicle",
                        System.StringComparison.Ordinal))
                    {
                        _vehicleField = fields[i];
                        break;
                    }
                }
            }

            if ((object)_vehicleField == null) return;

            Vehicle vehicle = _vehicleField.GetValue(__instance) as Vehicle;
            if ((object)vehicle == null) return;

            // Only for local player
            if (!string.Equals(vehicle.gameObject.name, "Player_Human",
                System.StringComparison.Ordinal)) return;

            // Cache tilt and inputAccel props once
            if (!_vcFieldsCached)
            {
                _vcFieldsCached = true;
                PropertyInfo[] vcProps = __instance.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                );
                for (int i = 0; i < vcProps.Length; i++)
                {
                    if (!vcProps[i].CanRead) continue;
                    if (!string.Equals(vcProps[i].PropertyType.Name, "Single",
                        System.StringComparison.Ordinal)) continue;
                    if ((object)_tiltProp == null &&
                        vcProps[i].Name.StartsWith("d") && vcProps[i].Name.Length > 4)
                    {
                        _tiltProp = vcProps[i];
                    }
                }
            }

            float tiltInput = 0f;
            if ((object)_tiltProp != null)
                tiltInput = (float)_tiltProp.GetValue(__instance, null);
            if (tiltInput <= 0.01f) return; // not pedaling, leave j[fCiJt at 0

            // Player is pedaling but game zeroed input because GetVelocity() > 55
            // Find j[fCiJt and restore it to the tilt value so speed control works normally
            if ((object)_inputAccProp == null)
            {
                PropertyInfo[] allProps = vehicle.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                );
                for (int i = 0; i < allProps.Length; i++)
                {
                    if (!allProps[i].CanWrite || !allProps[i].CanRead) continue;
                    if (!string.Equals(allProps[i].PropertyType.Name, "Single",
                        System.StringComparison.Ordinal)) continue;
                    if (!allProps[i].Name.StartsWith("j")) continue;
                    _inputAccProp = allProps[i];
                    break;
                }
            }

            if ((object)_inputAccProp != null)
            {
                float current = (float)_inputAccProp.GetValue(vehicle, null);
                if (Mathf.Approximately(current, 0f) && vehicle.GetVelocity() > 55f)
                    _inputAccProp.SetValue(vehicle, tiltInput, null);
            }
        }
    }

    public static class NoSpeedCap_EKzaPatch
    {
        // Cache fields via reflection
        private static FieldInfo _rbField = null; // Rigidbody
        private static FieldInfo _accelField = null; // cPkCE^ (acceleration force ~14)

        private static PropertyInfo _onGroundPropInfo = null;
        private static PropertyInfo _inputAccelProp = null;

        public static bool Prefix(Vehicle __instance)
        {
            if (!NoSpeedCap.Enabled) return true;
            if ((object)__instance == null) return true;
            if (!string.Equals(__instance.gameObject.name, "Player_Human",
                System.StringComparison.Ordinal)) return true;

            EnsureFields(__instance);

            // Check if on ground
            if ((object)_onGroundPropInfo != null)
            {
                object val = _onGroundPropInfo.GetValue(__instance, null);
                if (val is bool && !(bool)val) return true;
            }

            // Get rigidbody
            Rigidbody rb = null;
            if ((object)_rbField != null)
                rb = _rbField.GetValue(__instance) as Rigidbody;
            if ((object)rb == null) return true;

            // Get input acceleration
            float inputAccel = 0f;
            if ((object)_inputAccelProp != null)
            {
                object val = _inputAccelProp.GetValue(__instance, null);
                if (val is float) inputAccel = (float)val;
            }
            float speed = rb.velocity.magnitude;

            // Get acceleration force
            float accelForce = 14f;
            if ((object)_accelField != null)
            {
                object val = _accelField.GetValue(__instance);
                if (val is float) accelForce = (float)val;
            }

            // Calculate what the original num2 cap would be

            // If not pedaling, run the original (which will do nothing anyway)
            if (inputAccel <= 0.001f) return true;

            // Apply force with NO num2 cap
            rb.velocity += accelForce * inputAccel *
                __instance.transform.forward *
                Time.fixedDeltaTime;

            return false; // skip original
        }

        private static bool _fieldsCached = false;

        private static void EnsureFields(Vehicle v)
        {
            if (_fieldsCached) return;
            _fieldsCached = true;

            System.Type t = v.GetType();

            // Rigidbody - find the backing field
            FieldInfo[] fields = t.GetFields(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.FlattenHierarchy
            );
            for (int i = 0; i < fields.Length; i++)
            {
                if (string.Equals(fields[i].FieldType.Name, "Rigidbody",
                    System.StringComparison.Ordinal) &&
                    fields[i].Name.IndexOf("BackingField",
                    System.StringComparison.Ordinal) >= 0)
                {
                    _rbField = fields[i];
                }

                // cPkCE^ = acceleration force = ~14
                if (string.Equals(fields[i].FieldType.Name, "Single",
                    System.StringComparison.Ordinal) &&
                    (object)_accelField == null)
                {
                    object val = fields[i].GetValue(v);
                    if (val is float && Mathf.Approximately((float)val, 14f))
                    {
                        _accelField = fields[i];
                    }
                }
            }

            // TDEX{ib - onGround bool property
            // j[fCiJt - inputAcceleration float property
            PropertyInfo[] props = t.GetProperties(
                BindingFlags.Public | BindingFlags.Instance
            );
            for (int i = 0; i < props.Length; i++)
            {
                if (!props[i].CanRead) continue;

                if ((object)_onGroundPropInfo == null &&
                    string.Equals(props[i].PropertyType.Name, "Boolean",
                    System.StringComparison.Ordinal))
                {
                    // TDEX{ib starts with 'T' - pick the first readable bool prop
                    if (props[i].Name.StartsWith("T"))
                    {
                        _onGroundPropInfo = props[i];
                    }
                }

                if ((object)_inputAccelProp == null &&
                    string.Equals(props[i].PropertyType.Name, "Single",
                    System.StringComparison.Ordinal))
                {
                    // j[fCiJt is inputAcceleration - starts with 'j'
                    if (props[i].Name.StartsWith("j"))
                    {
                        _inputAccelProp = props[i];
                    }
                }
            }
        }
    }
}