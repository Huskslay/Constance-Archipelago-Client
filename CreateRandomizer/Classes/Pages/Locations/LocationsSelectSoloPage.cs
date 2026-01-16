using CheatMenu.Classes;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Regions;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Locations;

public class LocationsSelectSoloPage : SoloGUIPage
{
    private SavedDataOwnersSelectPage<ALocation, ALocationSavedData> selectPage;
    private LocationSoloPage soloPage;
    public override string Name => selectPage.Name;

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        selectPage = new(1);
        base.Init(modGUI, parent, id);
        soloPage = new GameObject().AddComponent<LocationSoloPage>();
        soloPage.Init(modGUI, parent, ModGUI.winId++);
    }

    public void Open(Region region)
    {
        if (selectPage.Name == region.GetName()) return;
        selectPage.Open(region.GetName(), region.locations.ConvertAll(LocationHandler.I.GetFromName), GetColor);
        Open();
    }
    public override void Open()
    {
        if (selectPage.Name == "") Close();
        base.Open();
    }

    private Color? GetColor(ALocation location)
    {
        return location.GetSavedData().completed ? null : Color.red;
    }

    public override void UpdateOpen()
    {
        base.UpdateOpen();
        ALocation location = selectPage.UpdateOpen();
        if (location != null) soloPage.Open(location);
    }

    public override void Close()
    {
        soloPage.Close();
        selectPage.Close();
        base.Close();
    }
}
