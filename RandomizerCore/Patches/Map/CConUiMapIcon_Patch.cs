using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Map;

[HarmonyPatch(typeof(CConUiMapIcon))]
public class CConUiMapIcon_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch("Constance.IConUiMapSelectTarget.Selectable", MethodType.Getter)]
    private static bool Selectable_Prefix(CConUiMapIcon __instance, ref bool __result)
    {
        if (!RandomStateHandler.Randomized) return true;
        if (__instance.name != "Custom") return true;

        __result = true;
        return false;
    }
}