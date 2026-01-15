using Constance;
using RandomizerCore.Classes.Handlers;
using RandomizerCore.Classes.UI.Elements;
using RandomizerCore.Patches.Files;
using UnityEngine.EventSystems;

namespace RandomizerCore.Classes.UI.Menus;

public class RandoSelectMenu : AConStartMenuPanel, IConSelectionLayer, ITransformProvider
{
    public override IConSelectionLayer SelectionLayer => this;

    public override bool AllowCancel => true;


    public void Init()
    {
        CConStartMenu_Patch.CreateButton("Randomizer", transform, Randomizer);
        CConStartMenu_Patch.CreateButton("> Vanilla <", transform, Vanilla);
        CConStartMenu_Patch.CreateBlock(50, 50, transform);
        CConStartMenu_Patch.CreateButton("<- Back <-", transform, Back);
    }



    public bool TryGetNewSelection(out ISelectHandler selectable)
    {
        return ConUiUtils.FindFirstSelectable(gameObject, out selectable);
    }

    private void Randomizer(RandoButton button)
    {
        RandomMenuHandler.randomizing = true;
        RandomMenuHandler.RandoMainMenu.newRando = true;
        CConStartMenu_Patch.SwitchMenu(RandomMenuHandler.RandoMainMenu, this);
    }
    private void Vanilla(RandoButton button)
    {
        RandomMenuHandler.randomizing = false;
        CConStartMenu_Patch.StartMenu.LoadSaveSlot(CConUiSaveSlot_Patch.latestSave);
    }
    private void Back(RandoButton button)
    {
        CConStartMenu_Patch.SwitchMenu(CConStartMenu_Patch.SaveMenu, this);
    }



    public override bool OpenPanel(IConPlayerEntity player, Leo.Void parameters)
    {
        gameObject.SetActive(true);
        return base.OpenPanel(player, parameters);
    }
    public override bool ClosePanel()
    {
        gameObject.SetActive(false);
        return base.ClosePanel();
    }
}
