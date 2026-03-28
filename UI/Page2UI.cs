using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page2UI
    {
        private static Text espVal, distVal, tracVal;
        private static Image espTrk, distTrk, tracTrk;
        private static RectTransform espKnb, distKnb, tracKnb;
        private static Text _modUsersText;

        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
                pg = UIHelpers.Obj("P2R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));
                var vlg = pg.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = UIHelpers.RowGap;
                vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;

                UIHelpers.SectionHeader("ESP", pg.transform);

                var er = UIHelpers.StatRow("ESP", pg.transform);
                espVal = UIHelpers.Txt("EV", er.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                var evle = espVal.gameObject.AddComponent<LayoutElement>(); evle.preferredWidth = 28; evle.preferredHeight = 18; evle.flexibleHeight = 0;
                UIHelpers.Toggle(er.transform, "ET", () => { ESP.Toggle(); RefreshTexts(); }, out espTrk, out espKnb);

                var dr = UIHelpers.StatRow("ESP Distance", pg.transform);
                distVal = UIHelpers.Txt("DV", dr.transform, "ON", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OnColor);
                var dvle = distVal.gameObject.AddComponent<LayoutElement>(); dvle.preferredWidth = 28; dvle.preferredHeight = 18; dvle.flexibleHeight = 0;
                UIHelpers.Toggle(dr.transform, "DT", () => { ESP.ToggleDistance(); RefreshTexts(); }, out distTrk, out distKnb);

                var tr = UIHelpers.StatRow("ESP Tracers", pg.transform);
                tracVal = UIHelpers.Txt("TV", tr.transform, "ON", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OnColor);
                var tvle = tracVal.gameObject.AddComponent<LayoutElement>(); tvle.preferredWidth = 28; tvle.preferredHeight = 18; tvle.flexibleHeight = 0;
                UIHelpers.Toggle(tr.transform, "TT", () => { ESP.ToggleTracers(); RefreshTexts(); }, out tracTrk, out tracKnb);

                var rr = UIHelpers.StatRow("Refresh Targets", pg.transform);
                UIHelpers.ActionBtn(rr.transform, "Refresh", () => { ESP.RefreshNow(); RefreshTexts(); });

                UIHelpers.Divider(pg.transform);
                UIHelpers.SectionHeader("TELEPORT", pg.transform);

                var tpr = UIHelpers.StatRow("Player", pg.transform);
                UIHelpers.SmallBtn(tpr.transform, "\u25C0", () => TeleportUI.PreviousPlayer());

                var nb = UIHelpers.Panel("NB", tpr.transform, UIHelpers.RowBg, UIHelpers.BtnSp);
                var nle = nb.AddComponent<LayoutElement>(); nle.flexibleWidth = 1; nle.preferredHeight = 26; nle.flexibleHeight = 0; nle.minWidth = 180;
                var nbd = UIHelpers.Panel("NBd", nb.transform, UIHelpers.RowBorder, UIHelpers.BtnSp);
                nbd.GetComponent<Image>().raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(nbd));
                TeleportUI.PlayerNameText = UIHelpers.Txt("TPN", nb.transform,
                    "No players \u2014 scan first", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                TeleportUI.PlayerNameText.horizontalOverflow = HorizontalWrapMode.Wrap;
                TeleportUI.PlayerNameText.verticalOverflow = VerticalWrapMode.Truncate;
                UIHelpers.Fill(UIHelpers.RT(TeleportUI.PlayerNameText.gameObject), 4, 4, 0, 0);

                UIHelpers.SmallBtn(tpr.transform, "\u25B6", () => TeleportUI.NextPlayer());
                UIHelpers.ActionBtnOrange(tpr.transform, "Teleport", () => TeleportUI.TeleportToSelected(), 76);

                var sr = UIHelpers.StatRow("Find Players", pg.transform);
                UIHelpers.ActionBtn(sr.transform, "Scan", () => TeleportUI.Scan());

                UIHelpers.Divider(pg.transform);
                UIHelpers.SectionHeader("CHECKPOINT", pg.transform);

                var cpr = UIHelpers.StatRow("Last Checkpoint", pg.transform);
                UIHelpers.ActionBtnOrange(cpr.transform, "Teleport", () =>
                {
                    try { TeleportToCheckpoint.Teleport(); }
                    catch (System.Exception ex) { MelonLogger.Error("[TeleportCP]: " + ex.Message); }
                }, 76);

                UIHelpers.Divider(pg.transform);
                UIHelpers.SectionHeader("MOD USERS", pg.transform);

                var mdr = UIHelpers.StatRow("Detect Mod Users", pg.transform);
                UIHelpers.ActionBtn(mdr.transform, "Scan", () => { ModDetection.Scan(); RefreshModUsers(); }, 52);

                _modUsersText = UIHelpers.Txt("MUT", pg.transform, "Press Scan to detect mod users", 11,
                    FontStyle.Normal, TextAnchor.UpperLeft, UIHelpers.TextMid);
                _modUsersText.horizontalOverflow = HorizontalWrapMode.Wrap;
                _modUsersText.verticalOverflow = VerticalWrapMode.Truncate;
                var mutle = _modUsersText.gameObject.AddComponent<LayoutElement>();
                mutle.preferredHeight = 60; mutle.flexibleWidth = 1;

                RefreshTexts();

            }
            catch (System.Exception ex) { MelonLogger.Error("Page2UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        public static void RefreshTexts()
        {
            Upd(espVal, espTrk, espKnb, ESP.Enabled);
            Upd(distVal, distTrk, distKnb, ESP.ShowDistance);
            Upd(tracVal, tracTrk, tracKnb, ESP.ShowTracers);
        }

        private static void RefreshModUsers()
        {
            if ((object)_modUsersText == null) return;
            var users = ModDetection.ModUsers;
            if (users.Count == 0)
            {
                _modUsersText.text = "No mod users found in lobby";
                _modUsersText.color = UIHelpers.TextDim;
                return;
            }
            _modUsersText.color = UIHelpers.Accent;
            string txt = "";
            for (int i = 0; i < users.Count; i++)
            {
                if (i > 0) txt += "\n";
                txt += users[i].Name + "  [v" + users[i].Version + "]";
            }
            _modUsersText.text = txt;
        }

        private static void Upd(Text l, Image t, RectTransform k, bool on)
        {
            if (l) { l.text = on ? "ON" : "OFF"; l.color = on ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(t, k, on);
        }
    }

    public static class TeleportUI
    {
        public static Text PlayerNameText;
        private static int _i;
        private static System.Collections.Generic.List<TeleportToPlayer.PlayerEntry> _pl
            = new System.Collections.Generic.List<TeleportToPlayer.PlayerEntry>();

        public static void Scan()
        { _pl = TeleportToPlayer.ScanForPlayers(); _i = 0; UL(); MelonLogger.Msg("[TP] Scanned: " + _pl.Count); }
        public static void NextPlayer()
        { if (_pl.Count == 0) return; _i = (_i + 1) % _pl.Count; UL(); }
        public static void PreviousPlayer()
        { if (_pl.Count == 0) return; _i = (_i - 1 + _pl.Count) % _pl.Count; UL(); }
        public static void TeleportToSelected()
        {
            if (_pl.Count == 0) { MelonLogger.Warning("[TP] No players."); UL("Scan first!"); return; }
            var e = _pl[_i]; bool ok = TeleportToPlayer.TeleportTo(e);
            UL(ok ? "Teleported to " + e.Name + "!" : "Failed");
        }
        private static void UL(string ov = null)
        {
            if ((object)PlayerNameText == null) return;
            if (ov != null) { PlayerNameText.text = ov; return; }
            if (_pl.Count == 0) { PlayerNameText.text = "No players \u2014 press Scan"; return; }
            var e = _pl[_i]; PlayerNameText.text = "(" + (_i + 1) + "/" + _pl.Count + ") " + e.Name;
        }
    }
}