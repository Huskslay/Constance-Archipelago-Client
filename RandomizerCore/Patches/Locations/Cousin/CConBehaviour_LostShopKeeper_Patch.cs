using Constance;
using HarmonyLib;
using Leo;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Handlers.State;
using System.Collections;

namespace RandomizerCore.Patches.Locations.Cousin;

[HarmonyPatch(typeof(CConBehaviour_LostShopKeeper))]
public class CConBehaviour_LostShopKeeper_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConBehaviour_LostShopKeeper.Start))]
    private static bool Start_Prefix(CConBehaviour_LostShopKeeper __instance)
    {
        if (!RandomStateHandler.Randomized) return true;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConBehaviour_LostShopKeeper.OnShopKeeperReturn))]
    private static bool OnShopKeeperReturn_Prefix(CConBehaviour_LostShopKeeper __instance, ref IEnumerator __result)
    {
        if (!RandomStateHandler.Randomized) return true;

        ALocation location = __instance.GetComponent<LocationComponent>().Location;
        RandomStateHandler.CheckLocation(location);

        //__instance.gameObject.SetActive(false);
        __instance.returnFeedback.TryPlay(false);
        __result = CoroutineUtils.WaitUntil(() => !__instance.returnFeedback.IsPlaying);

        return false;
    }
}