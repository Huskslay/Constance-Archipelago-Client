using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Locations.Pickups;

[HarmonyPatch(typeof(AConEntityDropBehaviour))]
public class AConEntityDropBehaviour_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(AConEntityDropBehaviour.Collect))]
    private static void Collect_Prefix(AConEntityDropBehaviour __instance)
    {
        if (!RandomStateHandler.Randomized) return;
        if (__instance is not CConEntityDropBehaviour_TouchToCollect) return;
        if (__instance.State == AConEntityDropBehaviour.DropState.Collected) return;

        LocationComponent locationComponent = __instance.GetComponent<LocationComponent>();
        if (locationComponent == null) Plugin.Logger.LogError("No LocationComponent on chest");
        else RandomStateHandler.CheckLocation(locationComponent.Location);
    }
}