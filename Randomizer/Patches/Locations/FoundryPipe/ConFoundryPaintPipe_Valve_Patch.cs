using Constance;
using HarmonyLib;
using Leo;
using RandomizerCore.Classes.Adapters;
using RandomizerCore.Classes.State;
using RandomizerCore.Classes.Storage.Locations;
using RandomizerCore.Classes.Storage.Locations.Types;
using RandomizerCore.Classes.Storage.Requirements.Entries;
using UnityEngine;
using static Constance.AConFoundryPaintPipe;

namespace Randomizer.Patches.Locations.FoundryPipe;

[HarmonyPatch(typeof(ConFoundryPaintPipe_Valve))]
public class ConFoundryPaintPipe_Valve_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ConFoundryPaintPipe_Valve.HandleIncomingAttack))]
    private static bool HandleIncomingAttack_Prefix(ConFoundryPaintPipe_Valve __instance, ref ConAttackResult __result, ConAttackRequest request)
    {
        __result = default;
        if (!RandomState.Randomized) return true;
        if (!RandomState.IsRandomized(RandomizableItems.FoundryPipe))
        {
            if (Mathf.Clamp01(__instance._fill + __instance.stabFillAmount) >= 0.85f)
                RandomState.AchieveEvents(ValveToValveEntry(__instance));
            return true;
        }

        ALocation location = __instance.GetComponent<LocationComponent>().Location;
        if (!RandomState.TryGetElement(location, out RandomStateElement element))
        {
            Plugin.Logger.LogError($"Could not get element for location {location.GetFullName()}");
            return false;
        }

        if (element.hasObtainedSource)
        {
            __result = ConAttackResult.Ignored;
            return false;
        }
        if (!ConAttackUtils.HasPaint(request, 2))
        {
            __result = ConAttackResult.Ignored;
            return false;
        }
        __instance._fillAnimStartPoint = __instance._fill;
        __instance._fillAnimTimer.Start(__instance.fillAnimCurve, null);
        __instance._fill = Mathf.Clamp01(__instance._fill + __instance.stabFillAmount);
        if (__instance._fill >= 0.85f)
        {
            // Changed
            RandomState.TryGetItem(location);
            // End Changed
        }
        __instance.onPipeHit.InvokeSafe(null);
        __result = ConAttackResult.Hit;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(ConFoundryPaintPipe_Valve.Start))]
    private static bool Start_Prefix()
    {
        if (!RandomState.Randomized) return true;
        if (!RandomState.IsRandomized(RandomizableItems.FoundryPipe)) return true;
        return false;
    }

    private static EventsEntries ValveToValveEntry(ConFoundryPaintPipe_Valve valve)
    {
        return valve.pipeType switch
        {
            PaintPipeType.Rectangle => EventsEntries.SquarePipe,
            PaintPipeType.Triangle => EventsEntries.TrianglePipe,
            _ => EventsEntries.None,
        };
    }
}
