using FileHandler.Classes;
using System;

namespace RandomizerCore.Classes.Data.Types.AItems;

public class AItemHandler : SavedDataOwnerHandler<AItem, AItemSavedData>
{
    public override string GetName() => "AItems";

    public static AItemHandler I;
    public override void Init()
    {
        I = this;
        base.Init();
    }

    protected override void LoadAll(Action<AItem> initiate)
    {
        foreach (AItem item in FileSaveLoader.LoadClasses<AItem>(FolderPath))
            initiate(item);
    }
}
