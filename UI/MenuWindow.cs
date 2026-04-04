using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using DescendersModMenu.BikeStats;
using DescendersModMenu.Mods;

namespace DescendersModMenu.UI
{
    public static class MenuWindow
    {
        // ── Page 1 fields ─────────────────────────────────────────────
        // Acceleration
        private static Text accelVal, accelTogVal;
        private static Image accelBar, accelTrack;
        private static RectTransform accelKnob;
        // Max Speed
        private static Text msVal, msTogVal;
        private static Image msBar, msTrack;
        private static RectTransform msKnob;
        // Landing Impact
        private static Text landTogVal;
        private static Image landTrack;
        private static RectTransform landKnob;
        private static Text landVal;
        private static Image landBar;
        // No Bail
        private static Text bailVal;
        private static Image bailTrack;
        private static RectTransform bailKnob;
        // Auto Balance
        private static Text autoBalVal, autoBalStrVal; private static Image autoBalBar;
        private static Image autoBalTrack; private static RectTransform autoBalKnob;
        // Bike
        private static Text bikeVal;
        // FOV
        private static Text fovVal, fovTogVal;
        private static Image fovBar, fovTrack;
        private static RectTransform fovKnob;
        // Slow Motion
        private static Text slowVal, slowSpeedVal;
        private static Image slowSpeedBar, slowTrack;
        private static RectTransform slowKnob;
        // Slow Mo on Bail / No Speed Wobbles / Speedrun Timer
        // Quick Brake UI fields
        private static Text _brakeTogVal = null;
        private static Image _brakeTrack = null;
        private static RectTransform _brakeKnob = null;
        private static Image _brakeLevelBar = null;
        private static Text _brakeLevelVal = null;
        private static Text smobVal, nswVal;
        private static Image smobTrack, nswTrack;
        private static RectTransform smobKnob, nswKnob;
        // No Speed Cap
        private static Image capBg, capBdr; private static Text capTxt;
        // ── Pages ─────────────────────────────────────────────────────
        private static GameObject pg1, pg2, pg3, pg4, pg5, pg6, pg7, pg8, pg9, pg10, pg11, pg12, pg13, pg14, pg15, pg16;
        private static int cur = 1;

        private static readonly int[] PageOrder = { 1, 16, 6, 8, 10, 7, 9, 11, 12, 13, 14, 15, 2, 5, 4, 3 };
        private static readonly string[] NavLabels = { "General", "Session", "Move", "Bike", "Graphics", "World", "Fun", "Outfit", "Chat", "Modes", "GhostReplay", "MapChange", "ESP", "Score", "Unlock", "Info/Customise" };
        private static readonly string[] GroupLabels = { "BIKE", null, null, null, "WORLD", null, "TOOLS", null, null, null, null, null, "SYSTEM", null, null, null };

        private static Image[] _navBars = new Image[16];
        private static Text[] _navTxts = new Text[16];
        private static Image[] _navBgs = new Image[16];
        private static Image[] _activeDots = new Image[16];
        private static UnityEngine.UI.Image _infoTabDot;

        public static CanvasGroup RootCanvasGroup { get; private set; }
        public static RectTransform RootRT { get; private set; }

        // ── Header button flash ───────────────────────────────────────
        private static Image _hdrSaveImg, _hdrLoadImg, _hdrResetImg;
        private static Image _hdrFlashImg = null;
        private static float _hdrFlashTimer = 0f;

