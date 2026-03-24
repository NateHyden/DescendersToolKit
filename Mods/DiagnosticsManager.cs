using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public class ModStatus
    {
        public string Name;
        public bool OK;
        public string Error;

        public ModStatus(string name, bool ok, string error = "")
        {
            Name = name;
            OK = ok;
            Error = error;
        }
    }

    public static class DiagnosticsManager
    {
        private static readonly List<ModStatus> _statuses = new List<ModStatus>();

        // Built for Unity 2017.4.40f1
        private const string BuiltForUnity = "2017.4.40f1";

        public static List<ModStatus> Statuses { get { return _statuses; } }

        public static void Report(string name, bool ok, string error = "")
        {
            _statuses.Add(new ModStatus(name, ok, error));
            if (ok)
                MelonLogger.Msg("[Diagnostics] " + name + ": OK");
            else
                MelonLogger.Warning("[Diagnostics] " + name + ": FAILED - " + error);
        }

        public static string UnityVersion
        {
            get
            {
                try { return Application.unityVersion; }
                catch { return "Unknown"; }
            }
        }

        public static string MelonLoaderVersion
        {
            get
            {
                try { return MelonLoader.BuildInfo.Version; }
                catch { return "Unknown"; }
            }
        }

        public static string BuiltForVersion { get { return BuiltForUnity; } }

        public static bool UnityVersionMatch
        {
            get { return string.Equals(UnityVersion, BuiltForUnity, System.StringComparison.Ordinal); }
        }

        public static int OKCount
        {
            get
            {
                int n = 0;
                for (int i = 0; i < _statuses.Count; i++)
                    if (_statuses[i].OK) n++;
                return n;
            }
        }

        public static int FailCount
        {
            get { return _statuses.Count - OKCount; }
        }
    }
}
