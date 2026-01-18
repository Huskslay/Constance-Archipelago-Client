using Constance;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Data.Types.AItems.Types;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;

namespace RandomizerCore.Classes.Data.Types.Locations.Types;

[Serializable]
public class TearLocation : ALocation
{
    public override Randomizable GetRandoType() => Randomizable.Tear;


    public TearLocation(SConCollectable tear, Region region) : base(GetName(tear), region, tear.name)
    {
        baseItem = CreateItem(tear, region).GetName();
        LocationHandler.I.Save(this);
    }


    public static string GetName(SConCollectable tear)
    {
        return tear.name.Replace("unlock_Tear_", "");
    }

    public Item CreateItem(SConCollectable tear, Region region)
    {
        Item item = new(GetName(tear), type, region, "progression", CollectableItems.Tear);
        item.aItems = [new CollectableItem(tear, item.GetName(), 0).GetName()];
        ItemHandler.I.Save(item);
        return item;
    }

    public static void PatchLoadedLevel(CConLevel_Flashback flashback, Region region)
    {
        if (flashback == null) return;

        TearLocation location = null;
        foreach (string locationName in region.locations)
        {
            if (LocationHandler.I.GetFromName(locationName) is TearLocation tearLocation)
            {
                location = tearLocation;
                break;
            }
        }
        if (location == null) return;
        flashback.gameObject.AddComponent<LocationComponent>().Set(location);
    }
}
