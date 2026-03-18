using System;
using System.IO;
using MelonLoader;
using UnityEngine;
using DescendersModMenu.Mods;

namespace DescendersModMenu.BikeStats
{
    public static class StatsManager
    {
        private static readonly string SaveFolder =
            Path.Combine(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData"),
                "DescendersModMenu"
            );

        private static readonly string SaveFile =
            Path.Combine(SaveFolder, "BikeStats.json");

        public static void SaveStats()
        {
            try
            {
                EnsureSaveFolder();

                BikeStatsData data = new BikeStatsData
                {
                    AccelerationLevel = Acceleration.Level,
                    MaxSpeedLevel = MaxSpeedMultiplier.Level,
                    LandingImpactLevel = LandingImpact.Level,
                    NoBailEnabled = NoBail.Enabled,
                    BikeIndex = BikeSwitcher.CurrentBikeIndex
                };

                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SaveFile, json);

                MelonLogger.Msg("Stats saved to: " + SaveFile);
            }
            catch (Exception ex)
            {
                MelonLogger.Error("SaveStats failed: " + ex.Message);
            }
        }

        public static void LoadStats()
        {
            try
            {
                if (!File.Exists(SaveFile))
                {
                    MelonLogger.Warning("No saved stats file found: " + SaveFile);
                    return;
                }

                string json = File.ReadAllText(SaveFile);
                BikeStatsData data = JsonUtility.FromJson<BikeStatsData>(json);

                if (data == null)
                {
                    MelonLogger.Warning("LoadStats failed: JSON returned null.");
                    return;
                }

                Acceleration.SetLevel(data.AccelerationLevel);
                MaxSpeedMultiplier.SetLevel(data.MaxSpeedLevel);
                LandingImpact.SetLevel(data.LandingImpactLevel);
                NoBail.SetEnabled(data.NoBailEnabled);
                BikeSwitcher.SetBike(data.BikeIndex);

                MelonLogger.Msg("Stats loaded from: " + SaveFile);
            }
            catch (Exception ex)
            {
                MelonLogger.Error("LoadStats failed: " + ex.Message);
            }
        }

        public static void ResetStats()
        {
            try
            {
                Acceleration.SetLevel(1);
                MaxSpeedMultiplier.SetLevel(1);
                LandingImpact.SetLevel(1);
                NoBail.SetEnabled(false);
                BikeSwitcher.SetBike(0);

                MelonLogger.Msg("Stats reset to default values.");
            }
            catch (Exception ex)
            {
                MelonLogger.Error("ResetStats failed: " + ex.Message);
            }
        }

        private static void EnsureSaveFolder()
        {
            if (!Directory.Exists(SaveFolder))
            {
                Directory.CreateDirectory(SaveFolder);
            }
        }
    }
}