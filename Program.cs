﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
namespace Attqol
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Attqol : BaseUnityPlugin
    {
        public static Attqol instance = null;
        public ManualLogSource logger = null;
        public ConfigEntry<bool> configIsJumpActive;
        public ConfigEntry<bool> configIsTankControlsActive;
        public ConfigEntry<float> configTankTurnSensitivity;
        public const string pluginGuid = "space.att.attqol";
        public const string pluginName = "Attqol";
        public const string pluginVersion = "1.0.0";
        Attqol()
        {
            instance = this;
        }
        public void Awake()
        {
            configIsJumpActive = Config.Bind("Jump", "IsJumpActive", false, "Is jump active");
            configIsTankControlsActive = Config.Bind("Tank controls", "IsTankControlsActive", false, "Is tank controls active");
            configTankTurnSensitivity = Config.Bind("Tank controls", "TankTurnSensitivity", 1f, "Tank turn sensitivity");
            Harmony harmony = new Harmony(pluginGuid);
            harmony.PatchAll();
            logger = Logger;
            Logger.LogInfo("Attqol loaded. this works");
            Logger.LogInfo(harmony.GetPatchedMethods().ToString());
        }
    }
}
