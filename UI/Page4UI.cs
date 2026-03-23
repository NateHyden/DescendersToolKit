using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page4UI
    {
        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
            pg = UIHelpers.Obj("P4R", parent);
            UIHelpers.Fill(UIHelpers.RT(pg));
            var vlg = pg.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = UIHelpers.RowGap;
            vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;

            UIHelpers.SectionHeader("COSMETICS & PROGRESSION", pg.transform);

            var cr = UIHelpers.StatRow("Cosmetics", pg.transform);
            UIHelpers.ActionBtn(cr.transform, "Unlock All", () => UnlockAll.Cosmetics(), 90);

            UIHelpers.InfoBox(pg.transform, "Unlocks all cosmetic items including bikes, helmets, jerseys and more.");

            var sr = UIHelpers.StatRow("Shortcuts", pg.transform);
            UIHelpers.ActionBtn(sr.transform, "Unlock All", () => UnlockAll.Shortcuts(), 90);

            UIHelpers.InfoBox(pg.transform, "Unlocks all world shortcuts across every biome.");

            UIHelpers.Divider(pg.transform);
            UIHelpers.SectionHeader("ACHIEVEMENTS & MISSIONS", pg.transform);

            var ar = UIHelpers.StatRow("Achievements", pg.transform);
            UIHelpers.ActionBtn(ar.transform, "Unlock All", () => UnlockAll.Achievements(), 90);

            UIHelpers.InfoBox(pg.transform, "Unlocks all Steam achievements.");

            var mr = UIHelpers.StatRow("Missions", pg.transform);
            UIHelpers.ActionBtn(mr.transform, "Complete All", () => UnlockAll.Missions(), 96);

            UIHelpers.InfoBox(pg.transform, "Marks all missions as complete.");


            }
            catch (System.Exception ex) { MelonLogger.Error("Page4UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }
    }
}
