using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Handlers;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Fixes.AllowDashlessWallDive;

[HarmonyPatch(typeof(CConPlayerInventoryManager))]
internal class CConPlayerInventoryManager_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConPlayerInventoryManager.Has))]
    private static bool Has_Prefix(ref bool __result, SConCollectable collectable)
    {
        if (!RandomStateHandler.Randomized) return true;

        SConCollectable dash = CollectableHandler.NameToCollectable("dash");
        if (collectable != null && collectable == dash && AConState_Player_LeoVoid_Patch.FakeDash)
        {
            __result = true;
            return false;
        }
        return true;
    }
}