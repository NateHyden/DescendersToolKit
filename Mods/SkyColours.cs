using MelonLoader;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace DescendersModMenu.Mods
{
    public static class SkyColours
    {
        public static readonly string[] PresetNames = {
            "Normal", "Blood Red", "Alien Green", "Synthwave", "Midnight", "Toxic"
        };

        private static readonly Color[] SunSkyColors = {
            new Color(0.99f, 0.87f, 0.76f, 1f),
            new Color(0.90f, 0.05f, 0.05f, 1f),
            new Color(0.05f, 0.80f, 0.10f, 1f),
            new Color(0.70f, 0.05f, 0.95f, 1f),
            new Color(0.02f, 0.02f, 0.12f, 1f),
            new Color(0.30f, 0.90f, 0.05f, 1f),
        };

        private static readonly Color[] MoonSkyColors = {
            new Color(0.10f, 0.16f, 0.25f, 1f),
            new Color(0.40f, 0.01f, 0.01f, 1f),
            new Color(0.01f, 0.30f, 0.05f, 1f),
            new Color(0.25f, 0.01f, 0.40f, 1f),
            new Color(0.01f, 0.01f, 0.08f, 1f),
            new Color(0.10f, 0.35f, 0.01f, 1f),
        };

        private static readonly Color[] FogColors = {
            new Color(0.87f, 0.59f, 0.60f, 1f),
            new Color(0.60f, 0.02f, 0.02f, 1f),
            new Color(0.02f, 0.55f, 0.05f, 1f),
            new Color(0.45f, 0.02f, 0.65f, 1f),
            new Color(0.01f, 0.01f, 0.10f, 1f),
            new Color(0.20f, 0.65f, 0.02f, 1f),
        };

        private static readonly Color[] AmbientColors = {
            new Color(0.99f, 0.90f, 0.81f, 1f),
            new Color(0.90f, 0.55f, 0.55f, 1f),
            new Color(0.55f, 0.90f, 0.55f, 1f),
            new Color(0.75f, 0.55f, 0.90f, 1f),
            new Color(0.35f, 0.35f, 0.55f, 1f),
            new Color(0.60f, 0.90f, 0.45f, 1f),
        };

        private static readonly float[] TODHours = { 12f, 19.5f, 22f, 21f, 0f, 14f };

        public static int CurrentPreset { get; private set; } = 0;
        public static bool StormEnabled { get; private set; } = false;

        // Rain intensity — scales particle emission rate on active storm EffectInstances
        // Level 1 = 0.5x (light), Level 5 = 1x (default), Level 10 = 3x (heavy)
        public static int RainIntensityLevel { get; private set; } = 5;
        private static readonly float[] RainMultipliers = { 0.5f, 0.65f, 0.8f, 0.9f, 1f, 1.3f, 1.7f, 2.0f, 2.5f, 3.0f };

        public static void RainIntensityIncrease()
        {
            if (RainIntensityLevel < 10) RainIntensityLevel++;
            if (StormEnabled) ApplyRainIntensity();
        }

        public static void RainIntensityDecrease()
        {
            if (RainIntensityLevel > 1) RainIntensityLevel--;
            if (StormEnabled) ApplyRainIntensity();
        }

        private static ParticleSystem[] _cachedRainPS = null;
        private static float[] _defaultEmissionRates = null;

        private static void ApplyRainIntensity()
        {
            try
            {
                float mult = RainMultipliers[RainIntensityLevel - 1];

                // Collect particle systems from EffectInstances only
                EffectInstance[] instances = Object.FindObjectsOfType<EffectInstance>();
                if ((object)instances == null || instances.Length == 0)
                { MelonLogger.Warning("[SkyColours] No EffectInstances found."); return; }

                var psList = new System.Collections.Generic.List<ParticleSystem>();
                System.Reflection.FieldInfo psField = typeof(EffectInstance).GetField(
                    "\u007FJm\u007DD\u0060\u007B",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if ((object)psField == null) { MelonLogger.Warning("[SkyColours] PS field not found."); return; }

                for (int i = 0; i < instances.Length; i++)
                {
                    if ((object)instances[i] == null) continue;
                    ParticleSystem[] pss = psField.GetValue(instances[i]) as ParticleSystem[];
                    if ((object)pss == null) continue;
                    for (int j = 0; j < pss.Length; j++)
                        if ((object)pss[j] != null) psList.Add(pss[j]);
                }

                if (psList.Count == 0) { MelonLogger.Warning("[SkyColours] No PS in EffectInstances."); return; }

                // Cache default rates on first call (only reset by storm toggle, not slider)
                if ((object)_cachedRainPS == null)
                {
                    var validPS = new System.Collections.Generic.List<ParticleSystem>();
                    var validRates = new System.Collections.Generic.List<float>();

                    for (int i = 0; i < psList.Count; i++)
                    {
                        var em = psList[i].emission;
                        float rot = em.rateOverTime.mode == ParticleSystemCurveMode.Constant
                            ? em.rateOverTime.constant : em.rateOverTime.curveMultiplier;
                        float rod = em.rateOverDistance.mode == ParticleSystemCurveMode.Constant
                            ? em.rateOverDistance.constant : em.rateOverDistance.curveMultiplier;

                        if (rot > 0f || rod > 0f)
                        {
                            float baseRate = rot > 0f ? rot : rod * 10f; // rOD*10 = approx equiv rOT
                            validPS.Add(psList[i]);
                            validRates.Add(baseRate);

                            // Convert rateOverDistance emitters to rateOverTime
                            // so emission happens regardless of transform movement
                            if (rod > 0f && rot <= 0f)
                            {
                                em.rateOverDistance = 0f;
                                em.rateOverTime = baseRate;
                            }
                        }
                    }

                    _cachedRainPS = validPS.ToArray();
                    _defaultEmissionRates = validRates.ToArray();

                    MelonLogger.Msg("[SkyColours] Storm PS cached: " + _cachedRainPS.Length
                        + " active emitters converted to rateOverTime. Base rate="
                        + (_defaultEmissionRates.Length > 0 ? _defaultEmissionRates[0].ToString("F1") : "none"));
                }

                if (_cachedRainPS.Length == 0)
                { MelonLogger.Warning("[SkyColours] No active emitters found."); return; }

                // Apply multiplier via rateOverTime
                int applied = 0;
                for (int i = 0; i < _cachedRainPS.Length; i++)
                {
                    if ((object)_cachedRainPS[i] == null) continue;
                    try
                    {
                        var em = _cachedRainPS[i].emission;
                        em.rateOverTime = _defaultEmissionRates[i] * mult;
                        applied++;
                    }
                    catch { }
                }
                MelonLogger.Msg("[SkyColours] Rain intensity x" + mult.ToString("F2") + " applied to " + applied + " systems.");
            }
            catch (System.Exception ex) { MelonLogger.Error("[SkyColours] ApplyRainIntensity: " + ex.Message); }
        }

        private static int _idSunSky = -1;
        private static int _idMoonSky = -1;
        private static int _idFog = -1;
        private static int _idAmbient = -1;
        private static bool _idsLoaded = false;

        private static MonoBehaviour _skyComp = null;
        private static FieldInfo _cycleField = null;
        private static FieldInfo _hourField = null;

        public static void ApplyPatch(HarmonyLib.Harmony harmony)
        {
            try
            {
                System.Type todSkyType = null;
                System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                for (int i = 0; i < assemblies.Length; i++)
                {
                    todSkyType = assemblies[i].GetType("TOD_Sky");
                    if ((object)todSkyType != null) break;
                }
                if ((object)todSkyType == null)
                {
                    MelonLogger.Warning("[SkyColours] TOD_Sky type not found for patching.");
                    DiagnosticsManager.Report("SkyColours", false, "TOD_Sky type not found");
                    return;
                }

                MethodInfo lateUpdate = todSkyType.GetMethod("LateUpdate",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if ((object)lateUpdate == null)
                {
                    MelonLogger.Warning("[SkyColours] TOD_Sky.LateUpdate not found.");
                    DiagnosticsManager.Report("SkyColours", false, "LateUpdate not found");
                    return;
                }

                MethodInfo postfix = typeof(SkyColours_Patch).GetMethod("Postfix",
                    BindingFlags.Public | BindingFlags.Static);
                harmony.Patch(lateUpdate, postfix: new HarmonyMethod(postfix));
                MelonLogger.Msg("[SkyColours] Patched TOD_Sky.LateUpdate.");

                // Patch TLJ\u0081Hrt — fires after game spawns fresh LoopingParticle instances
                // We apply intensity immediately to the brand-new PS before anything else runs
                MethodInfo tljMethod = typeof(EffectList).GetMethod("TLJ\u0081Hrt",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if ((object)tljMethod != null)
                {
                    MethodInfo tljPostfix = typeof(SkyColours_TLJPatch).GetMethod("Postfix",
                        BindingFlags.Public | BindingFlags.Static);
                    harmony.Patch(tljMethod, postfix: new HarmonyMethod(tljPostfix));
                    MelonLogger.Msg("[SkyColours] Patched EffectList.TLJ (spawn hook).");
                }
                else
                    MelonLogger.Warning("[SkyColours] TLJ method not found — intensity patch skipped.");

                DiagnosticsManager.Report("SkyColours", true);
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("[SkyColours] ApplyPatch: " + ex.Message);
                DiagnosticsManager.Report("SkyColours", false, ex.Message);
            }
        }

        public static void ApplyColours()
        {
            if (CurrentPreset == 0) return;
            try
            {
                if (!_idsLoaded)
                {
                    _idSunSky = Shader.PropertyToID("TOD_SunSkyColor");
                    _idMoonSky = Shader.PropertyToID("TOD_MoonSkyColor");
                    _idFog = Shader.PropertyToID("TOD_FogColor");
                    _idAmbient = Shader.PropertyToID("TOD_AmbientColor");
                    _idsLoaded = true;
                }
                Shader.SetGlobalColor(_idSunSky, SunSkyColors[CurrentPreset]);
                Shader.SetGlobalColor(_idMoonSky, MoonSkyColors[CurrentPreset]);
                Shader.SetGlobalColor(_idFog, FogColors[CurrentPreset]);
                Shader.SetGlobalColor(_idAmbient, AmbientColors[CurrentPreset]);
                RenderSettings.ambientLight = AmbientColors[CurrentPreset];
                RenderSettings.ambientSkyColor = AmbientColors[CurrentPreset];
                RenderSettings.fogColor = FogColors[CurrentPreset];
            }
            catch { }
        }

        public static void ApplyPreset(int index)
        {
            if (index < 0 || index >= PresetNames.Length) return;
            CurrentPreset = index;
            SetTODHour(TODHours[index]);
            MelonLogger.Msg("[SkyColours] Preset -> " + PresetNames[index]);
        }

        public static void ToggleStorm()
        {
            StormEnabled = !StormEnabled;
            ApplyStorm();
            MelonLogger.Msg("[SkyColours] Storm -> " + (StormEnabled ? "ON" : "OFF"));
        }

        private static VisualModifier _originalModifier = VisualModifier.None;
        private static bool _modifierStored = false;

        private static void ApplyStorm()
        {
            try
            {
                CameraEffects ce = Object.FindObjectOfType<CameraEffects>();
                if ((object)ce == null) { MelonLogger.Warning("[SkyColours] CameraEffects not found."); return; }

                SessionManager sm = Object.FindObjectOfType<SessionManager>();
                MethodInfo getWorld = typeof(SessionManager).GetMethod("GetWorld",
                    BindingFlags.Public | BindingFlags.Instance);
                World world = World.Highlands;
                if ((object)getWorld != null && (object)sm != null)
                {
                    world = (World)getWorld.Invoke(sm, null);
                    if (world == World.None) world = World.Highlands;
                }

                VisualModifier targetModifier = StormEnabled ? VisualModifier.Storm : _originalModifier;
                if ((object)sm != null)
                {
                    MethodInfo sessionStarted = typeof(SessionManager).GetMethod("SessionStarted",
                        BindingFlags.Public | BindingFlags.Instance);
                    bool inSession = (object)sessionStarted != null && (bool)sessionStarted.Invoke(sm, null);
                    if (inSession)
                    {
                        FieldInfo sessionDataFld = typeof(SessionManager).GetField(
                            "\u0083ESVMoz", BindingFlags.Public | BindingFlags.Instance);
                        if ((object)sessionDataFld != null)
                        {
                            object sessionData = sessionDataFld.GetValue(sm);
                            if ((object)sessionData != null)
                            {
                                FieldInfo levelInfoFld = sessionData.GetType().GetField(
                                    "vebf\u0081kn", BindingFlags.Public | BindingFlags.Instance);
                                if ((object)levelInfoFld != null)
                                {
                                    object levelInfo = levelInfoFld.GetValue(sessionData);
                                    if ((object)levelInfo != null)
                                    {
                                        FieldInfo visualModFld = levelInfo.GetType().GetField(
                                            "\u007CzvoQ\u0084\u005B", BindingFlags.Public | BindingFlags.Instance);
                                        if ((object)visualModFld != null)
                                        {
                                            if (StormEnabled && !_modifierStored)
                                            {
                                                _originalModifier = (VisualModifier)visualModFld.GetValue(levelInfo);
                                                _modifierStored = true;
                                                targetModifier = VisualModifier.Storm;
                                            }
                                            else if (!StormEnabled)
                                            {
                                                targetModifier = _modifierStored ? _originalModifier : VisualModifier.None;
                                                _modifierStored = false;
                                            }
                                            visualModFld.SetValue(levelInfo, targetModifier);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                MethodInfo destroySky = typeof(CameraEffects).GetMethod("S\u0083wckiX",
                    BindingFlags.Public | BindingFlags.Instance);
                if ((object)destroySky != null)
                    try { destroySky.Invoke(ce, null); } catch { }

                MethodInfo setCE = typeof(CameraEffects).GetMethod("SetCameraEffects",
                    BindingFlags.Public | BindingFlags.Instance);
                World ceWorld = world;
                if (ceWorld == World.Overworld || (int)ceWorld < 1 || (int)ceWorld > 11)
                    ceWorld = World.Highlands;
                if ((object)setCE != null)
                    try { setCE.Invoke(ce, new object[] { ceWorld, targetModifier, null }); }
                    catch (System.Exception ex4) { MelonLogger.Warning("[SkyColours] SetCameraEffects fail: " + (ex4.InnerException != null ? ex4.InnerException.Message : ex4.Message)); }

                EffectList[] allEffectLists = Object.FindObjectsOfType<EffectList>();
                MethodInfo refreshEffects = typeof(EffectList).GetMethod("RefreshEffects",
                    BindingFlags.Public | BindingFlags.Instance);
                MethodInfo setEnvFlag = typeof(EffectList).GetMethod("SetEffectEnvironment",
                    BindingFlags.Public | BindingFlags.Instance);
                MethodInfo spawnEffects = typeof(EffectList).GetMethod("TLJ\u0081Hrt",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                bool stormOn = StormEnabled;
                System.Type efhType = (object)setEnvFlag != null
                    ? setEnvFlag.GetParameters()[0].ParameterType : null;
                for (int i = 0; i < allEffectLists.Length; i++)
                {
                    try
                    {
                        if ((object)refreshEffects != null) refreshEffects.Invoke(allEffectLists[i], null);
                        if ((object)setEnvFlag != null && (object)efhType != null)
                        {
                            setEnvFlag.Invoke(allEffectLists[i], new object[] { System.Enum.ToObject(efhType, 7), stormOn });
                            setEnvFlag.Invoke(allEffectLists[i], new object[] { System.Enum.ToObject(efhType, 4), !stormOn });
                        }
                        if ((object)spawnEffects != null) spawnEffects.Invoke(allEffectLists[i], null);
                    }
                    catch { }
                }

                _cachedEffectLists = allEffectLists;
                _effectListCacheTime = Time.time;

                MelonLogger.Msg("[SkyColours] Storm -> " + (StormEnabled ? "ON" : "OFF")
                    + " | world=" + ceWorld + " mod=" + targetModifier
                    + " | lists=" + allEffectLists.Length
                    + " | spawnMethod=" + ((object)spawnEffects != null));

                if (StormEnabled)
                {
                    _cachedRainPS = null; // force recache since particles just respawned
                    ApplyRainIntensity();
                }
            }
            catch (System.Exception ex)
            {
                System.Exception inner = ex;
                while (inner.InnerException != null) inner = inner.InnerException;
                MelonLogger.Error("[SkyColours] ApplyStorm: " + inner.Message);
            }
        }

        private static void SetTODHour(float hour)
        {
            try
            {
                if ((object)_skyComp == null)
                {
                    MonoBehaviour[] all = Object.FindObjectsOfType<MonoBehaviour>();
                    for (int i = 0; i < all.Length; i++)
                        if (string.Equals(all[i].GetType().Name, "TOD_Sky", System.StringComparison.Ordinal))
                        { _skyComp = all[i]; break; }
                }
                if ((object)_skyComp == null) return;
                if ((object)_cycleField == null)
                    _cycleField = _skyComp.GetType().GetField("Cycle", BindingFlags.Public | BindingFlags.Instance);
                if ((object)_cycleField == null) return;
                object cycle = _cycleField.GetValue(_skyComp);
                if ((object)cycle == null) return;
                if ((object)_hourField == null)
                    _hourField = cycle.GetType().GetField("Hour", BindingFlags.Public | BindingFlags.Instance);
                if ((object)_hourField != null)
                    _hourField.SetValue(cycle, hour);
            }
            catch { }
        }

        public static void Tick() { }

        private static EffectList[] _cachedEffectLists = null;
        private static float _effectListCacheTime = -999f;

        public static void TickStorm(ref System.Type efhType, ref MethodInfo setEnvFlag)
        {
            if (!StormEnabled) return;
            try
            {
                if ((object)setEnvFlag == null)
                    setEnvFlag = typeof(EffectList).GetMethod("SetEffectEnvironment",
                        BindingFlags.Public | BindingFlags.Instance);
                if ((object)setEnvFlag == null) return;

                if ((object)_cachedEffectLists == null || Time.time - _effectListCacheTime > 5f)
                {
                    _cachedEffectLists = Object.FindObjectsOfType<EffectList>();
                    _effectListCacheTime = Time.time;
                }

                if ((object)efhType == null)
                    efhType = setEnvFlag.GetParameters()[0].ParameterType;
                if ((object)efhType == null) return;

                object stormVal = System.Enum.ToObject(efhType, 7);
                object normalVal = System.Enum.ToObject(efhType, 4);

                for (int i = 0; i < _cachedEffectLists.Length; i++)
                {
                    try
                    {
                        setEnvFlag.Invoke(_cachedEffectLists[i], new object[] { stormVal, true });
                        setEnvFlag.Invoke(_cachedEffectLists[i], new object[] { normalVal, false });
                    }
                    catch { }
                }

                // Re-assert emission enabled + intensity every frame
                // The game's UpdateEnvironmentStates resets emission.enabled constantly
                if ((object)_cachedRainPS != null && _cachedRainPS.Length > 0)
                {
                    float mult = RainMultipliers[RainIntensityLevel - 1];
                    for (int i = 0; i < _cachedRainPS.Length; i++)
                    {
                        if ((object)_cachedRainPS[i] == null) continue;
                        try
                        {
                            var em = _cachedRainPS[i].emission;
                            em.enabled = true;
                            em.rateOverTime = _defaultEmissionRates[i] * mult;
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        public static void Reset()
        {
            if (StormEnabled) { StormEnabled = false; try { ApplyStorm(); } catch { } }
            _modifierStored = false;
            _originalModifier = VisualModifier.None;
            CurrentPreset = 0;
            RainIntensityLevel = 5;
            _cachedRainPS = null;
            _defaultEmissionRates = null;
            _skyComp = null;
            _cycleField = null;
            _hourField = null;
            _idsLoaded = false;
            _cachedEffectLists = null;
        }

        // Called by TLJ patch immediately after fresh EffectInstances are spawned
        public static void ApplyIntensityToEffectList(EffectList effectList)
        {
            if (!StormEnabled) return;
            try
            {
                float mult = RainMultipliers[RainIntensityLevel - 1];
                if (System.Math.Abs(mult - 1f) < 0.001f) return; // default — no change needed

                // Read ltpCVSt — the list of active particle EffectInstances just populated
                System.Reflection.FieldInfo ltpField = typeof(EffectList).GetField("ltpCVSt",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if ((object)ltpField == null) return;

                System.Collections.Generic.List<EffectInstance> instances =
                    ltpField.GetValue(effectList) as System.Collections.Generic.List<EffectInstance>;
                if ((object)instances == null || instances.Count == 0) return;

                System.Reflection.FieldInfo psField = typeof(EffectInstance).GetField(
                    "\u007FJm\u007DD\u0060\u007B",
                    BindingFlags.Public | BindingFlags.Instance);
                if ((object)psField == null) return;

                for (int i = 0; i < instances.Count; i++)
                {
                    if ((object)instances[i] == null) continue;
                    ParticleSystem[] pss = psField.GetValue(instances[i]) as ParticleSystem[];
                    if ((object)pss == null) continue;
                    for (int j = 0; j < pss.Length; j++)
                    {
                        if ((object)pss[j] == null) continue;
                        try
                        {
                            var em = pss[j].emission;
                            float rod = em.rateOverDistance.mode == ParticleSystemCurveMode.Constant
                                ? em.rateOverDistance.constant : em.rateOverDistance.curveMultiplier;
                            float rot = em.rateOverTime.mode == ParticleSystemCurveMode.Constant
                                ? em.rateOverTime.constant : em.rateOverTime.curveMultiplier;

                            if (rod > 0f)
                            {
                                // Convert to rateOverTime and apply intensity
                                em.rateOverDistance = 0f;
                                em.rateOverTime = rod * 10f * mult;
                            }
                            else if (rot > 0f)
                            {
                                em.rateOverTime = rot * mult;
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error("[SkyColours] ApplyIntensityToEffectList: " + ex.Message);
            }
        }
    }

    public static class SkyColours_TLJPatch
    {
        public static void Postfix(EffectList __instance)
        {
            SkyColours.ApplyIntensityToEffectList(__instance);
        }
    }

    public static class SkyColours_Patch
    {
        private static System.Type _efhType = null;
        private static MethodInfo _setEnvFlag = null;

        public static void Postfix()
        {
            SkyColours.ApplyColours();
            SkyColours.TickStorm(ref _efhType, ref _setEnvFlag);
        }
    }
}