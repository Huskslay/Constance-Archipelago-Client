using Constance;
using RandomizerCore.Classes.Handlers;
using RandomizerCore.Classes.Handlers.Files;
using RandomizerCore.Classes.UI.Elements;
using RandomizerCore.Patches.Files;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RandomizerCore.Classes.UI.Menus;

public class RandoMainMenu : AConStartMenuPanel, IConSelectionLayer, ITransformProvider
{
    public bool isNewRando;


    public override IConSelectionLayer SelectionLayer => this;
    public override bool AllowCancel => true;


    private RandoInputField urlInput;
    private RandoInputField portInput;
    private RandoInputField slotInput;
    private RandoInputField pwInput;
    private string url = "localhost";//"archipelago.gg";
    private int port = 0;
    private string slot = "Player1";
    private string password = "";

    private RandoButton errorButton;


    public void Init()
    {
        urlInput = new(transform, "Url: ", url, false,
            (button, newValue) => OnUpdateText(ref url, button, newValue), DisableAll);
        portInput = new(transform, "Port: ", port.ToString(), true,
            (button, newValue) => OnUpdateInt(ref port, button, newValue), DisableAll);
        slotInput = new(transform, "Slot: ", slot, false,
            (button, newValue) => OnUpdateText(ref slot, button, newValue), DisableAll);
        pwInput = new(transform, "Password: ", password, false,
            (button, newValue) => OnUpdateText(ref password, button, newValue), DisableAll);

        CConStartMenu_Patch.CreateBlock(50, 60, transform);
        errorButton = CConStartMenu_Patch.CreateButton("", transform, null);
        CConStartMenu_Patch.CreateBlock(50, 25, transform);
        CConStartMenu_Patch.CreateButton("> Go! <", transform, Go);
        CConStartMenu_Patch.CreateBlock(50, 50, transform);
        CConStartMenu_Patch.CreateButton("<- Back <-", transform, Back);

        errorButton.tmp.color = Color.red;
        errorButton.tmp.transform.localScale *= 0.75f;
    }

    private void DisableAll()
    {
        urlInput.active = false;
        portInput.active = false;
        slotInput.active = false;
        pwInput.active = false;
    }

    public void UpdateValues(RandomFile file)
    {
        OnUpdateText(ref url, urlInput, file.url);
        OnUpdateInt(ref port, portInput, file.port.ToString());
        OnUpdateText(ref slot, slotInput, file.slotName.ToString());
        OnUpdateText(ref password, pwInput, "");
    }
    private void OnUpdateText(ref string value, RandoInputField inputButton, string newValue)
    {
        value = newValue;
        inputButton.SetInput(newValue);
    }
    private void OnUpdateInt(ref int value, RandoInputField inputButton, string newValue)
    {
        if (!int.TryParse(newValue, out value)) return;
        inputButton.SetInput(newValue);
    }


    public bool TryGetNewSelection(out ISelectHandler selectable)
    {
        return ConUiUtils.FindFirstSelectable(gameObject, out selectable);
    }

    private void Go(RandoButton button)
    {
        DisableAll();
        RandomFilesHandler.Connect(url, port, slot, password, (errorMessage) => errorButton.text = errorMessage);
    }
    private void Back(RandoButton button)
    {
        DisableAll();
        CConStartMenu_Patch.SwitchMenu(isNewRando ? RandomMenuHandler.RandoSelectMenu : CConStartMenu_Patch.SaveMenu, this);
    }



    public override bool OpenPanel(IConPlayerEntity player, Leo.Void parameters)
    {
        errorButton.text = "";
        gameObject.SetActive(true);
        return base.OpenPanel(player, parameters);
    }
    public override bool ClosePanel()
    {
        gameObject.SetActive(false);
        return base.ClosePanel();
    }
}
