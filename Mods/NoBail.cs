using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class NoBail
    {
        public static bool Enabled { get; private set; } = false;

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

        public static void Apply()
        {
            try
            {
                GameObject playerInfoObject = GameObject.Find("PlayerInfo_Human");
                if (playerInfoObject == null) return;
                PlayerInfoImpact playerInfo = playerInfoObject.GetComponent<PlayerInfoImpact>();
                if (playerInfo == null) return;
                playerInfo.Nobail(Enabled);
            }
            catch (System.Exception ex) { MelonLogger.Error("NoBail.Apply: " + ex.Message); }
        }
    }
}
