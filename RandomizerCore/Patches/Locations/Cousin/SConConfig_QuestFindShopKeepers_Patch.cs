using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Archipelago;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Locations.Cousin;

[HarmonyPatch(typeof(SConConfig_QuestFindShopKeepers))]
public class SConConfig_QuestFindShopKeepers_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(SConConfig_QuestFindShopKeepers.IsFound), [typeof(ConLevelId)])]
    private static bool Start_Prefix(ref bool __result, ConLevelId levelId)
    {
        if (!RandomStateHandler.Randomized) return true;

#pragma warning disable Harmony003 // Harmony non-ref patch parameters modified
        __result = DataStorage.GetFoundCousins().Contains(levelId.StringValue);
#pragma warning restore Harmony003 // Harmony non-ref patch parameters modified

        return true;
    }
}
