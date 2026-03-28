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

        private static Text wbVal, iacVal, psVal;
        private static Image wbBar, iacBar, psBar;
        private static Text cutBrakesVal;
        private static Image cutBrakesTrack;
        private static RectTransform cutBrakesKnob;

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

                // ── Physical Movement ──────────────────────────────────────────
                UIHelpers.SectionHeader("MOVEMENT", pg.transform);

                var sr = UIHelpers.StatRow("Rotation Speed", pg.transform);
                spinBar = UIHelpers.MakeBar("SpB", sr.transform, (Movement.SpinLevel - 1) / 9f);
                spinVal = UIHelpers.Txt("SpV", sr.transform, Movement.SpinLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                spinVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(sr.transform, "-", () => { Movement.SpinDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(sr.transform, "+", () => { Movement.SpinIncrease(); RefreshAll(); });

                var hr = UIHelpers.StatRow("Hop Force", pg.transform);
                hopBar = UIHelpers.MakeBar("HpB", hr.transform, (Movement.HopLevel - 1) / 9f);
                hopVal = UIHelpers.Txt("HpV", hr.transform, Movement.HopLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                hopVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(hr.transform, "-", () => { Movement.HopDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(hr.transform, "+", () => { Movement.HopIncrease(); RefreshAll(); });

                var wr = UIHelpers.StatRow("Wheelie Force", pg.transform);
                wheelieBar = UIHelpers.MakeBar("WlB", wr.transform, (Movement.WheelieLevel - 1) / 9f);
                wheelieVal = UIHelpers.Txt("WlV", wr.transform, Movement.WheelieLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                wheelieVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(wr.transform, "-", () => { Movement.WheelieDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(wr.transform, "+", () => { Movement.WheelieIncrease(); RefreshAll(); });

                var lr = UIHelpers.StatRow("Lean Strength", pg.transform);
                leanBar = UIHelpers.MakeBar("LnB", lr.transform, (Movement.LeanLevel - 1) / 9f);
                leanVal = UIHelpers.Txt("LnV", lr.transform, Movement.LeanLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                leanVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(lr.transform, "-", () => { Movement.LeanDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(lr.transform, "+", () => { Movement.LeanIncrease(); RefreshAll(); });

                UIHelpers.Divider(pg.transform);

                // ── Game Modifiers ─────────────────────────────────────────────
                UIHelpers.SectionHeader("BALANCE & PHYSICS", pg.transform);

                var wbr = UIHelpers.StatRow("Wheelie Balance", pg.transform);
                wbBar = UIHelpers.MakeBar("WbB", wbr.transform, (GameModifierMods.WheelieBalanceLevel - 1) / 9f);
                wbVal = UIHelpers.Txt("WbV", wbr.transform, GameModifierMods.WheelieBalanceLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                wbVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(wbr.transform, "-", () => { GameModifierMods.WheelieBalanceDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(wbr.transform, "+", () => { GameModifierMods.WheelieBalanceIncrease(); RefreshAll(); });

                var iacr = UIHelpers.StatRow("In-Air Correction", pg.transform);
                iacBar = UIHelpers.MakeBar("IaB", iacr.transform, (GameModifierMods.InAirCorrLevel - 1) / 9f);
                iacVal = UIHelpers.Txt("IaV", iacr.transform, GameModifierMods.InAirCorrLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                iacVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(iacr.transform, "-", () => { GameModifierMods.InAirCorrDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(iacr.transform, "+", () => { GameModifierMods.InAirCorrIncrease(); RefreshAll(); });

                var fbr = UIHelpers.StatRow("Pump Strength", pg.transform);
                psBar = UIHelpers.MakeBar("PsB", fbr.transform, (GameModifierMods.PumpStrengthLevel - 1) / 9f);
                psVal = UIHelpers.Txt("PsV", fbr.transform, GameModifierMods.PumpStrengthLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                psVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(fbr.transform, "-", () => { GameModifierMods.PumpStrengthDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(fbr.transform, "+", () => { GameModifierMods.PumpStrengthIncrease(); RefreshAll(); });

                UIHelpers.Divider(pg.transform);
                UIHelpers.SectionHeader("MISC", pg.transform);

                var cbr = UIHelpers.StatRow("Cut Brakes", pg.transform);
                cutBrakesVal = UIHelpers.Txt("CbV", cbr.transform, "OFF", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                cutBrakesVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(cbr.transform, "CbT", () => { CutBrakes.Toggle(); RefreshAll(); },
                    out cutBrakesTrack, out cutBrakesKnob);

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

            if (wbVal) wbVal.text = GameModifierMods.WheelieBalanceLevel.ToString();
            if (iacVal) iacVal.text = GameModifierMods.InAirCorrLevel.ToString();
            if (psVal) psVal.text = GameModifierMods.PumpStrengthLevel.ToString();
            UIHelpers.SetBar(wbBar, (GameModifierMods.WheelieBalanceLevel - 1) / 9f);
            UIHelpers.SetBar(iacBar, (GameModifierMods.InAirCorrLevel - 1) / 9f);
            UIHelpers.SetBar(psBar, (GameModifierMods.PumpStrengthLevel - 1) / 9f);

            if (cutBrakesVal) { cutBrakesVal.text = CutBrakes.Enabled ? "ON" : "OFF"; cutBrakesVal.color = CutBrakes.Enabled ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(cutBrakesTrack, cutBrakesKnob, CutBrakes.Enabled);
        }
    }
}