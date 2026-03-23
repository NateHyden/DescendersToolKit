using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page3UI
    {
        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
            pg = UIHelpers.Obj("P3R", parent);
            UIHelpers.Fill(UIHelpers.RT(pg));
            var vlg = pg.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = UIHelpers.RowGap;
            vlg.padding = new RectOffset((int)UIHelpers.ContentPad,(int)UIHelpers.ContentPad,8,8);
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;

            UIHelpers.SectionHeader("SCENE TOOLS", pg.transform);

            var dr = UIHelpers.StatRow("Scene Dumper", pg.transform);
            UIHelpers.ActionBtn(dr.transform, "Dump Scene", () => SceneDumper.DumpCurrentScene(), 90);

            UIHelpers.InfoBox(pg.transform,
                "Dumps scene hierarchy, vehicle forensics, player data and component index to your desktop.");

            var sw = UIHelpers.StatRow("Speed Watcher", pg.transform);
            var h = UIHelpers.Txt("SH", sw.transform, "Hold F10 while riding", 10,
                FontStyle.Italic, TextAnchor.MiddleRight, UIHelpers.TextDim);
            var hle = h.gameObject.AddComponent<LayoutElement>(); hle.preferredWidth = 140; hle.preferredHeight = 20; hle.flexibleHeight = 0;

            UIHelpers.InfoBox(pg.transform,
                "Records Vehicle field changes while held. Fields that stop changing near the speed cap are the limiter.");

            UIHelpers.Divider(pg.transform);
            UIHelpers.SectionHeader("HOTKEYS", pg.transform);

            UIHelpers.HotkeyRow(pg.transform, "Toggle mod menu",       "F6");
            UIHelpers.HotkeyRow(pg.transform, "Dump scene to desktop", "F12");
            UIHelpers.HotkeyRow(pg.transform, "Speed watcher (hold)",  "F10");


            }
            catch (System.Exception ex) { MelonLogger.Error("Page3UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }
    }
}
