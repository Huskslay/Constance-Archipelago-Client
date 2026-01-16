using CheatMenu.Classes;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Entrances;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Entrances;

public class EntrancesSoloPage : SoloGUIPage
{
    private SavedDataOwnerSoloPage<AEntrance, AEntranceSavedData> soloPage;
    public override string Name => soloPage.Name;
    private string selectedEntranceRule;

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        soloPage = new();
        base.Init(modGUI, parent, id);
        selectedEntranceRule = null;
    }

    public void Open(AEntrance entrance)
    {
        if (soloPage.OwnerSet && soloPage.Owner == entrance) return;
        soloPage.Open(entrance, DrawSavedData);
        Open();
    }
    public override void Open()
    {
        if (!soloPage.OwnerSet) Close();
        base.Open();
    }

    private void DrawSavedData(AEntranceSavedData savedData)
    {
        PageHelpers.DrawEntranceRuleSavedData(savedData, ref selectedEntranceRule);
    }

    public override void UpdateOpen()
    {
        base.UpdateOpen();
        soloPage.UpdateOpen();
    }

    public override void Close()
    {
        soloPage.Close();
        base.Close();
    }
}
