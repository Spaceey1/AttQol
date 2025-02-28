using Alta.QuickAccessActions;
using UnityEngine;
using HarmonyLib;
namespace Attqol
{
    public class ToggleMenu : QuickAccessMenuAction
    {
        public override bool IsValid => true;
        public override bool IsActive => true;
        public override void Run(Controller controller)
        {
            Attqol.instance.configIsJumpActive.Value = !Attqol.instance.configIsJumpActive.Value;
        }
    }
    [HarmonyPatch(typeof(QuickAccessMenuController))]
    public static class QuickAccessMenuControllerPatch{
        [HarmonyPrefix]
        [HarmonyPatch("InitializeControllerExtension")]
        public static void AddCustomButton(ref QuickAccessMenuAction[] ___quickAccessMenus){
            ___quickAccessMenus = ___quickAccessMenus.AddToArray(ScriptableObject.CreateInstance<ToggleMenu>());
        }
    }
}