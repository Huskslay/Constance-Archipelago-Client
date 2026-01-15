using Constance;
using FileHandler.Classes;
using RandomizerCore.Classes.Data.Types.AItems.Types;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;

namespace RandomizerCore.Classes.Data.Types.Locations.Types.Desposits;

[Serializable]
public class CurrencyFlowerLocation : ALocation
{
    public override Randomizable GetRandoType() => Randomizable.CurrencyFlower;


    public CurrencyFlowerLocation(CConCurrencyDepositEntity deposit, Region region) : base(GetName(deposit), region, deposit.name)
    {
        CreateItem(deposit, region);
        LocationHandler.I.Save(this);
    }


    public static string GetName(CConCurrencyDepositEntity deposit)
    {
        return FileSaveLoader.FourDigitHash(deposit.persistable.persistenceId.StringValue);
    }

    public Item CreateItem(CConCurrencyDepositEntity deposit, Region region)
    {
        Item item = new(GetName(deposit), type, region, "filler", CollectableItems.None)
        { aItems = [] };

        int i = 0;
        foreach (CConCurrencyDepositEntity.ConCurrencyDepositState lootBag in deposit.depositStates)
        {
            LootBagItem lootBagItem = new(lootBag.LootBag, item.GetName(), i++);
            item.aItems.Add(lootBagItem.GetName());
        }

        ItemHandler.I.Save(item);
        return item;
    }
}

