using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page8UI
    {
        // ── Suspension ────────────────────────────────────────────────
        private static Text _travelVal, _stiffVal, _dampVal;
        private static Image _travelBar, _stiffBar, _dampBar;

        // ── Invisible Bike ────────────────────────────────────────────
        private static bool _invisibleBike = false;

        // Public accessors for snapshot/save system
        public static float CurrentBikeScale = 1f;
        public static bool IsInvisibleBike => _invisibleBike;
        public static bool IsWheelSizeEnabled => _wheelSizeEnabled;
        public static int CurrentWheelSizeMode => _wheelSizeMode;
        private static Renderer[] _hiddenBikeRenderers = null;
        private static Image _invisBikeTrack; private static RectTransform _invisBikeKnob;
        private static Text _invisBikeVal;

        // ── Wheel Size ────────────────────────────────────────────────
        private static int _wheelSizeMode = 0;
        private static bool _wheelSizeEnabled = false;
        private static Image _wheelSizeTrack;
        private static RectTransform _wheelSizeKnob;
        private static Text _wheelSizeTogVal;

        // ── Wide Tyres ────────────────────────────────────────────────
        private static Image _wideTyresTrack; private static RectTransform _wideTyresKnob;
        private static Text _wideTyresVal, _wideTyresLvlVal; private static Image _wideTyresBar;
        private static UnityEngine.UI.Button _wideTyresMinus, _wideTyresPlus;

        // ── Sticky Tyres ──────────────────────────────────────────────
        private static Image _stickyTrack; private static RectTransform _stickyKnob;
        private static Text _stickyVal;

        // ── Reverse Steering ──────────────────────────────────────────
        private static Image _revSteerTrack; private static RectTransform _revSteerKnob;
        private static Text _revSteerVal;

        // ── Ice Mode ──────────────────────────────────────────────────
        private static Image _iceModeTrack; private static RectTransform _iceModeKnob;
        private static Text _iceModeVal;

        // ── Cut Brakes ────────────────────────────────────────────────
        private static Image _cutBrakesTrack; private static RectTransform _cutBrakesKnob;
        private static Text _cutBrakesVal;

        // ── Torch ─────────────────────────────────────────────────────
        private static Text _torchVal, _torchIntLbl;
        private static Image _torchTrack;
        private static RectTransform _torchKnob;

        // ── Row GO refs for highlight ─────────────────────────────────
        private static GameObject _invisBikeRow, _wheelSizeRow, _wideTyresRow, _stickyRow;
        private static GameObject _revSteerRow, _iceModeRow, _cutBrakesRow, _torchRow;

        // ── Wheel Size internals ──────────────────────────────────────
        private static readonly float[] WheelScales = { 1.0f, 0.25f, 0.5f, 1.5f, 3.0f };
        private static readonly string[] WheelLabels = { "Default", "Tiny", "Small", "Large", "Huge" };
        private static System.Reflection.FieldInfo _wheelRadiusField = null;
        private static float _defaultRadiusFront = -1f;
        private static float _defaultRadiusBack = -1f;
        private static System.Reflection.FieldInfo _backBoneField = null;
        private static System.Reflection.FieldInfo _frontBoneField = null;
        private static Transform _cachedFrontBone = null;
        private static Transform _cachedBackBone = null;

        // ── Default scale capture ─────────────────────────────────────
        private static Vector3 _defaultBikeScale = Vector3.one;
        private static bool _bikeScaleCaptured = false;

        public static void CaptureSceneDefaults()
        {
            _bikeScaleCaptured = false;
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                Transform bm = player.transform.Find("BikeModel");
                if ((object)bm != null) { _defaultBikeScale = bm.localScale; _bikeScaleCaptured = true; }
            }
            catch { }
        }

        public static bool IsAnyActive =>
            Suspension.TravelLevel != 5 || Suspension.StiffnessLevel != 5 || Suspension.DampingLevel != 5 ||
            _wheelSizeEnabled || _invisibleBike ||
            WideTyres.Enabled || StickyTyres.Enabled ||
            ReverseSteering.Enabled || IceMode.Enabled || CutBrakes.Enabled ||
            BikeTorch.Enabled;

        public static void GlobalReset()
        {
            if (_invisibleBike) { ToggleInvisibleBike(false); _invisibleBike = false; }
            ResetWheelSize();
            ResetBikeScaleToDefault();
        }

        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
                pg = UIHelpers.Obj("P8R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));

                // ── ScrollRect wrapper ────────────────────────────────
                var scrollObj = UIHelpers.Obj("Scroll", pg.transform);
                UIHelpers.Fill(UIHelpers.RT(scrollObj));
                var scrollRect = scrollObj.AddComponent<ScrollRect>();
                scrollRect.horizontal = false; scrollRect.vertical = true;
                scrollRect.movementType = ScrollRect.MovementType.Clamped;
                scrollRect.scrollSensitivity = 25f;
                scrollRect.inertia = false;

                var vp = UIHelpers.Obj("VP", scrollObj.transform);
                UIHelpers.Fill(UIHelpers.RT(vp));
                vp.AddComponent<Image>().color = new Color(0, 0, 0, 0.01f);
                vp.AddComponent<Mask>().showMaskGraphic = true;
                scrollRect.viewport = UIHelpers.RT(vp);

                var content = UIHelpers.Obj("Content", vp.transform);
                var crt = UIHelpers.RT(content);
                crt.anchorMin = new Vector2(0, 1); crt.anchorMax = new Vector2(1, 1);
                crt.pivot = new Vector2(0.5f, 1);
                crt.sizeDelta = new Vector2(0, 0);
                scrollRect.content = crt;

                content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                var vlg = content.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = UIHelpers.RowGap;
                vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true;
                vlg.childForceExpandHeight = false;

                var pg8 = content.transform;

                // ── RESET TAB ─────────────────────────────────────────
                var rstRow = UIHelpers.StatRow("", pg8);
                UIHelpers.ActionBtnOrange(rstRow.transform, "↺  Reset Tab to Defaults", () => { ResetBikeTab(); RefreshAll(); }, 186);
                UIHelpers.SectionHeader("SUSPENSION", pg8);

                var tr = UIHelpers.StatRow("Travel", pg8);
                _travelBar = UIHelpers.MakeBar("TvB", tr.transform, (Suspension.TravelLevel - 1) / 9f);
                _travelVal = UIHelpers.Txt("TvV", tr.transform, Suspension.TravelLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                _travelVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(tr.transform, "-", () => { Suspension.TravelDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(tr.transform, "+", () => { Suspension.TravelIncrease(); RefreshAll(); });

                var sr = UIHelpers.StatRow("Stiffness", pg8);
                _stiffBar = UIHelpers.MakeBar("StB", sr.transform, (Suspension.StiffnessLevel - 1) / 9f);
                _stiffVal = UIHelpers.Txt("StV", sr.transform, Suspension.StiffnessLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                _stiffVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(sr.transform, "-", () => { Suspension.StiffnessDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(sr.transform, "+", () => { Suspension.StiffnessIncrease(); RefreshAll(); });

                var dr = UIHelpers.StatRow("Damping", pg8);
                _dampBar = UIHelpers.MakeBar("DpB", dr.transform, (Suspension.DampingLevel - 1) / 9f);
                _dampVal = UIHelpers.Txt("DpV", dr.transform, Suspension.DampingLevel.ToString(), 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                _dampVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                UIHelpers.SmallBtn(dr.transform, "-", () => { Suspension.DampingDecrease(); RefreshAll(); });
                UIHelpers.SmallBtn(dr.transform, "+", () => { Suspension.DampingIncrease(); RefreshAll(); });

                UIHelpers.InfoBox(pg8, "Level 5 = default. Travel = how much the fork/shock moves. Stiffness = spring resistance. Damping = how fast it settles.");

                UIHelpers.Divider(pg8);

                // ── BIKE SIZE ─────────────────────────────────────────
                UIHelpers.SectionHeader("BIKE SIZE", pg8);

                var szr = UIHelpers.StatRow("Size", pg8);
                UIHelpers.ActionBtn(szr.transform, "Colossal", () => SetBikeScale(6.0f), 62);
                UIHelpers.ActionBtn(szr.transform, "Giant", () => SetBikeScale(3.5f), 50);
                UIHelpers.ActionBtn(szr.transform, "Huge", () => SetBikeScale(2.5f), 48);
                UIHelpers.ActionBtn(szr.transform, "Big", () => SetBikeScale(1.5f), 42);
                UIHelpers.ActionBtn(szr.transform, "Large", () => SetBikeScale(1.25f), 50);

                var szr2 = UIHelpers.StatRow("", pg8);
                UIHelpers.ActionBtn(szr2.transform, "Default", () => ResetBikeScaleToDefault(), 58);
                UIHelpers.ActionBtn(szr2.transform, "Medium", () => SetBikeScale(0.75f), 56);
                UIHelpers.ActionBtn(szr2.transform, "Small", () => SetBikeScale(0.5f), 48);
                UIHelpers.ActionBtn(szr2.transform, "Tiny", () => SetBikeScale(0.25f), 44);
                UIHelpers.ActionBtn(szr2.transform, "Micro", () => SetBikeScale(0.1f), 48);

                UIHelpers.Divider(pg8);

                // ── BIKE PARTS ────────────────────────────────────────
                UIHelpers.SectionHeader("BIKE PARTS", pg8);

                // Invisible Bike
                _invisBikeRow = UIHelpers.StatRow("Invisible Bike", pg8);
                var ibr = _invisBikeRow;
                _invisBikeVal = UIHelpers.Txt("IbV", ibr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _invisBikeVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(ibr.transform, "IbT", () =>
                {
                    _invisibleBike = !_invisibleBike;
                    ToggleInvisibleBike(_invisibleBike);
                    RefreshAll();
                }, out _invisBikeTrack, out _invisBikeKnob);

                // Wheel Size
                _wheelSizeRow = UIHelpers.StatRow("Wheel Size", pg8);
                var gwr = _wheelSizeRow;
                _wheelSizeTogVal = UIHelpers.Txt("WsTV", gwr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _wheelSizeTogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(gwr.transform, "WsT", () =>
                {
                    _wheelSizeEnabled = !_wheelSizeEnabled;
                    if (_wheelSizeEnabled) SetWheelSize(_wheelSizeMode == 0 ? 3 : _wheelSizeMode);
                    else SetWheelSize(0);
                    RefreshAll();
                }, out _wheelSizeTrack, out _wheelSizeKnob);
                UIHelpers.ActionBtn(gwr.transform, "Tiny", () => { _wheelSizeMode = 1; if (_wheelSizeEnabled) { SetWheelSize(1); RefreshAll(); } }, 44);
                UIHelpers.ActionBtn(gwr.transform, "Small", () => { _wheelSizeMode = 2; if (_wheelSizeEnabled) { SetWheelSize(2); RefreshAll(); } }, 50);
                UIHelpers.ActionBtn(gwr.transform, "Default", () => { _wheelSizeMode = 0; if (_wheelSizeEnabled) { SetWheelSize(0); RefreshAll(); } }, 58);
                UIHelpers.ActionBtn(gwr.transform, "Large", () => { _wheelSizeMode = 3; if (_wheelSizeEnabled) { SetWheelSize(3); RefreshAll(); } }, 50);
                UIHelpers.ActionBtn(gwr.transform, "Huge", () => { _wheelSizeMode = 4; if (_wheelSizeEnabled) { SetWheelSize(4); RefreshAll(); } }, 48);

                // Wide Tyres
                _wideTyresRow = UIHelpers.StatRow("Wide Tyres", pg8);
                var wtr = _wideTyresRow;
                _wideTyresVal = UIHelpers.Txt("WtV", wtr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _wideTyresVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(wtr.transform, "WtT", () => { WideTyres.Toggle(); RefreshAll(); }, out _wideTyresTrack, out _wideTyresKnob);
                _wideTyresBar = UIHelpers.MakeBar("WtB", wtr.transform, (WideTyres.Level - 1) / 19f);
                _wideTyresLvlVal = UIHelpers.Txt("WtL", wtr.transform, WideTyres.Level.ToString(), 12, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                _wideTyresLvlVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 18;
                _wideTyresMinus = UIHelpers.SmallBtn(wtr.transform, "-", () => { WideTyres.Decrease(); RefreshAll(); });
                _wideTyresPlus = UIHelpers.SmallBtn(wtr.transform, "+", () => { WideTyres.Increase(); RefreshAll(); });

                // Sticky Tyres
                _stickyRow = UIHelpers.StatRow("Sticky Tyres", pg8);
                var str2 = _stickyRow;
                _stickyVal = UIHelpers.Txt("StV", str2.transform, StickyTyres.Enabled ? "ON" : "OFF", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, StickyTyres.Enabled ? UIHelpers.OnColor : UIHelpers.OffColor);
                _stickyVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(str2.transform, "StT", () => { StickyTyres.Toggle(); RefreshAll(); }, out _stickyTrack, out _stickyKnob);

                UIHelpers.Divider(pg8);

                // ── CONTROLS ──────────────────────────────────────────
                UIHelpers.SectionHeader("CONTROLS", pg8);

                _revSteerRow = UIHelpers.StatRow("Reverse Steering", pg8);
                var rsr = _revSteerRow;
                _revSteerVal = UIHelpers.Txt("RsV", rsr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _revSteerVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(rsr.transform, "RsT", () => { ReverseSteering.Toggle(); RefreshAll(); }, out _revSteerTrack, out _revSteerKnob);

                _iceModeRow = UIHelpers.StatRow("Ice Grip", pg8);
                var imr = _iceModeRow;
                _iceModeVal = UIHelpers.Txt("ImV", imr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _iceModeVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(imr.transform, "ImT", () => { IceMode.Toggle(); RefreshAll(); }, out _iceModeTrack, out _iceModeKnob);
                UIHelpers.InfoBox(pg8, "Removes tyre grip entirely. For an opposite experience to Sticky Tyres.");

                _cutBrakesRow = UIHelpers.StatRow("Cut Brakes", pg8);
                var cbr = _cutBrakesRow;
                _cutBrakesVal = UIHelpers.Txt("CbV", cbr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _cutBrakesVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(cbr.transform, "CbT", () => { CutBrakes.Toggle(); RefreshAll(); }, out _cutBrakesTrack, out _cutBrakesKnob);

                UIHelpers.Divider(pg8);

                // ── TORCH ─────────────────────────────────────────────
                UIHelpers.SectionHeader("TORCH", pg8);

                _torchRow = UIHelpers.StatRow("Headlight", pg8);
                var tchr = _torchRow;
                _torchVal = UIHelpers.Txt("TchV", tchr.transform, "OFF", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _torchVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(tchr.transform, "TchT",
                    () => { BikeTorch.Toggle(); RefreshAll(); },
                    out _torchTrack, out _torchKnob);

                var tcir = UIHelpers.StatRow("Intensity", pg8);
                UIHelpers.SmallBtn(tcir.transform, "\u25C0",
                    () => { BikeTorch.PrevIntensity(); RefreshAll(); });
                _torchIntLbl = UIHelpers.Txt("TchIV", tcir.transform,
                    BikeTorch.IntensityDisplay, 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                _torchIntLbl.gameObject.AddComponent<LayoutElement>().preferredWidth = 56;
                UIHelpers.SmallBtn(tcir.transform, "\u25B6",
                    () => { BikeTorch.NextIntensity(); RefreshAll(); });

                UIHelpers.InfoBox(pg8,
                    "Enables the bike's headlight. If the game has no built-in light, " +
                    "a spotlight is added to the front of the bike.");

                RefreshAll();
                UIHelpers.AddScrollForwarders(pg8);
            }
            catch (System.Exception ex) { MelonLogger.Error("Page8UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        // ── Bike Scale ────────────────────────────────────────────────
        private static void SetBikeScale(float scale)
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) { MelonLogger.Warning("[BikeSize] Player_Human not found."); return; }
                Transform bikeModel = player.transform.Find("BikeModel");
                if ((object)bikeModel == null) { MelonLogger.Warning("[BikeSize] BikeModel not found."); return; }
                if (!_bikeScaleCaptured) { _defaultBikeScale = bikeModel.localScale; _bikeScaleCaptured = true; }
                bikeModel.localScale = new Vector3(scale, scale, scale);
                CurrentBikeScale = scale;
                MelonLogger.Msg("[BikeSize] Scale -> " + scale);
            }
            catch (System.Exception ex) { MelonLogger.Error("[BikeSize] " + ex.Message); }
        }

        private static void ResetBikeScaleToDefault()
        {
            try
            {
                CurrentBikeScale = 1f;
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                Transform bikeModel = player.transform.Find("BikeModel");
                if ((object)bikeModel != null) bikeModel.localScale = _defaultBikeScale;
            }
            catch (System.Exception ex) { MelonLogger.Error("[BikeSize] ResetDefault: " + ex.Message); }
        }

        // Called by reapply system to restore bike scale after scene change
        public static void ApplyBikeScale(float scale) { SetBikeScale(scale); }

        // Called by reapply system to restore invisible bike
        public static void SetInvisibleBike(bool v) { if (v != _invisibleBike) { _invisibleBike = v; ToggleInvisibleBike(v); } }

        // Called by reapply system to restore wheel size
        public static void ApplyWheelSize(bool enabled, int mode)
        {
            int oldMode = _wheelSizeMode;
            _wheelSizeEnabled = enabled;
            _wheelSizeMode = mode;
            if (enabled && mode != 0) SetWheelSize(mode);
            else if (!enabled && oldMode != 0) SetWheelSize(0); // restore physics to default
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

        // ── Wheel Size ────────────────────────────────────────────────
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
            // Restore wheel radius physics and bone scales to 1.0 BEFORE clearing caches
            if (_wheelSizeMode != 0) try { SetWheelSize(0); } catch { }
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
                MelonLogger.Msg("[Bike] Wheel size -> " + WheelLabels[mode] + " (scale " + scale + ")");
            }
            catch (System.Exception ex) { MelonLogger.Error("[Bike] SetWheelSize: " + ex.Message); }
        }

        // ── Reset Tab ─────────────────────────────────────────────────
        private static void ResetBikeTab()
        {
            Suspension.SetTravelLevel(5);
            Suspension.SetStiffnessLevel(5);
            Suspension.SetDampingLevel(5);
            if (_invisibleBike) { ToggleInvisibleBike(false); _invisibleBike = false; }
            ResetWheelSize();
            if (WideTyres.Enabled) WideTyres.Toggle();
            WideTyres.SetLevel(5);
            if (StickyTyres.Enabled) StickyTyres.Toggle();
            if (ReverseSteering.Enabled) ReverseSteering.Toggle();
            if (IceMode.Enabled) IceMode.Toggle();
            if (CutBrakes.Enabled) CutBrakes.Toggle();
            if (BikeTorch.Enabled) BikeTorch.Toggle();
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player != null)
                {
                    Transform bikeModel = player.transform.Find("BikeModel");
                    if ((object)bikeModel != null) bikeModel.localScale = Vector3.one;
                }
            }
            catch { }
        }

        // ── RefreshAll ────────────────────────────────────────────────
        public static void RefreshAll()
        {
            // Suspension
            if (_travelVal) _travelVal.text = Suspension.TravelLevel.ToString();
            if (_stiffVal) _stiffVal.text = Suspension.StiffnessLevel.ToString();
            if (_dampVal) _dampVal.text = Suspension.DampingLevel.ToString();
            UIHelpers.SetBar(_travelBar, (Suspension.TravelLevel - 1) / 9f);
            UIHelpers.SetBar(_stiffBar, (Suspension.StiffnessLevel - 1) / 9f);
            UIHelpers.SetBar(_dampBar, (Suspension.DampingLevel - 1) / 9f);

            // Invisible Bike
            if (_invisBikeVal) { _invisBikeVal.text = _invisibleBike ? "ON" : "OFF"; _invisBikeVal.color = _invisibleBike ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_invisBikeTrack, _invisBikeKnob, _invisibleBike);
            UIHelpers.SetRowActive(_invisBikeRow, _invisibleBike);

            // Wheel Size
            if (_wheelSizeTogVal) { _wheelSizeTogVal.text = _wheelSizeEnabled ? "ON" : "OFF"; _wheelSizeTogVal.color = _wheelSizeEnabled ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_wheelSizeTrack, _wheelSizeKnob, _wheelSizeEnabled);
            UIHelpers.SetRowActive(_wheelSizeRow, _wheelSizeEnabled);

            // Wide Tyres
            bool wtOn = WideTyres.Enabled;
            if (_wideTyresVal) { _wideTyresVal.text = wtOn ? "ON" : "OFF"; _wideTyresVal.color = wtOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_wideTyresTrack, _wideTyresKnob, wtOn);
            if (_wideTyresLvlVal) _wideTyresLvlVal.text = WideTyres.Level.ToString();
            UIHelpers.SetBar(_wideTyresBar, (WideTyres.Level - 1) / 19f);
            if ((object)_wideTyresMinus != null) _wideTyresMinus.interactable = wtOn;
            if ((object)_wideTyresPlus != null) _wideTyresPlus.interactable = wtOn;
            UIHelpers.SetRowActive(_wideTyresRow, wtOn);

            // Sticky Tyres
            bool stOn = StickyTyres.Enabled;
            if (_stickyVal) { _stickyVal.text = stOn ? "ON" : "OFF"; _stickyVal.color = stOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_stickyTrack, _stickyKnob, stOn);
            UIHelpers.SetRowActive(_stickyRow, stOn);

            // Reverse Steering
            bool revOn = ReverseSteering.Enabled;
            if (_revSteerVal) { _revSteerVal.text = revOn ? "ON" : "OFF"; _revSteerVal.color = revOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_revSteerTrack, _revSteerKnob, revOn);
            UIHelpers.SetRowActive(_revSteerRow, revOn);

            // Ice Grip
            bool imOn = IceMode.Enabled;
            if (_iceModeVal) { _iceModeVal.text = imOn ? "ON" : "OFF"; _iceModeVal.color = imOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_iceModeTrack, _iceModeKnob, imOn);
            UIHelpers.SetRowActive(_iceModeRow, imOn);

            // Cut Brakes
            bool cbOn = CutBrakes.Enabled;
            if (_cutBrakesVal) { _cutBrakesVal.text = cbOn ? "ON" : "OFF"; _cutBrakesVal.color = cbOn ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_cutBrakesTrack, _cutBrakesKnob, cbOn);
            UIHelpers.SetRowActive(_cutBrakesRow, cbOn);

            // Torch
            bool torch = BikeTorch.Enabled;
            if (_torchVal) { _torchVal.text = torch ? "ON" : "OFF"; _torchVal.color = torch ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_torchTrack, _torchKnob, torch);
            UIHelpers.SetRowActive(_torchRow, torch);
            if (_torchIntLbl) _torchIntLbl.text = BikeTorch.IntensityDisplay;
        }
    }
}