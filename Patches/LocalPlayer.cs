using HarmonyLib;
namespace Attqol
{
    class LocalPlayer
    {
        public static Player instance = null;
        [HarmonyPatch(typeof(Player), "SetCurrent")]
        static class PlayerPatches
        {
            [HarmonyPrefix]
            public static void Prefix(Player __instance)
            {
                if (Player.Current == null)
                    instance = __instance;
            }
        }
        
    }

}