using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page5UI
    {
        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
                pg = UIHelpers.Obj("P5R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));
                var vlg = pg.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = UIHelpers.RowGap;
                vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;

                // ── Add Score ─────────────────────────────────────────────────
                UIHelpers.SectionHeader("ADD SCORE", pg.transform);

                var r1 = UIHelpers.StatRow("REP Points", pg.transform);
                UIHelpers.ActionBtn(r1.transform, "+100", () => ScoreManager.AddScore(100), 44);
                UIHelpers.ActionBtn(r1.transform, "+500", () => ScoreManager.AddScore(500), 44);
                UIHelpers.ActionBtn(r1.transform, "+1K", () => ScoreManager.AddScore(1000), 44);
                UIHelpers.ActionBtn(r1.transform, "+5K", () => ScoreManager.AddScore(5000), 44);

                var r2 = UIHelpers.StatRow("REP Points", pg.transform);
                UIHelpers.ActionBtn(r2.transform, "+10K", () => ScoreManager.AddScore(10000), 50);
                UIHelpers.ActionBtn(r2.transform, "+50K", () => ScoreManager.AddScore(50000), 50);
                UIHelpers.ActionBtn(r2.transform, "+100K", () => ScoreManager.AddScore(100000), 56);
                UIHelpers.ActionBtn(r2.transform, "+1M", () => ScoreManager.AddScore(1000000), 50);

                UIHelpers.InfoBox(pg.transform, "Adds REP to your current session score. Must be in a game session.");

                UIHelpers.Divider(pg.transform);

                // ── Trick Multiplier ───────────────────────────────────────────
                UIHelpers.SectionHeader("TRICK MULTIPLIER", pg.transform);

                var mr = UIHelpers.StatRow("Multiplier", pg.transform);
                UIHelpers.ActionBtn(mr.transform, "x1", () => ScoreManager.SetMultiplier(1f), 36);
                UIHelpers.ActionBtn(mr.transform, "x2", () => ScoreManager.SetMultiplier(2f), 36);
                UIHelpers.ActionBtn(mr.transform, "x5", () => ScoreManager.SetMultiplier(5f), 36);
                UIHelpers.ActionBtn(mr.transform, "x10", () => ScoreManager.SetMultiplier(10f), 40);
                UIHelpers.ActionBtn(mr.transform, "x20", () => ScoreManager.SetMultiplier(20f), 40);

                UIHelpers.InfoBox(pg.transform, "Sets the trick score multiplier. Resets naturally when you bail or land.");

            }
            catch (System.Exception ex) { MelonLogger.Error("Page5UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }
    }
}