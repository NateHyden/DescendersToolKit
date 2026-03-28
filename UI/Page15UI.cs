using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using DescendersModMenu.Mods;

namespace DescendersModMenu.UI
{
    public static class Page15UI
    {
        private static Transform _listRoot = null;
        private static Text _statusText = null;

        public static void CreatePage(Transform parent)
        {
            try
            {
                var pg = UIHelpers.Obj("P15R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));

                var scrollObj = UIHelpers.Obj("Scroll", pg.transform);
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
                crt.pivot = new Vector2(0.5f, 1); crt.sizeDelta = Vector2.zero;
                sr.content = crt;
                content.AddComponent<ContentSizeFitter>().verticalFit =
                    ContentSizeFitter.FitMode.PreferredSize;
                var vlg = content.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = UIHelpers.RowGap;
                vlg.padding = new RectOffset(
                    (int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true;
                vlg.childForceExpandHeight = false;

                _listRoot = content.transform;

                RebuildList();
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("Page15UI.CreatePage: " + ex.Message);
            }
        }

        public static void RebuildList()
        {
            if ((object)_listRoot == null) return;
            try
            {
                // Clear everything
                while (_listRoot.childCount > 0)
                    GameObject.DestroyImmediate(_listRoot.GetChild(0).gameObject);

                UIHelpers.SectionHeader("MAP CHANGER", _listRoot);

                // Status row
                var statusRow = UIHelpers.StatRow("Maps", _listRoot);
                _statusText = UIHelpers.Txt("Status", statusRow.transform,
                    MapChanger.HasBikeParks
                        ? "Base + Bike Parks"
                        : "Open Freeride to scan parks",
                    11, FontStyle.Normal, TextAnchor.MiddleRight,
                    MapChanger.HasBikeParks ? UIHelpers.OnColor : UIHelpers.TextDim);
                _statusText.gameObject.AddComponent<LayoutElement>().preferredWidth = 150;

                UIHelpers.Divider(_listRoot);

                // Hint if bike parks not yet scanned
                if (!MapChanger.HasBikeParks)
                {
                    var hintRow = UIHelpers.Obj("HintRow", _listRoot);
                    hintRow.AddComponent<LayoutElement>().minHeight = 28;
                    var htxt = UIHelpers.Txt("HintTxt", hintRow.transform,
                        "Go to  Ride \u2192 Bike Parks  once to load all parks into this list",
                        10, FontStyle.Normal, TextAnchor.MiddleCenter, UIHelpers.TextDim);
                    htxt.horizontalOverflow = HorizontalWrapMode.Wrap;
                    UIHelpers.Fill(UIHelpers.RT(htxt.gameObject));
                    UIHelpers.Divider(_listRoot);
                }

                // Base worlds section
                UIHelpers.SectionHeader("BASE GAME MAPS", _listRoot);
                for (int i = 0; i < MapChanger.Count; i++)
                {
                    var entry = MapChanger.GetEntry(i);
                    if (!entry.IsBikePark)
                        BuildMapRow(i);
                }

                // Bike parks section — only if found
                if (MapChanger.HasBikeParks)
                {
                    UIHelpers.Divider(_listRoot);
                    UIHelpers.SectionHeader("BIKE PARKS & FREERIDE", _listRoot);
                    for (int i = 0; i < MapChanger.Count; i++)
                    {
                        var entry = MapChanger.GetEntry(i);
                        if (entry.IsBikePark)
                            BuildMapRow(i);
                    }
                }

                UIHelpers.AddScrollForwarders(_listRoot);
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("Page15UI.RebuildList: " + ex.Message);
            }
        }

        private static void BuildMapRow(int i)
        {
            int idx = i;
            var row = UIHelpers.StatRow(MapChanger.GetName(i), _listRoot);

            var goBtn = UIHelpers.Btn("Go" + i, row.transform, "GO",
                new Vector2(52, 30), 12,
                () =>
                {
                    SetStatus("LOADING " + MapChanger.GetName(idx) + "...", UIHelpers.Orange);
                    MapChanger.GoToMap(idx);
                },
                UIHelpers.Orange, Color.black);

            var le = goBtn.gameObject.AddComponent<LayoutElement>();
            le.preferredWidth = 52; le.minWidth = 52;
            le.preferredHeight = 30; le.minHeight = 30;
        }

        private static void SetStatus(string msg, Color col)
        {
            if ((object)_statusText == null) return;
            _statusText.text = msg;
            _statusText.color = col;
        }

        public static void RefreshAll()
        {
            MapChanger.BuildMapList();
            RebuildList();
        }
    }
}