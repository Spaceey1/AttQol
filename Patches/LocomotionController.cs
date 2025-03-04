using HarmonyLib;
using UnityEngine;
using Valve.VR;
namespace Attqol
{
    public class LocomotionController
    {
        public static SmoothLocomotion activeLocomotion = (PlayerController.Current as SmoothLocomotionPlayerController).SmoothLocomotion;
        public static void Jump()
        {
            if (activeLocomotion.IsGrounded && Attqol.instance.configIsJumpActive.Value)
            {
                activeLocomotion.ApplyMovement(new Vector3(0, 0.25f, 0), true);
                activeLocomotion.velocity.y = 4.5f;
            }
        }
        [HarmonyPatch(typeof(SmoothLocomotion))]
        static class SmoothLocomotionPatches
        {
            [HarmonyPatch("CheckInput")]
            [HarmonyPrefix]
            public static bool Prefix()
            {
                try
                {
                    Vector2 stickDirection = activeLocomotion.ActiveHand.Controller.PlayerInput.RawInput.SmoothLocomotion;
                    float rotateMagnitude = Math.Abs(stickDirection.x);
                    if (Attqol.instance.configIsTankControlsActive.Value && rotateMagnitude > 0.4)
                    {
                        PlayerController.Current.transform.Rotate(new Vector3(0, stickDirection.x * Attqol.instance.configTankTurnSensitivity.Value, 0));
                    }
                    return rotateMagnitude < 0.9;
                }
                catch
                {
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(Teleporter))]
        static class TeleporterPatches
        {
            [HarmonyPatch("StartShowPointer")]
            public static bool Prefix(Teleporter __instance)
            {
                return
                !Attqol.instance.configIsJumpActive.Value ||
                (activeLocomotion.LeftHand.Teleporter == __instance && !Attqol.instance.configJumpLeftHand.Value) ||
                (activeLocomotion.RightHand.Teleporter == __instance && !Attqol.instance.configJumpRightHand.Value);
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
                        if (configInput == (false, false))
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