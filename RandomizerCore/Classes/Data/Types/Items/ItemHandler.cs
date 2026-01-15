using FileHandler.Classes;
using System;

namespace RandomizerCore.Classes.Data.Types.Items;

public class ItemHandler : SavedDataOwnerHandler<Item, ItemSavedData>
{
    public override string GetName() => "Items";

    public static ItemHandler I;
    public override void Init()
    {
        I = this;
        base.Init();
    }

    protected override void LoadAll(Action<Item> initiate)
    {
        foreach (Item item in FileSaveLoader.LoadClasses<Item>(FolderPath))
            initiate(item);
    }
}
