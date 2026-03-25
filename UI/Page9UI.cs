using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page9UI
    {
        // ── Existing state ────────────────────────────────────────────────
        private static bool _invisiblePlayer = false;
        private static bool _turboWind = false;
        private static float _savedWindMain = -1f;
        private static Image _invisTrack; private static RectTransform _invisKnob;
        private static Image _windTrack; private static RectTransform _windKnob;
        private static Text _invisVal, _windVal;

        // ── New state: Invisible Bike ─────────────────────────────────────
        private static bool _invisibleBike = false;
        private static Image _invisBikeTrack; private static RectTransform _invisBikeKnob;
        private static Text _invisBikeVal;

        // ── New state: Moon Mode ──────────────────────────────────────────
        private static bool _moonModeActive = false;
        private static Image _moonBg, _moonBdr;
        private static Text _moonTxt;
        // Saved levels so we can restore on reset
        private static int _savedGravityLevel = -1;
        private static int _savedTravelLevel = -1;
        private static int _savedDampingLevel = -1;

        // ── New state: Giant Wheels ───────────────────────────────────────
        private static int _wheelSizeMode = 0;

        // ── Exploding Props state ────────────────────────────────────────
        private static bool _explodingProps = false;
        private static Image _explodeTrack; private static RectTransform _explodeKnob;
        private static Text _explodeVal;

        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
                pg = UIHelpers.Obj("P9R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));
                var vlg = pg.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = UIHelpers.RowGap;
                vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true;
                vlg.childForceExpandHeight = false;

                // ── Player Size ───────────────────────────────────────────
                UIHelpers.SectionHeader("PLAYER SIZE", pg.transform);

                var psr = UIHelpers.StatRow("Size", pg.transform);
                UIHelpers.ActionBtn(psr.transform, "Giant", () => SetPlayerScale(3.5f), 52);
                UIHelpers.ActionBtn(psr.transform, "Big", () => SetPlayerScale(1.5f), 44);
                UIHelpers.ActionBtn(psr.transform, "Default", () => SetPlayerScale(1.0f), 58);
                UIHelpers.ActionBtn(psr.transform, "Small", () => SetPlayerScale(0.6f), 52);
                UIHelpers.ActionBtn(psr.transform, "Tiny", () => SetPlayerScale(0.2f), 44);

                UIHelpers.Divider(pg.transform);

                // ── Bike ──────────────────────────────────────────────────
                UIHelpers.SectionHeader("BIKE", pg.transform);

                // Invisible Bike toggle
                var ibr = UIHelpers.StatRow("Invisible Bike", pg.transform);
                _invisBikeVal = UIHelpers.Txt("IbV", ibr.transform, "OFF", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _invisBikeVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(ibr.transform, "IbT", () =>
                {
                    _invisibleBike = !_invisibleBike;
                    ToggleInvisibleBike(_invisibleBike);
                    RefreshAll();
                }, out _invisBikeTrack, out _invisBikeKnob);

                // Giant Wheels
                var gwr = UIHelpers.StatRow("Wheel Size", pg.transform);
                UIHelpers.ActionBtn(gwr.transform, "Small", () => { SetWheelSize(1); RefreshAll(); }, 52);
                UIHelpers.ActionBtn(gwr.transform, "Default", () => { SetWheelSize(0); RefreshAll(); }, 58);
                UIHelpers.ActionBtn(gwr.transform, "Large", () => { SetWheelSize(2); RefreshAll(); }, 52);

                UIHelpers.Divider(pg.transform);

                // ── Presets ───────────────────────────────────────────────
                UIHelpers.SectionHeader("PRESETS", pg.transform);

                // Moon Mode — compound row like NoSpeedCap in Stats tab
                var mmo = UIHelpers.Panel("MMR", pg.transform, UIHelpers.RowBg, UIHelpers.RowSp);
                mmo.AddComponent<LayoutElement>().minHeight = UIHelpers.RowH + 38;
                var mmbd = UIHelpers.Panel("MMBd", mmo.transform, UIHelpers.RowBorder, UIHelpers.RowSp);
                mmbd.GetComponent<Image>().raycastTarget = false; UIHelpers.Fill(UIHelpers.RT(mmbd));
                mmbd.AddComponent<LayoutElement>().ignoreLayout = true;
                var mmvlg = mmo.AddComponent<VerticalLayoutGroup>();
                mmvlg.spacing = 4; mmvlg.padding = new RectOffset((int)UIHelpers.RowPad, (int)UIHelpers.RowPad, 6, 8);
                mmvlg.childAlignment = TextAnchor.UpperCenter;
                mmvlg.childForceExpandWidth = true; mmvlg.childForceExpandHeight = false;

                // Top row: label + description
                var mmtop = UIHelpers.Obj("MMTop", mmo.transform);
                mmtop.AddComponent<LayoutElement>().preferredHeight = 28;
                var mmhlg = mmtop.AddComponent<HorizontalLayoutGroup>();
                mmhlg.spacing = 8; mmhlg.childAlignment = TextAnchor.MiddleCenter;
                mmhlg.childForceExpandWidth = false; mmhlg.childForceExpandHeight = false;
                var mml = UIHelpers.Txt("MML", mmtop.transform, "Moon Mode", 12, FontStyle.Bold,
                    TextAnchor.MiddleLeft, UIHelpers.TextLight);
                var mmle = mml.gameObject.AddComponent<LayoutElement>();
                mmle.flexibleWidth = 1; mmle.preferredHeight = 28;
                var mmdesc = UIHelpers.Txt("MMD", mmtop.transform, "Low gravity + bouncy suspension", 10,
                    FontStyle.Italic, TextAnchor.MiddleRight, UIHelpers.TextDim);
                mmdesc.gameObject.AddComponent<LayoutElement>().preferredWidth = 200;

                // Bottom row: the big toggle button
                var mmBtn = UIHelpers.Obj("MMBtn", mmo.transform);
                _moonBg = mmBtn.AddComponent<Image>(); _moonBg.sprite = UIHelpers.BtnSp;
                _moonBg.type = Image.Type.Sliced; _moonBg.color = UIHelpers.Accent;
                var mbtn = mmBtn.AddComponent<Button>();
                mbtn.onClick.AddListener(() => { ToggleMoonMode(); RefreshAll(); });
                var mcb = mbtn.colors;
                mcb.normalColor = Color.white; mcb.highlightedColor = new Color(1, 1, 1, 1.15f);
                mcb.pressedColor = new Color(.7f, .7f, .7f, 1); mcb.colorMultiplier = 1; mcb.fadeDuration = .08f;
                mbtn.colors = mcb;
                mmBtn.AddComponent<LayoutElement>().preferredHeight = 30;
                var mbdr = UIHelpers.Panel("MBdr", mmBtn.transform, UIHelpers.AccentBdr, UIHelpers.BtnSp);
                _moonBdr = mbdr.GetComponent<Image>(); _moonBdr.raycastTarget = false;
                UIHelpers.Fill(UIHelpers.RT(mbdr));
                _moonTxt = UIHelpers.Txt("MT", mmBtn.transform, "ACTIVATE MOON MODE", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, Color.white);
                _moonTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
                UIHelpers.Fill(UIHelpers.RT(_moonTxt.gameObject));

                UIHelpers.Divider(pg.transform);

                // ── Multiplayer Chaos ─────────────────────────────────────
                UIHelpers.SectionHeader("MULTIPLAYER", pg.transform);

                var gsr = UIHelpers.StatRow("Giant Everyone", pg.transform);
                UIHelpers.ActionBtn(gsr.transform, "Giant", () => SetAllPlayersScale(3.5f), 52);
                UIHelpers.ActionBtn(gsr.transform, "Default", () => SetAllPlayersScale(1.0f), 58);
                UIHelpers.ActionBtn(gsr.transform, "Tiny", () => SetAllPlayersScale(0.2f), 44);

                UIHelpers.Divider(pg.transform);

                // ── World ─────────────────────────────────────────────────
                UIHelpers.SectionHeader("WORLD", pg.transform);

                // Invisible player
                var ir = UIHelpers.StatRow("Invisible Player", pg.transform);
                _invisVal = UIHelpers.Txt("InV", ir.transform, "OFF", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _invisVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(ir.transform, "InT", () =>
                {
                    _invisiblePlayer = !_invisiblePlayer;
                    ToggleInvisible(_invisiblePlayer);
                    RefreshAll();
                }, out _invisTrack, out _invisKnob);

                // Turbo wind
                var wr = UIHelpers.StatRow("Turbo Wind", pg.transform);
                _windVal = UIHelpers.Txt("WnV", wr.transform, "OFF", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _windVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(wr.transform, "WnT", () =>
                {
                    _turboWind = !_turboWind;
                    ToggleTurboWind(_turboWind);
                    RefreshAll();
                }, out _windTrack, out _windKnob);

                // No Mistakes
                var er = UIHelpers.StatRow("No Mistakes", pg.transform);
                _explodeVal = UIHelpers.Txt("ExV", er.transform, "OFF", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _explodeVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(er.transform, "ExT", () =>
                {
                    ExplodingProps.Toggle();
                    _explodingProps = ExplodingProps.Enabled;
                    RefreshAll();
                }, out _explodeTrack, out _explodeKnob);

                UIHelpers.InfoBox(pg.transform, "Hitting any wall, rock or obstacle will launch you backwards with a crash sound.");

                RefreshAll();
            }
            catch (System.Exception ex) { MelonLogger.Error("Page9UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        // ══════════════════════════════════════════════════════════════════
        //  INVISIBLE BIKE
        // ══════════════════════════════════════════════════════════════════
        private static void ToggleInvisibleBike(bool invisible)
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) { MelonLogger.Warning("[Silly] Player_Human not found."); return; }

                // BikeModel holds the bike mesh — confirmed from scene dump path
                Transform bikeModel = player.transform.Find("BikeModel");
                if ((object)bikeModel == null) { MelonLogger.Warning("[Silly] BikeModel not found."); return; }

                // Disable all renderers under BikeModel rather than deactivating the GO
                // This keeps physics and collision intact while hiding the visuals
                Renderer[] renderers = bikeModel.GetComponentsInChildren<Renderer>(true);
                for (int i = 0; i < renderers.Length; i++)
                    renderers[i].enabled = !invisible;

                MelonLogger.Msg("[Silly] Invisible Bike -> " + invisible + " (" + renderers.Length + " renderers)");
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] ToggleInvisibleBike: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════════════
        //  GIANT WHEELS
        // ══════════════════════════════════════════════════════════════════
        private static readonly float[] WheelScales = { 1.0f, 0.4f, 2.5f };
        private static readonly string[] WheelLabels = { "Default", "Small", "Large" };

        // Wheel.HqsqNkJ = wheel radius — from decompilation
        private static System.Reflection.FieldInfo _wheelRadiusField = null;
        private static float _defaultRadiusFront = -1f;
        private static float _defaultRadiusBack = -1f;

        // BikeAnimation has Transform fields pointing to the actual wheel bone joints:
        //   YLzyVuM -> backWheel_Jnt    RCNLpue -> frontWheel_Jnt
        // These are the bones that drive the SkinnedMeshRenderer wheel visuals
        private static System.Reflection.FieldInfo _backBoneField = null;
        private static System.Reflection.FieldInfo _frontBoneField = null;

        private static void SetWheelSize(int mode)
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) { MelonLogger.Warning("[Silly] Player_Human not found."); return; }

                float scale = WheelScales[mode];

                // ── 1. Scale the visual wheel BONES via BikeAnimation ─────
                // The wheel meshes are part of bike_skinning (SkinnedMeshRenderer)
                // Scaling the bone transforms deforms the mesh around the wheels
                Transform bikeModel = player.transform.Find("BikeModel");
                if ((object)bikeModel != null)
                {
                    BikeAnimation bikeAnim = bikeModel.GetComponent<BikeAnimation>();
                    if ((object)bikeAnim != null)
                    {
                        // Cache bone field references
                        if ((object)_backBoneField == null || (object)_frontBoneField == null)
                        {
                            System.Reflection.FieldInfo[] fields = bikeAnim.GetType().GetFields(
                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                            for (int i = 0; i < fields.Length; i++)
                            {
                                if (!string.Equals(fields[i].FieldType.Name, "Transform",
                                    System.StringComparison.Ordinal)) continue;

                                // YLzyVuM = backWheel_Jnt, RCNLpue = frontWheel_Jnt
                                object val = fields[i].GetValue(bikeAnim);
                                Transform t = val as Transform;
                                if ((object)t == null) continue;

                                if (t.name == "backWheel_Jnt")
                                { _backBoneField = fields[i]; MelonLogger.Msg("[Silly] Found backWheel bone: " + fields[i].Name); }
                                else if (t.name == "frontWheel_Jnt")
                                { _frontBoneField = fields[i]; MelonLogger.Msg("[Silly] Found frontWheel bone: " + fields[i].Name); }
                            }
                        }

                        // Scale the bones
                        if ((object)_backBoneField != null)
                        {
                            Transform backBone = _backBoneField.GetValue(bikeAnim) as Transform;
                            if ((object)backBone != null)
                                backBone.localScale = new Vector3(scale, scale, scale);
                        }
                        if ((object)_frontBoneField != null)
                        {
                            Transform frontBone = _frontBoneField.GetValue(bikeAnim) as Transform;
                            if ((object)frontBone != null)
                                frontBone.localScale = new Vector3(scale, scale, scale);
                        }
                    }
                    else
                    {
                        // Fallback: navigate bone hierarchy directly
                        Transform frontBone = bikeModel.Find("root_Jnt/Frame_Jnt/steer_Jnt/forkShockAbsorber_Jnt/frontWheel_Jnt");
                        Transform backBone = bikeModel.Find("root_Jnt/Frame_Jnt/backWheelRotator_Jnt/BackWheelShockAbsorber_Jnt/backWheel_Jnt");
                        if ((object)frontBone != null)
                            frontBone.localScale = new Vector3(scale, scale, scale);
                        if ((object)backBone != null)
                            backBone.localScale = new Vector3(scale, scale, scale);
                    }
                }

                // ── 2. Scale the physics radius on Wheel components ───────
                Wheel[] wheels = player.GetComponentsInChildren<Wheel>();
                if (wheels != null && wheels.Length > 0)
                {
                    for (int i = 0; i < wheels.Length; i++)
                    {
                        if ((object)_wheelRadiusField == null)
                            _wheelRadiusField = wheels[i].GetType().GetField("HqsqNkJ",
                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if ((object)_wheelRadiusField == null)
                        { MelonLogger.Warning("[Silly] Wheel radius field (HqsqNkJ) not found."); break; }

                        bool isFront = string.Equals(wheels[i].gameObject.name, "wheel_front",
                            System.StringComparison.Ordinal);

                        // Cache defaults per wheel (front/back may differ)
                        if (isFront && _defaultRadiusFront < 0f)
                            _defaultRadiusFront = (float)_wheelRadiusField.GetValue(wheels[i]);
                        else if (!isFront && _defaultRadiusBack < 0f)
                            _defaultRadiusBack = (float)_wheelRadiusField.GetValue(wheels[i]);

                        float def = isFront ? _defaultRadiusFront : _defaultRadiusBack;
                        if (def > 0f)
                            _wheelRadiusField.SetValue(wheels[i], def * scale);
                    }
                }

                _wheelSizeMode = mode;
                MelonLogger.Msg("[Silly] Wheel size -> " + WheelLabels[mode] + " (scale " + scale + ")");
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] SetWheelSize: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════════════
        //  MOON MODE
        // ══════════════════════════════════════════════════════════════════
        // Gravity level 1 (-5f) + Suspension Travel level 10 (max) + Damping level 1 (min)
        // Saves current levels so deactivating restores them
        private static void ToggleMoonMode()
        {
            try
            {
                if (!_moonModeActive)
                {
                    // Save current levels before applying moon mode
                    _savedGravityLevel = Gravity.Level;
                    _savedTravelLevel = Suspension.TravelLevel;
                    _savedDampingLevel = Suspension.DampingLevel;

                    // Apply moon mode settings
                    Gravity.SetLevel(1);             // -5f — floaty low gravity
                    Suspension.SetTravelLevel(10);   // Max travel — bouncy forks
                    Suspension.SetDampingLevel(1);   // Min damping — springy landings

                    _moonModeActive = true;
                    MelonLogger.Msg("[Silly] Moon Mode ACTIVATED (Gravity=1, Travel=10, Damping=1)");
                }
                else
                {
                    // Restore saved levels
                    if (_savedGravityLevel > 0)
                        Gravity.SetLevel(_savedGravityLevel);
                    else
                        Gravity.SetLevel(5);

                    if (_savedTravelLevel > 0)
                        Suspension.SetTravelLevel(_savedTravelLevel);
                    else
                        Suspension.SetTravelLevel(5);

                    if (_savedDampingLevel > 0)
                        Suspension.SetDampingLevel(_savedDampingLevel);
                    else
                        Suspension.SetDampingLevel(5);

                    _moonModeActive = false;
                    MelonLogger.Msg("[Silly] Moon Mode DEACTIVATED — levels restored");
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] ToggleMoonMode: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════════════
        //  EXISTING — Player Scale
        // ══════════════════════════════════════════════════════════════════
        private static void SetPlayerScale(float scale)
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                Transform cyclist = player.transform.Find("Cyclist");
                if ((object)cyclist == null) return;
                cyclist.localScale = new Vector3(scale, scale, scale);
                MelonLogger.Msg("[Silly] Player scale -> " + scale);
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] SetPlayerScale: " + ex.Message); }
        }

        private static void SetAllPlayersScale(float scale)
        {
            try
            {
                GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
                int count = 0;
                for (int i = 0; i < all.Length; i++)
                {
                    if (all[i].name == "Player_Networked")
                    {
                        Transform cyclist = all[i].transform.Find("Cyclist");
                        if ((object)cyclist != null)
                        { cyclist.localScale = new Vector3(scale, scale, scale); count++; }
                    }
                }
                MelonLogger.Msg("[Silly] Set " + count + " players to scale " + scale);
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] SetAllPlayersScale: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════════════
        //  EXISTING — Invisible Player
        // ══════════════════════════════════════════════════════════════════
        private static void ToggleInvisible(bool invisible)
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                Transform cyclist = player.transform.Find("Cyclist");
                if ((object)cyclist == null) return;
                Renderer[] renderers = cyclist.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                    renderers[i].enabled = !invisible;
                MelonLogger.Msg("[Silly] Invisible -> " + invisible);
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] ToggleInvisible: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════════════
        //  EXISTING — Turbo Wind
        // ══════════════════════════════════════════════════════════════════
        private static System.Type _windZoneType = null;
        private static System.Reflection.PropertyInfo _windMainProp = null;
        private static System.Reflection.PropertyInfo _windTurbProp = null;

        private static void ToggleTurboWind(bool enabled)
        {
            try
            {
                // WindZone is in UnityEngine.WindModule — find via assembly scan
                if ((object)_windZoneType == null)
                {
                    System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                    for (int a = 0; a < assemblies.Length; a++)
                    {
                        _windZoneType = assemblies[a].GetType("UnityEngine.WindZone");
                        if ((object)_windZoneType != null) break;
                    }
                }
                if ((object)_windZoneType == null) { MelonLogger.Warning("[Silly] WindZone type not found."); return; }

                UnityEngine.Object wz = GameObject.FindObjectOfType(_windZoneType);
                if ((object)wz == null) { MelonLogger.Warning("[Silly] WindZone not found."); return; }

                if ((object)_windMainProp == null)
                    _windMainProp = _windZoneType.GetProperty("windMain",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if ((object)_windTurbProp == null)
                    _windTurbProp = _windZoneType.GetProperty("windTurbulence",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

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
                MelonLogger.Msg("[Silly] TurboWind -> " + enabled);
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] ToggleTurboWind: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════════════
        //  REFRESH ALL
        // ══════════════════════════════════════════════════════════════════
        public static void RefreshAll()
        {
            // Invisible Player
            if (_invisVal) { _invisVal.text = _invisiblePlayer ? "ON" : "OFF"; _invisVal.color = _invisiblePlayer ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_invisTrack, _invisKnob, _invisiblePlayer);

            // Turbo Wind
            if (_windVal) { _windVal.text = _turboWind ? "ON" : "OFF"; _windVal.color = _turboWind ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_windTrack, _windKnob, _turboWind);

            // Invisible Bike
            if (_invisBikeVal) { _invisBikeVal.text = _invisibleBike ? "ON" : "OFF"; _invisBikeVal.color = _invisibleBike ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_invisBikeTrack, _invisBikeKnob, _invisibleBike);

            // Moon Mode button
            if (_moonTxt)
            {
                _moonTxt.text = _moonModeActive ? "MOON MODE ACTIVE" : "ACTIVATE MOON MODE";
                _moonTxt.color = Color.white;
            }
            if (_moonBg) _moonBg.color = _moonModeActive ? UIHelpers.OnColor : UIHelpers.Accent;
            if (_moonBdr) _moonBdr.color = _moonModeActive ? UIHelpers.OnBdr : UIHelpers.AccentBdr;

            // Exploding Props
            if (_explodeVal) { _explodeVal.text = _explodingProps ? "ON" : "OFF"; _explodeVal.color = _explodingProps ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_explodeTrack, _explodeKnob, _explodingProps);
        }
    }
}