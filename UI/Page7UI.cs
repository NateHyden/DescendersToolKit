using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page7UI
    {
        // ── Sky Colours ───────────────────────────────────────────────────
        private static Image _stormTrack; private static RectTransform _stormKnob;
        private static Text _stormVal, _skyPresetVal;

        private static Text _gravityVal;
        private static Image _gravityBar;
        private static Text _todVal;
        private static Image _todBar;
        private static Text _treesVal;
        private static Image _treesTrack;
        private static RectTransform _treesKnob;
        private static Text _musicVal;
        private static Image _musicTrack;
        private static RectTransform _musicKnob;
        private static Text _fogVal;
        private static Image _fogTrack;
        private static RectTransform _fogKnob;

        public static bool TreesEnabled = true;
        public static bool MusicEnabled = true;
        public static bool FogEnabled = true;

        // Cached terrain type to avoid repeated reflection
        private static System.Type _terrainType = null;
        private static System.Reflection.PropertyInfo _dtfProp = null;
        private static float _savedMusicVolume = 1f;
        private static float _savedFogDensity = -1f;
        private static bool _savedFogState = true;

        public static GameObject CreatePage(Transform parent)
        {
            GameObject pg = null;
            try
            {
                pg = UIHelpers.Obj("P7R", parent);
                UIHelpers.Fill(UIHelpers.RT(pg));

                // ScrollRect wrapper — same pattern as Page9UI
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

                var csf = content.AddComponent<ContentSizeFitter>();
                csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                var vlg = content.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = UIHelpers.RowGap;
                vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true;
                vlg.childForceExpandHeight = false;

                // All rows go into scrollable content
                var pg7 = content.transform;

                // ── Physics ───────────────────────────────────────────────
                // ── Sky ─────────────────────────────────────────────────────
                UIHelpers.SectionHeader("SKY", pg7);

                var skpr = UIHelpers.StatRow("Colour", pg7);
                _skyPresetVal = UIHelpers.Txt("SkV", skpr.transform, SkyColours.PresetNames[0], 11,
                    FontStyle.Bold, TextAnchor.MiddleLeft, UIHelpers.Accent);
                _skyPresetVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 60;
                UIHelpers.ActionBtn(skpr.transform, "Normal", () => { SkyColours.ApplyPreset(0); RefreshAll(); }, 46);
                UIHelpers.ActionBtn(skpr.transform, "Blood Red", () => { SkyColours.ApplyPreset(1); RefreshAll(); }, 58);
                UIHelpers.ActionBtn(skpr.transform, "Alien", () => { SkyColours.ApplyPreset(2); RefreshAll(); }, 40);
                UIHelpers.ActionBtn(skpr.transform, "Synthwave", () => { SkyColours.ApplyPreset(3); RefreshAll(); }, 60);
                UIHelpers.ActionBtn(skpr.transform, "Midnight", () => { SkyColours.ApplyPreset(4); RefreshAll(); }, 55);
                UIHelpers.ActionBtn(skpr.transform, "Toxic", () => { SkyColours.ApplyPreset(5); RefreshAll(); }, 40);

                var stmr = UIHelpers.StatRow("Storm", pg7);
                _stormVal = UIHelpers.Txt("StmV", stmr.transform, "OFF", 11, FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OffColor);
                _stormVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(stmr.transform, "StmT", () => { SkyColours.ToggleStorm(); RefreshAll(); }, out _stormTrack, out _stormKnob);

                UIHelpers.Divider(pg7);

                // ── Physics ───────────────────────────────────────────────
                UIHelpers.SectionHeader("PHYSICS", pg7);

                var gr = UIHelpers.StatRow("Gravity", pg7);
                _gravityBar = UIHelpers.MakeBar("GrB", gr.transform, (Gravity.Level - 1) / 9f);
                _gravityVal = UIHelpers.Txt("GrV", gr.transform, Gravity.DisplayValue, 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                _gravityVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 36;
                UIHelpers.SmallBtn(gr.transform, "-", () => { Gravity.Decrease(); RefreshAll(); });
                UIHelpers.SmallBtn(gr.transform, "+", () => { Gravity.Increase(); RefreshAll(); });
                UIHelpers.Txt("GrHint", gr.transform, "def:-17.5", 9, FontStyle.Italic, TextAnchor.MiddleRight, UIHelpers.TextDim)
                    .gameObject.AddComponent<LayoutElement>().preferredWidth = 52;

                UIHelpers.Divider(pg7);

                // ── Environment ───────────────────────────────────────────
                UIHelpers.SectionHeader("ENVIRONMENT", pg7);

                var tr = UIHelpers.StatRow("Time of Day", pg7);
                _todBar = UIHelpers.MakeBar("TdB", tr.transform, (TimeOfDay.Level - 1) / 9f);
                _todVal = UIHelpers.Txt("TdV", tr.transform, TimeOfDay.DisplayValue, 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                _todVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 70;
                UIHelpers.SmallBtn(tr.transform, "-", () => { TimeOfDay.Decrease(); RefreshAll(); });
                UIHelpers.SmallBtn(tr.transform, "+", () => { TimeOfDay.Increase(); RefreshAll(); });

                var ter = UIHelpers.StatRow("Trees & Foliage", pg7);
                _treesVal = UIHelpers.Txt("TrV", ter.transform, "ON", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OnColor);
                _treesVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(ter.transform, "TrT", () =>
                {
                    TreesEnabled = !TreesEnabled;
                    ToggleTrees(TreesEnabled);
                    RefreshAll();
                }, out _treesTrack, out _treesKnob);

                var mur = UIHelpers.StatRow("Music", pg7);
                _musicVal = UIHelpers.Txt("MuV", mur.transform, "ON", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OnColor);
                _musicVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(mur.transform, "MuT", () =>
                {
                    MusicEnabled = !MusicEnabled;
                    ToggleMusic(MusicEnabled);
                    RefreshAll();
                }, out _musicTrack, out _musicKnob);

                var fogr = UIHelpers.StatRow("Fog", pg7);
                _fogVal = UIHelpers.Txt("FgV", fogr.transform, "ON", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OnColor);
                _fogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(fogr.transform, "FgT", () =>
                {
                    FogEnabled = !FogEnabled;
                    ToggleFog(FogEnabled);
                    RefreshAll();
                }, out _fogTrack, out _fogKnob);

                UIHelpers.Divider(pg7);

                // ── Level Tools ───────────────────────────────────────────
                UIHelpers.SectionHeader("LEVEL", pg7);

                var jr = UIHelpers.StatRow("Jump to Finish", pg7);
                UIHelpers.ActionBtnOrange(jr.transform, "Jump", () =>
                {
                    try { DevCommandsGameplay.JumpToFinish(); }
                    catch (System.Exception ex) { MelonLogger.Error("[JumpToFinish]: " + ex.Message); }
                }, 60);

                var sr = UIHelpers.StatRow("Skip Song", pg7);
                UIHelpers.ActionBtn(sr.transform, "Skip", () =>
                {
                    try { DevCommandsGameplay.SkipSong(); }
                    catch (System.Exception ex) { MelonLogger.Error("[SkipSong]: " + ex.Message); }
                }, 60);

                RefreshAll();
                UIHelpers.AddScrollForwarders(pg7);
            }
            catch (System.Exception ex) { MelonLogger.Error("Page7UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        private static void ToggleTrees(bool enabled)
        {
            try
            {
                // Cache terrain type
                if ((object)_terrainType == null)
                {
                    System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                    for (int a = 0; a < assemblies.Length; a++)
                    {
                        _terrainType = assemblies[a].GetType("UnityEngine.Terrain");
                        if ((object)_terrainType != null) break;
                    }
                }
                if ((object)_terrainType == null) { MelonLogger.Warning("[Trees] Terrain type not found."); return; }

                if ((object)_dtfProp == null)
                    _dtfProp = _terrainType.GetProperty("drawTreesAndFoliage");
                if ((object)_dtfProp == null) { MelonLogger.Warning("[Trees] drawTreesAndFoliage not found."); return; }

                UnityEngine.Object[] terrains = UnityEngine.Object.FindObjectsOfType(_terrainType);
                for (int i = 0; i < terrains.Length; i++)
                    _dtfProp.SetValue(terrains[i], enabled, null);

                MelonLogger.Msg("[Trees] Trees & Foliage -> " + (enabled ? "ON" : "OFF"));
            }
            catch (System.Exception ex) { MelonLogger.Error("[Trees] ToggleTrees: " + ex.Message); }
        }

        private static void ToggleMusic(bool enabled)
        {
            try
            {
                AudioManager mgr = UnityEngine.Object.FindObjectOfType<AudioManager>();
                if ((object)mgr == null) { MelonLogger.Warning("[Music] AudioManager not found."); return; }

                System.Reflection.MethodInfo setMethod = mgr.GetType().GetMethod(
                    "SetCategoryVolume",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if ((object)setMethod == null) { MelonLogger.Warning("[Music] SetCategoryVolume not found."); return; }

                System.Reflection.ParameterInfo[] parms = setMethod.GetParameters();
                if (parms.Length < 2) { MelonLogger.Warning("[Music] wrong param count: " + parms.Length); return; }

                System.Type enumType = parms[0].ParameterType;
                // Enum: Master=0, Music=1, Sfx=2, Ambient=3, Voices=4
                object musicEnum = System.Enum.ToObject(enumType, 1);

                if (!enabled)
                {
                    // Cache current volume before muting
                    System.Reflection.MethodInfo getMethod = mgr.GetType().GetMethod(
                        "GetCategoryVolume",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if ((object)getMethod != null)
                    {
                        object vol = getMethod.Invoke(mgr, new object[] { musicEnum });
                        if (vol is float) _savedMusicVolume = (float)vol;
                    }
                    setMethod.Invoke(mgr, new object[] { musicEnum, 0f });
                    MelonLogger.Msg("[Music] Muted. Saved volume: " + _savedMusicVolume);
                }
                else
                {
                    // Restore saved volume
                    setMethod.Invoke(mgr, new object[] { musicEnum, _savedMusicVolume });
                    MelonLogger.Msg("[Music] Restored volume: " + _savedMusicVolume);
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("[Music] ToggleMusic: " + ex.Message); }
        }

        private static void ToggleFog(bool enabled)
        {
            try
            {
                if (!enabled)
                {
                    if (_savedFogDensity < 0f)
                    {
                        _savedFogDensity = RenderSettings.fogDensity;
                        _savedFogState = RenderSettings.fog;
                    }
                    RenderSettings.fog = false;
                    RenderSettings.fogDensity = 0f;
                    MelonLogger.Msg("[Fog] Disabled");
                }
                else
                {
                    RenderSettings.fog = _savedFogState;
                    RenderSettings.fogDensity = _savedFogDensity >= 0f ? _savedFogDensity : 0.01f;
                    MelonLogger.Msg("[Fog] Restored density: " + RenderSettings.fogDensity);
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("[Fog] ToggleFog: " + ex.Message); }
        }

        public static void RefreshAll()
        {
            if (_gravityVal) _gravityVal.text = Gravity.DisplayValue;
            UIHelpers.SetBar(_gravityBar, (Gravity.Level - 1) / 9f);
            if (_todVal) _todVal.text = TimeOfDay.DisplayValue;
            UIHelpers.SetBar(_todBar, (TimeOfDay.Level - 1) / 9f);

            if (_treesVal) { _treesVal.text = TreesEnabled ? "ON" : "OFF"; _treesVal.color = TreesEnabled ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_treesTrack, _treesKnob, TreesEnabled);

            if (_musicVal) { _musicVal.text = MusicEnabled ? "ON" : "OFF"; _musicVal.color = MusicEnabled ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_musicTrack, _musicKnob, MusicEnabled);

            if (_fogVal) { _fogVal.text = FogEnabled ? "ON" : "OFF"; _fogVal.color = FogEnabled ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_fogTrack, _fogKnob, FogEnabled);

            if (_skyPresetVal) _skyPresetVal.text = SkyColours.PresetNames[SkyColours.CurrentPreset];
            bool storm = SkyColours.StormEnabled;
            if (_stormVal) { _stormVal.text = storm ? "ON" : "OFF"; _stormVal.color = storm ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_stormTrack, _stormKnob, storm);
        }
    }
}