using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page6UI
    {
        // ── Movement row fields ───────────────────────────────────────
        private static Text  spinVal,    hopVal,    wheelieVal,    leanVal;
        private static Image spinBar,    hopBar,    wheelieBar,    leanBar;
        private static Text  spinTogVal, hopTogVal, wheelieTogVal, leanTogVal;
        private static Image spinTrack,  hopTrack,  wheelieTrack,  leanTrack;
        private static RectTransform spinKnob, hopKnob, wheelieKnob, leanKnob;

        // ── Balance & Physics row fields ──────────────────────────────
        private static Text  wbVal,  iacVal,  psVal;
        private static Image wbBar,  iacBar,  psBar;
        private static Text  _wbTogVal,  _iacTogVal;
        private static Image _wbTrack,   _iacTrack;
        private static RectTransform _wbKnob, _iacKnob;

        // ── Misc ──────────────────────────────────────────────────────
        private static Text          cutBrakesVal;
        private static Image         cutBrakesTrack;
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

                // ── MOVEMENT ─────────────────────────────────────────
                UIHelpers.SectionHeader("MOVEMENT", pg.transform);

                // Rotation Speed
                var sr = UIHelpers.StatRow("Rotation Speed", pg.transform);
                spinTogVal = UIHelpers.Txt("SpTV", sr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                spinTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(sr.transform, "SpT", () => { Movement.ToggleSpin(); RefreshAll(); }, out spinTrack, out spinKnob);
                spinBar = UIHelpers.MakeBar("SpB", sr.transform, (Movement.SpinLevel - 1) / 9f);
                spinVal = UIHelpers.Txt("SpV", sr.transform, Movement.SpinLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                spinVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(sr.transform, "-", () => { Movement.SpinDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(sr.transform, "+", () => { Movement.SpinIncrease(); RefreshAll(); });

                // Hop Force
                var hr = UIHelpers.StatRow("Hop Force", pg.transform);
                hopTogVal = UIHelpers.Txt("HpTV", hr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                hopTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(hr.transform, "HpT", () => { Movement.ToggleHop(); RefreshAll(); }, out hopTrack, out hopKnob);
                hopBar = UIHelpers.MakeBar("HpB", hr.transform, (Movement.HopLevel - 1) / 9f);
                hopVal = UIHelpers.Txt("HpV", hr.transform, Movement.HopLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                hopVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(hr.transform, "-", () => { Movement.HopDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(hr.transform, "+", () => { Movement.HopIncrease(); RefreshAll(); });

                // Wheelie Force
                var wr = UIHelpers.StatRow("Wheelie Force", pg.transform);
                wheelieTogVal = UIHelpers.Txt("WlTV", wr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                wheelieTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(wr.transform, "WlT", () => { Movement.ToggleWheelie(); RefreshAll(); }, out wheelieTrack, out wheelieKnob);
                wheelieBar = UIHelpers.MakeBar("WlB", wr.transform, (Movement.WheelieLevel - 1) / 9f);
                wheelieVal = UIHelpers.Txt("WlV", wr.transform, Movement.WheelieLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                wheelieVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(wr.transform, "-", () => { Movement.WheelieDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(wr.transform, "+", () => { Movement.WheelieIncrease(); RefreshAll(); });

                // Lean Strength
                var lr = UIHelpers.StatRow("Lean Strength", pg.transform);
                leanTogVal = UIHelpers.Txt("LnTV", lr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                leanTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(lr.transform, "LnT", () => { Movement.ToggleLean(); RefreshAll(); }, out leanTrack, out leanKnob);
                leanBar = UIHelpers.MakeBar("LnB", lr.transform, (Movement.LeanLevel - 1) / 9f);
                leanVal = UIHelpers.Txt("LnV", lr.transform, Movement.LeanLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                leanVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(lr.transform, "-", () => { Movement.LeanDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(lr.transform, "+", () => { Movement.LeanIncrease(); RefreshAll(); });

                UIHelpers.Divider(pg.transform);

                // ── BALANCE & PHYSICS ─────────────────────────────────
                UIHelpers.SectionHeader("BALANCE & PHYSICS", pg.transform);

                // Wheelie Angle Limit
                var wbr = UIHelpers.StatRow("Wheelie Angle Limit", pg.transform);
                wbBar = UIHelpers.MakeBar("WbB", wbr.transform, (WheelieAngleLimit.Level - 1) / 9f);
                wbVal = UIHelpers.Txt("WbV", wbr.transform, WheelieAngleLimit.DisplayValue, 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                wbVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                _wbTogVal = UIHelpers.Txt("WbTV", wbr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _wbTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                Image wbTrack; RectTransform wbKnob;
                UIHelpers.Toggle(wbr.transform, "WbT", () => { WheelieAngleLimit.Toggle(); RefreshAll(); }, out wbTrack, out wbKnob);
                UIHelpers.SmallBtn(wbr.transform, "-", () => { WheelieAngleLimit.Decrease(); RefreshAll(); });
                UIHelpers.SmallBtn(wbr.transform, "+", () => { WheelieAngleLimit.Increase(); RefreshAll(); });
                _wbTrack = wbTrack; _wbKnob = wbKnob;
                UIHelpers.InfoBox(pg.transform, "Caps pitch angle in a wheelie. Lower = tighter cap.");

                // Air Control
                var iacr = UIHelpers.StatRow("Air Control", pg.transform);
                iacBar = UIHelpers.MakeBar("IaB", iacr.transform, (AirControl.Level - 1) / 9f);
                iacVal = UIHelpers.Txt("IaV", iacr.transform, AirControl.DisplayValue, 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                iacVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                _iacTogVal = UIHelpers.Txt("IaTV", iacr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _iacTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                Image iacTrack; RectTransform iacKnob;
                UIHelpers.Toggle(iacr.transform, "IaT", () => { AirControl.Toggle(); RefreshAll(); }, out iacTrack, out iacKnob);
                UIHelpers.SmallBtn(iacr.transform, "-", () => { AirControl.Decrease(); RefreshAll(); });
                UIHelpers.SmallBtn(iacr.transform, "+", () => { AirControl.Increase(); RefreshAll(); });
                _iacTrack = iacTrack; _iacKnob = iacKnob;
                UIHelpers.InfoBox(pg.transform, "Damps rotation while airborne. Higher = more stable.");

                // Pump Strength
                var fbr = UIHelpers.StatRow("Pump Strength", pg.transform);
                psBar = UIHelpers.MakeBar("PsB", fbr.transform, (GameModifierMods.PumpStrengthLevel - 1) / 9f);
                psVal = UIHelpers.Txt("PsV", fbr.transform, GameModifierMods.PumpStrengthLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                psVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(fbr.transform, "-", () => { GameModifierMods.PumpStrengthDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(fbr.transform, "+", () => { GameModifierMods.PumpStrengthIncrease(); RefreshAll(); });

                UIHelpers.Divider(pg.transform);

                // ── MISC ──────────────────────────────────────────────
                UIHelpers.SectionHeader("MISC", pg.transform);

                var cbr = UIHelpers.StatRow("Cut Brakes", pg.transform);
                cutBrakesVal = UIHelpers.Txt("CbV", cbr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                cutBrakesVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(cbr.transform, "CbT", () => { CutBrakes.Toggle(); RefreshAll(); }, out cutBrakesTrack, out cutBrakesKnob);
            }
            catch (System.Exception ex) { MelonLogger.Error("Page6UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        public static void RefreshAll()
        {
            // ── Rotation Speed ────────────────────────────────────────
            bool spOn = Movement.SpinEnabled;
            if (spinTogVal)  { spinTogVal.text  = spOn ? "ON" : "OFF"; spinTogVal.color  = spOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(spinTrack, spinKnob, spOn);
            if (spinVal) spinVal.text = Movement.SpinLevel.ToString();
            UIHelpers.SetBar(spinBar, (Movement.SpinLevel - 1) / 9f);

            // ── Hop Force ─────────────────────────────────────────────
            bool hpOn = Movement.HopEnabled;
            if (hopTogVal)   { hopTogVal.text   = hpOn ? "ON" : "OFF"; hopTogVal.color   = hpOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(hopTrack, hopKnob, hpOn);
            if (hopVal) hopVal.text = Movement.HopLevel.ToString();
            UIHelpers.SetBar(hopBar, (Movement.HopLevel - 1) / 9f);

            // ── Wheelie Force ─────────────────────────────────────────
            bool wlOn = Movement.WheelieEnabled;
            if (wheelieTogVal) { wheelieTogVal.text = wlOn ? "ON" : "OFF"; wheelieTogVal.color = wlOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(wheelieTrack, wheelieKnob, wlOn);
            if (wheelieVal) wheelieVal.text = Movement.WheelieLevel.ToString();
            UIHelpers.SetBar(wheelieBar, (Movement.WheelieLevel - 1) / 9f);

            // ── Lean Strength ─────────────────────────────────────────
            bool lnOn = Movement.LeanEnabled;
            if (leanTogVal)  { leanTogVal.text  = lnOn ? "ON" : "OFF"; leanTogVal.color  = lnOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(leanTrack, leanKnob, lnOn);
            if (leanVal) leanVal.text = Movement.LeanLevel.ToString();
            UIHelpers.SetBar(leanBar, (Movement.LeanLevel - 1) / 9f);

            // ── Wheelie Angle Limit ───────────────────────────────────
            if (wbVal) wbVal.text = WheelieAngleLimit.DisplayValue;
            if (_wbTogVal) { _wbTogVal.text = WheelieAngleLimit.Enabled ? "ON" : "OFF"; _wbTogVal.color = WheelieAngleLimit.Enabled ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_wbTrack, _wbKnob, WheelieAngleLimit.Enabled);
            UIHelpers.SetBar(wbBar, (WheelieAngleLimit.Level - 1) / 9f);

            // ── Air Control ───────────────────────────────────────────
            if (iacVal) iacVal.text = AirControl.DisplayValue;
            if (_iacTogVal) { _iacTogVal.text = AirControl.Enabled ? "ON" : "OFF"; _iacTogVal.color = AirControl.Enabled ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_iacTrack, _iacKnob, AirControl.Enabled);
            UIHelpers.SetBar(iacBar, (AirControl.Level - 1) / 9f);

            // ── Pump Strength ─────────────────────────────────────────
            if (psVal) psVal.text = GameModifierMods.PumpStrengthLevel.ToString();
            UIHelpers.SetBar(psBar, (GameModifierMods.PumpStrengthLevel - 1) / 9f);

            // ── Cut Brakes ────────────────────────────────────────────
            if (cutBrakesVal) { cutBrakesVal.text = CutBrakes.Enabled ? "ON" : "OFF"; cutBrakesVal.color = CutBrakes.Enabled ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(cutBrakesTrack, cutBrakesKnob, CutBrakes.Enabled);
        }
    }
}
