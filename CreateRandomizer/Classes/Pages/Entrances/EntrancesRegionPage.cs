using CheatMenu.Classes;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Entrances;
using RandomizerCore.Classes.Data.Types.Regions;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Entrances;

public class EntrancesRegionPage : GUIPage
{
    private SavedDataOwnersSelectPage<Region, RegionSavedData> selectPage;
    private EntrancesSelectSoloPage soloPage;

    public override string Name => "Entrances";

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        base.Init(modGUI, parent, id);
        selectPage = new(3);
        soloPage = new GameObject($"{Name}-Solo").AddComponent<EntrancesSelectSoloPage>();
        soloPage.Init(modGUI, transform, ModGUI.winId++);
    }

    public override void Open()
    {
        selectPage.Open(Name, [.. RegionHandler.I.dataOwners.Values], GetColor);
    }

    private Color? GetColor(Region region)
    {
        foreach (string entranceName in region.entrances)
        {
            AEntrance entrance = EntranceHandler.I.GetFromName(entranceName);
            if (!entrance.GetSavedData().completed) return Color.red;
        }
        return null;
    }

    public override void UpdateOpen()
    {
        Region region = selectPage.UpdateOpen();
        if (region != null) soloPage.Open(region);
    }

    public override void Close()
    {
        soloPage.Close();
        selectPage.Close();
    }
}
