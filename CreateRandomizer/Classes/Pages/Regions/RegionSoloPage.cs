using CheatMenu.Classes;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Regions;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Regions;

public class RegionSoloPage : SoloGUIPage
{
    private SavedDataOwnerSoloPage<Region, RegionSavedData> soloPage;
    public override string Name => soloPage.Name;

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        soloPage = new();
        base.Init(modGUI, parent, id);
    }

    public void Open(Region region)
    {
        if (soloPage.OwnerSet && region == soloPage.Owner) return;
        soloPage.Open(region);
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
