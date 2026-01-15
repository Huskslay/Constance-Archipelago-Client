using CheatMenu.Classes;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Regions;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Regions;

public class RegionsPage : GUIPage
{
    private SavedDataOwnersPage<Region, RegionSavedData> page;
    private RegionSoloPage soloPage;

    public override string Name => "Regions";

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        base.Init(modGUI, parent, id);
        page = new(RegionHandler.I, 3);
        soloPage = new GameObject("Solo").AddComponent<RegionSoloPage>();
        soloPage.Init(modGUI, transform, ModGUI.winId++);
    }

    public override void Open()
    {
        page.Open();
    }

    public override void UpdateOpen()
    {
        Region region = page.UpdateOpen();
        if (region != null) soloPage.Open(region);
    }

    public override void Close()
    {
        page.Close();
    }
}
