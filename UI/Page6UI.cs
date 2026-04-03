using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page6UI
    {
        // ── Movement row fields ───────────────────────────────────────
        private static Text spinVal, hopVal, wheelieVal, leanVal;
        private static Image spinBar, hopBar, wheelieBar, leanBar;
        private static Text spinTogVal, hopTogVal, wheelieTogVal, leanTogVal;
        private static Image spinTrack, hopTrack, wheelieTrack, leanTrack;
        private static RectTransform spinKnob, hopKnob, wheelieKnob, leanKnob;

        // ── Balance & Physics row fields ──────────────────────────────
        private static Text wbVal, iacVal, psVal;
        private static Image wbBar, iacBar, psBar;
        private static Text _wbTogVal, _iacTogVal;
        private static Image _wbTrack, _iacTrack;
        private static RectTransform _wbKnob, _iacKnob;

        // ── Near Miss Sensitivity ─────────────────────────────────────
        private static Text _nmVal, _nmTogVal;
        private static Image _nmBar, _nmTrack;
        private static RectTransform _nmKnob;

        // ── Center of Mass ────────────────────────────────────────────
        private static Text _comLRVal, _comFBVal, _comUDVal;
        private static Image _comLRBar, _comFBBar, _comUDBar;

        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
                pg = UIHelpers.Obj("P6R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));

                // ── ScrollRect wrapper ────────────────────────────────
                var scrollObj = UIHelpers.Obj("Scroll", pg.transform);
                UIHelpers.Fill(UIHelpers.RT(scrollObj));
                var scrollRect = scrollObj.AddComponent<ScrollRect>();
                scrollRect.horizontal = false; scrollRect.vertical = true;
                scrollRect.movementType = ScrollRect.MovementType.Clamped;
                scrollRect.scrollSensitivity = 25f;
                scrollRect.inertia = false;

                var vp = UIHelpers.Obj("VP", scrollObj.transform);
                UIHelpers.Fill(UIHelpers.RT(vp));
                vp.AddComponent<Image>().color = new Color(0, 0, 0, 0.01f);
                vp.AddComponent<Mask>().showMaskGraphic = true;
                scrollRect.viewport = UIHelpers.RT(vp);

                var content = UIHelpers.Obj("Content", vp.transform);
                var crt = UIHelpers.RT(content);
                crt.anchorMin = new Vector2(0, 1); crt.anchorMax = new Vector2(1, 1);
                crt.pivot = new Vector2(0.5f, 1);
                crt.sizeDelta = new Vector2(0, 0);
                scrollRect.content = crt;

                var csf = content.AddComponent<ContentSizeFitter>();
                csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                var vlg = content.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = UIHelpers.RowGap;
                vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;

                // All rows go into scrollable content
                var pg6 = content.transform;

                // ── MOVEMENT ─────────────────────────────────────────
                UIHelpers.SectionHeader("MOVEMENT", pg6);

                // Rotation Speed
                var sr = UIHelpers.StatRow("Rotation Speed", pg6);
                spinTogVal = UIHelpers.Txt("SpTV", sr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                spinTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(sr.transform, "SpT", () => { Movement.ToggleSpin(); RefreshAll(); }, out spinTrack, out spinKnob);
                spinBar = UIHelpers.MakeBar("SpB", sr.transform, (Movement.SpinLevel - 1) / 9f);
                spinVal = UIHelpers.Txt("SpV", sr.transform, Movement.SpinLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                spinVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(sr.transform, "-", () => { Movement.SpinDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(sr.transform, "+", () => { Movement.SpinIncrease(); RefreshAll(); });

                // Hop Force
                var hr = UIHelpers.StatRow("Hop Force", pg6);
                hopTogVal = UIHelpers.Txt("HpTV", hr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                hopTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(hr.transform, "HpT", () => { Movement.ToggleHop(); RefreshAll(); }, out hopTrack, out hopKnob);
                hopBar = UIHelpers.MakeBar("HpB", hr.transform, (Movement.HopLevel - 1) / 9f);
                hopVal = UIHelpers.Txt("HpV", hr.transform, Movement.HopLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                hopVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(hr.transform, "-", () => { Movement.HopDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(hr.transform, "+", () => { Movement.HopIncrease(); RefreshAll(); });

                // Wheelie Force
                var wr = UIHelpers.StatRow("Wheelie Force", pg6);
                wheelieTogVal = UIHelpers.Txt("WlTV", wr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                wheelieTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(wr.transform, "WlT", () => { Movement.ToggleWheelie(); RefreshAll(); }, out wheelieTrack, out wheelieKnob);
                wheelieBar = UIHelpers.MakeBar("WlB", wr.transform, (Movement.WheelieLevel - 1) / 9f);
                wheelieVal = UIHelpers.Txt("WlV", wr.transform, Movement.WheelieLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                wheelieVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(wr.transform, "-", () => { Movement.WheelieDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(wr.transform, "+", () => { Movement.WheelieIncrease(); RefreshAll(); });

                // Lean Strength
                var lr = UIHelpers.StatRow("Lean Strength", pg6);
                leanTogVal = UIHelpers.Txt("LnTV", lr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                leanTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(lr.transform, "LnT", () => { Movement.ToggleLean(); RefreshAll(); }, out leanTrack, out leanKnob);
                leanBar = UIHelpers.MakeBar("LnB", lr.transform, (Movement.LeanLevel - 1) / 9f);
                leanVal = UIHelpers.Txt("LnV", lr.transform, Movement.LeanLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                leanVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(lr.transform, "-", () => { Movement.LeanDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(lr.transform, "+", () => { Movement.LeanIncrease(); RefreshAll(); });

                UIHelpers.Divider(pg6);

                // ── BALANCE & PHYSICS ─────────────────────────────────
                UIHelpers.SectionHeader("BALANCE & PHYSICS", pg6);

                // Wheelie Angle Limit
                var wbr = UIHelpers.StatRow("Wheelie Angle Limit", pg6);
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
                UIHelpers.InfoBox(pg6, "Caps pitch angle in a wheelie. Lower = tighter cap.");

                // Air Control
                var iacr = UIHelpers.StatRow("Air Control", pg6);
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
                UIHelpers.InfoBox(pg6, "Damps rotation while airborne. Higher = more stable.");

                // Pump Strength
                var fbr = UIHelpers.StatRow("Pump Strength", pg6);
                psBar = UIHelpers.MakeBar("PsB", fbr.transform, (GameModifierMods.PumpStrengthLevel - 1) / 9f);
                psVal = UIHelpers.Txt("PsV", fbr.transform, GameModifierMods.PumpStrengthLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                psVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(fbr.transform, "-", () => { GameModifierMods.PumpStrengthDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(fbr.transform, "+", () => { GameModifierMods.PumpStrengthIncrease(); RefreshAll(); });

                // Near Miss Sensitivity
                var nmr = UIHelpers.StatRow("Near Miss Sensitivity", pg6);
                _nmBar = UIHelpers.MakeBar("NmB", nmr.transform, (NearMissSensitivity.Level - 1) / 9f);
                _nmVal = UIHelpers.Txt("NmV", nmr.transform, NearMissSensitivity.DisplayValue, 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                _nmVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                _nmTogVal = UIHelpers.Txt("NmTV", nmr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _nmTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                Image nmTrack; RectTransform nmKnob;
                UIHelpers.Toggle(nmr.transform, "NmT", () => { NearMissSensitivity.Toggle(); RefreshAll(); }, out nmTrack, out nmKnob);
                UIHelpers.SmallBtn(nmr.transform, "-", () => { NearMissSensitivity.Decrease(); RefreshAll(); });
                UIHelpers.SmallBtn(nmr.transform, "+", () => { NearMissSensitivity.Increase(); RefreshAll(); });
                _nmTrack = nmTrack; _nmKnob = nmKnob;
                UIHelpers.InfoBox(pg6, "Level 5 = default. Lower = must get closer. Level 10 = near misses everywhere.");

                UIHelpers.Divider(pg6);

                // ── CENTER OF MASS ─────────────────────────────────────
                UIHelpers.SectionHeader("CENTER OF MASS", pg6);

                // Left / Right (X axis)
                var comLRr = UIHelpers.StatRow("Left / Right", pg6);
                _comLRBar = UIHelpers.MakeBar("CLrB", comLRr.transform, CenterOfMass.BarLR);
                _comLRVal = UIHelpers.Txt("CLrV", comLRr.transform, CenterOfMass.DisplayLR, 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                _comLRVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 36;
                UIHelpers.ActionBtn(comLRr.transform, "0", () => { CenterOfMass.ResetLR(); RefreshAll(); }, 22);
                UIHelpers.SmallBtn(comLRr.transform, "-", () => { CenterOfMass.DecreaseLR(); RefreshAll(); });
                UIHelpers.SmallBtn(comLRr.transform, "+", () => { CenterOfMass.IncreaseLR(); RefreshAll(); });

                // Forward / Back (Z axis)
                var comFBr = UIHelpers.StatRow("Forward / Back", pg6);
                _comFBBar = UIHelpers.MakeBar("CFbB", comFBr.transform, CenterOfMass.BarFB);
                _comFBVal = UIHelpers.Txt("CFbV", comFBr.transform, CenterOfMass.DisplayFB, 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                _comFBVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 36;
                UIHelpers.ActionBtn(comFBr.transform, "0", () => { CenterOfMass.ResetFB(); RefreshAll(); }, 22);
                UIHelpers.SmallBtn(comFBr.transform, "-", () => { CenterOfMass.DecreaseFB(); RefreshAll(); });
                UIHelpers.SmallBtn(comFBr.transform, "+", () => { CenterOfMass.IncreaseFB(); RefreshAll(); });

                // Up / Down (Y axis)
                var comUDr = UIHelpers.StatRow("Up / Down", pg6);
                _comUDBar = UIHelpers.MakeBar("CUdB", comUDr.transform, CenterOfMass.BarUD);
                _comUDVal = UIHelpers.Txt("CUdV", comUDr.transform, CenterOfMass.DisplayUD, 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                _comUDVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 36;
                UIHelpers.ActionBtn(comUDr.transform, "0", () => { CenterOfMass.ResetUD(); RefreshAll(); }, 22);
                UIHelpers.SmallBtn(comUDr.transform, "-", () => { CenterOfMass.DecreaseUD(); RefreshAll(); });
                UIHelpers.SmallBtn(comUDr.transform, "+", () => { CenterOfMass.IncreaseUD(); RefreshAll(); });

                UIHelpers.InfoBox(pg6, "Shifts the bike's balance point. All axes start at 0. + moves right / forward / up. Press 0 to reset that axis.");

                UIHelpers.AddScrollForwarders(pg6);
            }
            catch (System.Exception ex) { MelonLogger.Error("Page6UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        public static void RefreshAll()
        {
            // ── Rotation Speed ────────────────────────────────────────
            bool spOn = Movement.SpinEnabled;
            if (spinTogVal) { spinTogVal.text = spOn ? "ON" : "OFF"; spinTogVal.color = spOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(spinTrack, spinKnob, spOn);
            if (spinVal) spinVal.text = Movement.SpinLevel.ToString();
            UIHelpers.SetBar(spinBar, (Movement.SpinLevel - 1) / 9f);

            // ── Hop Force ─────────────────────────────────────────────
            bool hpOn = Movement.HopEnabled;
            if (hopTogVal) { hopTogVal.text = hpOn ? "ON" : "OFF"; hopTogVal.color = hpOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
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
            if (leanTogVal) { leanTogVal.text = lnOn ? "ON" : "OFF"; leanTogVal.color = lnOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
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

            // ── Center of Mass ────────────────────────────────────────
            if (_comLRVal) _comLRVal.text = CenterOfMass.DisplayLR;
            if (_comFBVal) _comFBVal.text = CenterOfMass.DisplayFB;
            if (_comUDVal) _comUDVal.text = CenterOfMass.DisplayUD;
            UIHelpers.SetBar(_comLRBar, CenterOfMass.BarLR);
            UIHelpers.SetBar(_comFBBar, CenterOfMass.BarFB);
            UIHelpers.SetBar(_comUDBar, CenterOfMass.BarUD);

            // Near Miss Sensitivity
            bool nmOn = NearMissSensitivity.Enabled;
            if (_nmTogVal) { _nmTogVal.text = nmOn ? "ON" : "OFF"; _nmTogVal.color = nmOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_nmTrack, _nmKnob, nmOn);
            if (_nmVal) _nmVal.text = NearMissSensitivity.DisplayValue;
            UIHelpers.SetBar(_nmBar, (NearMissSensitivity.Level - 1) / 9f);
        }
    }
}