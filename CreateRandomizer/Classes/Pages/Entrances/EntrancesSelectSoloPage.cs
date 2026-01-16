using CheatMenu.Classes;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Entrances;
using RandomizerCore.Classes.Data.Types.Regions;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Entrances;

public class EntrancesSelectSoloPage : SoloGUIPage
{
    private SavedDataOwnersSelectPage<AEntrance, AEntranceSavedData> selectPage;
    private EntrancesSoloPage soloPage;
    public override string Name => selectPage.Name;

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        selectPage = new(1);
        base.Init(modGUI, parent, id);
        soloPage = new GameObject().AddComponent<EntrancesSoloPage>();
        soloPage.Init(modGUI, parent, ModGUI.winId++);
    }

    public void Open(Region region)
    {
        if (selectPage.Name == region.GetName()) return;
        selectPage.Open(region.GetName(), region.entrances.ConvertAll(EntranceHandler.I.GetFromName), GetColor);
        Open();
    }
    public override void Open()
    {
        if (selectPage.Name == "") Close();
        base.Open();
    }

    private Color? GetColor(AEntrance entrance)
    {
        return entrance.GetSavedData().completed ? null : Color.red;
    }

    public override void UpdateOpen()
    {
        base.UpdateOpen();
        AEntrance entrance = selectPage.UpdateOpen();
        if (entrance != null) soloPage.Open(entrance);
    }

    public override void Close()
    {
        soloPage.Close();
        selectPage.Close();
        base.Close();
    }
}
