using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class ScoreManager
    {
        // Cached VehicleTricks multiplier property
        private static PropertyInfo _multiplierProp = null;

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

        public static float GetMultiplier()
        {
            try
            {
                VehicleTricks tricks = GetVehicleTricks();
                if ((object)tricks == null) return 1f;
                CacheMultiplierProp(tricks);
                if ((object)_multiplierProp == null) return 1f;
                return (float)_multiplierProp.GetValue(tricks, null);
            }
            catch { return 1f; }
        }

        public static void SetMultiplier(float value)
        {
            try
            {
                VehicleTricks tricks = GetVehicleTricks();
                if ((object)tricks == null) { MelonLogger.Warning("[ScoreManager] VehicleTricks not found."); return; }
                CacheMultiplierProp(tricks);
                if ((object)_multiplierProp == null) { MelonLogger.Warning("[ScoreManager] Multiplier prop not found."); return; }
                float clamped = Mathf.Clamp(value, 1f, 20f);
                _multiplierProp.SetValue(tricks, clamped, null);
                MelonLogger.Msg("[ScoreManager] Multiplier set to " + clamped);
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("[ScoreManager] SetMultiplier failed: " + ex.Message);
            }
        }

        private static VehicleTricks GetVehicleTricks()
        {
            GameObject local = GameObject.Find("Player_Human");
            if ((object)local == null) return null;
            return local.GetComponentInChildren<VehicleTricks>();
        }

        private static void CacheMultiplierProp(VehicleTricks tricks)
        {
            if ((object)_multiplierProp != null) return;
            // FnHLcjK is a public float property — find it by name
            _multiplierProp = tricks.GetType().GetProperty(
                "FnHLcjK",
                BindingFlags.Public | BindingFlags.Instance);
            if ((object)_multiplierProp != null)
                MelonLogger.Msg("[ScoreManager] Found multiplier prop: FnHLcjK");
        }
    }
}
