using System;

namespace DescendersModMenu.BikeStats
{
    [Serializable]
    public class BikeStatsData
    {
        public int AccelerationLevel = 1;
        public int MaxSpeedLevel = 1;
        public int LandingImpactLevel = 1;
        public bool NoBailEnabled = false;
        public int BikeIndex = 0;
    }
}