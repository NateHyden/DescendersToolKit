using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class TeleportToCheckpoint
    {
        public static void Teleport()
        {
            try
            {
                // Get RouteManager singleton
                RouteManager rm = UnityEngine.Object.FindObjectOfType<RouteManager>();
                if ((object)rm == null) { MelonLogger.Warning("[TeleportCP] RouteManager not found."); return; }

                // GetRaceModeRoute() is public and unobfuscated
                Route route = rm.GetRaceModeRoute();
                if ((object)route == null) { MelonLogger.Warning("[TeleportCP] No race route active."); return; }

                // GetLastTriggeredRouteCheckpoint() is on Route
                RouteCheckpoint cp = route.GetLastTriggeredRouteCheckpoint();
                if ((object)cp == null) { MelonLogger.Warning("[TeleportCP] No checkpoint found."); return; }

                GameObject local = GameObject.Find("Player_Human");
                if ((object)local == null) { MelonLogger.Warning("[TeleportCP] Player_Human not found."); return; }

                Vehicle vehicle = local.GetComponent<Vehicle>();
                Vector3 dest = cp.transform.position + cp.transform.up * 1.5f;

                local.transform.position = dest;

                if ((object)vehicle != null)
                {
                    Rigidbody rb = vehicle.GetComponent<Rigidbody>();
                    if ((object)rb == null) rb = vehicle.GetComponentInChildren<Rigidbody>();
                    if ((object)rb != null)
                    {
                        rb.position = dest;
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }

                    // Reset(false) preserves score
                    try
                    {
                        MethodInfo resetMethod = vehicle.GetType().GetMethod("Reset",
                            BindingFlags.Public | BindingFlags.Instance, null,
                            new System.Type[] { typeof(bool) }, null);
                        if ((object)resetMethod != null)
                            resetMethod.Invoke(vehicle, new object[] { false });
                    }
                    catch { }
                }

                MelonLogger.Msg("[TeleportCP] Teleported to checkpoint at " + dest);
            }
            catch (System.Exception ex) { MelonLogger.Error("[TeleportCP] Teleport: " + ex.Message); }
        }
    }
}
