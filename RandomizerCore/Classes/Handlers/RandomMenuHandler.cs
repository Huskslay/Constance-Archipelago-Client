using RandomizerCore.Classes.UI.Menus;
using RandomizerCore.Patches.Files;

namespace RandomizerCore.Classes.Handlers;

public static class RandomMenuHandler
{
    public static RandoSelectMenu RandoSelectMenu;
    public static RandoMainMenu RandoMainMenu;

    public static bool randomizing = false;


    public static void LoadSave()
    {
        CConStartMenu_Patch.StartMenu.LoadSaveSlot(CConUiSaveSlot_Patch.latestSave);
    }


    public static void CreateMenus()
    {
        RandoSelectMenu = CConStartMenu_Patch.CreateMenu<RandoSelectMenu>("RandoSelectMenu");
        RandoSelectMenu.Init();

        RandoMainMenu = CConStartMenu_Patch.CreateMenu<RandoMainMenu>("RandoMainMenu");
        RandoMainMenu.Init();
    }
}