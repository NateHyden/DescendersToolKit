using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    // Handles all j\x7FaU\x7Etd modifier-based mods via AddGameModifier
    // percentageValue on the modifier's first entry IS the multiplier
    // Default for all is 1.0 (100%)
    public static class GameModifierMods
    {
        public static int WheelieBalanceLevel { get; private set; } = 1;
        public static int InAirCorrLevel { get; private set; } = 1;
        public static int FakieBalanceLevel { get; private set; } = 1;
        public static int PumpStrengthLevel { get; private set; } = 1;
        public static int IcePhysicsLevel { get; private set; } = 1;

        // Multiplier: level 1 = 1x (default), level 10 = 4x
        private static float Mult(int level) { return level * 0.333f + 0.667f; }

        // ── Wheelie Balance ──────────────────────────────────────────────
        public static void WheelieBalanceIncrease() { if (WheelieBalanceLevel < 10) { WheelieBalanceLevel++; ApplyMod("WHEELIEBALANCE", WheelieBalanceLevel); } }
        public static void WheelieBalanceDecrease() { if (WheelieBalanceLevel > 1) { WheelieBalanceLevel--; ApplyMod("WHEELIEBALANCE", WheelieBalanceLevel); } }

        // ── In-Air Correction ────────────────────────────────────────────
        public static void InAirCorrIncrease() { if (InAirCorrLevel < 10) { InAirCorrLevel++; ApplyMod("AIRCORRECTION", InAirCorrLevel); } }
        public static void InAirCorrDecrease() { if (InAirCorrLevel > 1) { InAirCorrLevel--; ApplyMod("AIRCORRECTION", InAirCorrLevel); } }

        // ── Fakie Balance ─────────────────────────────────────────────────
        public static void FakieBalanceIncrease() { if (FakieBalanceLevel < 10) { FakieBalanceLevel++; ApplyMod("FAKIEBALANCE", FakieBalanceLevel); } }
        public static void FakieBalanceDecrease() { if (FakieBalanceLevel > 1) { FakieBalanceLevel--; ApplyMod("FAKIEBALANCE", FakieBalanceLevel); } }

        // ── Pump Strength ─────────────────────────────────────────────────
        public static void PumpStrengthIncrease() { if (PumpStrengthLevel < 10) { PumpStrengthLevel++; ApplyMod("PUMPSTRENGTH", PumpStrengthLevel); } }
        public static void PumpStrengthDecrease() { if (PumpStrengthLevel > 1) { PumpStrengthLevel--; ApplyMod("PUMPSTRENGTH", PumpStrengthLevel); } }

        // ── Ice Physics (lower = more slippy) ────────────────────────────
        // Level 1 = max ice (0.1x), Level 5 = default (1x), Level 10 = grippy (2x)
        public static void IcePhysicsIncrease() { if (IcePhysicsLevel < 10) { IcePhysicsLevel++; ApplyMod("OFFROADFRICTION", IcePhysicsLevel); } }
        public static void IcePhysicsDecrease() { if (IcePhysicsLevel > 1) { IcePhysicsLevel--; ApplyMod("OFFROADFRICTION", IcePhysicsLevel); } }

        private static float IceMult(int level)
        {
            // Level 1 = 0.1 (icy), Level 5 = 1.0 (default), Level 10 = 2.0 (grippy)
            if (level <= 5)
                return 0.1f + (level - 1) * 0.225f;
            else
                return 1.0f + (level - 5) * 0.2f;
        }

        public static void ApplyMod(string modName, int level)
        {
            try
            {
                GameData gameData = UnityEngine.Object.FindObjectOfType<GameData>();
                if ((object)gameData == null) { MelonLogger.Warning("[GameMod] GameData not found."); return; }

                // Get the gameModifiers array via reflection (field \u0081jU\u0080h\u0084c)
                FieldInfo modArrayField = gameData.GetType().GetField("\u0081jU\u0080h\u0084c",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if ((object)modArrayField == null) { MelonLogger.Warning("[GameMod] Mod array field not found."); return; }

                GameModifier[] mods = modArrayField.GetValue(gameData) as GameModifier[];
                if ((object)mods == null) { MelonLogger.Warning("[GameMod] Mod array is null."); return; }

                GameModifier target = null;
                for (int i = 0; i < mods.Length; i++)
                    if ((object)mods[i] != null && mods[i].name == modName)
                    { target = mods[i]; break; }

                if ((object)target == null) { MelonLogger.Warning("[GameMod] Modifier not found: " + modName); return; }

                float value = modName == "OFFROADFRICTION" ? IceMult(level) : Mult(level);
                target.modifiers[0].percentageValue = value;

                // Apply to local player
                PlayerManager pm = UnityEngine.Object.FindObjectOfType<PlayerManager>();
                if ((object)pm == null) { MelonLogger.Warning("[GameMod] PlayerManager not found."); return; }
                PlayerInfoImpact pi = pm.GetPlayerImpact();
                if ((object)pi == null) { MelonLogger.Warning("[GameMod] PlayerInfoImpact not found."); return; }

                pi.AddGameModifier(target);
                MelonLogger.Msg("[GameMod] " + modName + " level " + level + " (" + value + ")");
            }
            catch (System.Exception ex) { MelonLogger.Error("[GameMod] ApplyMod " + modName + ": " + ex.Message); }
        }
    }
}