using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page6UI
    {
        private static Text spinVal, hopVal, wheelieVal, leanVal;
        private static Image spinBar, hopBar, wheelieBar, leanBar;

        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
            pg = UIHelpers.Obj("P6R", parent);
            UIHelpers.Fill(UIHelpers.RT(pg));
            var vlg = pg.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = UIHelpers.RowGap;
            vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;

            UIHelpers.SectionHeader("MOVEMENT", pg.transform);

            // Spin / Rotation speed
            var sr = UIHelpers.StatRow("Rotation Speed", pg.transform);
            spinBar = UIHelpers.MakeBar("SpB", sr.transform, (Movement.SpinLevel - 1) / 9f);
            spinVal = UIHelpers.Txt("SpV", sr.transform, Movement.SpinLevel.ToString(), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            var svle = spinVal.gameObject.AddComponent<LayoutElement>();
            svle.preferredWidth = 18; svle.preferredHeight = 20; svle.flexibleHeight = 0;
            UIHelpers.SmallBtn(sr.transform, "-", () => { Movement.SpinDecrease(); RefreshAll(); });
            UIHelpers.SmallBtn(sr.transform, "+", () => { Movement.SpinIncrease(); RefreshAll(); });

            // Bunny Hop launch force
            var hr = UIHelpers.StatRow("Hop Force", pg.transform);
            hopBar = UIHelpers.MakeBar("HpB", hr.transform, (Movement.HopLevel - 1) / 9f);
            hopVal = UIHelpers.Txt("HpV", hr.transform, Movement.HopLevel.ToString(), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            var hvle = hopVal.gameObject.AddComponent<LayoutElement>();
            hvle.preferredWidth = 18; hvle.preferredHeight = 20; hvle.flexibleHeight = 0;
            UIHelpers.SmallBtn(hr.transform, "-", () => { Movement.HopDecrease(); RefreshAll(); });
            UIHelpers.SmallBtn(hr.transform, "+", () => { Movement.HopIncrease(); RefreshAll(); });

            // Wheelie force
            var wr = UIHelpers.StatRow("Wheelie Force", pg.transform);
            wheelieBar = UIHelpers.MakeBar("WlB", wr.transform, (Movement.WheelieLevel - 1) / 9f);
            wheelieVal = UIHelpers.Txt("WlV", wr.transform, Movement.WheelieLevel.ToString(), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            var wvle = wheelieVal.gameObject.AddComponent<LayoutElement>();
            wvle.preferredWidth = 18; wvle.preferredHeight = 20; wvle.flexibleHeight = 0;
            UIHelpers.SmallBtn(wr.transform, "-", () => { Movement.WheelieDecrease(); RefreshAll(); });
            UIHelpers.SmallBtn(wr.transform, "+", () => { Movement.WheelieIncrease(); RefreshAll(); });

            // Lean strength
            var lr = UIHelpers.StatRow("Lean Strength", pg.transform);
            leanBar = UIHelpers.MakeBar("LnB", lr.transform, (Movement.LeanLevel - 1) / 9f);
            leanVal = UIHelpers.Txt("LnV", lr.transform, Movement.LeanLevel.ToString(), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            var lvle = leanVal.gameObject.AddComponent<LayoutElement>();
            lvle.preferredWidth = 18; lvle.preferredHeight = 20; lvle.flexibleHeight = 0;
            UIHelpers.SmallBtn(lr.transform, "-", () => { Movement.LeanDecrease(); RefreshAll(); });
            UIHelpers.SmallBtn(lr.transform, "+", () => { Movement.LeanIncrease(); RefreshAll(); });



            }
            catch (System.Exception ex) { MelonLogger.Error("Page6UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        public static void RefreshAll()
        {
            if (spinVal) spinVal.text = Movement.SpinLevel.ToString();
            if (hopVal) hopVal.text = Movement.HopLevel.ToString();
            if (wheelieVal) wheelieVal.text = Movement.WheelieLevel.ToString();
            if (leanVal) leanVal.text = Movement.LeanLevel.ToString();

            UIHelpers.SetBar(spinBar, (Movement.SpinLevel - 1) / 9f);
            UIHelpers.SetBar(hopBar, (Movement.HopLevel - 1) / 9f);
            UIHelpers.SetBar(wheelieBar, (Movement.WheelieLevel - 1) / 9f);
            UIHelpers.SetBar(leanBar, (Movement.LeanLevel - 1) / 9f);
        }
    }
}