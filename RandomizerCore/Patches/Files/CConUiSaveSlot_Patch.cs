using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Handlers;
using RandomizerCore.Classes.Handlers.Files;

namespace RandomizerCore.Patches.Files;

[HarmonyPatch(typeof(CConUiSaveSlot))]
public class CConUiSaveSlot_Patch
{
    public static ConSaveStateId latestSave;
    public static bool latestLoaded;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConUiSaveSlot.OnSubmit))]
    private static bool OnSubmit_Prefix(CConUiSaveSlot __instance)
    {
        latestSave = __instance.SlotId;
        latestLoaded = __instance._validSave;

        // Loading file
        if (latestLoaded)
        {
            // Test is rando
            RandomFile file = RandomFilesHandler.GetRandomFile(latestSave);
            RandomMenuHandler.randomizing = file != null;
            if (!RandomMenuHandler.randomizing) return true;

            // Is a rando, prepare
            RandomMenuHandler.RandoMainMenu.isNewRando = false;
            RandomMenuHandler.RandoMainMenu.UpdateValues(file);
            // Open rando settings menu
            CConStartMenu_Patch.SwitchMenu(RandomMenuHandler.RandoMainMenu, CConStartMenu_Patch.SaveMenu);
            return false;
        }

        // Open select menu to choose rando or vanilla
        CConStartMenu_Patch.SwitchMenu(RandomMenuHandler.RandoSelectMenu, CConStartMenu_Patch.SaveMenu);
        return false;
    }
}