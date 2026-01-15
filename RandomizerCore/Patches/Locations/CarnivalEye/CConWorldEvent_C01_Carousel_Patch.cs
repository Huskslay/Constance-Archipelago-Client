using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Locations.CarnivalEye;

[HarmonyPatch(typeof(CConWorldEvent_C01_Carousel))]
public class CConWorldEvent_C01_Carousel_Patch
{
    private static bool jackieTentSpawned;

    // Prevent jackie tent being removed
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConWorldEvent_C01_Carousel.Start))]
    private static void Start_Prefix()
    {
        if (!RandomStateHandler.Randomized) return;
        jackieTentSpawned = CConCarnivalHeadlightEye.JackieTentSpawned;
    }
    [HarmonyPostfix]
    [HarmonyPatch(nameof(CConWorldEvent_C01_Carousel.Start))]
    private static void Start_Postfix()
    {
        if (!RandomStateHandler.Randomized) return;
        CConCarnivalHeadlightEye.JackieTentSpawned = jackieTentSpawned;
    }
}