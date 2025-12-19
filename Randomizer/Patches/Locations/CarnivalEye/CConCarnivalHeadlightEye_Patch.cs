using Constance;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using Leo;
using RandomizerCore.Classes.Adapters;
using RandomizerCore.Classes.State;
using RandomizerCore.Classes.Storage.Locations;
using RandomizerCore.Classes.Storage.Requirements.Entries;
using System;
using System.Threading;
using System.Xml.Linq;

namespace Randomizer.Patches.Locations.CarnivalEye;

[HarmonyPatch(typeof(CConCarnivalHeadlightEye))]
public class CConCarnivalHeadlightEye_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConCarnivalHeadlightEye.Interact))]
    private static bool Interact_Prefix(CConCarnivalHeadlightEye __instance)
    {
        if (!RandomState.Randomized) return true;

        if (!RandomState.IsRandomized(RandomizableItems.CarnivalEyes))
        {
            ConLevelId id = ConMonoBehaviour.SceneRegistry.PlayerOne.Level.Current;
            RandomState.AchieveEvents(LevelToEyeEntry(id.StringValue));
            return true;
        }


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
            RandomState.TryGetItem(location);
        }

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConCarnivalHeadlightEye.Start))]
    private static bool Start_Prefix()
    {
        if (!RandomState.Randomized) return true;
        if (!RandomState.IsRandomized(RandomizableItems.CarnivalEyes)) return true;
        return false;
    }


    private static EventsEntries LevelToEyeEntry(string level)
    {
        return level switch
        {
            "Prod_C03" => EventsEntries.C03Eye,
            "Prod_C04" => EventsEntries.C04Eye,
            "Prod_C05" => EventsEntries.C05Eye,
            _ => EventsEntries.None,
        };
    }
}
