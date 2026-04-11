using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class ScoreManager
    {
        private static PropertyInfo _multiplierProp = null;
        private static FieldInfo _maxMultField = null; // uDh\u005DdJt = int max cap (default 3)
        private static float _targetMultiplier = 0f;   // 0 = not locked
        private static bool _cached = false;

        public static void AddScore(int amount)
        {
            try
            {
                DevCommandsGameplay.AddScore(amount);
                MelonLogger.Msg("[ScoreManager] Added " + amount + " score.");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("[ScoreManager] AddScore failed: " + ex.Message);
            }
        }

        public static void SetMultiplier(float value)
        {
            try
            {
                VehicleTricks tricks = GetVehicleTricks();
                if ((object)tricks == null) { MelonLogger.Warning("[ScoreManager] VehicleTricks not found."); return; }
                CacheFields(tricks);
                if ((object)_multiplierProp == null) { MelonLogger.Warning("[ScoreManager] Multiplier prop not found."); return; }

                // Raise the max cap so the game's clamp allows our value
                if ((object)_maxMultField != null)
                    _maxMultField.SetValue(tricks, Mathf.Max((int)value + 1, 100));

                _multiplierProp.SetValue(tricks, value, null);
                _targetMultiplier = value;
                MelonLogger.Msg("[ScoreManager] Multiplier set to " + value);
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("[ScoreManager] SetMultiplier failed: " + ex.Message);
            }
        }

        public static void ResetMultiplier()
        {
            _targetMultiplier = 0f;
            // Restore default cap
            VehicleTricks tricks = GetVehicleTricks();
            if ((object)tricks == null) return;
            CacheFields(tricks);
            if ((object)_maxMultField != null) _maxMultField.SetValue(tricks, 3);
        }

        // Called from OnUpdate — re-applies multiplier every frame to beat the game's clamp
        public static void Tick()
        {
            if (_targetMultiplier <= 0f) return;
            try
            {
                VehicleTricks tricks = GetVehicleTricks();
                if ((object)tricks == null) return;
                CacheFields(tricks);
                if ((object)_multiplierProp == null) return;
                if ((object)_maxMultField != null)
                    _maxMultField.SetValue(tricks, Mathf.Max((int)_targetMultiplier + 1, 100));
                _multiplierProp.SetValue(tricks, _targetMultiplier, null);
            }
            catch { }
        }

        private static VehicleTricks GetVehicleTricks()
        {
            GameObject local = GameObject.Find("Player_Human");
            if ((object)local == null) return null;
            return local.GetComponentInChildren<VehicleTricks>();
        }

        private static void CacheFields(VehicleTricks tricks)
        {
            if (_cached) return;
            _cached = true;

            _multiplierProp = tricks.GetType().GetProperty("FnHLcjK",
                BindingFlags.Public | BindingFlags.Instance);
            if ((object)_multiplierProp != null)
                MelonLogger.Msg("[ScoreManager] Found multiplier prop: FnHLcjK");

            // uDh\u005DdJt = public int max multiplier cap
            _maxMultField = tricks.GetType().GetField("uDh\u005DdJt",
                BindingFlags.Public | BindingFlags.Instance);
            if ((object)_maxMultField != null)
                MelonLogger.Msg("[ScoreManager] Found max mult field: uDh]dJt default=" + _maxMultField.GetValue(tricks));
        }
    }
}