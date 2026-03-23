using MelonLoader;

namespace DescendersModMenu.Mods
{
    public static class UnlockAll
    {
        public static void Cosmetics()
        {
            try { DevCommandsGameplay.UnlockAll(); MelonLogger.Msg("UnlockAll: Cosmetics done."); }
            catch (System.Exception ex) { MelonLogger.Error("UnlockAll: Cosmetics failed: " + ex.Message); }
        }

        public static void Shortcuts()
        {
            try { DevCommandsGameplay.UnlockAllShortcuts(); MelonLogger.Msg("UnlockAll: Shortcuts done."); }
            catch (System.Exception ex) { MelonLogger.Error("UnlockAll: Shortcuts failed: " + ex.Message); }
        }

        public static void Achievements()
        {
            try { DevCommandsGameplay.UnlockAllAchievements(); MelonLogger.Msg("UnlockAll: Achievements done."); }
            catch (System.Exception ex) { MelonLogger.Error("UnlockAll: Achievements failed: " + ex.Message); }
        }

        public static void Missions()
        {
            try { DevCommandsGameplay.CompleteAllMissions(); MelonLogger.Msg("UnlockAll: Missions done."); }
            catch (System.Exception ex) { MelonLogger.Error("UnlockAll: Missions failed: " + ex.Message); }
        }
    }
}
