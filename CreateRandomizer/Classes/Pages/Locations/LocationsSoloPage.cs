using CheatMenu.Classes;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Regions;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Locations;

public class LocationsSoloPage : SoloGUIPage
{
    private SavedDataOwnerSoloPage<ALocation, ALocationSavedData> soloPage;
    public override string Name => soloPage.Name;

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        soloPage = new();
        base.Init(modGUI, parent, id);
    }

    public void Open(ALocation location)
    {
        if (soloPage.OwnerSet && location == soloPage.Owner) return;
        soloPage.Open(location);
        Open();
    }
    public override void Open()
    {
        if (!soloPage.OwnerSet) Close();
        base.Open();
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
