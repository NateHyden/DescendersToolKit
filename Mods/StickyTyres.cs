using MelonLoader;
using UnityEngine;
using System.Reflection;

namespace DescendersModMenu.Mods
{
    // Pushes each grounded wheel into the surface it's touching.
    // Works on any surface — floor, wall, ceiling.
    // Uses the wheel's own contact normal so it's always correct.
    public static class StickyTyres
    {
        public static bool Enabled { get; private set; } = false;

        // How hard to push the tyres into the surface.
        // 25f ≈ enough to counteract gravity (9.81 * ~60kg bike mass).
        // Raise to stick to walls/ceiling.
        public static float SuctionForce = 150f;

        private static PropertyInfo _wheelsProp = null; // Vehicle.OpYe84Kx  (Wheel[])  - property
        private static PropertyInfo _rbProp = null; // Vehicle.gL]xTm    (Rigidbody) - property
        private static PropertyInfo _isGroundedProp = null; // Wheel.TDEX{ib   (bool)      - property
        private static PropertyInfo _contactPtProp = null; // Wheel.aU7FbDfg   (Vector3)   - property
        private static FieldInfo _normalFld = null; // Wheel.WCWk5Ekn   (Vector3)   - private field
        private static bool _resolved = false;

        public static void Toggle()
        {
            Enabled = !Enabled;
            MelonLogger.Msg("[StickyTyres] -> " + (Enabled ? "ON" : "OFF"));
        }

        public static void FixedTick()
        {
            if (!Enabled) return;
            if (!_resolved) { Resolve(); if (!_resolved) return; }

            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;

                Vehicle vehicle = player.GetComponent<Vehicle>();
                if ((object)vehicle == null) return;

                Rigidbody rb = _rbProp.GetValue(vehicle, null) as Rigidbody;
                if ((object)rb == null) return;

                System.Array wheels = _wheelsProp.GetValue(vehicle, null) as System.Array;
                if ((object)wheels == null) return;

                // Count grounded wheels — only apply force while at least one tyre is touching
                int groundedCount = 0;
                foreach (var w in wheels)
                {
                    if ((object)w == null) continue;
                    if ((bool)_isGroundedProp.GetValue(w, null)) groundedCount++;
                }
                if (groundedCount == 0) return;

                // Apply force at the centre of mass along the vehicle's local down axis.
                // Using vehicle.transform.up (local up) negated so we push INTO whatever
                // surface the bike is currently sitting on — floor, wall, or ceiling.
                // Applying at COM (not contact points) avoids rotation/torque.
                Vector3 force = -vehicle.transform.up * SuctionForce;
                rb.AddForce(force, ForceMode.Force);
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("[StickyTyres] FixedTick: " + ex.Message);
                Enabled = false;
            }
        }

        private static void Resolve()
        {
            try
            {
                _wheelsProp = typeof(Vehicle).GetProperty(
                    "OpYe\u0084Kx",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                _rbProp = typeof(Vehicle).GetProperty(
                    "gL\u005DxT\u007Bm",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                System.Type wheelType = typeof(Wheel);

                _isGroundedProp = wheelType.GetProperty(
                    "TDEX\u007Bib",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                _contactPtProp = wheelType.GetProperty(
                    "aU\u007FbDfg",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                _normalFld = wheelType.GetField(
                    "WCWk\u005Ekn",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                _resolved = (object)_wheelsProp != null
                         && (object)_rbProp != null
                         && (object)_isGroundedProp != null
                         && (object)_contactPtProp != null
                         && (object)_normalFld != null;

                MelonLogger.Msg("[StickyTyres] Resolve: "
                    + "wheels=" + ((object)_wheelsProp != null)
                    + " rb=" + ((object)_rbProp != null)
                    + " grounded=" + ((object)_isGroundedProp != null)
                    + " contact=" + ((object)_contactPtProp != null)
                    + " normal=" + ((object)_normalFld != null)
                    + " -> " + (_resolved ? "OK" : "FAILED"));
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("[StickyTyres] Resolve: " + ex.Message);
                _resolved = false;
            }
        }

        public static void Reset()
        {
            Enabled = false;
            _resolved = false;
            _wheelsProp = _rbProp = _isGroundedProp = _contactPtProp = null;
            _normalFld = null;
        }
    }
}