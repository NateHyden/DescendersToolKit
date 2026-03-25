using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page3UI
    {
        private static RectTransform _contentRT;
        private static Text _unityVersionTxt;
        private static Text _unityMatchTxt;
        private static Text _mlVersionTxt;
        private static Text _summaryTxt;

        private const float ROW_H = 22f;
        private const float ROW_GAP = 2f;
        private const float PAD = 6f;

        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
                pg = UIHelpers.Obj("P3R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));

                var vlg = pg.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = 4;
                vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 6, 6);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true;
                vlg.childForceExpandHeight = false;

                UIHelpers.SectionHeader("SYSTEM", pg.transform);
                _unityVersionTxt = MakeInfoRow("Unity Version", pg.transform);
                _unityMatchTxt = MakeInfoRow("Version Match", pg.transform);
                _mlVersionTxt = MakeInfoRow("MelonLoader", pg.transform);
                UIHelpers.Divider(pg.transform);

                UIHelpers.SectionHeader("MOD STATUS", pg.transform);
                _summaryTxt = MakeInfoRow("Summary", pg.transform);

                // ScrollHost in the VLG
                var scrollHost = UIHelpers.Obj("ScrollHost", pg.transform);
                var hostLE = scrollHost.AddComponent<LayoutElement>();
                hostLE.preferredHeight = 180;
                hostLE.minHeight = 180;
                hostLE.flexibleHeight = 0;
                scrollHost.AddComponent<Image>().color = new Color(0.118f, 0.118f, 0.133f, 1f);

                var sr = scrollHost.AddComponent<ScrollRect>();
                sr.horizontal = false; sr.vertical = true;
                sr.scrollSensitivity = 20f;
                sr.movementType = ScrollRect.MovementType.Clamped;
                sr.inertia = false;

                // Viewport
                var vpGO = UIHelpers.Obj("Viewport", scrollHost.transform);
                var vpRT = UIHelpers.RT(vpGO);
                vpRT.anchorMin = Vector2.zero; vpRT.anchorMax = Vector2.one;
                vpRT.offsetMin = Vector2.zero; vpRT.offsetMax = Vector2.zero;
                vpGO.AddComponent<Image>().color = new Color(0.118f, 0.118f, 0.133f, 0.01f);
                vpGO.AddComponent<Mask>().showMaskGraphic = true;
                sr.viewport = vpRT;

                // Content with VLG — force rebuild after population
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
                cVlg.childForceExpandWidth = true;
                cVlg.childForceExpandHeight = false;
                cVlg.childAlignment = TextAnchor.UpperLeft;

                UIHelpers.Divider(pg.transform);
                UIHelpers.SectionHeader("HOTKEYS", pg.transform);
                UIHelpers.HotkeyRow(pg.transform, "Toggle mod menu", "F6");
                UIHelpers.HotkeyRow(pg.transform, "Toggle slow motion", "F2");

                Refresh();
            }
            catch (System.Exception ex) { MelonLogger.Error("Page3UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

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

        public static void Refresh()
        {
            try
            {
                if (_unityVersionTxt) _unityVersionTxt.text = DiagnosticsManager.UnityVersion;
                if (_mlVersionTxt) _mlVersionTxt.text = DiagnosticsManager.MelonLoaderVersion;

                bool match = DiagnosticsManager.UnityVersionMatch;
                if (_unityMatchTxt)
                {
                    _unityMatchTxt.text = match ? "OK — matches build target" : "Mismatch! Built for " + DiagnosticsManager.BuiltForVersion;
                    _unityMatchTxt.color = match ? UIHelpers.OnColor : UIHelpers.OffColor;
                }

                int ok = DiagnosticsManager.OKCount, fail = DiagnosticsManager.FailCount;
                if (_summaryTxt)
                {
                    _summaryTxt.text = ok + " OK  " + (fail > 0 ? fail + " FAILED" : "0 failed");
                    _summaryTxt.color = fail > 0 ? UIHelpers.OffColor : UIHelpers.OnColor;
                }

                if ((object)_contentRT == null) return;

                // Clear old rows
                for (int i = _contentRT.childCount - 1; i >= 0; i--)
                    UnityEngine.Object.Destroy(_contentRT.GetChild(i).gameObject);

                var statuses = DiagnosticsManager.Statuses;

                // Set content height manually
                float totalH = PAD * 2 + statuses.Count * (ROW_H + ROW_GAP);
                _contentRT.sizeDelta = new Vector2(0f, totalH);

                // Build rows
                for (int i = 0; i < statuses.Count; i++)
                {
                    var s = statuses[i];

                    // Each row is a simple horizontal container
                    var row = UIHelpers.Obj("R" + i, _contentRT);
                    var rle = row.AddComponent<LayoutElement>();
                    rle.preferredHeight = ROW_H;
                    rle.minHeight = ROW_H;
                    rle.flexibleHeight = 0;

                    var hlg = row.AddComponent<HorizontalLayoutGroup>();
                    hlg.spacing = 6;
                    hlg.padding = new RectOffset(2, 2, 0, 0);
                    hlg.childAlignment = TextAnchor.MiddleLeft;
                    hlg.childForceExpandWidth = false;
                    hlg.childForceExpandHeight = false;

                    // Dot — proper circle using DotSp (Simple, not Sliced)
                    var dot = UIHelpers.Obj("Dot", row.transform);
                    var dotImg = dot.AddComponent<Image>();
                    dotImg.sprite = UIHelpers.DotSp;
                    dotImg.type = Image.Type.Simple;
                    dotImg.color = s.OK ? UIHelpers.OnColor : UIHelpers.OffColor;
                    var dle = dot.AddComponent<LayoutElement>();
                    dle.preferredWidth = 8; dle.minWidth = 8;
                    dle.preferredHeight = 8; dle.minHeight = 8;
                    dle.flexibleWidth = 0; dle.flexibleHeight = 0;

                    // Name
                    var nameT = UIHelpers.Txt("N", row.transform, s.Name, 10,
                        FontStyle.Bold, TextAnchor.MiddleLeft,
                        s.OK ? UIHelpers.TextLight : UIHelpers.OffColor);
                    nameT.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;

                    // Status
                    string stat = s.OK ? "OK" : (string.IsNullOrEmpty(s.Error) ? "FAILED" : s.Error);
                    if (stat.Length > 25) stat = stat.Substring(0, 22) + "...";
                    var statT = UIHelpers.Txt("S", row.transform, stat, 9,
                        FontStyle.Normal, TextAnchor.MiddleRight,
                        s.OK ? UIHelpers.TextDim : UIHelpers.OffColor);
                    statT.gameObject.AddComponent<LayoutElement>().preferredWidth = 60;
                }

                // Force Unity to recalculate the layout immediately
                LayoutRebuilder.ForceRebuildLayoutImmediate(_contentRT);
            }
            catch (System.Exception ex) { MelonLogger.Error("Page3UI.Refresh: " + ex.Message); }
        }
    }
}