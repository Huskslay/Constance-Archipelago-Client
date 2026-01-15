using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Locations.Canvas;

[HarmonyPatch(typeof(ConState_Player_AbilityUnlock))]
public class ConState_Player_AbilityUnlock_Patch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ConState_Player_AbilityUnlock.Enter))]
    private static void Enter_Postfix(CConUnlockAbilityCanvas unlockCanvas)
    {
        if (!RandomStateHandler.Randomized) return;

        LocationComponent locationComponent = unlockCanvas.GetComponent<LocationComponent>();
        if (locationComponent == null) Plugin.Logger.LogError("No LocationComponent on canvas");
        else RandomStateHandler.CheckLocation(locationComponent.Location);
    }
}