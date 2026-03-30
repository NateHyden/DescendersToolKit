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

        // ── Turbo Wind ────────────────────────────────────────────────
        private static bool _turboWind = false;
        private static float _savedWindMain = -1f;
        private static Image _windTrack; private static RectTransform _windKnob;
        private static Text _windVal;

        // ── Invisible Bike ────────────────────────────────────────────
        private static bool _invisibleBike = false;
        private static Renderer[] _hiddenBikeRenderers = null;
        private static Image _invisBikeTrack; private static RectTransform _invisBikeKnob;
        private static Text _invisBikeVal;

        // ── Moon Mode ─────────────────────────────────────────────────
        private static bool _moonModeActive = false;
        private static Image _moonBg, _moonBdr;
        private static Text _moonTxt;
        private static int _savedGravityLevel = -1;
        private static int _savedTravelLevel = -1;
        private static int _savedDampingLevel = -1;

        // ── Wheel Size ────────────────────────────────────────────────
        private static int _wheelSizeMode = 0;   // 0=Default, 1=Small, 2=Large
        private static bool _wheelSizeEnabled = false;
        private static Image _wheelSizeTrack;
        private static RectTransform _wheelSizeKnob;
        private static Text _wheelSizeTogVal;

        // ── Exploding Props ───────────────────────────────────────────
        private static bool _explodingProps = false;
        private static Image _explodeTrack; private static RectTransform _explodeKnob;
        private static Text _explodeVal;

        // ── Reverse Steering ──────────────────────────────────────────
        private static Image _revSteerTrack; private static RectTransform _revSteerKnob;
        private static Text _revSteerVal;

        // ── Wide Tyres ────────────────────────────────────────────────
        private static Image _wideTyresTrack; private static RectTransform _wideTyresKnob;
        private static Text _wideTyresVal, _wideTyresLvlVal; private static Image _wideTyresBar;
        private static UnityEngine.UI.Button _wideTyresMinus, _wideTyresPlus;

        // ── Sticky Tyres ──────────────────────────────────────────────
        private static Image _stickyTrack; private static RectTransform _stickyKnob;
        private static Text _stickyVal;

        // ── Ice Mode ──────────────────────────────────────────────────
        private static Image _iceModeTrack; private static RectTransform _iceModeKnob;
        private static Text _iceModeVal;

        // ── Drunk / Fly / Mirror ──────────────────────────────────────
        private static Image _drunkTrack; private static RectTransform _drunkKnob; private static Text _drunkVal;
        private static Image _flyTrack; private static RectTransform _flyKnob; private static Text _flyVal;
        private static Image _mirrorTrack; private static RectTransform _mirrorKnob; private static Text _mirrorVal;

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

                // ── PLAYER SIZE ───────────────────────────────────────
                UIHelpers.SectionHeader("PLAYER SIZE", pg9);
                var psr = UIHelpers.StatRow("Size", pg9);
                UIHelpers.ActionBtn(psr.transform, "Giant", () => SetPlayerScale(3.5f), 52);
                UIHelpers.ActionBtn(psr.transform, "Big", () => SetPlayerScale(1.5f), 44);
                UIHelpers.ActionBtn(psr.transform, "Default", () => SetPlayerScale(1.0f), 58);
                UIHelpers.ActionBtn(psr.transform, "Small", () => SetPlayerScale(0.6f), 52);
                UIHelpers.ActionBtn(psr.transform, "Tiny", () => SetPlayerScale(0.2f), 44);
                UIHelpers.Divider(pg9);

                // ── BIKE ──────────────────────────────────────────────
                UIHelpers.SectionHeader("BIKE", pg9);

                // Invisible Bike
                var ibr = UIHelpers.StatRow("Invisible Bike", pg9);
                _invisBikeVal = UIHelpers.Txt("IbV", ibr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _invisBikeVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(ibr.transform, "IbT", () =>
                {
                    _invisibleBike = !_invisibleBike;
                    ToggleInvisibleBike(_invisibleBike);
                    RefreshAll();
                }, out _invisBikeTrack, out _invisBikeKnob);

                // Wheel Size — now has on/off toggle + preset buttons
                var gwr = UIHelpers.StatRow("Wheel Size", pg9);
                _wheelSizeTogVal = UIHelpers.Txt("WsTV", gwr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _wheelSizeTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(gwr.transform, "WsT", () =>
                {
                    _wheelSizeEnabled = !_wheelSizeEnabled;
                    if (_wheelSizeEnabled) SetWheelSize(_wheelSizeMode == 0 ? 2 : _wheelSizeMode);
                    else SetWheelSize(0);
                    RefreshAll();
                }, out _wheelSizeTrack, out _wheelSizeKnob);
                UIHelpers.ActionBtn(gwr.transform, "Small", () => { _wheelSizeMode = 1; if (_wheelSizeEnabled) { SetWheelSize(1); RefreshAll(); } }, 52);
                UIHelpers.ActionBtn(gwr.transform, "Default", () => { _wheelSizeMode = 0; if (_wheelSizeEnabled) { SetWheelSize(0); RefreshAll(); } }, 58);
                UIHelpers.ActionBtn(gwr.transform, "Large", () => { _wheelSizeMode = 2; if (_wheelSizeEnabled) { SetWheelSize(2); RefreshAll(); } }, 52);

                // Wide Tyres
                var wtr = UIHelpers.StatRow("Wide Tyres", pg9);
                _wideTyresVal = UIHelpers.Txt("WtV", wtr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _wideTyresVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(wtr.transform, "WtT", () => { WideTyres.Toggle(); RefreshAll(); }, out _wideTyresTrack, out _wideTyresKnob);
                _wideTyresBar = UIHelpers.MakeBar("WtB", wtr.transform, (WideTyres.Level - 1) / 19f);
                _wideTyresLvlVal = UIHelpers.Txt("WtL", wtr.transform, WideTyres.Level.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                _wideTyresLvlVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                _wideTyresMinus = UIHelpers.SmallBtn(wtr.transform, "-", () => { WideTyres.Decrease(); RefreshAll(); });
                _wideTyresPlus = UIHelpers.SmallBtn(wtr.transform, "+", () => { WideTyres.Increase(); RefreshAll(); });

                // Sticky Tyres
                var str2 = UIHelpers.StatRow("Sticky Tyres", pg9);
                _stickyVal = UIHelpers.Txt("StV", str2.transform, StickyTyres.Enabled ? "ON" : "OFF", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, StickyTyres.Enabled ? UIHelpers.OnColor : UIHelpers.OffColor);
                _stickyVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(str2.transform, "StT", () => { StickyTyres.Toggle(); RefreshAll(); }, out _stickyTrack, out _stickyKnob);

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
                UIHelpers.ActionBtnOrange(gsr.transform, "Giant", () => SetAllPlayersScale(3.5f), 52);
                UIHelpers.ActionBtn(gsr.transform, "Default", () => SetAllPlayersScale(1.0f), 58);
                UIHelpers.ActionBtn(gsr.transform, "Tiny", () => SetAllPlayersScale(0.2f), 44);

                UIHelpers.Divider(pg9);

                // ── CONTROLS ──────────────────────────────────────────
                UIHelpers.SectionHeader("CONTROLS", pg9);

                var rsr = UIHelpers.StatRow("Reverse Steering", pg9);
                _revSteerVal = UIHelpers.Txt("RsV", rsr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _revSteerVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(rsr.transform, "RsT", () => { ReverseSteering.Toggle(); RefreshAll(); }, out _revSteerTrack, out _revSteerKnob);

                // Ice Grip
                var imr = UIHelpers.StatRow("Ice Grip", pg9);
                _iceModeVal = UIHelpers.Txt("ImV", imr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _iceModeVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(imr.transform, "ImT", () => { IceMode.Toggle(); RefreshAll(); }, out _iceModeTrack, out _iceModeKnob);
                UIHelpers.InfoBox(pg9, "Removes tyre grip entirely. For an opposite experience to Sticky Tyres.");

                var mmr = UIHelpers.StatRow("Mirror Mode", pg9);
                _mirrorVal = UIHelpers.Txt("MmV", mmr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _mirrorVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(mmr.transform, "MmT", () => { MirrorMode.Toggle(); RefreshAll(); }, out _mirrorTrack, out _mirrorKnob);

                var flyr = UIHelpers.StatRow("Fly Mode", pg9);
                _flyVal = UIHelpers.Txt("FlV", flyr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _flyVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(flyr.transform, "FlT", () => { FlyMode.Toggle(); RefreshAll(); }, out _flyTrack, out _flyKnob);

                var drnkr = UIHelpers.StatRow("Drunk Mode", pg9);
                _drunkVal = UIHelpers.Txt("DrV", drnkr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _drunkVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(drnkr.transform, "DrT", () => { DrunkMode.Toggle(); RefreshAll(); }, out _drunkTrack, out _drunkKnob);

                UIHelpers.Divider(pg9);

                // ── WORLD ─────────────────────────────────────────────
                UIHelpers.SectionHeader("WORLD", pg9);

                var ir = UIHelpers.StatRow("Invisible Player", pg9);
                _invisVal = UIHelpers.Txt("InV", ir.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _invisVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(ir.transform, "InT", () =>
                {
                    _invisiblePlayer = !_invisiblePlayer;
                    ToggleInvisible(_invisiblePlayer);
                    RefreshAll();
                }, out _invisTrack, out _invisKnob);

                var wr = UIHelpers.StatRow("Turbo Wind", pg9);
                _windVal = UIHelpers.Txt("WnV", wr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _windVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(wr.transform, "WnT", () =>
                {
                    _turboWind = !_turboWind;
                    ToggleTurboWind(_turboWind);
                    RefreshAll();
                }, out _windTrack, out _windKnob);

                var er = UIHelpers.StatRow("No Mistakes", pg9);
                _explodeVal = UIHelpers.Txt("ExV", er.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _explodeVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                var erHint = UIHelpers.Txt("ExH", er.transform, "launch on impact", 9, FontStyle.Italic, TextAnchor.MiddleRight, UIHelpers.TextDim);
                erHint.gameObject.AddComponent<LayoutElement>().preferredWidth = 90;
                UIHelpers.Toggle(er.transform, "ExT", () =>
                {
                    ExplodingProps.Toggle();
                    _explodingProps = ExplodingProps.Enabled;
                    RefreshAll();
                }, out _explodeTrack, out _explodeKnob);

                RefreshAll();
                UIHelpers.AddScrollForwarders(pg9);
            }
            catch (System.Exception ex) { MelonLogger.Error("Page9UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        // ── Wheel Size toggle helper ──────────────────────────────────
        // Called from WheelSizeTick — only applies when enabled
        public static void WheelSizeTick()
        {
            if (_wheelSizeMode == 0) return;
            try
            {
                float scale = WheelScales[_wheelSizeMode];
                if ((object)_cachedFrontBone != null) _cachedFrontBone.localScale = new Vector3(scale, scale, scale);
                if ((object)_cachedBackBone != null) _cachedBackBone.localScale = new Vector3(scale, scale, scale);
            }
            catch { }
        }

        public static void ResetWheelSize()
        {
            _wheelSizeMode = 0;
            _wheelSizeEnabled = false;
            _cachedFrontBone = null;
            _cachedBackBone = null;
            _backBoneField = null;
            _frontBoneField = null;
            _wheelRadiusField = null;
            _defaultRadiusFront = -1f;
            _defaultRadiusBack = -1f;
        }

        // ── Invisible Bike ────────────────────────────────────────────
        private static void ToggleInvisibleBike(bool invisible)
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                Transform bikeModel = player.transform.Find("BikeModel");
                if ((object)bikeModel == null) return;
                if (invisible)
                {
                    Renderer[] all = bikeModel.GetComponentsInChildren<Renderer>(true);
                    var toHide = new System.Collections.Generic.List<Renderer>();
                    for (int i = 0; i < all.Length; i++) if (all[i].enabled) toHide.Add(all[i]);
                    _hiddenBikeRenderers = toHide.ToArray();
                    for (int i = 0; i < _hiddenBikeRenderers.Length; i++) _hiddenBikeRenderers[i].enabled = false;
                }
                else
                {
                    if ((object)_hiddenBikeRenderers != null)
                    {
                        for (int i = 0; i < _hiddenBikeRenderers.Length; i++)
                            if ((object)_hiddenBikeRenderers[i] != null) _hiddenBikeRenderers[i].enabled = true;
                        _hiddenBikeRenderers = null;
                    }
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] ToggleInvisibleBike: " + ex.Message); }
        }

        // ── Wheel Size internals ──────────────────────────────────────
        private static readonly float[] WheelScales = { 1.0f, 0.4f, 2.5f };
        private static readonly string[] WheelLabels = { "Default", "Small", "Large" };
        private static System.Reflection.FieldInfo _wheelRadiusField = null;
        private static float _defaultRadiusFront = -1f;
        private static float _defaultRadiusBack = -1f;
        private static System.Reflection.FieldInfo _backBoneField = null;
        private static System.Reflection.FieldInfo _frontBoneField = null;
        private static Transform _cachedFrontBone = null;
        private static Transform _cachedBackBone = null;

        private static void SetWheelSize(int mode)
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                float scale = WheelScales[mode];

                Transform bikeModel = player.transform.Find("BikeModel");
                if ((object)bikeModel != null)
                {
                    BikeAnimation bikeAnim = bikeModel.GetComponent<BikeAnimation>();
                    if ((object)bikeAnim != null)
                    {
                        if ((object)_backBoneField == null)
                            _backBoneField = typeof(BikeAnimation).GetField("YLzyVuM",
                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if ((object)_frontBoneField == null)
                            _frontBoneField = typeof(BikeAnimation).GetField("RCNLpue",
                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if ((object)_backBoneField != null)
                        {
                            Transform bb = _backBoneField.GetValue(bikeAnim) as Transform;
                            if ((object)bb != null) { bb.localScale = new Vector3(scale, scale, scale); _cachedBackBone = bb; }
                        }
                        if ((object)_frontBoneField != null)
                        {
                            Transform fb = _frontBoneField.GetValue(bikeAnim) as Transform;
                            if ((object)fb != null) { fb.localScale = new Vector3(scale, scale, scale); _cachedFrontBone = fb; }
                        }
                    }
                }

                Wheel[] wheels = player.GetComponentsInChildren<Wheel>();
                if (wheels != null)
                {
                    for (int i = 0; i < wheels.Length; i++)
                    {
                        if ((object)_wheelRadiusField == null)
                            _wheelRadiusField = wheels[i].GetType().GetField("HqsqNkJ",
                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if ((object)_wheelRadiusField == null) break;
                        bool isFront = string.Equals(wheels[i].gameObject.name, "wheel_front", System.StringComparison.Ordinal);
                        if (isFront && _defaultRadiusFront < 0f) _defaultRadiusFront = (float)_wheelRadiusField.GetValue(wheels[i]);
                        else if (!isFront && _defaultRadiusBack < 0f) _defaultRadiusBack = (float)_wheelRadiusField.GetValue(wheels[i]);
                        float def = isFront ? _defaultRadiusFront : _defaultRadiusBack;
                        if (def > 0f) _wheelRadiusField.SetValue(wheels[i], def * scale);
                    }
                }

                _wheelSizeMode = mode;
                if (mode == 0) { _cachedFrontBone = null; _cachedBackBone = null; }
                MelonLogger.Msg("[Silly] Wheel size -> " + WheelLabels[mode] + " (scale " + scale + ")");
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] SetWheelSize: " + ex.Message); }
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
                cyclist.localScale = new Vector3(scale, scale, scale);
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] SetPlayerScale: " + ex.Message); }
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

        // ── Turbo Wind ────────────────────────────────────────────────
        private static System.Type _windZoneType = null;
        private static System.Reflection.PropertyInfo _windMainProp = null;
        private static System.Reflection.PropertyInfo _windTurbProp = null;

        private static void ToggleTurboWind(bool enabled)
        {
            try
            {
                if ((object)_windZoneType == null)
                {
                    System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                    for (int a = 0; a < assemblies.Length; a++)
                    {
                        _windZoneType = assemblies[a].GetType("UnityEngine.WindZone");
                        if ((object)_windZoneType != null) break;
                    }
                }
                if ((object)_windZoneType == null) return;
                UnityEngine.Object wz = GameObject.FindObjectOfType(_windZoneType);
                if ((object)wz == null) return;
                if ((object)_windMainProp == null)
                    _windMainProp = _windZoneType.GetProperty("windMain", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if ((object)_windTurbProp == null)
                    _windTurbProp = _windZoneType.GetProperty("windTurbulence", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (enabled)
                {
                    if (_savedWindMain < 0f && (object)_windMainProp != null)
                        _savedWindMain = (float)_windMainProp.GetValue(wz, null);
                    if ((object)_windMainProp != null) _windMainProp.SetValue(wz, 50f, null);
                    if ((object)_windTurbProp != null) _windTurbProp.SetValue(wz, 1f, null);
                }
                else
                {
                    if ((object)_windMainProp != null) _windMainProp.SetValue(wz, _savedWindMain >= 0f ? _savedWindMain : 1f, null);
                    if ((object)_windTurbProp != null) _windTurbProp.SetValue(wz, 0.5f, null);
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] ToggleTurboWind: " + ex.Message); }
        }

        // ── RefreshAll ────────────────────────────────────────────────
        public static void RefreshAll()
        {
            if (_invisVal) { _invisVal.text = _invisiblePlayer ? "ON" : "OFF"; _invisVal.color = _invisiblePlayer ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_invisTrack, _invisKnob, _invisiblePlayer);
            if (_windVal) { _windVal.text = _turboWind ? "ON" : "OFF"; _windVal.color = _turboWind ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_windTrack, _windKnob, _turboWind);
            if (_invisBikeVal) { _invisBikeVal.text = _invisibleBike ? "ON" : "OFF"; _invisBikeVal.color = _invisibleBike ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_invisBikeTrack, _invisBikeKnob, _invisibleBike);

            // Wheel Size toggle
            if (_wheelSizeTogVal) { _wheelSizeTogVal.text = _wheelSizeEnabled ? "ON" : "OFF"; _wheelSizeTogVal.color = _wheelSizeEnabled ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_wheelSizeTrack, _wheelSizeKnob, _wheelSizeEnabled);

            // Moon Mode
            if (_moonTxt) { _moonTxt.text = _moonModeActive ? "MOON MODE ACTIVE" : "ACTIVATE MOON MODE"; _moonTxt.color = new Color(0, 0, 0, 1); }
            if (_moonBg) _moonBg.color = _moonModeActive ? UIHelpers.OnColor : UIHelpers.NeonBlue;
            if (_moonBdr) _moonBdr.color = _moonModeActive ? UIHelpers.OnColor : UIHelpers.NeonBlue;

            if (_explodeVal) { _explodeVal.text = _explodingProps ? "ON" : "OFF"; _explodeVal.color = _explodingProps ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_explodeTrack, _explodeKnob, _explodingProps);

            bool revOn = ReverseSteering.Enabled;
            if (_revSteerVal) { _revSteerVal.text = revOn ? "ON" : "OFF"; _revSteerVal.color = revOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_revSteerTrack, _revSteerKnob, revOn);

            bool imOn = IceMode.Enabled;
            if (_iceModeVal) { _iceModeVal.text = imOn ? "ON" : "OFF"; _iceModeVal.color = imOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_iceModeTrack, _iceModeKnob, imOn);

            bool mmOn = MirrorMode.Enabled;
            if (_mirrorVal) { _mirrorVal.text = mmOn ? "ON" : "OFF"; _mirrorVal.color = mmOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_mirrorTrack, _mirrorKnob, mmOn);

            bool flyOn = FlyMode.Enabled;
            if (_flyVal) { _flyVal.text = flyOn ? "ON" : "OFF"; _flyVal.color = flyOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_flyTrack, _flyKnob, flyOn);

            bool drunkOn = DrunkMode.Enabled;
            if (_drunkVal) { _drunkVal.text = drunkOn ? "ON" : "OFF"; _drunkVal.color = drunkOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_drunkTrack, _drunkKnob, drunkOn);

            bool wtOn = WideTyres.Enabled;
            if (_wideTyresVal) { _wideTyresVal.text = wtOn ? "ON" : "OFF"; _wideTyresVal.color = wtOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_wideTyresTrack, _wideTyresKnob, wtOn);
            if (_wideTyresLvlVal) _wideTyresLvlVal.text = WideTyres.Level.ToString();
            UIHelpers.SetBar(_wideTyresBar, (WideTyres.Level - 1) / 19f);
            if ((object)_wideTyresMinus != null) _wideTyresMinus.interactable = wtOn;
            if ((object)_wideTyresPlus != null) _wideTyresPlus.interactable = wtOn;

            bool stOn = StickyTyres.Enabled;
            if (_stickyVal) { _stickyVal.text = stOn ? "ON" : "OFF"; _stickyVal.color = stOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_stickyTrack, _stickyKnob, stOn);
        }
    }
}