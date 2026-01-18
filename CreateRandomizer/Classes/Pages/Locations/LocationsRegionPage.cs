using CheatMenu.Classes;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Regions;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Locations;

public class LocationsRegionPage : GUIPage
{
    private SavedDataOwnersSelectPage<Region, RegionSavedData> selectPage;
    private LocationsSelectSoloPage soloPage;

    public override string Name => "Locations";

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        base.Init(modGUI, parent, id);
        selectPage = new(3);
        soloPage = new GameObject($"{Name}-Solo").AddComponent<LocationsSelectSoloPage>();
        soloPage.Init(modGUI, transform, ModGUI.winId++);
    }

    public override void Open()
    {
        selectPage.Open(Name, [.. RegionHandler.I.dataOwners.Values], GetColor);
    }

    private Color? GetColor(Region region)
    {
        foreach (string locationName in region.locations)
        {
            ALocation location = LocationHandler.I.GetFromName(locationName);
            if (!location.GetSavedData().used) continue;
            if (!location.GetSavedData().completed) return Color.red;
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
