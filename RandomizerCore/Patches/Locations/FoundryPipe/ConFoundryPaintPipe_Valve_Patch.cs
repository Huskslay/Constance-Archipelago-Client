using Constance;
using HarmonyLib;
using Leo;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Handlers.State;
using UnityEngine;

namespace RandomizerCore.Patches.Locations.FoundryPipe;

[HarmonyPatch(typeof(ConFoundryPaintPipe_Valve))]
public class ConFoundryPaintPipe_Valve_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ConFoundryPaintPipe_Valve.HandleIncomingAttack))]
    private static bool HandleIncomingAttack_Prefix(ConFoundryPaintPipe_Valve __instance, ref ConAttackResult __result, ConAttackRequest request)
    {
        __result = default;
        if (!RandomStateHandler.Randomized) return true;

        LocationComponent locationComponent = __instance.GetComponent<LocationComponent>();
        if (locationComponent == null)
        {
            Plugin.Logger.LogError("No LocationComponent on foundry pipe");
            return false;
        }

        if (RandomStateHandler.HasObtainedLocation(locationComponent.Location))
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
            RandomStateHandler.CheckLocation(locationComponent.Location);
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
        if (!RandomStateHandler.Randomized) return true;
        return false;
    }
}