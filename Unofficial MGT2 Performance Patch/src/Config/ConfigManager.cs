using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace UnofficialMGT2PerformancePatch.Config
{
    [BepInPlugin(UnofficialMGT2PerformancePatch.PluginGuid, 
        UnofficialMGT2PerformancePatch.PluginName, 
        UnofficialMGT2PerformancePatch.PluginVersion)]
    [BepInProcess("Mad Games Tycoon 2.exe")]
    public class ConfigManager
    {
        private ConfigFile ConfigFile { get; set; }

        /// <summary>
        /// Constructor with LoadConfig
        /// </summary>
        /// <param name="configFile"></param>
        public ConfigManager(ConfigFile configFile)
        {
            ConfigFile = configFile;
            LoadConfig();
        }

        // =============================================================================================================
        // Config sections
        // =============================================================================================================
        private const string ModSettingsSection = "0. MOD Settings";
        private const string MainSettingSection = "1. General Setting";

        // =============================================================================================================
        // Config entries 
        // =============================================================================================================
        public static ConfigEntry<bool> IsModEnabled { get; private set; }

        // Theme Config ---------------------------------------------------------------
        public static ConfigEntry<bool> IsGameDevMenuInitOptimizationEnabled { get; private set; }

        // =============================================================================================================


        // =============================================================================================================
        /// <summary>
        /// Loading when the game starts
        /// </summary>
        private void LoadConfig()
        {
            // =============================================================================================================
            // Config setting definitions here
            // =============================================================================================================

            // Main Settings
            IsModEnabled = ConfigFile.Bind(
                ModSettingsSection,
                "Activate the MOD",
                true,
                "Toggle 'Enabled' to activate the mod");

            // ----------------------------------------------------------------------------------------------------------------
            // GameDev Initializing Performance Config
            IsGameDevMenuInitOptimizationEnabled = ConfigFile.Bind(
                MainSettingSection,
                "GameDev Menu - Performance Improvement",
                true,
                "Enabling this option improves the performance of the Game Dev Menu during initialization.");

            // =============================================================================================================
            //InitDropdownSets();
            // Config setting event handlers here
            ConfigFile.SettingChanged += OnConfigSettingChanged;
            // =============================================================================================================
        }

        /// <summary>
        /// DEBUG: Event handler for config setting changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfigSettingChanged(object sender, SettingChangedEventArgs e)
        {
#if DEBUG
            Debug.Log(UnofficialMGT2PerformancePatch.PluginName + " : Config setting is changed");
#endif
            //InitDropdownSets();
        }
    }
}
