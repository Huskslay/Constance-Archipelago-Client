using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Locations.Inspiration;

[HarmonyPatch(typeof(CConInspirationTriggerBehaviour))]
public class CConInspirationTriggerBehaviour_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConInspirationTriggerBehaviour.EnterSeq))]
    private static void EnterSeq_Prefix(CConInspirationTriggerBehaviour __instance)
    {
        if (!RandomStateHandler.Randomized) return;

        LocationComponent locationComponent = __instance.GetComponent<LocationComponent>();
        if (locationComponent == null)
        {
            Plugin.Logger.LogWarning($"Inspiration '{__instance.name}' is not randomized");
            return;
        }
        RandomStateHandler.CheckLocation(locationComponent.Location);

        __instance.FinishCollection();
    }
}