        // ─────────────────────────────────────────────────────────────
        public static GameObject CreateMenu()
        {
            try
            {
                if (UIHelpers.GetFont() == null) { MelonLogger.Error("Font null"); return null; }
                cur = 1;

                var cv = new GameObject("DescendersMenu");
                var c = cv.AddComponent<Canvas>();
                c.renderMode = RenderMode.ScreenSpaceOverlay; c.sortingOrder = 999;
                var cs = cv.AddComponent<CanvasScaler>();
                cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                cs.referenceResolution = new Vector2(1920, 1080);
                cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                cs.matchWidthOrHeight = 0.5f;
                cv.AddComponent<GraphicRaycaster>();

                var root = UIHelpers.Obj("Root", cv.transform);
                UIHelpers.Pin(UIHelpers.RT(root), new Vector2(.5f, .5f), new Vector2(.5f, .5f),
                    Vector2.zero, new Vector2(UIHelpers.WinW, UIHelpers.WinH));
                RootCanvasGroup = root.AddComponent<CanvasGroup>();
                RootRT = UIHelpers.RT(root);

                var win = UIHelpers.Panel("Win", root.transform, UIHelpers.WinPanel, UIHelpers.WinSp);
                UIHelpers.Fill(UIHelpers.RT(win));
                win.AddComponent<Mask>().showMaskGraphic = true;

                // Header
                var hdr = UIHelpers.Panel("Hdr", win.transform, UIHelpers.HeaderBg);
                var hrt = UIHelpers.RT(hdr);
                hrt.anchorMin = new Vector2(0, 1); hrt.anchorMax = new Vector2(1, 1);
                hrt.pivot = new Vector2(.5f, 1);
                hrt.sizeDelta = new Vector2(0, UIHelpers.HeaderH);
                hrt.anchoredPosition = Vector2.zero;

                var hTopBar = UIHelpers.Panel("HTopBar", win.transform, UIHelpers.Accent);
                var htRT = UIHelpers.RT(hTopBar);
                htRT.anchorMin = new Vector2(0, 1); htRT.anchorMax = new Vector2(0.65f, 1);
                htRT.pivot = new Vector2(0, 1); htRT.sizeDelta = new Vector2(0, 2); htRT.anchoredPosition = Vector2.zero;
                hTopBar.AddComponent<LayoutElement>().ignoreLayout = true;
                var hTopBar2 = UIHelpers.Panel("HTopBar2", win.transform, UIHelpers.NeonBlue);
                var ht2RT = UIHelpers.RT(hTopBar2);
                ht2RT.anchorMin = new Vector2(0.64f, 1); ht2RT.anchorMax = new Vector2(1, 1);
                ht2RT.pivot = new Vector2(0, 1); ht2RT.sizeDelta = new Vector2(0, 2); ht2RT.anchoredPosition = Vector2.zero;
                hTopBar2.AddComponent<LayoutElement>().ignoreLayout = true;

                var title = UIHelpers.Txt("T", hdr.transform, "DESCENDERS", 18, FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.TextLight);
                var trt = UIHelpers.RT(title.gameObject);
                trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
                trt.offsetMin = new Vector2(16, 0); trt.offsetMax = new Vector2(-220, 0);

                var sub = UIHelpers.Txt("Sub", hdr.transform, "TOOLKIT", 18, FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.Accent);
                var subrt = UIHelpers.RT(sub.gameObject);
                subrt.anchorMin = Vector2.zero; subrt.anchorMax = Vector2.one;
                subrt.offsetMin = new Vector2(155, 0); subrt.offsetMax = new Vector2(-80, 0);

                var slash = UIHelpers.Panel("HSlash", hdr.transform, UIHelpers.Accent);
                var slrt = UIHelpers.RT(slash);
                slrt.anchorMin = new Vector2(0, 0.5f); slrt.anchorMax = new Vector2(0, 0.5f);
                slrt.pivot = new Vector2(0, 0.5f); slrt.sizeDelta = new Vector2(2, 28);
                slrt.anchoredPosition = new Vector2(275, 0);

                var verBadge = UIHelpers.Panel("VBadge", hdr.transform, UIHelpers.AccentDim, UIHelpers.BtnSp);
                var vbrt = UIHelpers.RT(verBadge);
                vbrt.anchorMin = new Vector2(0, 0.5f); vbrt.anchorMax = new Vector2(0, 0.5f);
                vbrt.pivot = new Vector2(0, 0.5f); vbrt.sizeDelta = new Vector2(58f, 20f);
                vbrt.anchoredPosition = new Vector2(288, 0);
                var vbBdr = UIHelpers.Panel("VBBdr", verBadge.transform, UIHelpers.AccentBdr, UIHelpers.BtnSp);
                vbBdr.GetComponent<Image>().raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(vbBdr));
                vbBdr.AddComponent<LayoutElement>().ignoreLayout = true;
                var verTxt = UIHelpers.Txt("VT", verBadge.transform, "v3.6.2", 10, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                UIHelpers.Fill(UIHelpers.RT(verTxt.gameObject));

                // "Created by NateHyden" — pinned to top-right corner
                var byTxt = UIHelpers.Txt("By", hdr.transform, "Created by NateHyden", 9, FontStyle.Normal, TextAnchor.UpperRight, UIHelpers.TextMid);
                var byrt = UIHelpers.RT(byTxt.gameObject);
                byrt.anchorMin = new Vector2(1, 1); byrt.anchorMax = new Vector2(1, 1);
                byrt.pivot = new Vector2(1, 1);
                byrt.sizeDelta = new Vector2(160, 16);
                byrt.anchoredPosition = new Vector2(-8, -3);
                byrt.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;

                // SAVE / LOAD / RESET — centred in the gap between version badge and "Created by"
                // Badge right edge ~346px, Created by left edge ~632px, gap centre ~489px.
                // 3 × 52px buttons + 2 × 8px gaps = 172px. First left edge = 489 - 86 = 403px.
                _hdrSaveImg = HeaderBtn(hdr.transform, "SAVE", 403f, () => { StatsManager.SaveStats(); FlashHeader(_hdrSaveImg); });
                _hdrLoadImg = HeaderBtn(hdr.transform, "LOAD", 463f, () => { StatsManager.LoadStats(); RefreshAll(); FlashHeader(_hdrLoadImg); });
                _hdrResetImg = HeaderBtn(hdr.transform, "RESET", 523f, () => { StatsManager.ResetStats(); RefreshAll(); FlashHeader(_hdrResetImg); });

                // Body
                var body = UIHelpers.Obj("Body", win.transform);
                var bodyRT = UIHelpers.RT(body);
                bodyRT.anchorMin = Vector2.zero; bodyRT.anchorMax = Vector2.one;
                bodyRT.offsetMin = new Vector2(0, 0); bodyRT.offsetMax = new Vector2(0, -UIHelpers.HeaderH);

                // Sidebar
                var sidebar = UIHelpers.Panel("Sidebar", body.transform, UIHelpers.SidebarBg);
                var sibRT = UIHelpers.RT(sidebar);
                sibRT.anchorMin = Vector2.zero; sibRT.anchorMax = new Vector2(0, 1);
                sibRT.offsetMin = Vector2.zero; sibRT.offsetMax = new Vector2(UIHelpers.SidebarW, 0);

                // ScrollRect wrapper inside sidebar so tabs never clip at small heights
                var sibScroll = UIHelpers.Obj("SibScroll", sidebar.transform);
                UIHelpers.Fill(UIHelpers.RT(sibScroll));
                var sibSR = sibScroll.AddComponent<ScrollRect>();
                sibSR.horizontal = false; sibSR.vertical = true;
                sibSR.movementType = ScrollRect.MovementType.Clamped;
                sibSR.scrollSensitivity = 20f; sibSR.inertia = false;

                var sibVP = UIHelpers.Obj("SibVP", sibScroll.transform);
                UIHelpers.Fill(UIHelpers.RT(sibVP));
                sibVP.AddComponent<Image>().color = new Color(0, 0, 0, 0.01f);
                sibVP.AddComponent<Mask>().showMaskGraphic = true;
                sibSR.viewport = UIHelpers.RT(sibVP);

                var sibContent = UIHelpers.Obj("SibContent", sibVP.transform);
                var sibCRT = UIHelpers.RT(sibContent);
                sibCRT.anchorMin = new Vector2(0, 1); sibCRT.anchorMax = new Vector2(1, 1);
                sibCRT.pivot = new Vector2(0.5f, 1); sibCRT.sizeDelta = new Vector2(0, 0);
                sibSR.content = sibCRT;
                sibContent.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                var sVlg = sibContent.AddComponent<VerticalLayoutGroup>();
                sVlg.spacing = 1; sVlg.padding = new RectOffset(0, 0, 0, 6);
                sVlg.childAlignment = TextAnchor.UpperCenter;
                sVlg.childForceExpandWidth = true; sVlg.childForceExpandHeight = false;

                for (int i = 0; i < 16; i++)
                {
                    int navIdx = i;
                    int pageNum = PageOrder[i];
                    var item = UIHelpers.Obj("Nav" + i, sibContent.transform);
                    var ile = item.AddComponent<LayoutElement>();
                    ile.preferredHeight = 28; ile.minHeight = 28; ile.flexibleHeight = 0;
                    var bg = UIHelpers.Panel("Bg", item.transform, new Color(0, 0, 0, 0));
                    UIHelpers.Fill(UIHelpers.RT(bg)); _navBgs[i] = bg.GetComponent<Image>();
                    var barGlow = UIHelpers.Panel("BarGlow", item.transform, new Color(0.075f, 0.090f, 0.020f, 1.00f));
                    var bgRT2 = UIHelpers.RT(barGlow);
                    bgRT2.anchorMin = Vector2.zero; bgRT2.anchorMax = new Vector2(0, 1);
                    bgRT2.pivot = new Vector2(0, .5f); bgRT2.offsetMin = Vector2.zero; bgRT2.offsetMax = new Vector2(6, 0);
                    barGlow.GetComponent<Image>().enabled = false;
                    var bar = UIHelpers.Panel("Bar", item.transform, new Color(0, 0, 0, 0));
                    var barRT = UIHelpers.RT(bar);
                    barRT.anchorMin = Vector2.zero; barRT.anchorMax = new Vector2(0, 1);
                    barRT.pivot = new Vector2(0, .5f); barRT.offsetMin = Vector2.zero; barRT.offsetMax = new Vector2(3, 0);
                    _navBars[i] = bar.GetComponent<Image>();
                    var lbl = UIHelpers.Txt("L", item.transform, NavLabels[i], 11, FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.TextMid);
                    var lblRT = UIHelpers.RT(lbl.gameObject);
                    lblRT.anchorMin = Vector2.zero; lblRT.anchorMax = Vector2.one;
                    lblRT.offsetMin = new Vector2(18, 0); lblRT.offsetMax = Vector2.zero;
                    _navTxts[i] = lbl;

                    // ── Active mod dot (top-right of nav item) ────────
                    var dotObj = UIHelpers.Obj("ActiveDot", item.transform);
                    var dotImg = dotObj.AddComponent<Image>();
                    dotImg.sprite = UIHelpers.DotSp;
                    dotImg.type = Image.Type.Simple;
                    dotImg.color = UIHelpers.OnColor;
                    dotImg.enabled = false;
                    _activeDots[i] = dotImg;
                    var drt = UIHelpers.RT(dotObj);
                    drt.anchorMin = new Vector2(1f, 0.5f); drt.anchorMax = new Vector2(1f, 0.5f);
                    drt.pivot = new Vector2(1f, 0.5f);
                    drt.sizeDelta = new Vector2(6, 6);
                    drt.anchoredPosition = new Vector2(-8, 0);
                    dotObj.AddComponent<LayoutElement>().ignoreLayout = true;

                    if (pageNum == 3)
                    {
                        var infoDotObj = UIHelpers.Obj("InfoDot", item.transform);
                        var infoDotImg = infoDotObj.AddComponent<UnityEngine.UI.Image>();
                        infoDotImg.sprite = UIHelpers.DotSp; infoDotImg.type = UnityEngine.UI.Image.Type.Simple; infoDotImg.color = UIHelpers.OnColor;
                        _infoTabDot = infoDotImg;
                        var idrt = UIHelpers.RT(infoDotObj);
                        idrt.anchorMin = new Vector2(1f, 0.5f); idrt.anchorMax = new Vector2(1f, 0.5f);
                        idrt.pivot = new Vector2(1f, 0.5f); idrt.sizeDelta = new Vector2(7, 7); idrt.anchoredPosition = new Vector2(-10, 0);
                        infoDotObj.AddComponent<LayoutElement>().ignoreLayout = true;
                    }
                    var btn = item.AddComponent<Button>();
                    btn.onClick.AddListener(() => Switch(PageOrder[navIdx]));
                    btn.targetGraphic = bg.GetComponent<Image>();
                    var bcol = btn.colors;
                    bcol.normalColor = Color.white; bcol.highlightedColor = new Color(1.08f, 1.08f, 1.08f, 1);
                    bcol.pressedColor = new Color(0.85f, 0.85f, 0.85f, 1); bcol.colorMultiplier = 1; btn.colors = bcol;
                }

                // Content area
                var cont = UIHelpers.Obj("Cnt", body.transform);
                var crt = UIHelpers.RT(cont);
                crt.anchorMin = Vector2.zero; crt.anchorMax = Vector2.one;
                crt.offsetMin = new Vector2(UIHelpers.SidebarW, 0); crt.offsetMax = Vector2.zero;

                pg1 = UIHelpers.Obj("P1", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg1)); BuildPage1(pg1.transform);
                pg2 = UIHelpers.Obj("P2", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg2)); Page2UI.CreatePage(pg2.transform);
                pg3 = UIHelpers.Obj("P3", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg3)); Page3UI.CreatePage(pg3.transform);
                pg4 = UIHelpers.Obj("P4", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg4)); Page4UI.CreatePage(pg4.transform);
                pg5 = UIHelpers.Obj("P5", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg5)); Page5UI.CreatePage(pg5.transform);
                pg6 = UIHelpers.Obj("P6", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg6)); Page6UI.CreatePage(pg6.transform);
                pg7 = UIHelpers.Obj("P7", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg7)); Page7UI.CreatePage(pg7.transform);
                pg8 = UIHelpers.Obj("P8", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg8)); Page8UI.CreatePage(pg8.transform);
                pg9 = UIHelpers.Obj("P9", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg9)); Page9UI.CreatePage(pg9.transform);
                pg10 = UIHelpers.Obj("P10", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg10)); Page10UI.CreatePage(pg10.transform);
                pg11 = UIHelpers.Obj("P11", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg11)); Page11UI.CreatePage(pg11.transform);
                pg12 = UIHelpers.Obj("P12", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg12)); Page12UI.CreatePage(pg12.transform);
                pg13 = UIHelpers.Obj("P13", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg13)); PageModesUI.CreatePage(pg13.transform);
                pg14 = UIHelpers.Obj("P14", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg14)); Page14UI.CreatePage(pg14.transform);
                pg15 = UIHelpers.Obj("P15", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg15)); Page15UI.CreatePage(pg15.transform);

                pg16 = UIHelpers.Obj("P16", cont.transform); UIHelpers.Fill(UIHelpers.RT(pg16)); PageSessionUI.CreatePage(pg16.transform);

                RefreshAll(); RefreshTabs();
                Mods.MenuCustomiser.LoadFromFile();
                cv.SetActive(false);
                return cv;
            }
            catch (System.Exception ex) { MelonLogger.Error("CreateMenu: " + ex); return null; }
        }

        // ── Page 1 (General) ──────────────────────────────────────────
        private static void BuildPage1(Transform p)
        {
            var scrollObj = UIHelpers.Obj("Scroll", p);
            UIHelpers.Fill(UIHelpers.RT(scrollObj));
            var sr = scrollObj.AddComponent<ScrollRect>();
            sr.horizontal = false; sr.vertical = true;
            sr.movementType = ScrollRect.MovementType.Clamped;
            sr.scrollSensitivity = 25f; sr.inertia = false;

            var vp = UIHelpers.Obj("VP", scrollObj.transform);
            UIHelpers.Fill(UIHelpers.RT(vp));
            vp.AddComponent<Image>().color = new Color(0, 0, 0, 0.01f);
            vp.AddComponent<Mask>().showMaskGraphic = true;
            sr.viewport = UIHelpers.RT(vp);

            var content = UIHelpers.Obj("Content", vp.transform);
            var crt = UIHelpers.RT(content);
            crt.anchorMin = new Vector2(0, 1); crt.anchorMax = new Vector2(1, 1);
            crt.pivot = new Vector2(0.5f, 1); crt.sizeDelta = new Vector2(0, 0);
            sr.content = crt;
            content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            var vlg = content.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = UIHelpers.RowGap;
            vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;

            var pg = content.transform;

            UIHelpers.SectionHeader("BIKE PHYSICS", pg);

            // ── Acceleration (now with toggle) ────────────────────────
            var ar = UIHelpers.StatRow("Acceleration", pg);
            accelTogVal = UIHelpers.Txt("ATV", ar.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            accelTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(ar.transform, "AT", () => { Acceleration.Toggle(); RefreshAll(); }, out accelTrack, out accelKnob);
            accelBar = UIHelpers.MakeBar("AB", ar.transform, (Acceleration.Level - 1) / 9f);
            accelVal = UIHelpers.Txt("AV", ar.transform, Acceleration.Level.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            accelVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
            UIHelpers.SmallBtn(ar.transform, "-", () => { Acceleration.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(ar.transform, "+", () => { Acceleration.Increase(); RefreshAll(); });

            // ── Max Speed compound row (now with toggle) ───────────────
            var mso = UIHelpers.Panel("MSR", pg, UIHelpers.RowBg, UIHelpers.RowSp);
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

            msTogVal = UIHelpers.Txt("MSTV", mst.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            msTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(mst.transform, "MST2", () => { MaxSpeedMultiplier.Toggle(); RefreshAll(); }, out msTrack, out msKnob);
            msBar = UIHelpers.MakeBar("MSB", mst.transform, (MaxSpeedMultiplier.Level - 1) / 9f);
            msVal = UIHelpers.Txt("MSV", mst.transform, MaxSpeedMultiplier.Level.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            msVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
            UIHelpers.SmallBtn(mst.transform, "-", () => { MaxSpeedMultiplier.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(mst.transform, "+", () => { MaxSpeedMultiplier.Increase(); RefreshAll(); });

            // No Speed Cap button
            var cap = UIHelpers.Obj("Cap", mso.transform);
            capBg = cap.AddComponent<Image>(); capBg.sprite = UIHelpers.BtnSp;
            capBg.type = Image.Type.Sliced; capBg.color = UIHelpers.NeonBlue;
            var cbtn = cap.AddComponent<Button>();
            cbtn.onClick.AddListener(() => { NoSpeedCap.Toggle(); RefreshAll(); });
            var ccb = cbtn.colors; ccb.normalColor = Color.white; ccb.highlightedColor = new Color(1, 1, 1, 1.15f);
            ccb.pressedColor = new Color(.7f, .7f, .7f, 1); ccb.colorMultiplier = 1; ccb.fadeDuration = .08f;
            cbtn.colors = ccb;
            cap.AddComponent<LayoutElement>().preferredHeight = 30;
            var cbd = UIHelpers.Panel("CBd", cap.transform, UIHelpers.NeonBlue, UIHelpers.BtnSp);
            capBdr = cbd.GetComponent<Image>(); capBdr.raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(cbd));
            capTxt = UIHelpers.Txt("CT", cap.transform, "REMOVE SPEED CAP", 11, FontStyle.Bold, TextAnchor.MiddleCenter, new Color(0, 0, 0, 1));
            capTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
            UIHelpers.Fill(UIHelpers.RT(capTxt.gameObject));

            // ── Landing Impact ────────────────────────────────────────
            var lr = UIHelpers.StatRow("Landing Impact", pg);
            landTogVal = UIHelpers.Txt("LTV", lr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            landTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(lr.transform, "LT", () => { LandingImpact.Toggle(); RefreshAll(); }, out landTrack, out landKnob);
            landBar = UIHelpers.MakeBar("LB", lr.transform, (LandingImpact.Level - 1) / 9f);
            landVal = UIHelpers.Txt("LV", lr.transform, LandingImpact.DisplayValue, 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            landVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.SmallBtn(lr.transform, "-", () => { LandingImpact.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(lr.transform, "+", () => { LandingImpact.Increase(); RefreshAll(); });
            UIHelpers.InfoBox(pg, "Raises the impact speed required to bail. Level 10 = almost impossible to fall off.");

            // ── No Bail ───────────────────────────────────────────────
            var nr = UIHelpers.StatRow("No Bail", pg);
            bailVal = UIHelpers.Txt("NV", nr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            bailVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(nr.transform, "NT", () => { NoBail.Toggle(); RefreshAll(); }, out bailTrack, out bailKnob);

            // ── Auto Balance ──────────────────────────────────────────
            var abTogRow = UIHelpers.StatRow("Auto Balance", pg);
            autoBalVal = UIHelpers.Txt("ABV", abTogRow.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            autoBalVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(abTogRow.transform, "ABT", () => { AutoBalance.Toggle(); RefreshAll(); }, out autoBalTrack, out autoBalKnob);
            autoBalBar = UIHelpers.MakeBar("ABB", abTogRow.transform, (AutoBalance.StrengthLevel - 1) / 9f);
            autoBalStrVal = UIHelpers.Txt("ABS", abTogRow.transform, AutoBalance.StrengthLevel.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            autoBalStrVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
            UIHelpers.SmallBtn(abTogRow.transform, "-", () => { AutoBalance.StrengthDecrease(); RefreshAll(); });
            UIHelpers.SmallBtn(abTogRow.transform, "+", () => { AutoBalance.StrengthIncrease(); RefreshAll(); });

            // ── Bike Switcher ─────────────────────────────────────────
            var br = UIHelpers.StatRow("Bike", pg);
            UIHelpers.SmallBtn(br.transform, "\u25C0", () => { BikeSwitcher.PreviousBike(); RefreshAll(); });
            bikeVal = UIHelpers.Txt("BV", br.transform, "Enduro", 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
            bikeVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 80;
            UIHelpers.SmallBtn(br.transform, "\u25B6", () => { BikeSwitcher.NextBike(); RefreshAll(); });

            // ── FOV (now with toggle) ─────────────────────────────────
            var fr = UIHelpers.StatRow("FOV", pg);
            fovTogVal = UIHelpers.Txt("FTV", fr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            fovTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(fr.transform, "FT", () => { FOV.Toggle(); RefreshAll(); }, out fovTrack, out fovKnob);
            fovBar = UIHelpers.MakeBar("FB", fr.transform, (FOV.Level - 1) / 9f);
            fovVal = UIHelpers.Txt("FV", fr.transform, FOV.DisplayValue, 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            fovVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 26;
            UIHelpers.SmallBtn(fr.transform, "-", () => { FOV.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(fr.transform, "+", () => { FOV.Increase(); RefreshAll(); });

            // ── Slow Motion ───────────────────────────────────────────
            var smr = UIHelpers.StatRow("Slow Motion", pg);
            slowVal = UIHelpers.Txt("SMV", smr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            slowVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(smr.transform, "SMT", () => { SlowMotion.Toggle(); RefreshAll(); }, out slowTrack, out slowKnob);
            slowSpeedBar = UIHelpers.MakeBar("SmSB", smr.transform, (SlowMotion.Level - 1) / 8f);
            slowSpeedVal = UIHelpers.Txt("SmSV", smr.transform, SlowMotion.DisplayValue, 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            slowSpeedVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.SmallBtn(smr.transform, "-", () => { SlowMotion.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(smr.transform, "+", () => { SlowMotion.Increase(); RefreshAll(); });
            var smHint = UIHelpers.Txt("SMH", smr.transform, "F2", 10, FontStyle.Normal, TextAnchor.MiddleRight, UIHelpers.TextDim);
            smHint.gameObject.AddComponent<LayoutElement>().preferredWidth = 22;

            // ── Slow Mo on Bail ───────────────────────────────────────
            var smobr = UIHelpers.StatRow("Slow Mo on Bail", pg);
            smobVal = UIHelpers.Txt("SbV", smobr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            smobVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(smobr.transform, "SbT", () => { SlowMoOnBail.Toggle(); RefreshAll(); }, out smobTrack, out smobKnob);

            // ── No Speed Wobbles ──────────────────────────────────────
            var nswr = UIHelpers.StatRow("No Speed Wobbles", pg);
            nswVal = UIHelpers.Txt("NwV", nswr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            nswVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(nswr.transform, "NwT", () => { GameModifierMods.NoSpeedWobblesToggle(); RefreshAll(); }, out nswTrack, out nswKnob);

            // ── QUICK ACTIONS ─────────────────────────────────────────
            UIHelpers.Divider(pg);
            UIHelpers.SectionHeader("QUICK ACTIONS", pg);
            // ── Brake toggle row ─────────────────────────────────────
            var brakeRow = UIHelpers.StatRow("Quick Brake", pg);
            _brakeTogVal = UIHelpers.Txt("BkTV", brakeRow.transform, "OFF", 11,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
            _brakeTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.Toggle(brakeRow.transform, "BkT", () =>
            {
                QuickBrake.Toggle();
                RefreshAll();
            }, out _brakeTrack, out _brakeKnob);
            _brakeLevelBar = UIHelpers.MakeBar("BkB", brakeRow.transform, (QuickBrake.Level - 1) / 9f);
            _brakeLevelVal = UIHelpers.Txt("BkLV", brakeRow.transform,
                QuickBrake.Level == 10 ? "MAX" : QuickBrake.Level.ToString(), 11,
                FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
            _brakeLevelVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
            UIHelpers.SmallBtn(brakeRow.transform, "-", () =>
            { QuickBrake.Decrease(); RefreshAll(); });
            UIHelpers.SmallBtn(brakeRow.transform, "+", () =>
            { QuickBrake.Increase(); RefreshAll(); });
            UIHelpers.InfoBox(pg, "Level 1-9: fast drag deceleration. Level 10 (MAX): truly instant stop.");
            // ── Launch button row ─────────────────────────────────────
            var qar = UIHelpers.StatRow("Actions", pg);
            // Super Launch — fires player forward+up, does NOT touch any mod state
            UIHelpers.ActionBtn(qar.transform, "Launch", () =>
            {
                try
                {
                    GameObject player = GameObject.Find("Player_Human");
                    if ((object)player == null) return;
                    Vehicle v = player.GetComponent<Vehicle>();
                    if ((object)v == null) return;
                    System.Reflection.MethodInfo setVel = typeof(Vehicle).GetMethod(
                        "SetVelocity",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if ((object)setVel == null) return;
                    Vector3 launchVec = player.transform.forward * 80f + Vector3.up * 20f;
                    setVel.Invoke(v, new object[] { launchVec });
                }
                catch (System.Exception ex) { MelonLogger.Error("[SuperLaunch] " + ex.Message); }
            }, 60);
            UIHelpers.InfoBox(pg, "Launch: fires you forward at high speed.");

            UIHelpers.AddScrollForwarders(content.transform);
        }

        private static Image HeaderBtn(Transform hdr, string lbl, float x, UnityEngine.Events.UnityAction clk)
        {
            var g = UIHelpers.Obj(lbl + "HB", hdr);
            var rt = UIHelpers.RT(g);
            rt.anchorMin = new Vector2(0, 0.5f); rt.anchorMax = new Vector2(0, 0.5f);
            rt.pivot = new Vector2(0, 0.5f);
            rt.sizeDelta = new Vector2(52, 22);
            rt.anchoredPosition = new Vector2(x, 0);
            var im = g.AddComponent<Image>(); im.sprite = UIHelpers.BtnSp;
            im.type = Image.Type.Sliced; im.color = UIHelpers.NeonBlue;
            var btn = g.AddComponent<Button>(); btn.onClick.AddListener(clk);
            var bc = btn.colors;
            bc.normalColor = Color.white; bc.highlightedColor = new Color(1.2f, 1.2f, 1.2f, 1f);
            bc.pressedColor = new Color(0.65f, 0.65f, 0.65f, 1f); bc.colorMultiplier = 1; btn.colors = bc;
            var t = UIHelpers.Txt(lbl + "HBT", g.transform, lbl, 9, FontStyle.Bold,
                TextAnchor.MiddleCenter, new Color(0f, 0f, 0f, 1f));
            UIHelpers.Fill(UIHelpers.RT(t.gameObject));
            return im;
        }

        private static void BotBtn(string lbl, Transform p, Color bg, UnityEngine.Events.UnityAction clk)
        {
            var g = UIHelpers.Obj(lbl + "B", p);
            var im = g.AddComponent<Image>(); im.sprite = UIHelpers.BtnSp; im.type = Image.Type.Sliced; im.color = bg;
            var b = g.AddComponent<Button>(); b.onClick.AddListener(clk);
            var cb = b.colors; cb.normalColor = Color.white;
            cb.highlightedColor = new Color(1.1f, 1.1f, 1.1f, 1f);
            cb.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1); cb.colorMultiplier = 1; b.colors = cb;
            var t = UIHelpers.Txt("L", g.transform, lbl.ToUpper(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, new Color(0, 0, 0, 1));
            UIHelpers.Fill(UIHelpers.RT(t.gameObject));
        }

        // ── Navigation ────────────────────────────────────────────────
        private static bool IsPageActive(int pageNum)
        {
            try
            {
                switch (pageNum)
                {
                    case 1:
                        return Mods.Acceleration.Enabled || Mods.MaxSpeedMultiplier.Enabled ||
                                    Mods.NoSpeedCap.Enabled || Mods.LandingImpact.Enabled ||
                                    Mods.QuickBrake.Enabled || Mods.NoBail.Enabled ||
                                    Mods.AutoBalance.Enabled || Mods.FOV.Enabled ||
                                    Mods.SlowMotion.Enabled || Mods.SlowMoOnBail.Enabled ||
                                    Mods.GameModifierMods.NoSpeedWobblesEnabled;
                    case 6: return Page6UI.IsAnyActive;
                    case 7: return Page7UI.IsAnyActive;
                    case 8: return Page8UI.IsAnyActive;
                    case 9: return Page9UI.IsAnyActive;
                    case 10: return Page10UI.IsAnyActive;
                    case 13: return PageModesUI.IsAnyActive;
                    case 14: return Mods.GhostReplay.Enabled;
                    case 16: return PageSessionUI.IsAnyActive;
                    default: return false;
                }
            }
            catch { return false; }
        }

        private static void Switch(int pg) { cur = pg; RefreshTabs(); }

        private static void RefreshTabs()
        {
            if (pg1) pg1.SetActive(cur == 1); if (pg2) pg2.SetActive(cur == 2);
            if (pg3) pg3.SetActive(cur == 3); if (pg4) pg4.SetActive(cur == 4);
            if (pg5) pg5.SetActive(cur == 5); if (pg6) pg6.SetActive(cur == 6);
            if (pg7) pg7.SetActive(cur == 7); if (pg8) pg8.SetActive(cur == 8);
            if (pg9) pg9.SetActive(cur == 9); if (pg10) pg10.SetActive(cur == 10);
            if (pg11) pg11.SetActive(cur == 11);
            // Cancel outfit rename when navigating away from outfit page
            if (cur != 11) Page11UI.CancelRename(); if (pg12) pg12.SetActive(cur == 12);
            if (pg13) pg13.SetActive(cur == 13); if (pg14) pg14.SetActive(cur == 14);
            if (pg15) pg15.SetActive(cur == 15);
            if (pg16) pg16.SetActive(cur == 16);

            for (int i = 0; i < 16; i++)
            {
                bool on = PageOrder[i] == cur;
                bool active = IsPageActive(PageOrder[i]);
                if (_navBars[i]) _navBars[i].color = on ? UIHelpers.Accent : new Color(0, 0, 0, 0);
                if (_navTxts[i]) _navTxts[i].color = on ? UIHelpers.Accent : UIHelpers.TextMid;
                if (_navBgs[i]) _navBgs[i].color = on ? UIHelpers.NavActive : new Color(0, 0, 0, 0);
                // Show active dot only when tab is not currently selected
                if (_activeDots[i]) _activeDots[i].enabled = active && !on;
                var navItem = _navBgs[i] != null ? _navBgs[i].transform.parent : null;
                if ((object)navItem != null)
                {
                    var glow = navItem.Find("BarGlow");
                    if ((object)glow != null) glow.GetComponent<Image>().enabled = on;
                }
            }
            if (cur == 2) Page2UI.RefreshTexts();
            if (cur == 3) Page3UI.Refresh();
            if (_infoTabDot) _infoTabDot.color = DiagnosticsManager.FailCount > 0 ? UIHelpers.OffColor : UIHelpers.OnColor;
        }

        // ── RefreshAll ────────────────────────────────────────────────
        public static void RefreshAll()
        {
            // ── Acceleration ──────────────────────────────────────────
            bool acOn = Acceleration.Enabled;
            if (accelTogVal) { accelTogVal.text = acOn ? "ON" : "OFF"; accelTogVal.color = acOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(accelTrack, accelKnob, acOn);
            if (accelVal) accelVal.text = Acceleration.Level.ToString();
            UIHelpers.SetBar(accelBar, (Acceleration.Level - 1) / 9f);

            // ── Max Speed ─────────────────────────────────────────────
            bool msOn = MaxSpeedMultiplier.Enabled;
            if (msTogVal) { msTogVal.text = msOn ? "ON" : "OFF"; msTogVal.color = msOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(msTrack, msKnob, msOn);
            if (msVal) msVal.text = MaxSpeedMultiplier.Level.ToString();
            UIHelpers.SetBar(msBar, (MaxSpeedMultiplier.Level - 1) / 9f);

            // ── No Speed Cap ──────────────────────────────────────────
            bool cap2 = NoSpeedCap.Enabled;
            if (capTxt) { capTxt.text = cap2 ? "SPEED CAP REMOVED" : "REMOVE SPEED CAP"; capTxt.color = new Color(0, 0, 0, 1); }
            if (capBg) capBg.color = cap2 ? UIHelpers.OnColor : UIHelpers.NeonBlue;
            if (capBdr) capBdr.color = cap2 ? UIHelpers.OnColor : UIHelpers.NeonBlue;

            // ── Landing Impact ────────────────────────────────────────
            bool liOn = LandingImpact.Enabled;
            if (landTogVal) { landTogVal.text = liOn ? "ON" : "OFF"; landTogVal.color = liOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(landTrack, landKnob, liOn);
            if (landVal) landVal.text = LandingImpact.DisplayValue;
            UIHelpers.SetBar(landBar, (LandingImpact.Level - 1) / 9f);

            // ── Quick Brake ───────────────────────────────────────────
            bool qbOn = QuickBrake.Enabled;
            if (_brakeTogVal) { _brakeTogVal.text = qbOn ? "ON" : "OFF"; _brakeTogVal.color = qbOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_brakeTrack, _brakeKnob, qbOn);
            if (_brakeLevelBar) UIHelpers.SetBar(_brakeLevelBar, (QuickBrake.Level - 1) / 9f);
            if (_brakeLevelVal) { _brakeLevelVal.text = QuickBrake.Level == 10 ? "MAX" : QuickBrake.Level.ToString(); }

            // ── No Bail ───────────────────────────────────────────────
            bool bail = NoBail.Enabled;
            if (bailVal) { bailVal.text = bail ? "ON" : "OFF"; bailVal.color = bail ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(bailTrack, bailKnob, bail);

            // ── Auto Balance ──────────────────────────────────────────
            bool ab = AutoBalance.Enabled;
            if (autoBalVal) { autoBalVal.text = ab ? "ON" : "OFF"; autoBalVal.color = ab ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(autoBalTrack, autoBalKnob, ab);
            if (autoBalStrVal) autoBalStrVal.text = AutoBalance.StrengthLevel.ToString();
            UIHelpers.SetBar(autoBalBar, (AutoBalance.StrengthLevel - 1) / 9f);

            // ── Bike ──────────────────────────────────────────────────
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

            // ── FOV ───────────────────────────────────────────────────
            bool fovOn = FOV.Enabled;
            if (fovTogVal) { fovTogVal.text = fovOn ? "ON" : "OFF"; fovTogVal.color = fovOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(fovTrack, fovKnob, fovOn);
            if (fovVal) fovVal.text = FOV.DisplayValue;
            UIHelpers.SetBar(fovBar, (FOV.Level - 1) / 9f);

            // ── Slow Motion ───────────────────────────────────────────
            bool slow = SlowMotion.Enabled;
            if (slowVal) { slowVal.text = slow ? "ON" : "OFF"; slowVal.color = slow ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(slowTrack, slowKnob, slow);
            if (slowSpeedVal) slowSpeedVal.text = SlowMotion.DisplayValue;
            UIHelpers.SetBar(slowSpeedBar, (SlowMotion.Level - 1) / 8f);

            // ── Slow Mo on Bail ───────────────────────────────────────
            bool smob = SlowMoOnBail.Enabled;
            if (smobVal) { smobVal.text = smob ? "ON" : "OFF"; smobVal.color = smob ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(smobTrack, smobKnob, smob);

            // ── No Speed Wobbles ──────────────────────────────────────
            bool nsw = GameModifierMods.NoSpeedWobblesEnabled;
            if (nswVal) { nswVal.text = nsw ? "ON" : "OFF"; nswVal.color = nsw ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(nswTrack, nswKnob, nsw);

            // ── Speedrun Timer ────────────────────────────────────────
            // ── Session live values ───────────────────────────────────
            PageSessionUI.RefreshAll();
        }

        private static void FlashHeader(Image img)
        {
            if ((object)img == null) return;
            if ((object)_hdrFlashImg != null && (object)_hdrFlashImg != (object)img)
                _hdrFlashImg.color = UIHelpers.NeonBlue;
            _hdrFlashImg = img;
            _hdrFlashTimer = 1.5f;
            img.color = UIHelpers.OnColor;
        }

        public static void TickLive()
        {
            PageSessionUI.TickLive();

            if (_hdrFlashTimer > 0f)
            {
                _hdrFlashTimer -= UnityEngine.Time.deltaTime;
                if (_hdrFlashTimer <= 0f && (object)_hdrFlashImg != null)
                {
                    _hdrFlashImg.color = UIHelpers.NeonBlue;
                    _hdrFlashImg = null;
                }
            }
        }
    }
}