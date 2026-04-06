using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page3UI
    {
        // ── Sub-tab state ─────────────────────────────────────────────
        private static int _activeTab = 0; // 0=System 1=Mod Status 2=Hotkeys 3=Credits 4=Customise 5=Dev Tools

        private static readonly string[] TabLabels = { "System", "Mod Status", "Hotkeys", "Credits", "Customise", "Dev Tools" };

        // Sub-tab bar buttons
        private static Image[] _tabBgs = new Image[6];
        private static Text[] _tabTxts = new Text[6];

        // Page root GameObjects
        private static GameObject _pgSystem;
        private static GameObject _pgStatus;
        private static GameObject _pgHotkeys;
        private static GameObject _pgCredits;
        private static GameObject _pgCustomise;
        private static GameObject _pgScanner;

        // Customise tab refs
        private static Text _custPosLbl;
        private static Text _custScaleLbl;
        private static Text _custOpacityLbl;
        private static GameObject _custSavedRow;

        // Scanner tab refs
        private static RectTransform _scanContentRT;
        private static Text _scanSummaryTxt;
        private static Text _scanFileTxt;

        // System tab
        private static Text _unityVersionTxt;
        private static Text _steamPlayerTxt;
        private static Text _unityMatchTxt;
        private static Text _mlVersionTxt;

        // Mod Status tab
        private static RectTransform _contentRT;
        private static Text _summaryTxt;
        private const float ROW_H = 22f;
        private const float ROW_GAP = 2f;
        private const float PAD = 6f;

        // ── CreatePage ────────────────────────────────────────────────
        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
                // Root — fills the content slot
                pg = UIHelpers.Obj("P3R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));
                var rootVlg = pg.AddComponent<VerticalLayoutGroup>();
                rootVlg.spacing = 0;
                rootVlg.padding = new RectOffset(0, 0, 0, 0);
                rootVlg.childAlignment = TextAnchor.UpperLeft;
                rootVlg.childForceExpandWidth = true;
                rootVlg.childForceExpandHeight = false;

                // ── Sub-tab bar ───────────────────────────────────────
                var tabBar = UIHelpers.Obj("TabBar", pg.transform);
                tabBar.AddComponent<Image>().color = UIHelpers.WinOuter;
                var tbLE = tabBar.AddComponent<LayoutElement>();
                tbLE.preferredHeight = 38; tbLE.minHeight = 38; tbLE.flexibleHeight = 0;
                var tbHlg = tabBar.AddComponent<HorizontalLayoutGroup>();
                tbHlg.spacing = 1;
                tbHlg.padding = new RectOffset(8, 8, 0, 0);
                tbHlg.childAlignment = TextAnchor.LowerLeft;
                tbHlg.childForceExpandWidth = false;
                tbHlg.childForceExpandHeight = false;

                for (int i = 0; i < TabLabels.Length; i++)
                {
                    int idx = i;
                    var tab = UIHelpers.Obj("Tab" + i, tabBar.transform);
                    var tabImg = tab.AddComponent<Image>();
                    tabImg.color = new Color(0, 0, 0, 0);
                    _tabBgs[i] = tabImg;
                    var tabLE = tab.AddComponent<LayoutElement>();
                    tabLE.preferredHeight = 30; tabLE.minHeight = 30;
                    tabLE.flexibleHeight = 0; tabLE.flexibleWidth = 0;

                    var tabHlg = tab.AddComponent<HorizontalLayoutGroup>();
                    tabHlg.padding = new RectOffset(12, 12, 0, 0);
                    tabHlg.childAlignment = TextAnchor.MiddleCenter;
                    tabHlg.childForceExpandWidth = false;
                    tabHlg.childForceExpandHeight = true;

                    var tabTxt = UIHelpers.Txt("T" + i, tab.transform, TabLabels[i], 11,
                        FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextDim);
                    _tabTxts[i] = tabTxt;

                    var btn = tab.AddComponent<Button>();
                    btn.targetGraphic = tabImg;
                    var bc = btn.colors;
                    bc.normalColor = Color.white;
                    bc.highlightedColor = new Color(1.05f, 1.05f, 1.05f, 1f);
                    bc.pressedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
                    bc.colorMultiplier = 1; btn.colors = bc;
                    btn.onClick.AddListener(() => SwitchTab(idx));
                }

                // ── Content area ──────────────────────────────────────
                var contentArea = UIHelpers.Obj("Content", pg.transform);
                var caLE = contentArea.AddComponent<LayoutElement>();
                caLE.flexibleHeight = 1; caLE.flexibleWidth = 1;
                UIHelpers.Fill(UIHelpers.RT(contentArea));

                // ── System page ───────────────────────────────────────
                _pgSystem = UIHelpers.Obj("PgSystem", contentArea.transform);
                UIHelpers.Fill(UIHelpers.RT(_pgSystem));
                BuildSystemPage(_pgSystem.transform);

                // ── Mod Status page ───────────────────────────────────
                _pgStatus = UIHelpers.Obj("PgStatus", contentArea.transform);
                UIHelpers.Fill(UIHelpers.RT(_pgStatus));
                BuildStatusPage(_pgStatus.transform);

                // ── Hotkeys page ──────────────────────────────────────
                _pgHotkeys = UIHelpers.Obj("PgHotkeys", contentArea.transform);
                UIHelpers.Fill(UIHelpers.RT(_pgHotkeys));
                BuildHotkeysPage(_pgHotkeys.transform);

                // ── Credits page ──────────────────────────────────────
                _pgCredits = UIHelpers.Obj("PgCredits", contentArea.transform);
                UIHelpers.Fill(UIHelpers.RT(_pgCredits));
                BuildCreditsPage(_pgCredits.transform);

                // ── Customise page ────────────────────────────────────
                _pgCustomise = UIHelpers.Obj("PgCustomise", contentArea.transform);
                UIHelpers.Fill(UIHelpers.RT(_pgCustomise));
                BuildCustomisePage(_pgCustomise.transform);

                // ── Scanner page ──────────────────────────────────────
                _pgScanner = UIHelpers.Obj("PgScanner", contentArea.transform);
                UIHelpers.Fill(UIHelpers.RT(_pgScanner));
                BuildScannerPage(_pgScanner.transform);

                SwitchTab(0);
                Refresh();
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("Page3UI.CreatePage: " + ex.Message);
                return null;
            }
            return pg;
        }

        // ── Tab switching ─────────────────────────────────────────────
        private static void SwitchTab(int idx)
        {
            _activeTab = idx;
            if ((object)_pgSystem != null) _pgSystem.SetActive(idx == 0);
            if ((object)_pgStatus != null) _pgStatus.SetActive(idx == 1);
            if ((object)_pgHotkeys != null) _pgHotkeys.SetActive(idx == 2);
            if ((object)_pgCredits != null) _pgCredits.SetActive(idx == 3);
            if ((object)_pgCustomise != null) _pgCustomise.SetActive(idx == 4);
            if ((object)_pgScanner != null) _pgScanner.SetActive(idx == 5);

            for (int i = 0; i < TabLabels.Length; i++)
            {
                bool active = i == idx;
                if ((object)_tabBgs[i] != null)
                    _tabBgs[i].color = active ? UIHelpers.RowBg : new Color(0, 0, 0, 0);
                if ((object)_tabTxts[i] != null)
                    _tabTxts[i].color = active ? UIHelpers.Accent : UIHelpers.TextDim;
            }

            // Refresh status data when switching to its tab
            if (idx == 1) RebuildStatusRows();
        }

        // ── System page ───────────────────────────────────────────────
        private static void BuildSystemPage(Transform p)
        {
            var vlg = UIHelpers.Obj("SysVlg", p);
            UIHelpers.Fill(UIHelpers.RT(vlg));
            var v = vlg.AddComponent<VerticalLayoutGroup>();
            v.spacing = UIHelpers.RowGap;
            v.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            v.childAlignment = TextAnchor.UpperCenter;
            v.childForceExpandWidth = true; v.childForceExpandHeight = false;

            UIHelpers.SectionHeader("ENGINE", vlg.transform);
            _unityVersionTxt = MakeInfoRow("Unity Version", vlg.transform);
            _unityMatchTxt = MakeInfoRow("Version Match", vlg.transform);
            _mlVersionTxt = MakeInfoRow("MelonLoader", vlg.transform);
            MakeInfoRow2("Scripting Backend", "Mono", vlg.transform);
            MakeInfoRow2("Build Target", ".NET 4.7.2", vlg.transform);

            UIHelpers.Divider(vlg.transform);
            UIHelpers.SectionHeader("TOOLKIT", vlg.transform);
            MakeInfoRow2("Version", BuildInfo.Version, vlg.transform, UIHelpers.Accent);
            MakeInfoRow2("Output DLL", "DescendersToolKit.dll", vlg.transform);
            MakeInfoRow2("Author", "NateHyden", vlg.transform);

            UIHelpers.Divider(vlg.transform);
            UIHelpers.SectionHeader("COMMUNITY", vlg.transform);
            _steamPlayerTxt = MakeInfoRow("Steam Players Online", vlg.transform);
        }

        // ── Mod Status page ───────────────────────────────────────────
        private static void BuildStatusPage(Transform p)
        {
            var vlg = UIHelpers.Obj("StatVlg", p);
            UIHelpers.Fill(UIHelpers.RT(vlg));
            var v = vlg.AddComponent<VerticalLayoutGroup>();
            v.spacing = UIHelpers.RowGap;
            v.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            v.childAlignment = TextAnchor.UpperCenter;
            v.childForceExpandWidth = true; v.childForceExpandHeight = false;

            UIHelpers.SectionHeader("SUMMARY", vlg.transform);
            _summaryTxt = MakeInfoRow("Status", vlg.transform);

            UIHelpers.Divider(vlg.transform);
            UIHelpers.SectionHeader("ALL MODS", vlg.transform);

            // Scrollable status list
            var scrollHost = UIHelpers.Obj("ScrollHost", vlg.transform);
            var hostLE = scrollHost.AddComponent<LayoutElement>();
            hostLE.flexibleHeight = 1; hostLE.minHeight = 100;
            scrollHost.AddComponent<Image>().color = new Color(0.118f, 0.118f, 0.133f, 1f);
            var sr = scrollHost.AddComponent<ScrollRect>();
            sr.horizontal = false; sr.vertical = true;
            sr.scrollSensitivity = 20f;
            sr.movementType = ScrollRect.MovementType.Clamped;
            sr.inertia = false;

            var vpGO = UIHelpers.Obj("Viewport", scrollHost.transform);
            var vpRT = UIHelpers.RT(vpGO);
            vpRT.anchorMin = Vector2.zero; vpRT.anchorMax = Vector2.one;
            vpRT.offsetMin = Vector2.zero; vpRT.offsetMax = Vector2.zero;
            vpGO.AddComponent<Image>().color = new Color(0.118f, 0.118f, 0.133f, 0.01f);
            vpGO.AddComponent<Mask>().showMaskGraphic = true;
            sr.viewport = vpRT;

            var contGO = UIHelpers.Obj("Content", vpGO.transform);
            _contentRT = UIHelpers.RT(contGO);
            _contentRT.anchorMin = new Vector2(0f, 1f);
            _contentRT.anchorMax = new Vector2(1f, 1f);
            _contentRT.pivot = new Vector2(0.5f, 1f);
            _contentRT.anchoredPosition = Vector2.zero;
            _contentRT.sizeDelta = new Vector2(0f, 130f);
            sr.content = _contentRT;

            var cVlg = contGO.AddComponent<VerticalLayoutGroup>();
            cVlg.spacing = ROW_GAP;
            cVlg.padding = new RectOffset((int)PAD, (int)PAD, (int)PAD, (int)PAD);
            cVlg.childForceExpandWidth = true; cVlg.childForceExpandHeight = false;
            cVlg.childAlignment = TextAnchor.UpperLeft;
        }

        // ── Hotkeys page ──────────────────────────────────────────────
        private static void BuildHotkeysPage(Transform p)
        {
            var vlg = UIHelpers.Obj("HkVlg", p);
            UIHelpers.Fill(UIHelpers.RT(vlg));
            var v = vlg.AddComponent<VerticalLayoutGroup>();
            v.spacing = UIHelpers.RowGap;
            v.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            v.childAlignment = TextAnchor.UpperCenter;
            v.childForceExpandWidth = true; v.childForceExpandHeight = false;

            UIHelpers.SectionHeader("MENU", vlg.transform);
            UIHelpers.HotkeyRow(vlg.transform, "Toggle mod menu", "F6");

            UIHelpers.SectionHeader("GAMEPLAY", vlg.transform);
            UIHelpers.HotkeyRow(vlg.transform, "Toggle slow motion", "F2");
            UIHelpers.HotkeyRow(vlg.transform, "Ghost Replay — toggle", "F3 / RS Dbl Click");
            UIHelpers.HotkeyRow(vlg.transform, "Ghost Replay — save run", "F4 / RS Click");
            UIHelpers.HotkeyRow(vlg.transform, "Ghost Replay — set spawn", "LS Click");

            UIHelpers.SectionHeader("DEVELOPER", vlg.transform);
            UIHelpers.HotkeyRow(vlg.transform, "Scene Dumper", "F12");
            UIHelpers.HotkeyRow(vlg.transform, "Speed Watcher", "F10 Hold");
        }

        // ── Credits page ──────────────────────────────────────────────
        private static void BuildCreditsPage(Transform p)
        {
            var vlg = UIHelpers.Obj("CredVlg", p);
            UIHelpers.Fill(UIHelpers.RT(vlg));
            var v = vlg.AddComponent<VerticalLayoutGroup>();
            v.spacing = UIHelpers.RowGap;
            v.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            v.childAlignment = TextAnchor.UpperCenter;
            v.childForceExpandWidth = true; v.childForceExpandHeight = false;

            // Hero block
            var hero = UIHelpers.Panel("Hero", vlg.transform, UIHelpers.AccentDim, UIHelpers.RowSp);
            var heroLE = hero.AddComponent<LayoutElement>();
            heroLE.preferredHeight = 72; heroLE.minHeight = 72;
            var heroBdr = UIHelpers.Panel("HBdr", hero.transform, UIHelpers.AccentBdr, UIHelpers.RowSp);
            heroBdr.GetComponent<Image>().raycastTarget = false;
            UIHelpers.Fill(UIHelpers.RT(heroBdr));
            heroBdr.AddComponent<LayoutElement>().ignoreLayout = true;
            var heroVlg = hero.AddComponent<VerticalLayoutGroup>();
            heroVlg.spacing = 4;
            heroVlg.padding = new RectOffset(16, 16, 12, 12);
            heroVlg.childAlignment = TextAnchor.UpperLeft;
            heroVlg.childForceExpandWidth = true; heroVlg.childForceExpandHeight = false;

            var heroTitle = UIHelpers.Txt("HT", hero.transform, "DESCENDERS TOOLKIT", 15,
                FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.Accent);
            heroTitle.gameObject.AddComponent<LayoutElement>().preferredHeight = 22;

            var heroAuthor = UIHelpers.Txt("HA", hero.transform, "Created by NateHyden", 11,
                FontStyle.Normal, TextAnchor.MiddleLeft, UIHelpers.TextLight);
            heroAuthor.gameObject.AddComponent<LayoutElement>().preferredHeight = 18;


            // Official links
            UIHelpers.SectionHeader("OFFICIAL LINKS", vlg.transform);
            MakeLinkRow("GitHub", "github.com/NateHyden/DescendersToolKit", vlg.transform);
            MakeLinkRow("Nexus Mods", "nexusmods.com/descenders/mods/7", vlg.transform);

            // Attribution
            UIHelpers.SectionHeader("ATTRIBUTION", vlg.transform);
            UIHelpers.InfoBox(vlg.transform,
                "If you downloaded this from anywhere other than the official GitHub or Nexus page above, you may have an outdated or modified version.");

            // Game credit
            UIHelpers.SectionHeader("GAME", vlg.transform);
            UIHelpers.InfoBox(vlg.transform,
                "Descenders is created by Ragesquid and published by No More Robots. " +
                "Huge credit to them for building such an incredible game that makes mods like this possible.");
            MakeInfoRow2("Developer", "Ragesquid", vlg.transform, UIHelpers.Accent);
            MakeInfoRow2("Publisher", "No More Robots", vlg.transform, UIHelpers.Accent);

            // Built with
            UIHelpers.SectionHeader("BUILT WITH", vlg.transform);
            MakeInfoRow2("MelonLoader", "0.5.7", vlg.transform);
            MakeInfoRow2("HarmonyLib", "2.x", vlg.transform);
            MakeInfoRow2("Unity Engine", "2017.4.40f1", vlg.transform);
        }

        // ── Customise page ────────────────────────────────────────────
        // ── Scanner page ──────────────────────────────────────────────
        private static void BuildScannerPage(Transform p)
        {
            var vlg = UIHelpers.Obj("ScanVlg", p);
            UIHelpers.Fill(UIHelpers.RT(vlg));
            var v = vlg.AddComponent<VerticalLayoutGroup>();
            v.spacing = UIHelpers.RowGap;
            v.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            v.childAlignment = TextAnchor.UpperCenter;
            v.childForceExpandWidth = true; v.childForceExpandHeight = false;

            UIHelpers.SectionHeader("DEV TOOLS — ASSEMBLY SCANNER", vlg.transform);
            UIHelpers.InfoBox(vlg.transform,
                "Checks every reflected field, property and method used by the mods.");
            UIHelpers.InfoBox(vlg.transform,
                "Run after a game update to confirm everything still works. " +
                "Report is saved to UserData/DescendersModMenu/AssemblyReport.txt");

            // Run button + summary on same row
            var btnRow = UIHelpers.StatRow("", vlg.transform);
            UIHelpers.ActionBtn(btnRow.transform, "Run Scan", () =>
            {
                Mods.AssemblyScanner.Run();
                RebuildScanResults();
            }, 72);
            _scanSummaryTxt = UIHelpers.Txt("ScanSum", btnRow.transform, "Not run yet",
                11, FontStyle.Normal, TextAnchor.MiddleLeft, UIHelpers.TextDim);
            _scanSummaryTxt.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;

            UIHelpers.Divider(vlg.transform);

            // Scrollable results list
            var scrollHost = UIHelpers.Obj("ScanScroll", vlg.transform);
            var hostLE = scrollHost.AddComponent<LayoutElement>();
            hostLE.flexibleHeight = 1; hostLE.minHeight = 100;
            scrollHost.AddComponent<Image>().color = new Color(0.118f, 0.118f, 0.133f, 1f);
            var sr = scrollHost.AddComponent<ScrollRect>();
            sr.horizontal = false; sr.vertical = true;
            sr.scrollSensitivity = 20f;
            sr.movementType = ScrollRect.MovementType.Clamped;
            sr.inertia = false;

            var vpGO = UIHelpers.Obj("Viewport", scrollHost.transform);
            var vpRT = UIHelpers.RT(vpGO);
            vpRT.anchorMin = Vector2.zero; vpRT.anchorMax = Vector2.one;
            vpRT.offsetMin = Vector2.zero; vpRT.offsetMax = Vector2.zero;
            vpGO.AddComponent<Image>().color = new Color(0.118f, 0.118f, 0.133f, 0.01f);
            vpGO.AddComponent<Mask>().showMaskGraphic = true;
            sr.viewport = vpRT;

            var contGO = UIHelpers.Obj("Content", vpGO.transform);
            _scanContentRT = UIHelpers.RT(contGO);
            _scanContentRT.anchorMin = new Vector2(0f, 1f);
            _scanContentRT.anchorMax = new Vector2(1f, 1f);
            _scanContentRT.pivot = new Vector2(0.5f, 1f);
            _scanContentRT.anchoredPosition = Vector2.zero;
            _scanContentRT.sizeDelta = new Vector2(0f, 100f);
            sr.content = _scanContentRT;

            var cVlg = contGO.AddComponent<VerticalLayoutGroup>();
            cVlg.spacing = 2f;
            cVlg.padding = new RectOffset(6, 6, 6, 6);
            cVlg.childForceExpandWidth = true; cVlg.childForceExpandHeight = false;
            cVlg.childAlignment = TextAnchor.UpperLeft;

            // File path row at bottom
            UIHelpers.Divider(vlg.transform);
            _scanFileTxt = UIHelpers.Txt("ScanFile", vlg.transform,
                "Report: UserData/DescendersModMenu/AssemblyReport.txt",
                9, FontStyle.Italic, TextAnchor.MiddleLeft, UIHelpers.TextDim);
            _scanFileTxt.gameObject.AddComponent<LayoutElement>().preferredHeight = 18;
        }

        private static void RebuildScanResults()
        {
            if ((object)_scanContentRT == null) return;

            // Clear existing rows
            for (int i = _scanContentRT.childCount - 1; i >= 0; i--)
                UnityEngine.Object.DestroyImmediate(_scanContentRT.GetChild(i).gameObject);

            var results = Mods.AssemblyScanner.Results;
            const float ROW_H = 22f;
            float totalH = 12f + results.Count * (ROW_H + 2f);
            _scanContentRT.sizeDelta = new Vector2(0f, totalH);

            string lastCat = "";
            for (int i = 0; i < results.Count; i++)
            {
                var r = results[i];

                // Category separator
                if (!string.Equals(r.Category, lastCat, System.StringComparison.Ordinal))
                {
                    var catRow = UIHelpers.Obj("Cat_" + r.Category, _scanContentRT);
                    var catle = catRow.AddComponent<LayoutElement>();
                    catle.preferredHeight = ROW_H; catle.minHeight = ROW_H;
                    var catTxt = UIHelpers.Txt("CatT", catRow.transform,
                        r.Category.ToUpper(), 10, FontStyle.Bold,
                        TextAnchor.MiddleLeft, UIHelpers.Accent);
                    UIHelpers.Fill(UIHelpers.RT(catTxt.gameObject));
                    lastCat = r.Category;
                    totalH += ROW_H + 2f;
                }

                var row = UIHelpers.Obj("R" + i, _scanContentRT);
                var rle = row.AddComponent<LayoutElement>();
                rle.preferredHeight = ROW_H; rle.minHeight = ROW_H; rle.flexibleHeight = 0;
                var hlg = row.AddComponent<HorizontalLayoutGroup>();
                hlg.spacing = 6;
                hlg.padding = new RectOffset(4, 4, 0, 0);
                hlg.childAlignment = TextAnchor.MiddleLeft;
                hlg.childForceExpandWidth = false; hlg.childForceExpandHeight = false;

                // WARN = OK but flagged (amber), FAIL = red, OK = green
                bool isWarn = r.OK && r.Detail.StartsWith("WARN", System.StringComparison.Ordinal);
                Color dotCol = isWarn ? UIHelpers.Orange : (r.OK ? UIHelpers.OnColor : UIHelpers.OffColor);
                Color textCol = isWarn ? UIHelpers.Orange : (r.OK ? UIHelpers.TextLight : UIHelpers.OffColor);

                // Dot
                var dot = UIHelpers.Obj("Dot", row.transform);
                var dotImg = dot.AddComponent<Image>();
                dotImg.sprite = UIHelpers.DotSp;
                dotImg.color = dotCol;
                var dle = dot.AddComponent<LayoutElement>();
                dle.preferredWidth = 8; dle.minWidth = 8;
                dle.preferredHeight = 8; dle.minHeight = 8;
                dle.flexibleWidth = 0; dle.flexibleHeight = 0;

                // Name — append detail for WARN/FAILED so user sees the reason
                string displayName = r.Name;
                if (isWarn)
                    displayName = r.Name + "  ·  " + r.Detail.Substring(6); // strip "WARN: "
                else if (!r.OK && r.Detail.Length > 0 && !string.Equals(r.Detail, "NOT FOUND", System.StringComparison.Ordinal))
                    displayName = r.Name + "  ·  " + r.Detail;
                var nameT = UIHelpers.Txt("N", row.transform, displayName, 10,
                    FontStyle.Normal, TextAnchor.MiddleLeft, textCol);
                nameT.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;

                // Status
                string stat = isWarn ? "WARN" : (r.OK ? "OK" : "FAILED");
                var statT = UIHelpers.Txt("S", row.transform, stat, 10,
                    FontStyle.Bold, TextAnchor.MiddleRight, dotCol);
                statT.gameObject.AddComponent<LayoutElement>().preferredWidth = 48;
            }

            _scanContentRT.sizeDelta = new Vector2(0f, totalH);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_scanContentRT);

            // Update summary
            if (_scanSummaryTxt)
            {
                int ok = Mods.AssemblyScanner.OKCount;
                int fail = Mods.AssemblyScanner.FailCount;
                _scanSummaryTxt.text = ok + " OK  " + (fail > 0 ? fail + " FAILED" : "0 failed");
                _scanSummaryTxt.color = fail > 0 ? UIHelpers.OffColor : UIHelpers.OnColor;
            }
        }

        private static void BuildCustomisePage(Transform p)
        {
            var vlg = UIHelpers.Obj("CustVlg", p);
            UIHelpers.Fill(UIHelpers.RT(vlg));
            var v = vlg.AddComponent<VerticalLayoutGroup>();
            v.spacing = UIHelpers.RowGap;
            v.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
            v.childAlignment = TextAnchor.UpperCenter;
            v.childForceExpandWidth = true; v.childForceExpandHeight = false;

            var c = vlg.transform;

            // ── How it works ──────────────────────────────────────────
            UIHelpers.SectionHeader("HOW IT WORKS", c);
            UIHelpers.InfoBox(c,
                "Your layout saves automatically whenever you make a change.");
            UIHelpers.InfoBox(c,
                "It loads back every time you launch the game — no manual save needed.");
            UIHelpers.InfoBox(c,
                "Use Reset to go back to the default position, scale and opacity.");

            UIHelpers.Divider(c);

            // ── Position ──────────────────────────────────────────────
            UIHelpers.SectionHeader("POSITION", c);

            var posRow = UIHelpers.StatRow("Position", c);
            UIHelpers.ActionBtn(posRow.transform, "Centre",
                () => { Mods.MenuCustomiser.SetPosition(0); RefreshCustomise(); }, 52);
            UIHelpers.ActionBtn(posRow.transform, "Top Left",
                () => { Mods.MenuCustomiser.SetPosition(1); RefreshCustomise(); }, 58);
            UIHelpers.ActionBtn(posRow.transform, "Top Right",
                () => { Mods.MenuCustomiser.SetPosition(2); RefreshCustomise(); }, 60);
            _custPosLbl = UIHelpers.Txt("CustPosV", posRow.transform,
                Mods.MenuCustomiser.PositionLabels[Mods.MenuCustomiser.PositionPreset],
                11, FontStyle.Bold, TextAnchor.MiddleRight, UIHelpers.Accent);
            _custPosLbl.gameObject.AddComponent<LayoutElement>().preferredWidth = 60;

            UIHelpers.Divider(c);

            // ── Scale ─────────────────────────────────────────────────
            UIHelpers.SectionHeader("SCALE", c);

            var scaleRow = UIHelpers.StatRow("Scale", c);
            UIHelpers.SmallBtn(scaleRow.transform, "\u25C0",
                () => { Mods.MenuCustomiser.PrevScale(); RefreshCustomise(); });
            _custScaleLbl = UIHelpers.Txt("CustScaleV", scaleRow.transform,
                Mods.MenuCustomiser.ScaleDisplay,
                12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
            _custScaleLbl.gameObject.AddComponent<LayoutElement>().preferredWidth = 52;
            UIHelpers.SmallBtn(scaleRow.transform, "\u25B6",
                () => { Mods.MenuCustomiser.NextScale(); RefreshCustomise(); });

            UIHelpers.Divider(c);

            // ── Opacity ───────────────────────────────────────────────
            UIHelpers.SectionHeader("OPACITY", c);

            var opacityRow = UIHelpers.StatRow("Opacity", c);
            UIHelpers.SmallBtn(opacityRow.transform, "\u25C0",
                () => { Mods.MenuCustomiser.PrevOpacity(); RefreshCustomise(); });
            _custOpacityLbl = UIHelpers.Txt("CustOpacityV", opacityRow.transform,
                Mods.MenuCustomiser.OpacityDisplay,
                12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
            _custOpacityLbl.gameObject.AddComponent<LayoutElement>().preferredWidth = 52;
            UIHelpers.SmallBtn(opacityRow.transform, "\u25B6",
                () => { Mods.MenuCustomiser.NextOpacity(); RefreshCustomise(); });

            UIHelpers.InfoBox(c, "Below 50% opacity the menu becomes hard to read.");

            UIHelpers.Divider(c);

            // ── Save / Reset buttons ──────────────────────────────────
            var btnRow = UIHelpers.StatRow("", c);
            UIHelpers.ActionBtn(btnRow.transform, "Save Now",
                () => { Mods.MenuCustomiser.SaveToFile(); }, 72);
            UIHelpers.ActionBtn(btnRow.transform, "Reset to Defaults",
                () => { Mods.MenuCustomiser.Reset(); RefreshCustomise(); }, 120);

            // ── Saved indicator (hidden until save fires) ─────────────
            _custSavedRow = UIHelpers.Obj("SavedIndicator", c);
            var siLE = _custSavedRow.AddComponent<LayoutElement>();
            siLE.preferredHeight = 22; siLE.minHeight = 22;
            var siHlg = _custSavedRow.AddComponent<HorizontalLayoutGroup>();
            siHlg.childAlignment = TextAnchor.MiddleCenter;
            siHlg.childForceExpandWidth = false;
            siHlg.childForceExpandHeight = false;
            siHlg.spacing = 6;

            var dot = UIHelpers.Txt("SavedDot", _custSavedRow.transform,
                "\u25CF", 10, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OnColor);
            dot.gameObject.AddComponent<LayoutElement>().preferredWidth = 12;

            var savedLbl = UIHelpers.Txt("SavedLbl", _custSavedRow.transform,
                "Layout saved", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OnColor);
            savedLbl.gameObject.AddComponent<LayoutElement>().preferredWidth = 80;

            _custSavedRow.SetActive(false);

            UIHelpers.AddScrollForwarders(c);
            RefreshCustomise();
        }

        private static void RefreshCustomise()
        {
            if (_custPosLbl)
                _custPosLbl.text = Mods.MenuCustomiser.PositionLabels[Mods.MenuCustomiser.PositionPreset];
            if (_custScaleLbl)
                _custScaleLbl.text = Mods.MenuCustomiser.ScaleDisplay;
            if (_custOpacityLbl)
                _custOpacityLbl.text = Mods.MenuCustomiser.OpacityDisplay;
        }

        // ── Helpers ───────────────────────────────────────────────────
        private static Text MakeInfoRow(string label, Transform parent)
        {
            var row = UIHelpers.Panel(label + "R", parent, UIHelpers.RowBg, UIHelpers.RowSp);
            var le = row.AddComponent<LayoutElement>();
            le.preferredHeight = 28; le.minHeight = 28; le.flexibleHeight = 0;
            var hlg = row.AddComponent<HorizontalLayoutGroup>();
            hlg.spacing = 8;
            hlg.padding = new RectOffset((int)UIHelpers.RowPad, (int)UIHelpers.RowPad, 0, 0);
            hlg.childAlignment = TextAnchor.MiddleCenter;
            hlg.childForceExpandWidth = false; hlg.childForceExpandHeight = false;
            var bd = UIHelpers.Panel("Bd", row.transform, UIHelpers.RowBorder, UIHelpers.RowSp);
            bd.GetComponent<Image>().raycastTarget = false;
            UIHelpers.Fill(UIHelpers.RT(bd));
            bd.AddComponent<LayoutElement>().ignoreLayout = true;
            var lbl = UIHelpers.Txt(label + "L", row.transform, label, 11,
                FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.TextLight);
            lbl.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
            var val = UIHelpers.Txt(label + "V", row.transform, "...", 11,
                FontStyle.Normal, TextAnchor.MiddleRight, UIHelpers.TextMid);
            val.gameObject.AddComponent<LayoutElement>().preferredWidth = 200;
            return val;
        }

        private static void MakeInfoRow2(string label, string value, Transform parent,
            Color? valueColor = null)
        {
            var row = UIHelpers.Panel(label + "R", parent, UIHelpers.RowBg, UIHelpers.RowSp);
            var le = row.AddComponent<LayoutElement>();
            le.preferredHeight = 28; le.minHeight = 28; le.flexibleHeight = 0;
            var hlg = row.AddComponent<HorizontalLayoutGroup>();
            hlg.spacing = 8;
            hlg.padding = new RectOffset((int)UIHelpers.RowPad, (int)UIHelpers.RowPad, 0, 0);
            hlg.childAlignment = TextAnchor.MiddleCenter;
            hlg.childForceExpandWidth = false; hlg.childForceExpandHeight = false;
            var bd = UIHelpers.Panel("Bd", row.transform, UIHelpers.RowBorder, UIHelpers.RowSp);
            bd.GetComponent<Image>().raycastTarget = false;
            UIHelpers.Fill(UIHelpers.RT(bd));
            bd.AddComponent<LayoutElement>().ignoreLayout = true;
            var lbl = UIHelpers.Txt(label + "L", row.transform, label, 11,
                FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.TextLight);
            lbl.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
            var val = UIHelpers.Txt(label + "V", row.transform, value, 11,
                FontStyle.Normal, TextAnchor.MiddleRight, valueColor ?? UIHelpers.TextMid);
            val.gameObject.AddComponent<LayoutElement>().preferredWidth = 200;
        }

        private static void MakeLinkRow(string label, string url, Transform parent)
        {
            var row = UIHelpers.Panel(label + "R", parent, UIHelpers.RowBg, UIHelpers.RowSp);
            var le = row.AddComponent<LayoutElement>();
            le.preferredHeight = 28; le.minHeight = 28; le.flexibleHeight = 0;
            var hlg = row.AddComponent<HorizontalLayoutGroup>();
            hlg.spacing = 8;
            hlg.padding = new RectOffset((int)UIHelpers.RowPad, (int)UIHelpers.RowPad, 0, 0);
            hlg.childAlignment = TextAnchor.MiddleCenter;
            hlg.childForceExpandWidth = false; hlg.childForceExpandHeight = false;
            var bd = UIHelpers.Panel("Bd", row.transform, UIHelpers.RowBorder, UIHelpers.RowSp);
            bd.GetComponent<Image>().raycastTarget = false;
            UIHelpers.Fill(UIHelpers.RT(bd));
            bd.AddComponent<LayoutElement>().ignoreLayout = true;
            var lbl = UIHelpers.Txt(label + "L", row.transform, label, 11,
                FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.TextLight);
            lbl.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
            var val = UIHelpers.Txt(label + "V", row.transform, url, 11,
                FontStyle.Bold, TextAnchor.MiddleRight, UIHelpers.NeonBlue);
            val.gameObject.AddComponent<LayoutElement>().preferredWidth = 280;
        }

        // ── Tick ──────────────────────────────────────────────────────
        public static void Tick()
        {
            if ((object)_custSavedRow != null)
                _custSavedRow.SetActive(Mods.MenuCustomiser.ShowSavedIndicator);

            // Refresh system tab once steam fetch completes
            if (_steamPlayerTxt && Mods.SteamPlayerCount.FetchComplete
                && _steamPlayerTxt.text == "...")
                Refresh();
        }

        // ── Refresh / Rebuild ─────────────────────────────────────────
        public static void Refresh()
        {
            try
            {
                // System tab values
                if (_unityVersionTxt) _unityVersionTxt.text = DiagnosticsManager.UnityVersion;
                if (_mlVersionTxt) _mlVersionTxt.text = DiagnosticsManager.MelonLoaderVersion;
                bool match = DiagnosticsManager.UnityVersionMatch;
                if (_unityMatchTxt)
                {
                    _unityMatchTxt.text = match
                        ? "OK \u2014 matches build target"
                        : "Mismatch! Built for " + DiagnosticsManager.BuiltForVersion;
                    _unityMatchTxt.color = match ? UIHelpers.OnColor : UIHelpers.OffColor;
                }

                // Summary
                int ok = DiagnosticsManager.OKCount, fail = DiagnosticsManager.FailCount;
                if (_summaryTxt)
                {
                    _summaryTxt.text = ok + " OK   " + (fail > 0 ? fail + " FAILED" : "0 failed");
                    _summaryTxt.color = fail > 0 ? UIHelpers.OffColor : UIHelpers.OnColor;
                }

                // Steam player count — updates once fetch completes
                if (_steamPlayerTxt)
                {
                    _steamPlayerTxt.text = Mods.SteamPlayerCount.DisplayValue;
                    _steamPlayerTxt.color = Mods.SteamPlayerCount.FetchFailed
                        ? UIHelpers.OffColor : UIHelpers.Accent;
                }

                // Only rebuild rows if status tab is visible
                if (_activeTab == 1) RebuildStatusRows();
            }
            catch (System.Exception ex) { MelonLogger.Error("Page3UI.Refresh: " + ex.Message); }
        }

        private static void RebuildStatusRows()
        {
            try
            {
                if ((object)_contentRT == null) return;

                for (int i = _contentRT.childCount - 1; i >= 0; i--)
                    UnityEngine.Object.Destroy(_contentRT.GetChild(i).gameObject);

                var statuses = DiagnosticsManager.Statuses;
                float totalH = PAD * 2 + statuses.Count * (ROW_H + ROW_GAP);
                _contentRT.sizeDelta = new Vector2(0f, totalH);

                for (int i = 0; i < statuses.Count; i++)
                {
                    var s = statuses[i];
                    var row = UIHelpers.Obj("R" + i, _contentRT);
                    var rle = row.AddComponent<LayoutElement>();
                    rle.preferredHeight = ROW_H; rle.minHeight = ROW_H; rle.flexibleHeight = 0;
                    var hlg = row.AddComponent<HorizontalLayoutGroup>();
                    hlg.spacing = 6;
                    hlg.padding = new RectOffset(2, 2, 0, 0);
                    hlg.childAlignment = TextAnchor.MiddleLeft;
                    hlg.childForceExpandWidth = false; hlg.childForceExpandHeight = false;

                    var dot = UIHelpers.Obj("Dot", row.transform);
                    var dotImg = dot.AddComponent<Image>();
                    dotImg.sprite = UIHelpers.DotSp;
                    dotImg.type = Image.Type.Simple;
                    dotImg.color = s.OK ? UIHelpers.OnColor : UIHelpers.OffColor;
                    var dle = dot.AddComponent<LayoutElement>();
                    dle.preferredWidth = 8; dle.minWidth = 8;
                    dle.preferredHeight = 8; dle.minHeight = 8;
                    dle.flexibleWidth = 0; dle.flexibleHeight = 0;

                    var nameT = UIHelpers.Txt("N", row.transform, s.Name, 10,
                        FontStyle.Bold, TextAnchor.MiddleLeft,
                        s.OK ? UIHelpers.TextLight : UIHelpers.OffColor);
                    nameT.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;

                    string stat = s.OK ? "OK"
                        : (string.IsNullOrEmpty(s.Error) ? "FAILED" : s.Error);
                    if (stat.Length > 25) stat = stat.Substring(0, 22) + "...";
                    var statT = UIHelpers.Txt("S", row.transform, stat, 9,
                        FontStyle.Normal, TextAnchor.MiddleRight,
                        s.OK ? UIHelpers.TextDim : UIHelpers.OffColor);
                    statT.gameObject.AddComponent<LayoutElement>().preferredWidth = 60;
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(_contentRT);
            }
            catch (System.Exception ex) { MelonLogger.Error("Page3UI.RebuildStatusRows: " + ex.Message); }
        }
    }
}