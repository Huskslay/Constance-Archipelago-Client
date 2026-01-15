using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Handlers.Files;

namespace RandomizerCore.Patches.Files;

[HarmonyPatch(typeof(CConPlayerManager_Game))]
public class CConPlayerManager_Game_Patch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(CConPlayerManager_Game.Start))]
    private static void Start_Postfix()
    {
        Plugin.Logger.LogMessage("Loaded Game");
        if (CConUiSaveSlot_Patch.latestLoaded) RandomFilesHandler.OnFileLoad(CConUiSaveSlot_Patch.latestSave);
        else RandomFilesHandler.OnFileCreate(CConUiSaveSlot_Patch.latestSave);
    }
}