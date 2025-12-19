using Constance;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Randomizer.Patches.Locations.CarnivalEye;

[HarmonyPatch(typeof(CConWorldEvent_C01_Carousel))]
public class CConWorldEvent_C01_Carousel_Patch
{
    private static bool jackieTentSpawned;

    // Prevent jackie tent being removed
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConWorldEvent_C01_Carousel.Start))]
    private static void Start_Prefix()
    {
        jackieTentSpawned = CConCarnivalHeadlightEye.JackieTentSpawned;
    }
    [HarmonyPostfix]
    [HarmonyPatch(nameof(CConWorldEvent_C01_Carousel.Start))]
    private static void Start_Postfix()
    {
        CConCarnivalHeadlightEye.JackieTentSpawned = jackieTentSpawned;
    }
}
