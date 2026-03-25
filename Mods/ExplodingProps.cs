using System;
using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class ExplodingProps
    {
        public static bool Enabled { get; private set; } = false;
        private static PropCollisionHandler _handler = null;

        public static void Toggle()
        {
            Enabled = !Enabled;
            if (Enabled) Attach(); else Detach();
            MelonLogger.Msg("[NoMistakes] -> " + (Enabled ? "ON" : "OFF"));
        }

        public static void SetEnabled(bool enabled)
        {
            Enabled = enabled;
            if (Enabled) Attach(); else Detach();
        }

        private static void Attach()
        {
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null)
                { MelonLogger.Warning("[NoMistakes] Player_Human not found."); return; }

                _handler = player.GetComponent<PropCollisionHandler>();
                if ((object)_handler == null)
                    _handler = player.AddComponent<PropCollisionHandler>();

                _handler.enabled = true;
            }
            catch (Exception ex)
            { MelonLogger.Error("[NoMistakes] Attach: " + ex.Message); }
        }

        private static void Detach()
        {
            if ((object)_handler != null)
                _handler.enabled = false;
        }

        public static void Reset()
        {
            Enabled = false;
            Detach();
            _handler = null;
        }
    }

    public class PropCollisionHandler : MonoBehaviour
    {
        private Vehicle _vehicle;
        private Rigidbody _rb;

        // Vehicle.SetVelocity — same method the OOB boundary uses
        private MethodInfo _setVelocityMethod;

        // FMODUnity.RuntimeManager.PlayOneShot(string, Vector3)
        // The actual crash sound is "event:/sfx/impact/surface/vehicle"
        private MethodInfo _playOneShotMethod;
        private bool _cached = false;

        private float _lastBounceTime = -999f;
        private const float BounceCooldown = 0.5f;
        private const float BounceSpeed = 18f;
        private const float MinImpactSpeed = 5f;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _vehicle = GetComponent<Vehicle>();
        }

        private void CacheReflection()
        {
            if (_cached) return;
            _cached = true;

            try
            {
                // Vehicle.SetVelocity(Vector3) — public, unobfuscated
                _setVelocityMethod = typeof(Vehicle).GetMethod("SetVelocity",
                    BindingFlags.Public | BindingFlags.Instance, null,
                    new Type[] { typeof(Vector3) }, null);

                if ((object)_setVelocityMethod != null)
                    MelonLogger.Msg("[NoMistakes] Found Vehicle.SetVelocity");

                // Find FMODUnity.RuntimeManager.PlayOneShot(string, Vector3)
                // RuntimeManager is in a separate assembly — find by scanning
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (int a = 0; a < assemblies.Length; a++)
                {
                    Type rtType = assemblies[a].GetType("FMODUnity.RuntimeManager");
                    if ((object)rtType == null) continue;

                    // Look for PlayOneShot(string, Vector3) — plays a one-shot at a position
                    MethodInfo[] methods = rtType.GetMethods(BindingFlags.Public | BindingFlags.Static);
                    for (int m = 0; m < methods.Length; m++)
                    {
                        if (methods[m].Name != "PlayOneShot") continue;
                        ParameterInfo[] parms = methods[m].GetParameters();
                        if (parms.Length == 2 &&
                            string.Equals(parms[0].ParameterType.Name, "String", StringComparison.Ordinal) &&
                            string.Equals(parms[1].ParameterType.Name, "Vector3", StringComparison.Ordinal))
                        {
                            _playOneShotMethod = methods[m];
                            MelonLogger.Msg("[NoMistakes] Found RuntimeManager.PlayOneShot(string, Vector3)");
                            break;
                        }
                    }

                    // Fallback: PlayOneShot(string) — no position
                    if ((object)_playOneShotMethod == null)
                    {
                        for (int m = 0; m < methods.Length; m++)
                        {
                            if (methods[m].Name != "PlayOneShot") continue;
                            ParameterInfo[] parms = methods[m].GetParameters();
                            if (parms.Length == 1 &&
                                string.Equals(parms[0].ParameterType.Name, "String", StringComparison.Ordinal))
                            {
                                _playOneShotMethod = methods[m];
                                MelonLogger.Msg("[NoMistakes] Found RuntimeManager.PlayOneShot(string) [fallback]");
                                break;
                            }
                        }
                    }
                    break;
                }

                if ((object)_playOneShotMethod == null)
                    MelonLogger.Warning("[NoMistakes] RuntimeManager.PlayOneShot not found.");
            }
            catch (Exception ex) { MelonLogger.Error("[NoMistakes] CacheReflection: " + ex.Message); }
        }

        private void PlayCrashSound()
        {
            if ((object)_playOneShotMethod == null) return;
            try
            {
                ParameterInfo[] parms = _playOneShotMethod.GetParameters();
                if (parms.Length == 2)
                    _playOneShotMethod.Invoke(null, new object[] { "event:/sfx/impact/surface/vehicle", transform.position });
                else
                    _playOneShotMethod.Invoke(null, new object[] { "event:/sfx/impact/surface/vehicle" });
            }
            catch { }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!ExplodingProps.Enabled) return;
            if ((object)_vehicle == null) return;
            if ((object)_rb == null) return;

            // Cooldown
            if (Time.unscaledTime - _lastBounceTime < BounceCooldown) return;

            if (collision.contacts.Length == 0) return;
            Vector3 normal = collision.contacts[0].normal;

            // ── LANDING CHECK ─────────────────────────────────────────
            // Normal pointing UP = ground/ramp landing, skip
            if (normal.y > 0.5f) return;

            // ── SPEED THRESHOLD ───────────────────────────────────────
            float impactSpeed = collision.relativeVelocity.magnitude;
            if (impactSpeed < MinImpactSpeed) return;

            // ── IGNORE SELF ───────────────────────────────────────────
            GameObject other = collision.gameObject;
            if ((object)other == null) return;
            string otherName = other.name;
            if (otherName == "Player_Human") return;
            if (otherName == "wheel_front" || otherName == "wheel_back") return;

            // Cache on first hit
            if (!_cached) CacheReflection();

            // ── LAUNCH — same approach as OOB boundary ────────────────
            Vector3 bounceDir = Vector3.Reflect(_rb.velocity.normalized, normal);
            bounceDir.y = Mathf.Max(bounceDir.y, 0.4f);
            bounceDir = bounceDir.normalized;

            float speed = _rb.velocity.magnitude;
            float force = Mathf.Max(speed * 0.8f, BounceSpeed);
            Vector3 bounceVelocity = (bounceDir + Vector3.up * 0.3f) * force;

            // Use Vehicle.SetVelocity — moves bike + rider together
            if ((object)_setVelocityMethod != null)
                _setVelocityMethod.Invoke(_vehicle, new object[] { bounceVelocity });
            else
                _rb.velocity = bounceVelocity;

            // ── CRASH SOUND — direct FMOD PlayOneShot ─────────────────
            PlayCrashSound();

            // ── LAUNCH THE THING WE HIT (if it has a rigidbody) ───────
            Rigidbody propRb = other.GetComponent<Rigidbody>();
            if ((object)propRb != null)
            {
                propRb.isKinematic = false;
                Vector3 awayDir = (other.transform.position - transform.position).normalized;
                awayDir.y = 0.5f;
                propRb.AddForce(awayDir * 20f, ForceMode.VelocityChange);
            }

            _lastBounceTime = Time.unscaledTime;
        }
    }
}