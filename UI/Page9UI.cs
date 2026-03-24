using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class Page9UI
    {
        private static bool _invisiblePlayer = false;
        private static bool _turboWind = false;
        private static float _savedWindMain = -1f;
        private static Image _invisTrack; private static RectTransform _invisKnob;
        private static Image _windTrack; private static RectTransform _windKnob;
        private static Text _invisVal, _windVal;

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
                var ir = UIHelpers.StatRow("Invisible", pg.transform);
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

                // Exploding props
                var er = UIHelpers.StatRow("Exploding Props", pg.transform);
                UIHelpers.ActionBtn(er.transform, "Enable", () => SetBounceForce(2000f), 60);
                UIHelpers.ActionBtn(er.transform, "Reset", () => SetBounceForce(400f), 52);

                RefreshAll();
            }
            catch (System.Exception ex) { MelonLogger.Error("Page9UI.CreatePage: " + ex.Message); return null; }
            return pg;
        }

        // ── Player Scale ──────────────────────────────────────────────────
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

        // ── Invisible ─────────────────────────────────────────────────────
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

        // ── Turbo Wind ────────────────────────────────────────────────────
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

        // ── Exploding Props ───────────────────────────────────────────────
        private static void SetBounceForce(float force)
        {
            try
            {
                BounceVolume[] volumes = GameObject.FindObjectsOfType<BounceVolume>();
                System.Reflection.FieldInfo forceField = null;
                for (int i = 0; i < volumes.Length; i++)
                {
                    if ((object)forceField == null)
                        forceField = volumes[i].GetType().GetField("_force",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if ((object)forceField != null)
                        forceField.SetValue(volumes[i], new Vector3(0f, force, 0f));
                }
                MelonLogger.Msg("[Silly] BounceForce -> " + force + " on " + volumes.Length + " volumes.");
            }
            catch (System.Exception ex) { MelonLogger.Error("[Silly] SetBounceForce: " + ex.Message); }
        }

        public static void RefreshAll()
        {
            if (_invisVal) { _invisVal.text = _invisiblePlayer ? "ON" : "OFF"; _invisVal.color = _invisiblePlayer ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_invisTrack, _invisKnob, _invisiblePlayer);
            if (_windVal) { _windVal.text = _turboWind ? "ON" : "OFF"; _windVal.color = _turboWind ? UIHelpers.OnColor : UIHelpers.OffColor; }
            UIHelpers.SetToggle(_windTrack, _windKnob, _turboWind);
        }
    }
}