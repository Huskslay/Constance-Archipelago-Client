using CheatMenu.Classes;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Regions;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Locations;

public class LocationsPage : GUIPage
{
    private SavedDataOwnersPage<ALocation, ALocationSavedData> page;
    private LocationsSoloPage soloPage;

    public override string Name => "Locations";

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        base.Init(modGUI, parent, id);
        page = new(LocationHandler.I, 1);
        soloPage = new GameObject("Solo").AddComponent<LocationsSoloPage>();
        soloPage.Init(modGUI, transform, ModGUI.winId++);
    }

    public override void Open()
    {
        page.Open();
    }

    public override void UpdateOpen()
    {
        ALocation location = page.UpdateOpen();
        if (location != null) soloPage.Open(location);
    }

    public override void Close()
    {
        page.Close();
    }
}
