using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page8UI
    {
        private static Text _travelVal, _stiffVal, _dampVal;
        private static Image _travelBar, _stiffBar, _dampBar;

        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
                pg = UIHelpers.Obj("P8R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));
                var vlg = pg.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = UIHelpers.RowGap;
                vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true;
                vlg.childForceExpandHeight = false;

                UIHelpers.SectionHeader("SUSPENSION", pg.transform);

                // Travel
                var tr = UIHelpers.StatRow("Travel", pg.transform);
                _travelBar = UIHelpers.MakeBar("TvB", tr.transform, (Suspension.TravelLevel - 1) / 9f);
                _travelVal = UIHelpers.Txt("TvV", tr.transform, Suspension.TravelLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                _travelVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(tr.transform, "-", () => { Suspension.TravelDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(tr.transform, "+", () => { Suspension.TravelIncrease(); RefreshAll(); });

                // Stiffness
                var sr = UIHelpers.StatRow("Stiffness", pg.transform);
                _stiffBar = UIHelpers.MakeBar("StB", sr.transform, (Suspension.StiffnessLevel - 1) / 9f);
                _stiffVal = UIHelpers.Txt("StV", sr.transform, Suspension.StiffnessLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                _stiffVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(sr.transform, "-", () => { Suspension.StiffnessDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(sr.transform, "+", () => { Suspension.StiffnessIncrease(); RefreshAll(); });

                // Damping
                var dr = UIHelpers.StatRow("Damping", pg.transform);
                _dampBar = UIHelpers.MakeBar("DpB", dr.transform, (Suspension.DampingLevel - 1) / 9f);
                _dampVal = UIHelpers.Txt("DpV", dr.transform, Suspension.DampingLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                _dampVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(dr.transform, "-", () => { Suspension.DampingDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(dr.transform, "+", () => { Suspension.DampingIncrease(); RefreshAll(); });

                UIHelpers.InfoBox(pg.transform, "Level 5 = default. Travel = how much the fork/shock moves. Stiffness = spring resistance. Damping = how fast it settles.");

                UIHelpers.Divider(pg.transform);
                UIHelpers.SectionHeader("BIKE SIZE", pg.transform);

                var szr = UIHelpers.StatRow("Size", pg.transform);
                UIHelpers.ActionBtn(szr.transform, "Giant", () => SetBikeScale(3.5f), 52);
                UIHelpers.ActionBtn(szr.transform, "Big", () => SetBikeScale(1.5f), 44);
                UIHelpers.ActionBtn(szr.transform, "Default", () => SetBikeScale(1.0f), 58);
                UIHelpers.ActionBtn(szr.transform, "Small", () => SetBikeScale(0.6f), 52);
                UIHelpers.ActionBtn(szr.transform, "Tiny", () => SetBikeScale(0.25f), 44);

                RefreshAll();
            }
            catch (System.Exception ex) { MelonLogger.Error("Page8UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        private static void SetBikeScale(float scale)
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) { MelonLoader.MelonLogger.Warning("[BikeSize] Player_Human not found."); return; }
                Transform bikeModel = player.transform.Find("BikeModel");
                if ((object)bikeModel == null) { MelonLoader.MelonLogger.Warning("[BikeSize] BikeModel not found."); return; }
                bikeModel.localScale = new Vector3(scale, scale, scale);
                MelonLoader.MelonLogger.Msg("[BikeSize] Scale -> " + scale);
            }
            catch (System.Exception ex) { MelonLoader.MelonLogger.Error("[BikeSize] " + ex.Message); }
        }

        public static void RefreshAll()
        {
            if (_travelVal) _travelVal.text = Suspension.TravelLevel.ToString();
            if (_stiffVal) _stiffVal.text = Suspension.StiffnessLevel.ToString();
            if (_dampVal) _dampVal.text = Suspension.DampingLevel.ToString();
            UIHelpers.SetBar(_travelBar, (Suspension.TravelLevel - 1) / 9f);
            UIHelpers.SetBar(_stiffBar, (Suspension.StiffnessLevel - 1) / 9f);
            UIHelpers.SetBar(_dampBar, (Suspension.DampingLevel - 1) / 9f);
        }
    }
}