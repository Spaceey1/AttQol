using HarmonyLib;
using Oculus.Interaction.Locomotion;
using UnityEngine;
using Valve.VR;
namespace Attqol
{
    public class LocomotionController
    {
        public static PlayerLocomotor playerLocomotor = null;
        public static void Jump()
        {
            if (LocalPlayer.instance.PlayerCharacter.SmoothLocomotion.IsGrounded && Attqol.instance.configIsJumpActive.Value)
            {
                LocalPlayer.instance.PlayerCharacter.SmoothLocomotion.ApplyMovement(new Vector3(0, 0.25f, 0), true);
                LocalPlayer.instance.PlayerCharacter.SmoothLocomotion.velocity.y = 4.5f;
            }
        }
        [HarmonyPatch(typeof(SmoothLocomotion))]
        static class SmoothLocomotionPatches
        {
            [HarmonyPatch("CheckInput")]
            [HarmonyPrefix]
            public static bool PreFix()
            {
                Vector2 stickDirection = LocalPlayer.instance.PlayerCharacter.SmoothLocomotion.ActiveHand.Controller.PlayerInput.RawInput.SmoothLocomotion;
                float rotateMagnitude = Math.Abs(stickDirection.x);
                if (Attqol.instance.configIsTankControlsActive.Value && rotateMagnitude > 0.4)
                {
                    LocalPlayer.instance.PlayerCharacter.transform.Rotate(new Vector3(0, stickDirection.x * Attqol.instance.configTankTurnSensitivity.Value, 0));
                }
                return rotateMagnitude < 0.9;
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
                SteamVR_Input_Sources jumpInputSource;
                var configInput = (Attqol.instance.configJumpLeftHand.Value, Attqol.instance.configJumpRightHand.Value);
                if(configInput == (false, false))
                    return;
                jumpInputSource = configInput switch
                {
                    (true, true) => SteamVR_Input_Sources.Any,
                    (true, false) => SteamVR_Input_Sources.LeftHand,
                    (false, true) => SteamVR_Input_Sources.RightHand,
                };
                if (Attqol.instance.configIsJumpActive.Value && SteamVR_Input.GetBooleanAction("Teleport").GetStateDown(jumpInputSource))
                    Jump();
            }
        }
    }
}