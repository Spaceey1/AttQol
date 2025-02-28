using HarmonyLib;
using UnityEngine;
using Alta.Character;
using Valve.VR;
namespace Atthacc
{
    public class LocomotionController
    {
        public static SmoothLocomotion playerController = null;
        [HarmonyPatch(typeof(SmoothLocomotion))]
        static class OVRPlayerControllerPatches
        {
            [HarmonyPatch("Awake")]
            public static void Postfix(SmoothLocomotion __instance)
            {
                playerController = __instance;
            }
        }
        [HarmonyPatch(typeof(UnityUpdateManager))]
        static class PlayerlocomotionControllerPatches
        {
            [HarmonyPatch("Update")]
            public static void Postfix()
            {
                if (SteamVR_Input.GetBooleanAction("Teleport").GetStateDown(SteamVR_Input_Sources.Any) && playerController.IsGrounded && Attqol.instance.configIsJumpActive.Value)
                {
                    Attqol.instance.logger.LogInfo("Jump");
                    playerController.ApplyMovement(new Vector3(0,0.25f,0), true);
                    playerController.velocity = new Vector3(playerController.velocity.x, 4.5f, playerController.velocity.z);
                    Attqol.instance.logger.LogInfo(playerController.velocity.ToString());
                }
            }
        }
    }
}