using Constance;
using FileHandler.Classes;
using RandomizerCore.Classes.Data.Types.AItems.Types;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomizerCore.Classes.Data.Types.Locations.Types;

[Serializable]
public class ChestLocation : ALocation
{
    public static FindObjectsInactive FindInactive => FindObjectsInactive.Include;
    public override Randomizable GetRandoType() => Randomizable.Chest;


    public ChestLocation(CConChestEntity chest, Region region) : base(GetName(chest), region, chest.name)
    {
        CreateItem(chest, region);
        LocationHandler.I.Save(this);
    }


    public static string GetName(CConChestEntity chest)
    {
        return FileSaveLoader.FourDigitHash(chest.persistable.persistenceId.StringValue);
    }

    public Item CreateItem(CConChestEntity chest, Region region)
    {
        Item item = new(GetName(chest), type, region, "filler", CollectableItems.None)
        { aItems = [] };

        int i = 0;
        foreach (IConLootBag lootBag in chest.LootBags)
        {
            LootBagItem lootBagItem = new(lootBag, item.GetName(), i++);
            item.aItems.Add(lootBagItem.GetName());
        }

        ItemHandler.I.Save(item);
        return item;
    }

    public static void PatchLoadedLevel(List<CConChestEntity> chests, Region region)
    {
        BasicPatch<CConChestEntity, ChestLocation>(chests, region);
    }
}
