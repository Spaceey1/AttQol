// See https://aka.ms/new-console-template for more information
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
namespace Atthacc
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Attqol : BaseUnityPlugin
    {
        public static Attqol instance = null;
        public ManualLogSource logger = null;
        public ConfigEntry<bool> configIsJumpActive;
        public const string pluginGuid = "space.att.attqol";
        public const string pluginName = "Attqol";
        public const string pluginVersion = "1.0.0";
        Attqol()
        {
            instance = this;
        }
        public void Awake()
        {
            configIsJumpActive = Config.Bind("General", "IsJumpActive", false, "Is jump active");
            Harmony harmony = new Harmony(pluginGuid);
            harmony.PatchAll();
            logger = Logger;
            Logger.LogInfo("Attqol loaded. this works");
            Logger.LogInfo(harmony.GetPatchedMethods().ToString());
        }
    }
}
