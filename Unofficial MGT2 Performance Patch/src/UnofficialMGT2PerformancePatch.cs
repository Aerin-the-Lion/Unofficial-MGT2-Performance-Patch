using BepInEx;
using UnofficialMGT2PerformancePatch.Config;
using HarmonyLib;
using BepInEx.Logging;
using System.Diagnostics;

namespace UnofficialMGT2PerformancePatch
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("Mad Games Tycoon 2.exe")]
    internal class UnofficialMGT2PerformancePatch : BaseUnityPlugin
    {
        public const string PluginGuid = "me.Aerin.MGT2mod.UnofficialMGT2PerformancePatch";
        public const string PluginName = "Unofficial MGT2 Performance Patch";
        public const string PluginVersion = "1.0.0.0";

        void Awake()
        {
            ConfigManager configManager = new ConfigManager(Config);
            LoadHooks();
        }

        void LoadHooks()
        {
            Logger.LogInfo(nameof(LoadHooks));
            Harmony.CreateAndPatchAll(typeof(Hooks), PluginGuid);
            Harmony.CreateAndPatchAll(typeof(Hooks.OptimizeInitGameplayFeatures), PluginGuid);
        }
    }
}
