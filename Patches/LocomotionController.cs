using System.Drawing.Text;
using HarmonyLib;
using Oculus.Interaction.Locomotion;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
namespace Attqol
{
    public class LocomotionController
    {
        //public static SmoothLocomotion LocalPlayer.instance.PlayerCharacter.SmoothLocomotion = null;
        public static PlayerLocomotor playerLocomotor = null;
        public static void Jump()
        {
            Attqol.instance.logger.LogInfo("Jump");
            if (LocalPlayer.instance.PlayerCharacter.SmoothLocomotion.IsGrounded && Attqol.instance.configIsJumpActive.Value)
            {
                Attqol.instance.logger.LogInfo("Jump start");
                LocalPlayer.instance.PlayerCharacter.SmoothLocomotion.ApplyMovement(new Vector3(0, 0.25f, 0), true);
                LocalPlayer.instance.PlayerCharacter.SmoothLocomotion.velocity.y = 4.5f;
            }
        }
        [HarmonyPatch(typeof(SmoothLocomotion))]
        static class SmoothLocomotionPatches
        {
            [HarmonyPatch("Awake")]
            public static void Postfix(SmoothLocomotion __instance)
            {
                //if (LocalPlayer.instance.PlayerCharacter.SmoothLocomotion == null)
                //    LocalPlayer.instance.PlayerCharacter.SmoothLocomotion = __instance;
            }

            [HarmonyPatch("CheckInput")]
            [HarmonyPrefix]
            public static bool PreFix()
            {
                Vector2 stickDirection = LocalPlayer.instance.PlayerCharacter.SmoothLocomotion.ActiveHand.Controller.PlayerInput.RawInput.SmoothLocomotion;
                if (Attqol.instance.configIsTankControlsActive.Value && Math.Abs(stickDirection.x) > 0.5)
                {
                    LocalPlayer.instance.PlayerCharacter.transform.Rotate(new Vector3(0, stickDirection.x * Attqol.instance.configTankTurnSensitivity.Value, 0));
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Teleporter))]
        static class TeleporterPatches
        {
            [HarmonyPatch("StartShowPointer")]
            public static bool Prefix(Teleporter __instance)
            {
                return !Attqol.instance.configIsJumpActive.Value || LocalPlayer.instance.PlayerCharacter.SmoothLocomotion.LeftHand.Teleporter == __instance;
            }
        }
        [HarmonyPatch(typeof(PlayerLocomotor))]
        static class OVRPlayerControllerPatches
        {
            [HarmonyPatch("Start")]
            public static void Postfix(PlayerLocomotor __instance)
            {
                if (playerLocomotor == null)
                {
                    playerLocomotor = __instance;
                }
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