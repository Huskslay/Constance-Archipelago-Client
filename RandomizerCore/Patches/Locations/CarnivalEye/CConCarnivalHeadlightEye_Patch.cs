using Constance;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Handlers.State;
using System.Threading;

namespace RandomizerCore.Patches.Locations.CarnivalEye;

[HarmonyPatch(typeof(CConCarnivalHeadlightEye))]
public class CConCarnivalHeadlightEye_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConCarnivalHeadlightEye.Interact))]
    private static bool Interact_Prefix(CConCarnivalHeadlightEye __instance)
    {
        if (!RandomStateHandler.Randomized) return true;

        if (!__instance.Activated)
        {
            SootheTask().Forget();
        }

        async UniTask SootheTask()
        {
            __instance.interactable.interactable = false;
            CancellationToken cts = __instance.destroyCancellationToken;
            __instance.Activated = true;
            await __instance.interactNavigator.Value.StartNavigatorAsync(cts);
            __instance.openEyeTimeline.Play();
            __instance.openEyeTimeline.Evaluate();
            await __instance.openEyeTimeline.WaitUntilFinishedAsync(cts);

            ALocation location = __instance.GetComponent<LocationComponent>().Location;
            RandomStateHandler.CheckLocation(location);
        }

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConCarnivalHeadlightEye.Start))]
    private static bool Start_Prefix()
    {
        if (!RandomStateHandler.Randomized) return true;
        return false;
    }
}