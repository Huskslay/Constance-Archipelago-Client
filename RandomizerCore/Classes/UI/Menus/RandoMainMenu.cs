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

    public bool newRando;

    public void Init()
    {
        urlInput = new(transform, "Url: ", url, false, OnUpdateUrl, DisableAll);
        portInput = new(transform, "Port: ", port.ToString(), true, OnUpdatePort, DisableAll);
        slotInput = new(transform, "Slot: ", slot, false, OnUpdateSlot, DisableAll);
        pwInput = new(transform, "Password: ", password, false, OnUpdatePassword, DisableAll);

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
        OnUpdateUrl(urlInput, file.url);
        OnUpdatePort(portInput, file.port.ToString());
        OnUpdateSlot(slotInput, file.slotName.ToString());
        OnUpdatePassword(pwInput, "");
    }
    private void OnUpdateUrl(RandoInputField inputButton, string newValue)
    {
        url = newValue;
        inputButton.SetInput(newValue);
    }
    private void OnUpdatePort(RandoInputField inputButton, string newValue)
    {
        if (!int.TryParse(newValue, out port)) return;
        inputButton.SetInput(newValue);
    }
    private void OnUpdateSlot(RandoInputField inputButton, string newValue)
    {
        slot = newValue;
        inputButton.SetInput(newValue);
    }
    private void OnUpdatePassword(RandoInputField inputButton, string newValue)
    {
        password = newValue;
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
        CConStartMenu_Patch.SwitchMenu(newRando ? RandomMenuHandler.RandoSelectMenu : CConStartMenu_Patch.SaveMenu, this);
    }



    public override bool OpenPanel(IConPlayerEntity player, Leo.Void parameters)
    {
        gameObject.SetActive(true);
        errorButton.text = "";
        return base.OpenPanel(player, parameters);
    }
    public override bool ClosePanel()
    {
        gameObject.SetActive(false);
        return base.ClosePanel();
    }
}
