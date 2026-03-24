using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class TopSpeed
    {
        public static float SessionTopSpeed { get; private set; } = 0f;
        private static bool _tracking = false;

        public static string DisplayValue
        {
            get { return SessionTopSpeed > 0f ? SessionTopSpeed.ToString("F1") + " km/h" : "--"; }
        }

        public static void StartTracking() { _tracking = true; }
        public static void StopTracking()  { _tracking = false; }
        public static void Reset()         { SessionTopSpeed = 0f; }

        // Call from OnUpdate every frame
        public static void Tick()
        {
            if (!_tracking) return;
            try
            {
                GameObject player = GameObject.Find("Player_Human");
                if ((object)player == null) return;
                Vehicle vehicle = player.GetComponent<Vehicle>();
                if ((object)vehicle == null) return;
                // GetVelocity(true) returns magnitude * 2 which matches km/h display in game
                float speed = vehicle.GetVelocity(true);
                if (speed > SessionTopSpeed)
                    SessionTopSpeed = speed;
            }
            catch { }
        }
    }
}
