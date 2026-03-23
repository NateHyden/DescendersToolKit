using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using DescendersModMenu.BikeStats;
using DescendersModMenu.Mods;

namespace DescendersModMenu.UI
{
    public static class MenuWindow
    {
        private static Text accelVal, msVal, landVal, bailVal, bikeVal, fovVal;
        private static Image accelBar, msBar, landBar, fovBar;
        private static Image bailTrack; private static RectTransform bailKnob;
        private static Text slowVal;
        private static Image slowTrack; private static RectTransform slowKnob;
        private static Image capBg, capBdr; private static Text capTxt;

        private static GameObject pg1, pg2, pg3, pg4, pg5, pg6, botBar;
        private static Text[] tabTx = new Text[6];
        private static GameObject[] tabLn = new GameObject[6];
        private static int cur = 1;

        public static CanvasGroup RootCanvasGroup { get; private set; }

        public static GameObject CreateMenu()
        {
            try
            {
                if (UIHelpers.GetFont() == null) { MelonLogger.Error("Font null"); return null; }
                cur = 1;

                var cv = new GameObject("DescendersMenu");
                var c = cv.AddComponent<Canvas>(); c.renderMode = RenderMode.ScreenSpaceOverlay; c.sortingOrder = 999;
                var cs = cv.AddComponent<CanvasScaler>();
                cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                cs.referenceResolution = new Vector2(1920, 1080);
                cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight; cs.matchWidthOrHeight = 0.5f;
                cv.AddComponent<GraphicRaycaster>();

                // Root
                var root = UIHelpers.Obj("Root", cv.transform);
                UIHelpers.Pin(UIHelpers.RT(root), new Vector2(.5f, .5f), new Vector2(.5f, .5f),
                    Vector2.zero, new Vector2(UIHelpers.WinW, UIHelpers.WinH));
                RootCanvasGroup = root.AddComponent<CanvasGroup>();

                // Window panel
                var win = UIHelpers.Panel("Win", root.transform, UIHelpers.WinPanel, UIHelpers.WinSp);
                UIHelpers.Fill(UIHelpers.RT(win));
                win.AddComponent<Mask>().showMaskGraphic = true;  // clips children to rounded shape
                var wbd = UIHelpers.Panel("WBd", win.transform, UIHelpers.WinBorder, UIHelpers.WinSp);
                wbd.GetComponent<Image>().raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(wbd));
                wbd.AddComponent<LayoutElement>().ignoreLayout = true;

                float y = 0;

                // ── Header ──────────────────────────────────────────────
                var hdr = UIHelpers.Panel("Hdr", win.transform, UIHelpers.HeaderBg);
                var hrt = UIHelpers.RT(hdr);
                hrt.anchorMin = new Vector2(0, 1); hrt.anchorMax = new Vector2(1, 1);
                hrt.pivot = new Vector2(.5f, 1); hrt.sizeDelta = new Vector2(0, UIHelpers.HeaderH);
                hrt.anchoredPosition = Vector2.zero;

                // Header bottom border
                var hln = UIHelpers.Panel("HLn", hdr.transform, UIHelpers.WinBorder);
                var hlrt = UIHelpers.RT(hln);
                hlrt.anchorMin = new Vector2(0, 0); hlrt.anchorMax = new Vector2(1, 0);
                hlrt.pivot = new Vector2(.5f, 0); hlrt.sizeDelta = new Vector2(0, 1); hlrt.anchoredPosition = Vector2.zero;

                var title = UIHelpers.Txt("T", hdr.transform, "DESCENDERS MOD MENU", 14,
                    FontStyle.Bold, TextAnchor.MiddleLeft, new Color(0.933f, 0.933f, 0.933f, 1f));
                var trt = UIHelpers.RT(title.gameObject);
                trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
                trt.offsetMin = new Vector2(18, 0); trt.offsetMax = new Vector2(-100, 0);

                var by = UIHelpers.Txt("By", hdr.transform, "v1.2.0 \u2014 Natehyden", 10,
                    FontStyle.Normal, TextAnchor.MiddleRight, UIHelpers.TextDim);
                var brt = UIHelpers.RT(by.gameObject);
                brt.anchorMin = Vector2.zero; brt.anchorMax = Vector2.one;
                brt.offsetMin = Vector2.zero; brt.offsetMax = new Vector2(-14, 0);

                y = UIHelpers.HeaderH;

                // ── Tabs ────────────────────────────────────────────────
                var tb = UIHelpers.Obj("Tabs", win.transform);
                var tbrt = UIHelpers.RT(tb);
                tbrt.anchorMin = new Vector2(0, 1); tbrt.anchorMax = new Vector2(1, 1);
                tbrt.pivot = new Vector2(.5f, 1); tbrt.sizeDelta = new Vector2(0, UIHelpers.TabH);
                tbrt.anchoredPosition = new Vector2(0, -y);

                // bottom line
                var tbl = UIHelpers.Panel("TBL", tb.transform, UIHelpers.WinBorder);
                var tblrt = UIHelpers.RT(tbl);
                tblrt.anchorMin = new Vector2(0, 0); tblrt.anchorMax = new Vector2(1, 0);
                tblrt.pivot = new Vector2(.5f, 0); tblrt.sizeDelta = new Vector2(0, 1); tblrt.anchoredPosition = Vector2.zero;

                string[] names = { "Stats", "ESP", "Tools", "Unlock", "Score", "Move" };
                float[] anch = { 0f, .1667f, .3333f, .5f, .6667f, .8333f, 1f };
                for (int i = 0; i < 6; i++)
                {
                    int pi = i + 1;
                    var t = UIHelpers.Obj("T" + i, tb.transform);
                    t.AddComponent<Image>().color = new Color(0, 0, 0, 0);
                    var btn = t.AddComponent<Button>(); btn.onClick.AddListener(() => Switch(pi));
                    var tcb = btn.colors; tcb.normalColor = Color.white; tcb.highlightedColor = Color.white;
                    tcb.pressedColor = Color.white; tcb.colorMultiplier = 1; btn.colors = tcb;
                    var tr = UIHelpers.RT(t);
                    tr.anchorMin = new Vector2(anch[i], 0); tr.anchorMax = new Vector2(anch[i + 1], 1);
                    tr.offsetMin = Vector2.zero; tr.offsetMax = Vector2.zero;

                    tabTx[i] = UIHelpers.Txt("TT", t.transform, names[i], 11,
                        FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextDim);
                    UIHelpers.Fill(UIHelpers.RT(tabTx[i].gameObject));

                    tabLn[i] = UIHelpers.Panel("TLn", t.transform, new Color(0, 0, 0, 0));
                    var tlr = UIHelpers.RT(tabLn[i]);
                    tlr.anchorMin = new Vector2(0, 0); tlr.anchorMax = new Vector2(1, 0);
                    tlr.pivot = new Vector2(.5f, 0); tlr.sizeDelta = new Vector2(0, 2); tlr.anchoredPosition = Vector2.zero;
                }

                y += UIHelpers.TabH;

                // ── Content ─────────────────────────────────────────────
                float cH = UIHelpers.WinH - y - UIHelpers.BottomH - 10;
                var cont = UIHelpers.Obj("Cnt", win.transform);
                var crt = UIHelpers.RT(cont);
                crt.anchorMin = new Vector2(0, 1); crt.anchorMax = new Vector2(1, 1);
                crt.pivot = new Vector2(.5f, 1); crt.sizeDelta = new Vector2(0, cH);
                crt.anchoredPosition = new Vector2(0, -y);

                pg1 = UIHelpers.Obj("P1", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg1));
                BuildPage1(pg1.transform);

