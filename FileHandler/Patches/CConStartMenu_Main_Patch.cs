using Constance;
using FileHandler.Classes;
using HarmonyLib;

namespace FileHandler.Patches;

[HarmonyPatch(typeof(CConQuitButton))]
public class CConStartMenu_Main_Patch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(CConQuitButton.InitQuit))]
    private static void OpenPanel_Postfix(bool toStartMenu)
    {
        if (toStartMenu) GameDataActions.OnFileQuit.Invoke();
    }
}