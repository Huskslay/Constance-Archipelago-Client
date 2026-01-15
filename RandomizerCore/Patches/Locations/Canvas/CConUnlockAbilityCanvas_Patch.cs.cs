using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Locations.Canvas;

[HarmonyPatch(typeof(CConUnlockAbilityCanvas))]
public class CConUnlockAbilityCanvas_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConUnlockAbilityCanvas.Start))]
    private static bool Start_Prefix()
    {
        if (!RandomStateHandler.Randomized) return true;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch("Constance.IConAttackable.HandleIncomingAttack")]
    private static bool HandleIncomingAttack_Prefix(CConUnlockAbilityCanvas __instance, ref ConAttackResult __result)
    {
        if (!RandomStateHandler.Randomized) return true;

        LocationComponent locationComponent = __instance.GetComponent<LocationComponent>();
        if (locationComponent == null)
        {
            Plugin.Logger.LogError("No LocationComponent on canvas");
            return false;
        }

        if (RandomStateHandler.HasObtainedLocation(locationComponent.Location))
        {
            __result = ConAttackResult.Ignored;
            return false;
        }
        return true;
    }
}