                pg2 = UIHelpers.Obj("P2", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg2));
                Page2UI.CreatePage(pg2.transform);

                pg3 = UIHelpers.Obj("P3", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg3));
                Page3UI.CreatePage(pg3.transform);

                pg4 = UIHelpers.Obj("P4", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg4));
                Page4UI.CreatePage(pg4.transform);

                pg5 = UIHelpers.Obj("P5", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg5));
                Page5UI.CreatePage(pg5.transform);

                pg6 = UIHelpers.Obj("P6", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg6));
                Page6UI.CreatePage(pg6.transform);

                // ── Bottom bar ──────────────────────────────────────────
                botBar = UIHelpers.Obj("Bot", win.transform);
                var bbrt = UIHelpers.RT(botBar);
                bbrt.anchorMin = new Vector2(0, 0); bbrt.anchorMax = new Vector2(1, 0);
                bbrt.pivot = new Vector2(.5f, 0); bbrt.sizeDelta = new Vector2(0, UIHelpers.BottomH);
                bbrt.anchoredPosition = new Vector2(0, 6);
                var bhlg = botBar.AddComponent<HorizontalLayoutGroup>();
                bhlg.spacing = 5; bhlg.padding = new RectOffset(14, 14, 0, 0);
                bhlg.childAlignment = TextAnchor.MiddleCenter;
                bhlg.childForceExpandWidth = true; bhlg.childForceExpandHeight = true;

                BotBtn("Save", botBar.transform, UIHelpers.Accent, () => StatsManager.SaveStats());
                BotBtn("Load", botBar.transform, UIHelpers.Accent, () => { StatsManager.LoadStats(); RefreshAll(); });
                BotBtn("Reset", botBar.transform, UIHelpers.OffColor, () => { StatsManager.ResetStats(); RefreshAll(); });

                RefreshAll(); RefreshTabs();
                cv.SetActive(false);
                return cv;
            }
            catch (System.Exception ex) { MelonLogger.Error("CreateMenu: " + ex); return null; }
        }

        // =====================================================================
        //  PAGE 1
        // =====================================================================

        private static void BuildPage1(Transform p)
        {
            var vlg = p.gameObject.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = UIHelpers.RowGap;
            vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;

            // Acceleration
            var ar = UIHelpers.StatRow("Acceleration", p);
            accelBar = UIHelpers.MakeBar("AB", ar.transform, Acceleration.Level / 10f);
            accelVal = UIHelpers.Txt("AV", ar.transform, Acceleration.Level.ToString(), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            var avle = accelVal.gameObject.AddComponent<LayoutElement>();
            avle.preferredWidth = 18; avle.preferredHeight = 20; avle.flexibleHeight = 0;
            UIHelpers.SmallBtn(ar.transform, "-", () => { Acceleration.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(ar.transform, "+", () => { Acceleration.Increase(); RefreshAll(); });

            // Max Speed (compound row)
            var mso = UIHelpers.Panel("MSR", p, UIHelpers.RowBg, UIHelpers.RowSp);
            mso.AddComponent<LayoutElement>().minHeight = UIHelpers.RowH + 38;
            var mbd = UIHelpers.Panel("MBd", mso.transform, UIHelpers.RowBorder, UIHelpers.RowSp);
            mbd.GetComponent<Image>().raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(mbd));
            mbd.AddComponent<LayoutElement>().ignoreLayout = true;

            var mvlg = mso.AddComponent<VerticalLayoutGroup>();
            mvlg.spacing = 4; mvlg.padding = new RectOffset((int)UIHelpers.RowPad, (int)UIHelpers.RowPad, 6, 8);
            mvlg.childAlignment = TextAnchor.UpperCenter;
            mvlg.childForceExpandWidth = true; mvlg.childForceExpandHeight = false;

            // Top strip
            var mst = UIHelpers.Obj("MST", mso.transform);
            mst.AddComponent<LayoutElement>().preferredHeight = 28;
            var mhlg = mst.AddComponent<HorizontalLayoutGroup>();
            mhlg.spacing = 8; mhlg.childAlignment = TextAnchor.MiddleCenter;
            mhlg.childForceExpandWidth = false; mhlg.childForceExpandHeight = false;

            var msll = UIHelpers.Txt("MSL", mst.transform, "Max Speed", 12, FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.TextLight)
                .gameObject.AddComponent<LayoutElement>();
            msll.flexibleWidth = 1; msll.preferredHeight = 28;
            msBar = UIHelpers.MakeBar("MSB", mst.transform, MaxSpeedMultiplier.Level / 10f);
            msVal = UIHelpers.Txt("MSV", mst.transform, MaxSpeedMultiplier.Level.ToString(), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            var mvle = msVal.gameObject.AddComponent<LayoutElement>();
            mvle.preferredWidth = 18; mvle.preferredHeight = 20; mvle.flexibleHeight = 0;
            UIHelpers.SmallBtn(mst.transform, "-", () => { MaxSpeedMultiplier.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(mst.transform, "+", () => { MaxSpeedMultiplier.Increase(); RefreshAll(); });

            // Big cap button
            var cap = UIHelpers.Obj("Cap", mso.transform);
            capBg = cap.AddComponent<Image>(); capBg.sprite = UIHelpers.BtnSp;
            capBg.type = Image.Type.Sliced; capBg.color = UIHelpers.AccentDim;
            var cbtn = cap.AddComponent<Button>();
            cbtn.onClick.AddListener(() => { NoSpeedCap.Toggle(); RefreshAll(); });
            var ccb = cbtn.colors; ccb.normalColor = Color.white; ccb.highlightedColor = new Color(1, 1, 1, 1.15f);
            ccb.pressedColor = new Color(.7f, .7f, .7f, 1); ccb.colorMultiplier = 1; ccb.fadeDuration = .08f;
            cbtn.colors = ccb;
            cap.AddComponent<LayoutElement>().preferredHeight = 30;

            var cbd = UIHelpers.Panel("CBd", cap.transform, UIHelpers.AccentBdr, UIHelpers.BtnSp);
            capBdr = cbd.GetComponent<Image>(); capBdr.raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(cbd));

            capTxt = UIHelpers.Txt("CT", cap.transform, "REMOVE SPEED CAP", 11,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
            UIHelpers.Fill(UIHelpers.RT(capTxt.gameObject));

            // Landing Impact
            var lr = UIHelpers.StatRow("Landing Impact", p);
            landBar = UIHelpers.MakeBar("LB", lr.transform, LandingImpact.Level / 10f);
            landVal = UIHelpers.Txt("LV", lr.transform, LandingImpact.Level.ToString(), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            var lvle = landVal.gameObject.AddComponent<LayoutElement>();
            lvle.preferredWidth = 18; lvle.preferredHeight = 20; lvle.flexibleHeight = 0;
            UIHelpers.SmallBtn(lr.transform, "-", () => { LandingImpact.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(lr.transform, "+", () => { LandingImpact.Increase(); RefreshAll(); });

            // No Bail
            var nr = UIHelpers.StatRow("No Bail", p);
            bailVal = UIHelpers.Txt("NV", nr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            var bvle = bailVal.gameObject.AddComponent<LayoutElement>();
            bvle.preferredWidth = 28; bvle.preferredHeight = 18; bvle.flexibleHeight = 0;
            UIHelpers.Toggle(nr.transform, "NT", () => { NoBail.Toggle(); RefreshAll(); }, out bailTrack, out bailKnob);

            // Bike
            var br = UIHelpers.StatRow("Bike", p);
            UIHelpers.SmallBtn(br.transform, "\u25C0", () => { BikeSwitcher.PreviousBike(); RefreshAll(); });
            bikeVal = UIHelpers.Txt("BV", br.transform, "Enduro", 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
            var bikle = bikeVal.gameObject.AddComponent<LayoutElement>();
            bikle.preferredWidth = 80; bikle.preferredHeight = 20; bikle.flexibleHeight = 0;
            UIHelpers.SmallBtn(br.transform, "\u25B6", () => { BikeSwitcher.NextBike(); RefreshAll(); });

            // FOV
            var fr = UIHelpers.StatRow("FOV", p);
            fovBar = UIHelpers.MakeBar("FB", fr.transform, (FOV.Level - 1) / 9f);
            fovVal = UIHelpers.Txt("FV", fr.transform, FOV.DisplayValue, 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            var fvle = fovVal.gameObject.AddComponent<LayoutElement>();
            fvle.preferredWidth = 26; fvle.preferredHeight = 20; fvle.flexibleHeight = 0;
            UIHelpers.SmallBtn(fr.transform, "-", () => { FOV.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(fr.transform, "+", () => { FOV.Increase(); RefreshAll(); });

            // Slow Motion
            var smr = UIHelpers.StatRow("Slow Motion", p);
            slowVal = UIHelpers.Txt("SMV", smr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            var smvle = slowVal.gameObject.AddComponent<LayoutElement>();
            smvle.preferredWidth = 28; smvle.preferredHeight = 18; smvle.flexibleHeight = 0;
            UIHelpers.Toggle(smr.transform, "SMT", () => { SlowMotion.Toggle(); RefreshAll(); }, out slowTrack, out slowKnob);
            var smHint = UIHelpers.Txt("SMH", smr.transform, "F2", 10, FontStyle.Normal, TextAnchor.MiddleRight, UIHelpers.TextDim);
            smHint.gameObject.AddComponent<LayoutElement>().preferredWidth = 22;
        }

        // =====================================================================

        private static void BotBtn(string lbl, Transform p, Color tc, UnityEngine.Events.UnityAction clk)
        {
            var g = UIHelpers.Obj(lbl + "B", p);
            var im = g.AddComponent<Image>(); im.sprite = UIHelpers.RowSp; im.type = Image.Type.Sliced; im.color = UIHelpers.RowBg;
            var b = g.AddComponent<Button>(); b.onClick.AddListener(clk);
            var cb = b.colors; cb.normalColor = Color.white; cb.highlightedColor = new Color(1, 1, 1, 1.15f);
            cb.pressedColor = new Color(.7f, .7f, .7f, 1); cb.colorMultiplier = 1; cb.fadeDuration = .08f; b.colors = cb;
            var bd = UIHelpers.Panel("Bd", g.transform, UIHelpers.RowBorder, UIHelpers.RowSp);
            bd.GetComponent<Image>().raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(bd));
            var t = UIHelpers.Txt("L", g.transform, lbl.ToUpper(), 10, FontStyle.Bold, TextAnchor.MiddleCenter, tc);
            UIHelpers.Fill(UIHelpers.RT(t.gameObject));
        }

        private static void Switch(int pg) { cur = pg; RefreshTabs(); }

        private static void RefreshTabs()
        {
            if (pg1) pg1.SetActive(cur == 1);
            if (pg2) pg2.SetActive(cur == 2);
            if (pg3) pg3.SetActive(cur == 3);
            if (pg4) pg4.SetActive(cur == 4);
            if (pg5) pg5.SetActive(cur == 5);
            if (pg6) pg6.SetActive(cur == 6);
            if (botBar) botBar.SetActive(cur == 1);
            for (int i = 0; i < 6; i++)
            {
                bool on = (i + 1) == cur;
                if (tabTx[i]) tabTx[i].color = on ? UIHelpers.Accent : UIHelpers.TextDim;
                if (tabLn[i]) tabLn[i].GetComponent<Image>().color = on ? UIHelpers.Accent : new Color(0, 0, 0, 0);
            }
            if (cur == 2) Page2UI.RefreshTexts();
        }

        public static void RefreshAll()
        {
            if (accelVal) accelVal.text = Acceleration.Level.ToString();
            UIHelpers.SetBar(accelBar, Acceleration.Level / 10f);
            if (msVal) msVal.text = MaxSpeedMultiplier.Level.ToString();
            UIHelpers.SetBar(msBar, MaxSpeedMultiplier.Level / 10f);
            if (landVal) landVal.text = LandingImpact.Level.ToString();
            UIHelpers.SetBar(landBar, LandingImpact.Level / 10f);

            bool bail = NoBail.Enabled;
            if (bailVal) { bailVal.text = bail ? "ON" : "OFF"; bailVal.color = bail ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(bailTrack, bailKnob, bail);

            bool cap = NoSpeedCap.Enabled;
            if (capTxt)
            {
                capTxt.text = cap ? "SPEED CAP REMOVED" : "REMOVE SPEED CAP";
                capTxt.color = cap ? UIHelpers.OnColor : UIHelpers.Accent;
            }
            if (capBg) capBg.color = cap ? UIHelpers.OnBg : UIHelpers.AccentDim;
            if (capBdr) capBdr.color = cap ? UIHelpers.OnBdr : UIHelpers.AccentBdr;

            if (bikeVal)
            {
                switch (BikeSwitcher.CurrentBikeIndex)
                {
                    case 0: bikeVal.text = "Enduro"; break;
                    case 1: bikeVal.text = "Downhill"; break;
                    case 2: bikeVal.text = "Hardtail"; break;
                    case 3: bikeVal.text = "BRNZL Enduro"; break;
                    default: bikeVal.text = "Unknown"; break;
                }
            }

            if (fovVal) fovVal.text = FOV.DisplayValue;
            UIHelpers.SetBar(fovBar, (FOV.Level - 1) / 9f);

            bool slow = SlowMotion.Enabled;
            if (slowVal) { slowVal.text = slow ? "ON" : "OFF"; slowVal.color = slow ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(slowTrack, slowKnob, slow);
        }
    }
}