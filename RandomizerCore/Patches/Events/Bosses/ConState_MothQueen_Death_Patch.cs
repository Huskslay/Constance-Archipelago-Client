using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Enums;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Events.Bosses;

[HarmonyPatch(typeof(ConState_MothQueen_Death))]
public class ConState_MothQueen_Death_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ConState_MothQueen_Death.OnEnter))]
    private static void OnEnter_Prefix()
    {
        if (!RandomStateHandler.Randomized) return;

        Plugin.Logger.LogMessage("High Patia (Moth Queen) defeated");
        RandomStateHandler.AchieveEvents(GameEvents.HighPatiaDeath);
    }
}