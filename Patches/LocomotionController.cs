using HarmonyLib;
using UnityEngine;
using Valve.VR;
namespace Attqol
{
    public class LocomotionController
    {
        public static SmoothLocomotion playerController = null;
        public static void Jump()
        {
            Attqol.instance.logger.LogInfo("Jump");
            if (playerController.IsGrounded && Attqol.instance.configIsJumpActive.Value)
            {
                Attqol.instance.logger.LogInfo("Jump start");
                playerController.ApplyMovement(new Vector3(0, 0.25f, 0), true);
                playerController.velocity.y = 4.5f;
            }
        }
        [HarmonyPatch(typeof(SmoothLocomotion))]
        static class SmoothLocomotionPatches
        {
            [HarmonyPatch("Awake")]
            public static void Postfix(SmoothLocomotion __instance)
            {
                if (playerController == null)
                {
                    playerController = __instance;
                }
            }
        }
        [HarmonyPatch(typeof(Teleporter))]
        static class TeleporterPatches
        {
            [HarmonyPatch("StartShowPointer")]
            public static bool Prefix(Teleporter __instance)
            {
                return !Attqol.instance.configIsJumpActive.Value || playerController.LeftHand.Teleporter == __instance;
            }
        }
        [HarmonyPatch(typeof(UnityUpdateManager))]
        static class UnityUpdateManagerPatches
        {
            [HarmonyPatch("Update")]
            public static void Postfix()
            {
                if (Attqol.instance.configIsJumpActive.Value && SteamVR_Input.GetBooleanAction("Teleport").GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    Jump();
                }
            }
        }
    }
}