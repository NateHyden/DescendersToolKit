using DescendersModMenu.Mods;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page7UI
    {
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
                var vlg = pg.AddComponent<VerticalLayoutGroup>();
                vlg.spacing = UIHelpers.RowGap;
                vlg.padding = new RectOffset((int)UIHelpers.ContentPad, (int)UIHelpers.ContentPad, 8, 8);
                vlg.childAlignment = TextAnchor.UpperCenter;
                vlg.childForceExpandWidth = true;
                vlg.childForceExpandHeight = false;

                // ── Physics ───────────────────────────────────────────────
                UIHelpers.SectionHeader("PHYSICS", pg.transform);

                var gr = UIHelpers.StatRow("Gravity", pg.transform);
                _gravityBar = UIHelpers.MakeBar("GrB", gr.transform, (Gravity.Level - 1) / 9f);
                _gravityVal = UIHelpers.Txt("GrV", gr.transform, Gravity.DisplayValue, 12,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.TextMid);
                _gravityVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 36;
                UIHelpers.SmallBtn(gr.transform, "-", () => { Gravity.Decrease(); RefreshAll(); });
                UIHelpers.SmallBtn(gr.transform, "+", () => { Gravity.Increase(); RefreshAll(); });

                UIHelpers.Divider(pg.transform);

                // ── Environment ───────────────────────────────────────────
                UIHelpers.SectionHeader("ENVIRONMENT", pg.transform);

                var tr = UIHelpers.StatRow("Time of Day", pg.transform);
                _todBar = UIHelpers.MakeBar("TdB", tr.transform, (TimeOfDay.Level - 1) / 9f);
                _todVal = UIHelpers.Txt("TdV", tr.transform, TimeOfDay.DisplayValue, 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.Accent);
                _todVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 70;
                UIHelpers.SmallBtn(tr.transform, "-", () => { TimeOfDay.Decrease(); RefreshAll(); });
                UIHelpers.SmallBtn(tr.transform, "+", () => { TimeOfDay.Increase(); RefreshAll(); });

                var ter = UIHelpers.StatRow("Trees & Foliage", pg.transform);
                _treesVal = UIHelpers.Txt("TrV", ter.transform, "ON", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OnColor);
                _treesVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(ter.transform, "TrT", () =>
                {
                    TreesEnabled = !TreesEnabled;
                    ToggleTrees(TreesEnabled);
                    RefreshAll();
                }, out _treesTrack, out _treesKnob);

                var mur = UIHelpers.StatRow("Music", pg.transform);
                _musicVal = UIHelpers.Txt("MuV", mur.transform, "ON", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OnColor);
                _musicVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(mur.transform, "MuT", () =>
                {
                    MusicEnabled = !MusicEnabled;
                    ToggleMusic(MusicEnabled);
                    RefreshAll();
                }, out _musicTrack, out _musicKnob);

                var fogr = UIHelpers.StatRow("Fog", pg.transform);
                _fogVal = UIHelpers.Txt("FgV", fogr.transform, "ON", 11,
                    FontStyle.Bold, TextAnchor.MiddleCenter, UIHelpers.OnColor);
                _fogVal.gameObject.AddComponent<LayoutElement>().preferredWidth = 28;
                UIHelpers.Toggle(fogr.transform, "FgT", () =>
                {
                    FogEnabled = !FogEnabled;
                    ToggleFog(FogEnabled);
                    RefreshAll();
                }, out _fogTrack, out _fogKnob);

                UIHelpers.Divider(pg.transform);

                // ── Level Tools ───────────────────────────────────────────
                UIHelpers.SectionHeader("LEVEL", pg.transform);

                var jr = UIHelpers.StatRow("Jump to Finish", pg.transform);
                UIHelpers.ActionBtn(jr.transform, "Jump", () =>
                {
                    try { DevCommandsGameplay.JumpToFinish(); }
                    catch (System.Exception ex) { MelonLogger.Error("[JumpToFinish]: " + ex.Message); }
                }, 60);

                var sr = UIHelpers.StatRow("Skip Song", pg.transform);
                UIHelpers.ActionBtn(sr.transform, "Skip", () =>
                {
                    try { DevCommandsGameplay.SkipSong(); }
                    catch (System.Exception ex) { MelonLogger.Error("[SkipSong]: " + ex.Message); }
                }, 60);

                RefreshAll();
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
        }
    }
}