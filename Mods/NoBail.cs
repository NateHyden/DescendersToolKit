using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class NoBail
    {
        public static bool Enabled { get; private set; } = false;

        private static PlayerInfoImpact _cached = null;

        public static void Toggle()
        {
            Enabled = !Enabled;
            Apply();
            MelonLogger.Msg("No Bail -> " + (Enabled ? "ON" : "OFF"));
        }

        public static void SetEnabled(bool enabled)
        {
            Enabled = enabled;
            Apply();
        }

        // Called from OnUpdate — only does real work when toggled, not every frame
        public static void Apply()
        {
            try
            {
                if ((object)_cached == null)
                {
                    GameObject playerInfoObject = GameObject.Find("PlayerInfo_Human");
                    if ((object)playerInfoObject == null) return;
                    _cached = playerInfoObject.GetComponent<PlayerInfoImpact>();
                }
                if ((object)_cached == null) return;
                _cached.Nobail(Enabled);
            }
            catch (System.Exception ex) { MelonLogger.Error("NoBail.Apply: " + ex.Message); }
        }

        public static void ClearCache() { _cached = null; }
    }
}