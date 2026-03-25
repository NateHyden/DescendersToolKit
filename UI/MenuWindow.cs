using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using DescendersModMenu.BikeStats;
using DescendersModMenu.Mods;

namespace DescendersModMenu.UI
{
    public static class MenuWindow
    {
        // Page 1 fields (Stats tab - built inline)
        private static Text accelVal, msVal, landVal, bailVal, bikeVal, fovVal;
        private static Image accelBar, msBar, landBar, fovBar;
        private static Image bailTrack; private static RectTransform bailKnob;
        private static Text slowVal;
        private static Image slowTrack; private static RectTransform slowKnob;
        private static Text topSpeedVal;
        private static Text sessionTimeVal, bailCountVal, airtimeVal;
        private static Image capBg, capBdr; private static Text capTxt;

        private static GameObject pg1, pg2, pg3, pg4, pg5, pg6, pg7, pg8, pg9;
        private static GameObject botBar;
        private static int cur = 1;

        // Sidebar nav — maps nav index to page number
        // Groups: BIKE(Stats,Move,Bike) | WORLD(World,Silly) | TOOLS(ESP,Score,Unlock) | SYSTEM(Info)
        private static readonly int[] PageOrder = { 1, 6, 8, 7, 9, 2, 5, 4, 3 };
        private static readonly string[] NavLabels = { "Stats", "Move", "Bike", "World", "Silly", "ESP", "Score", "Unlock", "Info" };
        private static readonly string[] GroupLabels = { "BIKE", null, null, "WORLD", null, "TOOLS", null, null, "SYSTEM" };

        private static Image[] _navBars = new Image[9];
        private static Text[] _navTxts = new Text[9];
        private static Image[] _navBgs = new Image[9];
        private static UnityEngine.UI.Image _infoTabDot;

        public static CanvasGroup RootCanvasGroup { get; private set; }

