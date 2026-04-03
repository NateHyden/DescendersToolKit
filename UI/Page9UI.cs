using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page9UI
    {
        // ── Invisible Player ──────────────────────────────────────────
        private static bool _invisiblePlayer = false;
        private static Renderer[] _hiddenPlayerRenderers = null;
        private static Image _invisTrack; private static RectTransform _invisKnob;
        private static Text _invisVal;

        // ── Moon Mode ─────────────────────────────────────────────────
        private static bool _moonModeActive = false;
        private static Image _moonBg, _moonBdr;
        private static Text _moonTxt;
        private static int _savedGravityLevel = -1;
        private static int _savedTravelLevel = -1;
        private static int _savedDampingLevel = -1;

        // ── Set Player Name ───────────────────────────────────────
        private static string _nameBuffer = "";
        private static Text _nameInputText = null;
        private static Text _nameCursor = null;
        private static Text _nameActiveText = null;
        private static string _activeName = "";
        private const int NameMaxLength = 20;
        private const int NameDisplayChars = 18;

        // ── Camera Shake ─────────────────────────────────────────────
        private static Text _shakeVal, _shakeTogVal;
        private static Image _shakeBar, _shakeTrack;
        private static RectTransform _shakeKnob;

        // ── Drunk / Fly / Mirror ──────────────────────────────────────
        private static Image _drunkTrack; private static RectTransform _drunkKnob; private static Text _drunkVal;
        private static Image _flyTrack; private static RectTransform _flyKnob; private static Text _flyVal;
        private static Image _mirrorTrack; private static RectTransform _mirrorKnob; private static Text _mirrorVal;

        // ── Row GO refs for highlight ─────────────────────────────────
        private static GameObject _invisPlayerRow, _mirrorRow, _flyRow, _drunkRow;

        // ── Default scale capture ─────────────────────────────────────
        private static Vector3 _defaultPlayerScale = Vector3.one;
        private static bool _playerScaleCaptured = false;

        public static void CaptureSceneDefaults()
        {
            _playerScaleCaptured = false;
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                Transform cy = player.transform.Find("Cyclist");
                if ((object)cy != null) { _defaultPlayerScale = cy.localScale; _playerScaleCaptured = true; }
            }
            catch { }
        }

        public static bool IsAnyActive =>
            _invisiblePlayer || _moonModeActive ||
            MirrorMode.Enabled || FlyMode.Enabled || DrunkMode.Enabled ||
            CameraShake.Enabled;

        public static void GlobalReset()
        {
            if (_invisiblePlayer) { ToggleInvisible(false); _invisiblePlayer = false; }
            if (_moonModeActive) ToggleMoonMode();
            ResetPlayerScaleToDefault();
        }

        // ─────────────────────────────────────────────────────────────
        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
                pg = UIHelpers.Obj("P9R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));

                // ScrollRect wrapper
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
                crt.pivot = new Vector2(0.5f, 1); crt.sizeDelta = new Vector2(0, 0);
                sr.content = crt;
                content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                var vlg = content.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = UIHelpers.RowGap;
                vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;

                var pg9 = content.transform;

                // ── RESET TAB ─────────────────────────────────────────
                var rstRow = UIHelpers.StatRow("", pg9);
                UIHelpers.ActionBtnOrange(rstRow.transform, "↺  Reset Tab to Defaults", () => { GlobalReset(); RefreshAll(); }, 186);
                UIHelpers.SectionHeader("PLAYER SIZE", pg9);
                var psr = UIHelpers.StatRow("Size", pg9);
                UIHelpers.ActionBtn(psr.transform, "Giant", () => SetPlayerScale(3.5f), 52);
                UIHelpers.ActionBtn(psr.transform, "Big", () => SetPlayerScale(1.5f), 44);
                UIHelpers.ActionBtn(psr.transform, "Default", () => ResetPlayerScaleToDefault(), 58);
                UIHelpers.ActionBtn(psr.transform, "Small", () => SetPlayerScale(0.6f), 52);
                UIHelpers.ActionBtn(psr.transform, "Tiny", () => SetPlayerScale(0.2f), 44);
                UIHelpers.Divider(pg9);

                // ── PRESETS ───────────────────────────────────────────
                UIHelpers.SectionHeader("PRESETS", pg9);

                // Moon Mode compound row
                var mmo = UIHelpers.Panel("MMR", pg9, UIHelpers.RowBg, UIHelpers.RowSp);
                mmo.AddComponent<LayoutElement>().minHeight = UIHelpers.RowH + 38;
                var mmbd = UIHelpers.Panel("MMBd", mmo.transform, UIHelpers.RowBorder, UIHelpers.RowSp);
                mmbd.GetComponent<Image>().raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(mmbd));
                mmbd.AddComponent<LayoutElement>().ignoreLayout = true;
                var mmvlg = mmo.AddComponent<VerticalLayoutGroup>();
                mmvlg.spacing = 4; mmvlg.padding = new RectOffset((int)UIHelpers.RowPad, (int)UIHelpers.RowPad, 6, 8);
                mmvlg.childAlignment = TextAnchor.UpperCenter;
                mmvlg.childForceExpandWidth = true; mmvlg.childForceExpandHeight = false;
                var mmtop = UIHelpers.Obj("MMTop", mmo.transform);
                mmtop.AddComponent<LayoutElement>().preferredHeight = 28;
                var mmhlg = mmtop.AddComponent<HorizontalLayoutGroup>();
                mmhlg.spacing = 8; mmhlg.childAlignment = TextAnchor.MiddleCenter;
                mmhlg.childForceExpandWidth = false; mmhlg.childForceExpandHeight = false;
                var mml = UIHelpers.Txt("MML", mmtop.transform, "Moon Mode", 12, FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.TextLight);
                mml.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
                var mmdesc = UIHelpers.Txt("MMD", mmtop.transform, "Low gravity + bouncy suspension", 10, FontStyle.Italic, TextAnchor.MiddleRight, UIHelpers.TextDim);
                mmdesc.gameObject.AddComponent<LayoutElement>().preferredWidth = 200;
                var mmBtn = UIHelpers.Obj("MMBtn", mmo.transform);
                _moonBg = mmBtn.AddComponent<Image>(); _moonBg.sprite = UIHelpers.BtnSp;
                _moonBg.type = Image.Type.Sliced; _moonBg.color = UIHelpers.NeonBlue;
                var mbtn = mmBtn.AddComponent<Button>();
                mbtn.onClick.AddListener(() => { ToggleMoonMode(); RefreshAll(); });
                var mcb = mbtn.colors;
                mcb.normalColor = Color.white; mcb.highlightedColor = new Color(1, 1, 1, 1.15f);
                mcb.pressedColor = new Color(.7f, .7f, .7f, 1); mcb.colorMultiplier = 1; mcb.fadeDuration = .08f;
                mbtn.colors = mcb;
                mmBtn.AddComponent<LayoutElement>().preferredHeight = 30;
                var mbdr = UIHelpers.Panel("MBdr", mmBtn.transform, UIHelpers.NeonBlue, UIHelpers.BtnSp);
                _moonBdr = mbdr.GetComponent<Image>(); _moonBdr.raycastTarget = false;
                UIHelpers.Fill(UIHelpers.RT(mbdr));
                _moonTxt = UIHelpers.Txt("MT", mmBtn.transform, "ACTIVATE MOON MODE", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, new Color(0, 0, 0, 1));
                _moonTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
                UIHelpers.Fill(UIHelpers.RT(_moonTxt.gameObject));

                UIHelpers.Divider(pg9);

                // ── MULTIPLAYER ───────────────────────────────────────
                UIHelpers.SectionHeader("MULTIPLAYER", pg9);
                var gsr = UIHelpers.StatRow("Giant Everyone", pg9);
                UIHelpers.ActionBtnOrange(gsr.transform, "Colossal", () => SetAllPlayersScale(6.0f), 62);
                UIHelpers.ActionBtnOrange(gsr.transform, "Giant", () => SetAllPlayersScale(3.5f), 52);
                UIHelpers.ActionBtn(gsr.transform, "Big", () => SetAllPlayersScale(1.5f), 42);
                UIHelpers.ActionBtn(gsr.transform, "Default", () => SetAllPlayersScale(1.0f), 58);
                UIHelpers.ActionBtn(gsr.transform, "Small", () => SetAllPlayersScale(0.6f), 48);
                UIHelpers.ActionBtn(gsr.transform, "Tiny", () => SetAllPlayersScale(0.2f), 44);
                UIHelpers.ActionBtn(gsr.transform, "Micro", () => SetAllPlayersScale(0.05f), 48);

                UIHelpers.Divider(pg9);

                // ── EFFECTS ───────────────────────────────────────────
                UIHelpers.SectionHeader("EFFECTS", pg9);

                _mirrorRow = UIHelpers.StatRow("Mirror Mode", pg9);
                var mmr = _mirrorRow;
                _mirrorVal = UIHelpers.Txt("MmV", mmr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _mirrorVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(mmr.transform, "MmT", () => { MirrorMode.Toggle(); RefreshAll(); }, out _mirrorTrack, out _mirrorKnob);

                _flyRow = UIHelpers.StatRow("Fly Mode", pg9);
                var flyr = _flyRow;
                _flyVal = UIHelpers.Txt("FlV", flyr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _flyVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(flyr.transform, "FlT", () => { FlyMode.Toggle(); RefreshAll(); }, out _flyTrack, out _flyKnob);

                _drunkRow = UIHelpers.StatRow("Drunk Mode", pg9);
                var drnkr = _drunkRow;
                _drunkVal = UIHelpers.Txt("DrV", drnkr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _drunkVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(drnkr.transform, "DrT", () => { DrunkMode.Toggle(); RefreshAll(); }, out _drunkTrack, out _drunkKnob);

                UIHelpers.Divider(pg9);

                // ── CAMERA ────────────────────────────────────────────
                UIHelpers.SectionHeader("CAMERA", pg9);

                var csr = UIHelpers.StatRow("Camera Shake", pg9);
                _shakeBar = UIHelpers.MakeBar("ShB", csr.transform, (CameraShake.Level - 1) / 9f);
                _shakeVal = UIHelpers.Txt("ShV", csr.transform, CameraShake.DisplayValue, 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                _shakeVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                _shakeTogVal = UIHelpers.Txt("ShTV", csr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _shakeTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                Image shakeTrack; RectTransform shakeKnob;
                UIHelpers.Toggle(csr.transform, "ShT", () => { CameraShake.Toggle(); RefreshAll(); }, out shakeTrack, out shakeKnob);
                UIHelpers.SmallBtn(csr.transform, "-", () => { CameraShake.Decrease(); RefreshAll(); });
                UIHelpers.SmallBtn(csr.transform, "+", () => { CameraShake.Increase(); RefreshAll(); });
                _shakeTrack = shakeTrack; _shakeKnob = shakeKnob;
                UIHelpers.InfoBox(pg9, "Level 5 = default. Amplifies camera shake at speed. Level 10 = 4x default.");

                UIHelpers.Divider(pg9);

                // ── PLAYER ────────────────────────────────────────────
                UIHelpers.SectionHeader("PLAYER", pg9);

                _invisPlayerRow = UIHelpers.StatRow("Invisible Player", pg9);
                var ir = _invisPlayerRow;
                _invisVal = UIHelpers.Txt("InV", ir.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _invisVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(ir.transform, "InT", () =>
                {
                    _invisiblePlayer = !_invisiblePlayer;
                    ToggleInvisible(_invisiblePlayer);
                    RefreshAll();
                }, out _invisTrack, out _invisKnob);

                UIHelpers.Divider(pg9);

                // ── IDENTITY ──────────────────────────────────────────
                UIHelpers.SectionHeader("IDENTITY", pg9);

                // Input row
                var nameInputRow = UIHelpers.Obj("NameInputRow", pg9);
                nameInputRow.AddComponent<UnityEngine.UI.Image>().color = UIHelpers.RowBg;
                var nirLe = nameInputRow.AddComponent<LayoutElement>();
                nirLe.preferredHeight = 36; nirLe.minHeight = 36;
                var nirHlg = nameInputRow.AddComponent<HorizontalLayoutGroup>();
                nirHlg.padding = new RectOffset(8, 8, 4, 4);
                nirHlg.spacing = 6; nirHlg.childAlignment = TextAnchor.MiddleLeft;
                nirHlg.childForceExpandHeight = true; nirHlg.childForceExpandWidth = false;

                var nameBg = UIHelpers.Obj("NmBg", nameInputRow.transform);
                nameBg.AddComponent<UnityEngine.UI.Image>().color = UIHelpers.WinOuter;
                var nbgLe = nameBg.AddComponent<LayoutElement>();
                nbgLe.flexibleWidth = 1; nbgLe.minHeight = 26; nbgLe.preferredHeight = 26;
                var nbgHlg = nameBg.AddComponent<HorizontalLayoutGroup>();
                nbgHlg.padding = new RectOffset(8, 8, 0, 0);
                nbgHlg.childAlignment = TextAnchor.MiddleLeft;
                nbgHlg.childForceExpandWidth = true; nbgHlg.childForceExpandHeight = true;

                _nameInputText = UIHelpers.Txt("NmIT", nameBg.transform, "Enter name...",
                    11, FontStyle.Normal, TextAnchor.MiddleLeft, UIHelpers.TextDim);
                _nameInputText.horizontalOverflow = HorizontalWrapMode.Overflow;
                _nameInputText.verticalOverflow = VerticalWrapMode.Truncate;
                _nameInputText.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;

                // Green dot — anchored to far right, never moves with text
                _nameCursor = UIHelpers.Txt("NmCur", nameBg.transform, "\u25CF",
                    10, FontStyle.Normal, TextAnchor.MiddleCenter, UIHelpers.OnColor);
                _nameCursor.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
                var ncRT = UIHelpers.RT(_nameCursor.gameObject);
                ncRT.anchorMin = new Vector2(1, 0); ncRT.anchorMax = new Vector2(1, 1);
                ncRT.pivot = new Vector2(1, 0.5f);
                ncRT.sizeDelta = new Vector2(14, 0);
                ncRT.anchoredPosition = new Vector2(-6, 0);
                _nameCursor.gameObject.SetActive(false);

                UIHelpers.ActionBtn(nameInputRow.transform, "Set", () =>
                {
                    string name = _nameBuffer.Trim();
                    if (!string.IsNullOrEmpty(name))
                    {
                        DevCommandsGameplay.SetName(name);
                        _activeName = name;
                        _nameBuffer = "";
                        MelonLogger.Msg("[Identity] Name set: " + name);
                        RefreshAll();
                    }
                }, 46);

                // Active name display
                var nameActiveRow = UIHelpers.StatRow("Active Name", pg9);
                _nameActiveText = UIHelpers.Txt("NmAV", nameActiveRow.transform, "—",
                    11, FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.TextDim);
                _nameActiveText.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;

                UIHelpers.InfoBox(pg9, "Name updates in chat and in other players’ ESP. Resets on map change.");

                RefreshAll();
                UIHelpers.AddScrollForwarders(pg9);
            }
            catch (System.Exception ex) { MelonLogger.Error("Page9UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        // ── Moon Mode ─────────────────────────────────────────────────
        private static void ToggleMoonMode()
        {
            try
            {
                if (!_moonModeActive)
                {
                    _savedGravityLevel = Gravity.Level;
                    _savedTravelLevel = Suspension.TravelLevel;
                    _savedDampingLevel = Suspension.DampingLevel;
                    Gravity.SetLevel(1);
                    Suspension.SetTravelLevel(10);
                    Suspension.SetDampingLevel(1);
                    _moonModeActive = true;
                }
                else
                {
                    Gravity.SetLevel(_savedGravityLevel > 0 ? _savedGravityLevel : 5);
                    Suspension.SetTravelLevel(_savedTravelLevel > 0 ? _savedTravelLevel : 5);
                    Suspension.SetDampingLevel(_savedDampingLevel > 0 ? _savedDampingLevel : 5);
                    _moonModeActive = false;
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] ToggleMoonMode: " + ex.Message); }
        }

        // ── Player scale ──────────────────────────────────────────────
        private static void SetPlayerScale(float scale)
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                Transform cyclist = player.transform.Find("Cyclist");
                if ((object)cyclist == null) return;
                if (!_playerScaleCaptured) { _defaultPlayerScale = cyclist.localScale; _playerScaleCaptured = true; }
                cyclist.localScale = new Vector3(scale, scale, scale);
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] SetPlayerScale: " + ex.Message); }
        }

        private static void ResetPlayerScaleToDefault()
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                Transform cyclist = player.transform.Find("Cyclist");
                if ((object)cyclist != null) cyclist.localScale = _defaultPlayerScale;
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] ResetPlayerScale: " + ex.Message); }
        }

        private static void SetAllPlayersScale(float scale)
        {
            try
            {
                GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
                for (int i = 0; i < all.Length; i++)
                {
                    if (all[i].name == "Player_Networked")
                    {
                        Transform cyclist = all[i].transform.Find("Cyclist");
                        if ((object)cyclist != null) cyclist.localScale = new Vector3(scale, scale, scale);
                    }
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] SetAllPlayersScale: " + ex.Message); }
        }

        // ── Invisible Player ──────────────────────────────────────────
        private static void ToggleInvisible(bool invisible)
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                Transform cyclist = player.transform.Find("Cyclist");
                if ((object)cyclist == null) return;
                if (invisible)
                {
                    Renderer[] all = cyclist.GetComponentsInChildren<Renderer>(true);
                    var toHide = new System.Collections.Generic.List<Renderer>();
                    for (int i = 0; i < all.Length; i++) if (all[i].enabled) toHide.Add(all[i]);
                    _hiddenPlayerRenderers = toHide.ToArray();
                    for (int i = 0; i < _hiddenPlayerRenderers.Length; i++) _hiddenPlayerRenderers[i].enabled = false;
                }
                else
                {
                    if ((object)_hiddenPlayerRenderers != null)
                    {
                        for (int i = 0; i < _hiddenPlayerRenderers.Length; i++)
                            if ((object)_hiddenPlayerRenderers[i] != null) _hiddenPlayerRenderers[i].enabled = true;
                        _hiddenPlayerRenderers = null;
                    }
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] ToggleInvisible: " + ex.Message); }
        }

        // ── Identity keyboard tick ───────────────────────────────
        // Called from ModEntry.OnUpdate every frame while menu is open.
        public static void IdentityTick()
        {
            if ((object)_nameInputText == null) return;
            foreach (char ch in Input.inputString)
            {
                if (ch == '\b')
                {
                    if (_nameBuffer.Length > 0)
                        _nameBuffer = _nameBuffer.Substring(0, _nameBuffer.Length - 1);
                }
                else if (ch == '\n' || ch == '\r')
                {
                    string name = _nameBuffer.Trim();
                    if (!string.IsNullOrEmpty(name))
                    {
                        DevCommandsGameplay.SetName(name);
                        _activeName = name;
                        _nameBuffer = "";
                        MelonLogger.Msg("[Identity] Name set via Enter: " + name);
                        RefreshAll();
                    }
                }
                else if (_nameBuffer.Length < NameMaxLength)
                {
                    _nameBuffer += ch;
                }
            }

            if (_nameBuffer.Length > 0)
            {
                string display = _nameBuffer.Length > NameDisplayChars
                    ? "..." + _nameBuffer.Substring(_nameBuffer.Length - NameDisplayChars + 3)
                    : _nameBuffer;
                _nameInputText.text = display;
                _nameInputText.color = UIHelpers.TextLight;
            }
            else
            {
                _nameInputText.text = "Enter name...";
                _nameInputText.color = UIHelpers.TextDim;
            }

            // Green dot pulses when buffer has content
            if ((object)_nameCursor != null)
            {
                bool typing = _nameBuffer.Length > 0;
                _nameCursor.gameObject.SetActive(typing);
                if (typing)
                {
                    float alpha = Mathf.Abs(Mathf.Sin(Time.unscaledTime * 4f));
                    Color col = UIHelpers.OnColor;
                    col.a = alpha;
                    _nameCursor.color = col;
                }
            }
        }

        // ── RefreshAll ────────────────────────────────────────────────
        public static void RefreshAll()
        {
            if (_invisVal) { _invisVal.text = _invisiblePlayer ? "ON" : "OFF"; _invisVal.color = _invisiblePlayer ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_invisTrack, _invisKnob, _invisiblePlayer);
            UIHelpers.SetRowActive(_invisPlayerRow, _invisiblePlayer);

            // Moon Mode
            if (_moonTxt) { _moonTxt.text = _moonModeActive ? "MOON MODE ACTIVE" : "ACTIVATE MOON MODE"; _moonTxt.color = new Color(0, 0, 0, 1); }
            if (_moonBg) _moonBg.color = _moonModeActive ? UIHelpers.OnColor : UIHelpers.NeonBlue;
            if (_moonBdr) _moonBdr.color = _moonModeActive ? UIHelpers.OnColor : UIHelpers.NeonBlue;

            bool mmOn = MirrorMode.Enabled;
            if (_mirrorVal) { _mirrorVal.text = mmOn ? "ON" : "OFF"; _mirrorVal.color = mmOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_mirrorTrack, _mirrorKnob, mmOn);
            UIHelpers.SetRowActive(_mirrorRow, mmOn);

            bool flyOn = FlyMode.Enabled;
            if (_flyVal) { _flyVal.text = flyOn ? "ON" : "OFF"; _flyVal.color = flyOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_flyTrack, _flyKnob, flyOn);
            UIHelpers.SetRowActive(_flyRow, flyOn);

            bool drunkOn = DrunkMode.Enabled;
            if (_drunkVal) { _drunkVal.text = drunkOn ? "ON" : "OFF"; _drunkVal.color = drunkOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_drunkTrack, _drunkKnob, drunkOn);
            UIHelpers.SetRowActive(_drunkRow, drunkOn);

            // Camera Shake
            bool shOn = CameraShake.Enabled;
            if (_shakeTogVal) { _shakeTogVal.text = shOn ? "ON" : "OFF"; _shakeTogVal.color = shOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_shakeTrack, _shakeKnob, shOn);
            if (_shakeVal) _shakeVal.text = CameraShake.DisplayValue;
            UIHelpers.SetBar(_shakeBar, (CameraShake.Level - 1) / 9f);

            // Identity — active name display
            if ((object)_nameActiveText != null)
            {
                if (!string.IsNullOrEmpty(_activeName))
                { _nameActiveText.text = _activeName; _nameActiveText.color = UIHelpers.OnColor; }
                else
                { _nameActiveText.text = "—"; _nameActiveText.color = UIHelpers.TextDim; }
            }
        }
    }
}