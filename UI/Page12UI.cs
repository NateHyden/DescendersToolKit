using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using DescendersModMenu.Mods;

namespace DescendersModMenu.UI
{
    public static class Page12UI
    {
        private static ScrollRect _chatScroll = null;
        private static Transform _chatContent = null;
        private static string _inputBuffer = "";
        private static Text _inputText = null;
        private static Text _statusText = null;
        private static Text _onlineText = null;

        // How many chars fit visibly in the input box
        private const int DisplayChars = 48;

        public static void CreatePage(Transform parent)
        {
            try
            {
                // Root fills parent
                var pg = UIHelpers.Obj("P12R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));
                var root = pg.AddComponent<VerticalLayoutGroup>();
                root.spacing = UIHelpers.RowGap;
                root.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                root.childAlignment = TextAnchor.UpperLeft;
                root.childForceExpandWidth = true;
                root.childForceExpandHeight = false;

                // ── Header ────────────────────────────────────────────────
                UIHelpers.SectionHeader("MOD CHAT", pg.transform);

                // Online row
                var onlineRow = UIHelpers.Obj("ORow", pg.transform);
                onlineRow.AddComponent<Image>().color = UIHelpers.RowBg;
                var orLe = onlineRow.AddComponent<LayoutElement>();
                orLe.preferredHeight = 28; orLe.minHeight = 28;
                var orHlg = onlineRow.AddComponent<HorizontalLayoutGroup>();
                orHlg.padding = new RectOffset(10, 10, 0, 0);
                orHlg.childAlignment = TextAnchor.MiddleLeft;
                _onlineText = UIHelpers.Txt("OL", onlineRow.transform,
                    "● 0 mod users online in this session",
                    10, FontStyle.Normal, TextAnchor.MiddleLeft, UIHelpers.OnColor);

                // ── Chat box ──────────────────────────────────────────────
                var chatBox = UIHelpers.Obj("ChatBox", pg.transform);
                chatBox.AddComponent<Image>().color = UIHelpers.WinPanel;
                var cbLe = chatBox.AddComponent<LayoutElement>();
                cbLe.preferredHeight = 320; cbLe.minHeight = 200;

                var scrollObj = UIHelpers.Obj("Scroll", chatBox.transform);
                UIHelpers.Fill(UIHelpers.RT(scrollObj));
                _chatScroll = scrollObj.AddComponent<ScrollRect>();
                _chatScroll.horizontal = false; _chatScroll.vertical = true;
                _chatScroll.movementType = ScrollRect.MovementType.Clamped;
                _chatScroll.scrollSensitivity = 30f; _chatScroll.inertia = false;

                var vp = UIHelpers.Obj("VP", scrollObj.transform);
                UIHelpers.Fill(UIHelpers.RT(vp));
                vp.AddComponent<Image>().color = new Color(0, 0, 0, 0.01f);
                vp.AddComponent<Mask>().showMaskGraphic = true;
                _chatScroll.viewport = UIHelpers.RT(vp);

                var content = UIHelpers.Obj("Content", vp.transform);
                var crt = UIHelpers.RT(content);
                crt.anchorMin = new Vector2(0, 1); crt.anchorMax = new Vector2(1, 1);
                crt.pivot = new Vector2(0.5f, 1); crt.sizeDelta = Vector2.zero;
                _chatScroll.content = crt;
                content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                var cvlg = content.AddComponent<VerticalLayoutGroup>();
                cvlg.spacing = 1; cvlg.padding = new RectOffset(6, 6, 4, 4);
                cvlg.childAlignment = TextAnchor.UpperLeft;
                cvlg.childForceExpandWidth = true; cvlg.childForceExpandHeight = false;
                _chatContent = content.transform;

                // ── Input row — built manually, no StatRow ────────────────
                var inputRow = UIHelpers.Obj("InputRow", pg.transform);
                inputRow.AddComponent<Image>().color = UIHelpers.RowBg;
                var irLe = inputRow.AddComponent<LayoutElement>();
                irLe.preferredHeight = 36; irLe.minHeight = 36;
                var irHlg = inputRow.AddComponent<HorizontalLayoutGroup>();
                irHlg.padding = new RectOffset(8, 8, 4, 4);
                irHlg.spacing = 6;
                irHlg.childAlignment = TextAnchor.MiddleLeft;
                irHlg.childForceExpandHeight = true;
                irHlg.childForceExpandWidth = false;

                // Input text background — fixed on left, takes remaining width
                var inputBg = UIHelpers.Obj("IB", inputRow.transform);
                inputBg.AddComponent<Image>().color = UIHelpers.WinOuter;
                var ibLe = inputBg.AddComponent<LayoutElement>();
                ibLe.flexibleWidth = 1; ibLe.minHeight = 26; ibLe.preferredHeight = 26;
                var ibHlg = inputBg.AddComponent<HorizontalLayoutGroup>();
                ibHlg.padding = new RectOffset(8, 8, 0, 0);
                ibHlg.childAlignment = TextAnchor.MiddleLeft;
                ibHlg.childForceExpandWidth = true;
                ibHlg.childForceExpandHeight = true;

                // Text — never expands layout, just draws
                _inputText = UIHelpers.Txt("IT", inputBg.transform, "Type a message...",
                    11, FontStyle.Normal, TextAnchor.MiddleLeft, UIHelpers.TextDim);
                _inputText.horizontalOverflow = HorizontalWrapMode.Wrap;
                _inputText.verticalOverflow = VerticalWrapMode.Truncate;
                // Don't let text drive layout width
                var itLe = _inputText.gameObject.AddComponent<LayoutElement>();
                itLe.flexibleWidth = 1; itLe.minWidth = 0;

                // Send button — fixed 70px on right
                var sendBtn = UIHelpers.Btn("SB", inputRow.transform, "SEND",
                    new Vector2(70, 26), 12, () => { SendMessage(); },
                    UIHelpers.NeonBlue, Color.black);
                var sbLe = sendBtn.gameObject.AddComponent<LayoutElement>();
                sbLe.preferredWidth = 70; sbLe.minWidth = 70;
                sbLe.preferredHeight = 26; sbLe.minHeight = 26;

                // ── Status ────────────────────────────────────────────────
                var statusRow = UIHelpers.Obj("SR", pg.transform);
                statusRow.AddComponent<Image>().color = UIHelpers.RowBg;
                var srLe = statusRow.AddComponent<LayoutElement>();
                srLe.preferredHeight = 22; srLe.minHeight = 22;
                var srHlg = statusRow.AddComponent<HorizontalLayoutGroup>();
                srHlg.padding = new RectOffset(10, 10, 0, 0);
                srHlg.childAlignment = TextAnchor.MiddleLeft;
                _statusText = UIHelpers.Txt("ST", statusRow.transform,
                    "0/" + ModChat.MaxLength + " • Enter to send • Only visible to mod users",
                    9, FontStyle.Italic, TextAnchor.MiddleLeft, UIHelpers.TextDim);

                RebuildMessages();
            }
            catch (System.Exception ex) { MelonLogger.Error("Page12UI: " + ex.Message); }
        }

        public static void Tick()
        {
            if (ModChat.HasNewMessages) { RebuildMessages(); ModChat.ClearNewFlag(); }
            if ((object)_inputText == null) return;

            foreach (char c in Input.inputString)
            {
                if (c == '\b') { if (_inputBuffer.Length > 0) _inputBuffer = _inputBuffer.Substring(0, _inputBuffer.Length - 1); }
                else if (c == '\n' || c == '\r') { SendMessage(); return; }
                else if (_inputBuffer.Length < ModChat.MaxLength) _inputBuffer += c;
            }

            if (_inputBuffer.Length > 0)
            {
                // Show last DisplayChars chars so text stays visible on right — no layout shift
                string display = _inputBuffer.Length > DisplayChars
                    ? _inputBuffer.Substring(_inputBuffer.Length - DisplayChars)
                    : _inputBuffer;
                _inputText.text = display + "_";
                _inputText.color = UIHelpers.TextLight;
            }
            else
            {
                _inputText.text = "Type a message...";
                _inputText.color = UIHelpers.TextDim;
            }

            if (_statusText) _statusText.text = _inputBuffer.Length + "/" + ModChat.MaxLength + " • Enter to send • Only visible to mod users";
            if (_onlineText) _onlineText.text = "● " + ModDetection.ModUsers.Count + " mod users online in this session";
        }

        private static void SendMessage()
        {
            string msg = _inputBuffer.Trim();
            _inputBuffer = "";
            if (string.IsNullOrEmpty(msg)) return;
            ModChat.Send(msg);
        }

        private static void RebuildMessages()
        {
            if ((object)_chatContent == null) return;
            for (int i = _chatContent.childCount - 1; i >= 0; i--)
                GameObject.Destroy(_chatContent.GetChild(i).gameObject);

            foreach (var msg in ModChat.Messages)
            {
                var row = UIHelpers.Obj("MR", _chatContent);
                row.AddComponent<Image>().color = msg.IsSelf ? new Color(0f, 0.16f, 0.32f, 0.35f) : new Color(0, 0, 0, 0);
                var le = row.AddComponent<LayoutElement>(); le.minHeight = 22; le.preferredHeight = 22;
                var hlg = row.AddComponent<HorizontalLayoutGroup>();
                hlg.padding = new RectOffset(msg.IsSelf ? 4 : 2, 2, 2, 2);
                hlg.spacing = 4; hlg.childAlignment = TextAnchor.MiddleLeft;
                hlg.childForceExpandHeight = false;
                hlg.childForceExpandWidth = false;

                if (msg.IsSelf)
                {
                    var bar = UIHelpers.Obj("B", row.transform);
                    bar.AddComponent<Image>().color = UIHelpers.NeonBlue;
                    bar.AddComponent<LayoutElement>().preferredWidth = 2;
                }

                UIHelpers.Txt("T", row.transform, msg.Time, 9, FontStyle.Normal, TextAnchor.MiddleLeft, UIHelpers.TextDim).gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Txt("G", row.transform, "[MOD]", 9, FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.Accent).gameObject.AddComponent<LayoutElement>().preferredWidth = 32;
                UIHelpers.Txt("N", row.transform, msg.PlayerName, 11, FontStyle.Bold, TextAnchor.MiddleLeft, msg.IsSelf ? UIHelpers.NeonBlue : UIHelpers.Orange).gameObject.AddComponent<LayoutElement>().preferredWidth = 80;
                UIHelpers.Txt("M", row.transform, msg.Text, 11, FontStyle.Normal, TextAnchor.MiddleLeft, UIHelpers.TextLight).gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
            }

            Canvas.ForceUpdateCanvases();
            if ((object)_chatScroll != null) _chatScroll.verticalNormalizedPosition = 0f;
        }

        public static void RefreshAll() => RebuildMessages();
    }
}