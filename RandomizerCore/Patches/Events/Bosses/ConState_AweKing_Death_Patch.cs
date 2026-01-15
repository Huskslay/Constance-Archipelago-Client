using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Enums;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Events.Bosses;

[HarmonyPatch(typeof(ConState_AweKing_Death))]
public class ConState_AweKing_Death_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ConState_AweKing_Death.OnEnter))]
    private static void OnEnter_Prefix()
    {
        if (!RandomStateHandler.Randomized) return;

        Plugin.Logger.LogMessage("Awe King defeated");
        RandomStateHandler.AchieveEvents(GameEvents.AweKingDeath);
    }
}