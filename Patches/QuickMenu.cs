using Alta.QuickAccessActions;
using UnityEngine;
using HarmonyLib;
using Alta.Chunks;
using Alta.Inventory;
namespace Atthacc
{
    public class ToggleMenu : QuickAccessMenuAction
    {
        public override bool IsValid => true;

        public override bool IsActive => Attqol.instance.isJumpActive;
        public override void Run(Controller controller)
        {
            
            Attqol.instance.isJumpActive = !Attqol.instance.isJumpActive;
        }
    }
    public class SpawnMenu : QuickAccessMenuAction
    {
        public override bool IsValid => true;

        public override bool IsActive => Attqol.instance.isJumpActive;
        public override void Run(Controller controller)
        {
            Ingot ingot = CreateInstance<Ingot>();
            SpawnHelper.SpawnPouchOrPickup(ingot.Item, 5, SpawnData.Default, controller.Entity.Chunk, controller.transform.position);
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