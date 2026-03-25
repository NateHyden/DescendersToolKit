using System.Reflection;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class SessionTrackers
    {
        // ── Session Timer ─────────────────────────────────────────────────
        private static float _sessionStartTime = -1f;

        public static float SessionTime
        {
            get
            {
                if (_sessionStartTime < 0f) return 0f;
                return Time.unscaledTime - _sessionStartTime;
            }
        }

        public static string SessionTimeDisplay
        {
            get
            {
                float t = SessionTime;
                if (t <= 0f) return "--:--";
                int mins = (int)(t / 60f);
                int secs = (int)(t % 60f);
                return mins.ToString("D2") + ":" + secs.ToString("D2");
            }
        }

        // ── Bail Counter ──────────────────────────────────────────────────
        public static int BailCount { get; private set; } = 0;
        private static float _lastBailTime = -999f;
        private const float BailCooldown = 1.0f; // ignore duplicate Bail() calls within 1 second

        public static string BailCountDisplay
        {
            get { return BailCount.ToString(); }
        }

        // Called by Harmony postfix on Cyclist.Bail()
        public static void OnBailDetected()
        {
            // Game calls Bail() twice per wipeout from two VehicleController code paths
            // Debounce so we only count once per actual crash
            float now = Time.unscaledTime;
            if (now - _lastBailTime < BailCooldown) return;
            _lastBailTime = now;
            BailCount++;
            MelonLogger.Msg("[SessionTrackers] Bail #" + BailCount);
        }

        // ── Longest Airtime ───────────────────────────────────────────────
        public static float LongestAirtime { get; private set; } = 0f;
        private static float _currentAirtimeStart = -1f;
        private static bool _wasOnGround = true;

        // Cached for ground check — same property NoSpeedCap uses
        private static PropertyInfo _onGroundProp = null;
        private static bool _groundPropCached = false;

        // Cached refs
        private static GameObject _cachedPlayer = null;
        private static Vehicle _cachedVehicle = null;

        public static string AirtimeDisplay
        {
            get { return LongestAirtime > 0.1f ? LongestAirtime.ToString("F1") + "s" : "--"; }
        }

        // ── Tick (call from OnUpdate) ─────────────────────────────────────
        public static void Tick()
        {
            try
            {
                // Start session timer on first tick
                if (_sessionStartTime < 0f)
                    _sessionStartTime = Time.unscaledTime;

                // Re-find player if cache is stale
                if ((object)_cachedPlayer == null || !_cachedPlayer.activeInHierarchy)
                {
                    _cachedPlayer = GameObject.Find("Player_Human");
                    _cachedVehicle = null;
                }
                if ((object)_cachedPlayer == null) return;

                if ((object)_cachedVehicle == null)
                    _cachedVehicle = _cachedPlayer.GetComponent<Vehicle>();
                if ((object)_cachedVehicle == null) return;

                // ── Airtime tracking ─────────────────────────────────────
                bool onGround = true;

                if (!_groundPropCached)
                {
                    _groundPropCached = true;
                    // Find the onGround bool property — starts with 'T' (TDEX{ib)
                    PropertyInfo[] props = _cachedVehicle.GetType().GetProperties(
                        BindingFlags.Public | BindingFlags.Instance);
                    for (int i = 0; i < props.Length; i++)
                    {
                        if (!props[i].CanRead) continue;
                        if (!string.Equals(props[i].PropertyType.Name, "Boolean",
                            System.StringComparison.Ordinal)) continue;
                        if (props[i].Name.StartsWith("T"))
                        {
                            _onGroundProp = props[i];
                            MelonLogger.Msg("[SessionTrackers] Found onGround prop: " + props[i].Name);
                            break;
                        }
                    }
                }

                if ((object)_onGroundProp != null)
                {
                    object val = _onGroundProp.GetValue(_cachedVehicle, null);
                    if (val is bool) onGround = (bool)val;
                }

                if (!onGround)
                {
                    // In the air
                    if (_wasOnGround)
                    {
                        // Just left ground
                        _currentAirtimeStart = Time.unscaledTime;
                    }
                    else if (_currentAirtimeStart > 0f)
                    {
                        float airtime = Time.unscaledTime - _currentAirtimeStart;
                        if (airtime > LongestAirtime)
                            LongestAirtime = airtime;
                    }
                }
                else
                {
                    // On ground — finalize any airtime
                    if (!_wasOnGround && _currentAirtimeStart > 0f)
                    {
                        float airtime = Time.unscaledTime - _currentAirtimeStart;
                        if (airtime > LongestAirtime)
                            LongestAirtime = airtime;
                    }
                    _currentAirtimeStart = -1f;
                }

                _wasOnGround = onGround;
            }
            catch { }
        }

        // ── Reset ─────────────────────────────────────────────────────────
        public static void Reset()
        {
            _sessionStartTime = -1f;
            BailCount = 0;
            _lastBailTime = -999f;
            LongestAirtime = 0f;
            _currentAirtimeStart = -1f;
            _wasOnGround = true;
            _cachedPlayer = null;
            _cachedVehicle = null;
            _groundPropCached = false;
            _onGroundProp = null;
        }

        public static void ResetBails() { BailCount = 0; _lastBailTime = -999f; }
        public static void ResetAirtime() { LongestAirtime = 0f; _currentAirtimeStart = -1f; }

        // ── Harmony Patch for bail detection ──────────────────────────────
        // Patches Cyclist.Bail() — the actual crash/bail method
        // NOT Vehicle.Reset(bool) which is manual reset
        public static void ApplyBailPatch(HarmonyLib.Harmony harmony)
        {
            try
            {
                // Cyclist.Bail() is public and unobfuscated
                MethodInfo bailMethod = typeof(Cyclist).GetMethod("Bail",
                    BindingFlags.Public | BindingFlags.Instance);

                if ((object)bailMethod == null)
                {
                    MelonLogger.Warning("[SessionTrackers] Cyclist.Bail() not found.");
                    return;
                }

                MethodInfo postfix = typeof(BailDetector_Patch).GetMethod("Postfix",
                    BindingFlags.Public | BindingFlags.Static);

                harmony.Patch(bailMethod, postfix: new HarmonyMethod(postfix));
                MelonLogger.Msg("[SessionTrackers] Patched Cyclist.Bail() for bail detection.");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("[SessionTrackers] ApplyBailPatch: " + ex.Message);
            }
        }
    }

    // Harmony postfix on Cyclist.Bail()
    // Fires when the player actually crashes/wipes out
    public static class BailDetector_Patch
    {
        public static void Postfix(Cyclist __instance)
        {
            if ((object)__instance == null) return;

            // Only count local player bails
            if (!string.Equals(__instance.gameObject.name, "Player_Human",
                System.StringComparison.Ordinal)) return;

            SessionTrackers.OnBailDetected();
        }
    }
}