        public static GameObject CreateMenu()
        {
            try
            {
                if (UIHelpers.GetFont() == null) { MelonLogger.Error("Font null"); return null; }
                cur = 1;

                // Canvas
                var cv = new GameObject("DescendersMenu");
                var c = cv.AddComponent<Canvas>();
                c.renderMode = RenderMode.ScreenSpaceOverlay; c.sortingOrder = 999;
                var cs = cv.AddComponent<CanvasScaler>();
                cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                cs.referenceResolution = new Vector2(1920, 1080);
                cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                cs.matchWidthOrHeight = 0.5f;
                cv.AddComponent<GraphicRaycaster>();

                // Root
                var root = UIHelpers.Obj("Root", cv.transform);
                UIHelpers.Pin(UIHelpers.RT(root), new Vector2(.5f, .5f), new Vector2(.5f, .5f),
                    Vector2.zero, new Vector2(UIHelpers.WinW, UIHelpers.WinH));
                RootCanvasGroup = root.AddComponent<CanvasGroup>();

                // Window
                var win = UIHelpers.Panel("Win", root.transform, UIHelpers.WinPanel, UIHelpers.WinSp);
                UIHelpers.Fill(UIHelpers.RT(win));
                win.AddComponent<Mask>().showMaskGraphic = true;
                var wbd = UIHelpers.Panel("WBd", win.transform, UIHelpers.WinBorder, UIHelpers.WinSp);
                wbd.GetComponent<Image>().raycastTarget = false;
                UIHelpers.Fill(UIHelpers.RT(wbd));
                wbd.AddComponent<LayoutElement>().ignoreLayout = true;

                // ── Header ────────────────────────────────────────────────
                var hdr = UIHelpers.Panel("Hdr", win.transform, UIHelpers.HeaderBg);
                var hrt = UIHelpers.RT(hdr);
                hrt.anchorMin = new Vector2(0, 1); hrt.anchorMax = new Vector2(1, 1);
                hrt.pivot = new Vector2(.5f, 1);
                hrt.sizeDelta = new Vector2(0, UIHelpers.HeaderH);
                hrt.anchoredPosition = Vector2.zero;

                // Orange accent line at bottom of header
                var hAcc = UIHelpers.Panel("HAcc", hdr.transform, UIHelpers.Accent);
                var hart = UIHelpers.RT(hAcc);
                hart.anchorMin = new Vector2(0, 0); hart.anchorMax = new Vector2(1, 0);
                hart.pivot = new Vector2(.5f, 0); hart.sizeDelta = new Vector2(0, 2);
                hart.anchoredPosition = Vector2.zero;

                // Sidebar divider line (vertical)
                var sdiv = UIHelpers.Panel("SDv", hdr.transform, UIHelpers.WinBorder);
                var sdrt = UIHelpers.RT(sdiv);
                sdrt.anchorMin = new Vector2(0, 0); sdrt.anchorMax = new Vector2(0, 1);
                sdrt.pivot = new Vector2(0, .5f);
                sdrt.offsetMin = new Vector2(UIHelpers.SidebarW, 0);
                sdrt.offsetMax = new Vector2(UIHelpers.SidebarW + 1, 0);

                // Title
                var title = UIHelpers.Txt("T", hdr.transform, "DESCENDERS", 15,
                    FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.TextLight);
                var trt = UIHelpers.RT(title.gameObject);
                trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
                trt.offsetMin = new Vector2(UIHelpers.SidebarW + 14, 0);
                trt.offsetMax = new Vector2(-200, 0);

                // Orange subtitle — same size as title
                var sub = UIHelpers.Txt("Sub", hdr.transform, "MOD MENU", 15,
                    FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.Accent);
                var subrt = UIHelpers.RT(sub.gameObject);
                subrt.anchorMin = Vector2.zero; subrt.anchorMax = Vector2.one;
                subrt.offsetMin = new Vector2(UIHelpers.SidebarW + 152, 0);
                subrt.offsetMax = new Vector2(-100, 0);

                // ── Header text ──────────────────────────────────────────
                // v2.0.0 — left side inside sidebar area
                var ver = UIHelpers.Txt("V", hdr.transform, "v2.1.0", 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextDim);
                var vrt = UIHelpers.RT(ver.gameObject);
                vrt.anchorMin = new Vector2(0f, 0f); vrt.anchorMax = new Vector2(0f, 1f);
                vrt.pivot = new Vector2(0f, 0.5f);
                vrt.offsetMin = new Vector2(0f, 0f); vrt.offsetMax = new Vector2(UIHelpers.SidebarW, 0f);

                // By Natehyden — right aligned
                var byTxt = UIHelpers.Txt("By", hdr.transform, "By Natehyden", 10,
                    FontStyle.Normal, TextAnchor.MiddleRight, UIHelpers.TextDim);
                var byrt = UIHelpers.RT(byTxt.gameObject);
                byrt.anchorMin = Vector2.zero; byrt.anchorMax = Vector2.one;
                byrt.offsetMin = Vector2.zero; byrt.offsetMax = new Vector2(-14, 0f);

                // ── Body (sidebar + content, below header) ────────────────
                var body = UIHelpers.Obj("Body", win.transform);
                var bodyRT = UIHelpers.RT(body);
                bodyRT.anchorMin = Vector2.zero; bodyRT.anchorMax = Vector2.one;
                bodyRT.offsetMin = new Vector2(0, 0);
                bodyRT.offsetMax = new Vector2(0, -UIHelpers.HeaderH);

                // ── Sidebar ───────────────────────────────────────────────
                var sidebar = UIHelpers.Panel("Sidebar", body.transform, UIHelpers.SidebarBg);
                var sibRT = UIHelpers.RT(sidebar);
                sibRT.anchorMin = Vector2.zero; sibRT.anchorMax = new Vector2(0, 1);
                sibRT.offsetMin = Vector2.zero; sibRT.offsetMax = new Vector2(UIHelpers.SidebarW, 0);

                var sVlg = sidebar.AddComponent<VerticalLayoutGroup>();
                sVlg.spacing = 1; sVlg.padding = new RectOffset(0, 0, 6, 6);
                sVlg.childAlignment = TextAnchor.UpperCenter;
                sVlg.childForceExpandWidth = true; sVlg.childForceExpandHeight = false;

                // Sidebar border right
                var sbdr = UIHelpers.Panel("SBdr", body.transform, UIHelpers.WinBorder);
                var sbdrt = UIHelpers.RT(sbdr);
                sbdrt.anchorMin = Vector2.zero; sbdrt.anchorMax = new Vector2(0, 1);
                sbdrt.pivot = new Vector2(0, .5f);
                sbdrt.offsetMin = new Vector2(UIHelpers.SidebarW, 0);
                sbdrt.offsetMax = new Vector2(UIHelpers.SidebarW + 1, 0);

                for (int i = 0; i < 9; i++)
                {
                    int navIdx = i;
                    int pageNum = PageOrder[i];

                    // Nav item
                    var item = UIHelpers.Obj("Nav" + i, sidebar.transform);
                    var ile = item.AddComponent<LayoutElement>();
                    ile.preferredHeight = 34; ile.minHeight = 34; ile.flexibleHeight = 0;

                    var bg = UIHelpers.Panel("Bg", item.transform, new Color(0, 0, 0, 0));
                    UIHelpers.Fill(UIHelpers.RT(bg));
                    _navBgs[i] = bg.GetComponent<Image>();

                    // Left orange active indicator bar
                    var bar = UIHelpers.Panel("Bar", item.transform, new Color(0, 0, 0, 0));
                    var barRT = UIHelpers.RT(bar);
                    barRT.anchorMin = Vector2.zero; barRT.anchorMax = new Vector2(0, 1);
                    barRT.pivot = new Vector2(0, .5f);
                    barRT.offsetMin = Vector2.zero; barRT.offsetMax = new Vector2(3, 0);
                    _navBars[i] = bar.GetComponent<Image>();

                    var lbl = UIHelpers.Txt("L", item.transform, NavLabels[i], 11,
                        FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.TextDim);
                    var lblRT = UIHelpers.RT(lbl.gameObject);
                    lblRT.anchorMin = Vector2.zero; lblRT.anchorMax = Vector2.one;
                    lblRT.offsetMin = new Vector2(16, 0); lblRT.offsetMax = Vector2.zero;
                    _navTxts[i] = lbl;

                    // Info dot on Info tab - proper circle
                    if (pageNum == 3)
                    {
                        var dotObj = UIHelpers.Obj("InfoDot", item.transform);
                        var dotImg = dotObj.AddComponent<UnityEngine.UI.Image>();
                        dotImg.sprite = UIHelpers.DotSp;
                        dotImg.type = UnityEngine.UI.Image.Type.Simple;
                        dotImg.color = UIHelpers.OnColor;
                        _infoTabDot = dotImg;
                        var drt = UIHelpers.RT(dotObj);
                        drt.anchorMin = new Vector2(1f, 0.5f);
                        drt.anchorMax = new Vector2(1f, 0.5f);
                        drt.pivot = new Vector2(1f, 0.5f);
                        drt.sizeDelta = new Vector2(7, 7);
                        drt.anchoredPosition = new Vector2(-10, 0);
                        dotObj.AddComponent<LayoutElement>().ignoreLayout = true;
                    }

                    // Button
                    var btn = item.AddComponent<Button>();
                    btn.onClick.AddListener(() => Switch(PageOrder[navIdx]));
                    btn.targetGraphic = bg.GetComponent<Image>();
                    var bcol = btn.colors;
                    bcol.normalColor = Color.white;
                    bcol.highlightedColor = new Color(1.08f, 1.08f, 1.08f, 1);
                    bcol.pressedColor = new Color(0.85f, 0.85f, 0.85f, 1);
                    bcol.colorMultiplier = 1; btn.colors = bcol;
                }

                // ── Content area ──────────────────────────────────────────
                var cont = UIHelpers.Obj("Cnt", body.transform);
                var crt = UIHelpers.RT(cont);
                crt.anchorMin = Vector2.zero; crt.anchorMax = Vector2.one;
                crt.offsetMin = new Vector2(UIHelpers.SidebarW + 1, UIHelpers.BottomH);
                crt.offsetMax = Vector2.zero;

                // Build pages
                pg1 = UIHelpers.Obj("P1", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg1)); BuildPage1(pg1.transform);
                pg2 = UIHelpers.Obj("P2", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg2)); Page2UI.CreatePage(pg2.transform);
                pg3 = UIHelpers.Obj("P3", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg3)); Page3UI.CreatePage(pg3.transform);
                pg4 = UIHelpers.Obj("P4", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg4)); Page4UI.CreatePage(pg4.transform);
                pg5 = UIHelpers.Obj("P5", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg5)); Page5UI.CreatePage(pg5.transform);
                pg6 = UIHelpers.Obj("P6", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg6)); Page6UI.CreatePage(pg6.transform);
                pg7 = UIHelpers.Obj("P7", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg7)); Page7UI.CreatePage(pg7.transform);
                pg8 = UIHelpers.Obj("P8", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg8)); Page8UI.CreatePage(pg8.transform);
                pg9 = UIHelpers.Obj("P9", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg9)); Page9UI.CreatePage(pg9.transform);

                // ── Bottom bar (Save/Load/Reset) ──────────────────────────
                botBar = UIHelpers.Obj("Bot", win.transform);
                var bbrt = UIHelpers.RT(botBar);
                bbrt.anchorMin = new Vector2(0, 0); bbrt.anchorMax = new Vector2(1, 0);
                bbrt.pivot = new Vector2(.5f, 0);
                bbrt.sizeDelta = new Vector2(0, UIHelpers.BottomH);
                bbrt.anchoredPosition = new Vector2(0, 0);
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

        // ── Page 1 (Stats) ────────────────────────────────────────────────
        private static void BuildPage1(Transform p)
        {
            var vlg = p.gameObject.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = UIHelpers.RowGap;
            vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;

            var ar = UIHelpers.StatRow("Acceleration", p);
            accelBar = UIHelpers.MakeBar("AB", ar.transform, Acceleration.Level / 10f);
            accelVal = UIHelpers.Txt("AV", ar.transform, Acceleration.Level.ToString(), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            accelVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
            UIHelpers.SmallBtn(ar.transform, "-", () => { Acceleration.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(ar.transform, "+", () => { Acceleration.Increase(); RefreshAll(); });

            // Max Speed compound row
            var mso = UIHelpers.Panel("MSR", p, UIHelpers.RowBg, UIHelpers.RowSp);
            mso.AddComponent<LayoutElement>().minHeight = UIHelpers.RowH + 38;
            var mbd = UIHelpers.Panel("MBd", mso.transform, UIHelpers.RowBorder, UIHelpers.RowSp);
            mbd.GetComponent<Image>().raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(mbd));
            mbd.AddComponent<LayoutElement>().ignoreLayout = true;
            var mvlg = mso.AddComponent<VerticalLayoutGroup>();
            mvlg.spacing = 4; mvlg.padding = new RectOffset((int)UIHelpers.RowPad, (int)UIHelpers.RowPad, 6, 8);
            mvlg.childAlignment = TextAnchor.UpperCenter;
            mvlg.childForceExpandWidth = true; mvlg.childForceExpandHeight = false;
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
            msVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
            UIHelpers.SmallBtn(mst.transform, "-", () => { MaxSpeedMultiplier.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(mst.transform, "+", () => { MaxSpeedMultiplier.Increase(); RefreshAll(); });
            var cap = UIHelpers.Obj("Cap", mso.transform);
            capBg = cap.AddComponent<Image>(); capBg.sprite = UIHelpers.BtnSp;
            capBg.type = Image.Type.Sliced; capBg.color = UIHelpers.Accent;
            var cbtn = cap.AddComponent<Button>();
            cbtn.onClick.AddListener(() => { NoSpeedCap.Toggle(); RefreshAll(); });
            var ccb = cbtn.colors; ccb.normalColor = Color.white; ccb.highlightedColor = new Color(1, 1, 1, 1.15f);
            ccb.pressedColor = new Color(.7f, .7f, .7f, 1); ccb.colorMultiplier = 1; ccb.fadeDuration = .08f;
            cbtn.colors = ccb;
            cap.AddComponent<LayoutElement>().preferredHeight = 30;
            var cbd = UIHelpers.Panel("CBd", cap.transform, UIHelpers.AccentBdr, UIHelpers.BtnSp);
            capBdr = cbd.GetComponent<Image>(); capBdr.raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(cbd));
            capTxt = UIHelpers.Txt("CT", cap.transform, "REMOVE SPEED CAP", 11,
                FontStyle.Bold, TextAnchor.MiddleCenter, Color.white);
            capTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
            UIHelpers.Fill(UIHelpers.RT(capTxt.gameObject));

            var lr = UIHelpers.StatRow("Landing Impact", p);
            landBar = UIHelpers.MakeBar("LB", lr.transform, LandingImpact.Level / 10f);
            landVal = UIHelpers.Txt("LV", lr.transform, LandingImpact.Level.ToString(), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            landVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
            UIHelpers.SmallBtn(lr.transform, "-", () => { LandingImpact.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(lr.transform, "+", () => { LandingImpact.Increase(); RefreshAll(); });

            var nr = UIHelpers.StatRow("No Bail", p);
            bailVal = UIHelpers.Txt("NV", nr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            bailVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(nr.transform, "NT", () => { NoBail.Toggle(); RefreshAll(); }, out bailTrack, out bailKnob);

            var br = UIHelpers.StatRow("Bike", p);
            UIHelpers.SmallBtn(br.transform, "\u25C0", () => { BikeSwitcher.PreviousBike(); RefreshAll(); });
            bikeVal = UIHelpers.Txt("BV", br.transform, "Enduro", 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
            bikeVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 80;
            UIHelpers.SmallBtn(br.transform, "\u25B6", () => { BikeSwitcher.NextBike(); RefreshAll(); });

            var fr = UIHelpers.StatRow("FOV", p);
            fovBar = UIHelpers.MakeBar("FB", fr.transform, (FOV.Level - 1) / 9f);
            fovVal = UIHelpers.Txt("FV", fr.transform, FOV.DisplayValue, 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            fovVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 26;
            UIHelpers.SmallBtn(fr.transform, "-", () => { FOV.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(fr.transform, "+", () => { FOV.Increase(); RefreshAll(); });

            var smr = UIHelpers.StatRow("Slow Motion", p);
            slowVal = UIHelpers.Txt("SMV", smr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            slowVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(smr.transform, "SMT", () => { SlowMotion.Toggle(); RefreshAll(); }, out slowTrack, out slowKnob);
            var smHint = UIHelpers.Txt("SMH", smr.transform, "F2", 10, FontStyle.Normal, TextAnchor.MiddleRight, UIHelpers.TextDim);
            smHint.gameObject.AddComponent<LayoutElement>().preferredWidth = 22;

            var tsr = UIHelpers.StatRow("Top Speed", p);
            topSpeedVal = UIHelpers.Txt("TSV", tsr.transform, TopSpeed.DisplayValue, 12,
                FontStyle.Bold, TextAnchor.MiddleRight, UIHelpers.Accent);
            var tsle = topSpeedVal.gameObject.AddComponent<LayoutElement>();
            tsle.flexibleWidth = 1; tsle.preferredHeight = 20; tsle.flexibleHeight = 0;
            UIHelpers.ActionBtn(tsr.transform, "Reset", () => { TopSpeed.Reset(); RefreshAll(); }, 52);

            UIHelpers.Divider(p);
            UIHelpers.SectionHeader("SESSION", p);

            var str = UIHelpers.StatRow("Session Timer", p);
            sessionTimeVal = UIHelpers.Txt("StV", str.transform, SessionTrackers.SessionTimeDisplay, 12,
                FontStyle.Bold, TextAnchor.MiddleRight, UIHelpers.Accent);
            var stle = sessionTimeVal.gameObject.AddComponent<LayoutElement>();
            stle.flexibleWidth = 1; stle.preferredHeight = 20; stle.flexibleHeight = 0;

            var bcr = UIHelpers.StatRow("Bails", p);
            bailCountVal = UIHelpers.Txt("BcV", bcr.transform, SessionTrackers.BailCountDisplay, 12,
                FontStyle.Bold, TextAnchor.MiddleRight, UIHelpers.Accent);
            var bcle = bailCountVal.gameObject.AddComponent<LayoutElement>();
            bcle.flexibleWidth = 1; bcle.preferredHeight = 20; bcle.flexibleHeight = 0;
            UIHelpers.ActionBtn(bcr.transform, "Reset", () => { SessionTrackers.ResetBails(); RefreshAll(); }, 52);

            var atr = UIHelpers.StatRow("Longest Airtime", p);
            airtimeVal = UIHelpers.Txt("AtV", atr.transform, SessionTrackers.AirtimeDisplay, 12,
                FontStyle.Bold, TextAnchor.MiddleRight, UIHelpers.Accent);
            var atle = airtimeVal.gameObject.AddComponent<LayoutElement>();
            atle.flexibleWidth = 1; atle.preferredHeight = 20; atle.flexibleHeight = 0;
            UIHelpers.ActionBtn(atr.transform, "Reset", () => { SessionTrackers.ResetAirtime(); RefreshAll(); }, 52);
        }

        // ── Bottom button ─────────────────────────────────────────────────
        private static void BotBtn(string lbl, Transform p, Color bg, UnityEngine.Events.UnityAction clk)
        {
            var g = UIHelpers.Obj(lbl + "B", p);
            var im = g.AddComponent<Image>(); im.sprite = UIHelpers.BtnSp; im.type = Image.Type.Sliced; im.color = bg;
            var b = g.AddComponent<Button>(); b.onClick.AddListener(clk);
            var cb = b.colors; cb.normalColor = Color.white; cb.highlightedColor = new Color(1, 1, 1, 1.15f);
            cb.pressedColor = new Color(.7f, .7f, .7f, 1); cb.colorMultiplier = 1; cb.fadeDuration = .08f; b.colors = cb;
            var t = UIHelpers.Txt("L", g.transform, lbl.ToUpper(), 10, FontStyle.Bold, TextAnchor.MiddleCenter, Color.white);
            UIHelpers.Fill(UIHelpers.RT(t.gameObject));
        }

        // ── Navigation ────────────────────────────────────────────────────
        private static void Switch(int pg) { cur = pg; RefreshTabs(); }

        private static void RefreshTabs()
        {
            if (pg1) pg1.SetActive(cur == 1);
            if (pg2) pg2.SetActive(cur == 2);
            if (pg3) pg3.SetActive(cur == 3);
            if (pg4) pg4.SetActive(cur == 4);
            if (pg5) pg5.SetActive(cur == 5);
            if (pg6) pg6.SetActive(cur == 6);
            if (pg7) pg7.SetActive(cur == 7);
            if (pg8) pg8.SetActive(cur == 8);
            if (pg9) pg9.SetActive(cur == 9);
            if (botBar) botBar.SetActive(cur == 1);

            for (int i = 0; i < 9; i++)
            {
                bool on = PageOrder[i] == cur;
                if (_navBars[i]) _navBars[i].color = on ? UIHelpers.Accent : new Color(0, 0, 0, 0);
                if (_navTxts[i]) _navTxts[i].color = on ? UIHelpers.TextLight : UIHelpers.TextDim;
                if (_navBgs[i]) _navBgs[i].color = on ? UIHelpers.NavActive : new Color(0, 0, 0, 0);
            }

            if (cur == 2) Page2UI.RefreshTexts();
            if (cur == 3) Page3UI.Refresh();
            if (_infoTabDot) _infoTabDot.color = DiagnosticsManager.FailCount > 0 ? UIHelpers.OffColor : UIHelpers.OnColor;
        }

        // ── RefreshAll ────────────────────────────────────────────────────
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

            bool cap2 = NoSpeedCap.Enabled;
            if (capTxt)
            {
                capTxt.text = cap2 ? "SPEED CAP REMOVED" : "REMOVE SPEED CAP";
                capTxt.color = Color.white;
            }
            if (capBg) capBg.color = cap2 ? UIHelpers.OnColor : UIHelpers.Accent;
            if (capBdr) capBdr.color = cap2 ? UIHelpers.OnBdr : UIHelpers.AccentBdr;

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
            if (topSpeedVal) topSpeedVal.text = TopSpeed.DisplayValue;
            if (sessionTimeVal) sessionTimeVal.text = SessionTrackers.SessionTimeDisplay;
            if (bailCountVal) bailCountVal.text = SessionTrackers.BailCountDisplay;
            if (airtimeVal) airtimeVal.text = SessionTrackers.AirtimeDisplay;

            bool slow = SlowMotion.Enabled;
            if (slowVal) { slowVal.text = slow ? "ON" : "OFF"; slowVal.color = slow ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(slowTrack, slowKnob, slow);
        }

        // Lightweight per-frame update for live tracker values
        // Called from ModEntry.OnUpdate — only touches the 4 text fields
        public static void TickLive()
        {
            if (topSpeedVal) topSpeedVal.text = TopSpeed.DisplayValue;
            if (sessionTimeVal) sessionTimeVal.text = SessionTrackers.SessionTimeDisplay;
            if (bailCountVal) bailCountVal.text = SessionTrackers.BailCountDisplay;
            if (airtimeVal) airtimeVal.text = SessionTrackers.AirtimeDisplay;
        }
    }
}