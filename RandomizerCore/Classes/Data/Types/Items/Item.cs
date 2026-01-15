using Constance;
using RandomizerCore.Classes.Data.Types.AItems;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Data.Types.Items;

[Serializable]
public class Item : ISavedDataOwner<ItemSavedData>
{
    public string name;
    public string type;
    public string region;
    public string classification;

    public CollectableItems collectable;
    public List<string> aItems = [];
    public string defaultLocation = "";

    public void Init() { }
    public string DisplayName() => $"{type}-{name}-{region}";
    public string GetName() => $"{DisplayName()}-Item";


    public Item(string name, string type, Region region, string classification, CollectableItems collectable)
    {
        this.name = name;
        this.type = type;
        this.region = region.name;
        this.classification = classification;

        this.collectable = collectable;
        SetSavedData(new(GetName()));
    }


    public void GiveToPlayer(IConPlayerEntity player, IConPlayerInventory inventoryManager)
    {
        foreach (string aItemName in aItems)
        {
            AItem aItem = AItemHandler.I.GetFromName(aItemName);

            if (aItem == null)
                Plugin.Logger.LogError($"Could not find aItem of name: {aItemName}");
            else aItem.GiveToPlayer(player, inventoryManager);
        }
    }


    [NonSerialized]
    private ItemSavedData savedData;
    public ItemSavedData GetSavedData() { return savedData; }
    public void SetSavedData(ItemSavedData savedData, bool save = true)
    {
        if (save) ItemHandler.I.Save(savedData, this);
        this.savedData = savedData;
    }
}
