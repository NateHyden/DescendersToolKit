using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class Movement
    {
        // ── Spin / Rotation speed (Cyclist Stunting: \x5EE\x5DVx\x81\x82, default 15f) ──
        public static int SpinLevel { get; private set; } = 1;
        private static FieldInfo _spinField = null;
        private static float _spinDefault = -1f;

        // ── Bunny Hop launch velocity (Cyclist: kV\u005B\u0083SnO, default 10f) ──
        // Only applied during the actual hop release - safe to modify
        public static int HopLevel { get; private set; } = 1;
        private static FieldInfo _hopField = null;
        private static float _hopDefault = -1f;

        // ── Wheelie balance force (Cyclist Stunting: \x7D\x60\x82zWt\x7C, default 10f) ──
        public static int WheelieLevel { get; private set; } = 1;
        private static FieldInfo _wheelieField = null;
        private static float _wheelieDefault = -1f;

        // ── Lean strength (Cyclist Leaning: Pl\x7Fvrb\x82, default 1f) ──
        public static int LeanLevel { get; private set; } = 1;
        private static FieldInfo _leanField = null;
        private static float _leanDefault = -1f;

        // Level 5 = default (1x)
        private static float Mult(int level) { return level * 0.3f + 0.7f; }  // level 1 = 1x, level 10 = 3.7x
        private static float MultStrong(int level) { return level * 0.3f + 0.7f; }  // level 1 = 1x, level 10 = 3.7x

        // ── Spin ────────────────────────────────────────────────────────────
        public static void SpinIncrease() { if (SpinLevel < 10) { SpinLevel++; ApplySpin(); } }
        public static void SpinDecrease() { if (SpinLevel > 1) { SpinLevel--; ApplySpin(); } }

        public static void ApplySpin()
        {

            try
            {
            Cyclist c = GetCyclist();
            if ((object)c == null) return;
            if ((object)_spinField == null)
                _spinField = FindField(c, "\u005EE\u005DVx\u0081\u0082");
            if ((object)_spinField == null) { MelonLogger.Warning("[Movement] Spin field not found."); return; }
            if (_spinDefault < 0f) _spinDefault = (float)_spinField.GetValue(c);
            _spinField.SetValue(c, _spinDefault * Mult(SpinLevel));
            MelonLogger.Msg("[Movement] Spin -> " + SpinLevel + " (" + (_spinDefault * Mult(SpinLevel)) + ")");
            }
            catch (System.Exception ex) { MelonLogger.Error("Movement.ApplySpin: " + ex.Message); }
        
        }

        // ── Hop ─────────────────────────────────────────────────────────────
        public static void HopIncrease() { if (HopLevel < 10) { HopLevel++; ApplyHop(); } }
        public static void HopDecrease() { if (HopLevel > 1) { HopLevel--; ApplyHop(); } }

        public static void ApplyHop()
        {

            try
            {
            Cyclist c = GetCyclist();
            if ((object)c == null) return;
            if ((object)_hopField == null)
                _hopField = FindField(c, "kV\u005B\u0083SnO");
            if ((object)_hopField == null) { MelonLogger.Warning("[Movement] Hop field not found."); return; }
            if (_hopDefault < 0f) _hopDefault = (float)_hopField.GetValue(c);
            _hopField.SetValue(c, _hopDefault * MultStrong(HopLevel));
            MelonLogger.Msg("[Movement] Hop -> " + HopLevel + " (" + (_hopDefault * MultStrong(HopLevel)) + ")");
            }
            catch (System.Exception ex) { MelonLogger.Error("Movement.ApplyHop: " + ex.Message); }
        
        }

        // ── Wheelie ─────────────────────────────────────────────────────────
        public static void WheelieIncrease() { if (WheelieLevel < 10) { WheelieLevel++; ApplyWheelie(); } }
        public static void WheelieDecrease() { if (WheelieLevel > 1) { WheelieLevel--; ApplyWheelie(); } }

        public static void ApplyWheelie()
        {

            try
            {
            Cyclist c = GetCyclist();
            if ((object)c == null) return;
            if ((object)_wheelieField == null)
                _wheelieField = FindField(c, "\u007D\u0060\u0082zWt\u007C");
            if ((object)_wheelieField == null) { MelonLogger.Warning("[Movement] Wheelie field not found."); return; }
            if (_wheelieDefault < 0f) _wheelieDefault = (float)_wheelieField.GetValue(c);
            _wheelieField.SetValue(c, _wheelieDefault * Mult(WheelieLevel));
            MelonLogger.Msg("[Movement] Wheelie -> " + WheelieLevel + " (" + (_wheelieDefault * Mult(WheelieLevel)) + ")");
            }
            catch (System.Exception ex) { MelonLogger.Error("Movement.ApplyWheelie: " + ex.Message); }
        
        }

        // ── Lean ────────────────────────────────────────────────────────────
        public static void LeanIncrease() { if (LeanLevel < 10) { LeanLevel++; ApplyLean(); } }
        public static void LeanDecrease() { if (LeanLevel > 1) { LeanLevel--; ApplyLean(); } }

        public static void ApplyLean()
        {

            try
            {
            Cyclist c = GetCyclist();
            if ((object)c == null) return;
            if ((object)_leanField == null)
                _leanField = FindField(c, "Pl\u007Fvrb\u0082");
            if ((object)_leanField == null) { MelonLogger.Warning("[Movement] Lean field not found."); return; }
            if (_leanDefault < 0f) _leanDefault = (float)_leanField.GetValue(c);
            _leanField.SetValue(c, _leanDefault * MultStrong(LeanLevel));
            MelonLogger.Msg("[Movement] Lean -> " + LeanLevel + " (" + (_leanDefault * MultStrong(LeanLevel)) + ")");
            }
            catch (System.Exception ex) { MelonLogger.Error("Movement.ApplyLean: " + ex.Message); }
        
        }

        // ── Helpers ─────────────────────────────────────────────────────────
        private static Cyclist GetCyclist()
        {
            GameObject local = GameObject.Find("Player_Human");
            if ((object)local == null) return null;
            return local.GetComponentInChildren<Cyclist>();
        }

        private static FieldInfo FindField(Cyclist c, string name)
        {
            FieldInfo f = c.GetType().GetField(name,
                BindingFlags.Public | BindingFlags.Instance);
            if ((object)f != null)
                MelonLogger.Msg("[Movement] Found field: " + name);
            return f;
        }
    }
}