using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Locations.Chest;

[HarmonyPatch(typeof(CConChestEntity))]
public class CConChestEntity_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConChestEntity.SpawnLoot))]
    private static bool CConChestEntity_Prefix(CConChestEntity __instance)
    {
        if (!RandomStateHandler.Randomized) return true;

        LocationComponent locationComponent = __instance.GetComponent<LocationComponent>();
        if (locationComponent == null)
        {
            Plugin.Logger.LogError("No LocationComponent on chest");
            return false;
        }
        RandomStateHandler.CheckLocation(locationComponent.Location);

        return false;
    }
}