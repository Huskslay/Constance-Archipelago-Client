using Constance;
using RandomizerCore.Classes.Data.Types.AItems.Types;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomizerCore.Classes.Data.Types.Locations.Types;

[Serializable]
public class PickupLocation : ALocation
{
    public static FindObjectsInactive FindInactive => FindObjectsInactive.Exclude;
    public override Randomizable GetRandoType() => Randomizable.Pickup;


    public PickupLocation(CConEntityDropBehaviour_TouchToCollect pickup, Region region) : base(GetName(pickup), region, pickup.name)
    {
        baseItem = CreateItem(pickup, region).GetName();
        LocationHandler.I.Save(this);
    }


    public static string GetName(CConEntityDropBehaviour_TouchToCollect pickup)
    {
        return pickup.collectable.name.Replace("item_", "");
    }

    public Item CreateItem(CConEntityDropBehaviour_TouchToCollect pickup, Region region)
    {
        Item item = new(GetName(pickup), type, region, "progression", CollectableItems.None);
        item.aItems = [new CollectableItem(pickup.collectable, item.GetName(), 0).GetName()];
        ItemHandler.I.Save(item);
        return item;
    }

    public static void PatchLoadedLevel(List<CConEntityDropBehaviour_TouchToCollect> pickups, Region region)
    {
        BasicPatch<CConEntityDropBehaviour_TouchToCollect, PickupLocation>(pickups, region, (dropBehaviour, location) =>
        { dropBehaviour.collectable = null; });
    }
}
