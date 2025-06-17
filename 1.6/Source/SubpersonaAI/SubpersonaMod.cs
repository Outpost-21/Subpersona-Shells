using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

using HarmonyLib;
using System.Reflection;
using System.IO;

namespace SubpersonaAI
{
    public class SubpersonaMod : Mod
    {
        public static SubpersonaMod mod;

        public static string VersionDir => Path.Combine(mod.Content.ModMetaData.RootDir.FullName, "Version.txt");

        public static string CurrentVersion { get; private set; }

        public SubpersonaMod(ModContentPack content) : base(content)
        {
            mod = this;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            Log.Message($":: Subpersona Shells :: {CurrentVersion} ::");

            if (Prefs.DevMode)
            {
                File.WriteAllText(VersionDir, CurrentVersion);
            }

            new Harmony("Neronix17.SubpersonaShells.RimWorld").PatchAll();
        }
    }


}
