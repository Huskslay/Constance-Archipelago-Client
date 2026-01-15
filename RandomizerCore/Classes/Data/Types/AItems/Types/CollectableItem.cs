using Constance;
using RandomizerCore.Classes.Handlers;
using System;

namespace RandomizerCore.Classes.Data.Types.AItems.Types;

[Serializable]
public class CollectableItem : AItem
{
    public string collectable;
    public int amount;

    public CollectableItem(SConCollectable collectable, string item, int index, int amount = 1) : base(item, index)
    {
        this.collectable = collectable.name;
        this.amount = amount;

        AItemHandler.I.Save(this);
    }

    public override void GiveToPlayer(IConPlayerEntity player, IConPlayerInventory inventoryManager)
    {
        Plugin.Logger.LogMessage($"Giving {amount} {collectable}");
        inventoryManager.Collect(player, CollectableHandler.dict[collectable], amount);
    }
}
