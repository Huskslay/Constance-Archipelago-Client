using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Locations.FoundryPipe;

[HarmonyPatch(typeof(AConFoundryPaintPipe))]
public class AConFoundryPaintPipe_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(AConFoundryPaintPipe.UpdatePropertyBlock))]
    private static void UpdatePropertyBlock_Prefix(AConFoundryPaintPipe __instance, ref float fill)
    {
        if (!RandomStateHandler.Randomized) return;
        if (__instance is not ConFoundryPaintPipe_Valve valve) return;

        LocationComponent locationComponent = __instance.GetComponent<LocationComponent>();
        if (locationComponent == null) return;
        ALocation location = locationComponent.Location;

        fill = RandomStateHandler.HasObtainedLocation(location) ? 1f : (fill < 0.85f ? fill : 0f);
        if (fill > 0.85f) valve.corruptedVfx.Stop();
    }
}