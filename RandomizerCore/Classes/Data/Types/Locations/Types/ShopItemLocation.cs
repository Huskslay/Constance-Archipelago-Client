using Constance;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Data.Types.AItems.Types;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Data.Types.Locations.Types;

[Serializable]
public class ShopItemLocation : ALocation
{
    public override Randomizable GetRandoType() => Randomizable.ShopItem;


    public ShopItemLocation(SConCollectable_ShopItem shopItem, Region region) : base(GetName(shopItem), region, shopItem.name)
    {
        CreateItem(shopItem, region);
        LocationHandler.I.Save(this);
    }


    public static string GetName(SConCollectable_ShopItem shopItem)
    {
        return shopItem.name.Replace("shopItem_", "");
    }

    public Item CreateItem(SConCollectable_ShopItem shopItem, Region region)
    {
        Item item = new(GetName(shopItem), type, region, "filler", CollectableItems.None);
        item.aItems = [new CollectableItem(shopItem, item.GetName(), 0, shopItem.ItemAmount).GetName()];
        ItemHandler.I.Save(item);
        return item;
    }

    public static void PatchLoadedLevel(CConUiPanel_Shop shop, Region region)
    {
        if (shop != null && region.GetName() == "J04")
        {
            List<ShopItemLocation> locations = [];
            foreach (string locationName in region.locations)
            {
                ALocation location = LocationHandler.I.GetFromName(locationName);
                if (location is ShopItemLocation shopItemLocation) locations.Add(shopItemLocation);
            }

            List<ALocation> itemLocations = [];
            foreach (SConCollectable_ShopItem shopItem in ConMonoBehaviour.SceneRegistry.Collectables.ShopItems)
            {
                ALocation location = locations.Find(location => location.goName == shopItem.name);
                if (location == null)
                {
                    Plugin.Logger.LogWarning($"Could not find a location for shop item: {shopItem.name}");
                    continue;
                }
                itemLocations.Add(location);
            }
            shop.gameObject.AddComponent<ManyLocationComponent>().Set(locations);
        }
    }
}
