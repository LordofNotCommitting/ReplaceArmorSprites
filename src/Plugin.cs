using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MGSC;
using UnityEngine;

namespace ReplaceArmorSprites
{
    public static class Plugin
    {

        public static string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string tsv_filename = "ArmorSpriteConversionList.tsv";
        public static ConfigDirectories ConfigDirectories = new ConfigDirectories();

        public static ModConfig Config { get; private set; }

        public static Logger Logger = new Logger();
        public static string ConfigPath = ConfigDirectories.ConfigPath;

        [Hook(ModHookType.AfterConfigsLoaded)]
        public static void AfterConfig(IModContext context)
        {
            Directory.CreateDirectory(ConfigDirectories.ModPersistenceFolder);
            Config = ModConfig.LoadConfig(ConfigDirectories.ConfigPath);
            FileCopyHelper.CopyDefaultTsvIfMissing();

            new Harmony("LoC_" + ConfigDirectories.ModAssemblyName).PatchAll();
        }

        

    }
}
