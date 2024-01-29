using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace ScrapMerging
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ScrapMerging : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "GiGaGon";
        public const string PluginName = "ScrapMerging";
        public const string PluginVersion = "1.0.0";
        private void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            SharedComponents.ConfigFile = new ConfigFile(Path.Combine(Paths.ConfigPath, $"{PluginName}.cfg"), true);
            SharedComponents.MergeDistance = SharedComponents.ConfigFile.Bind("General", "Merge Distance", 1F, "Maximum distance at which scrap will merge.");
        }
    }

    public static class SharedComponents
    {
        public static ConfigFile ConfigFile { get; set; } = null!;
        public static ConfigEntry<float> MergeDistance { get; set; } = null!;
    }
}
