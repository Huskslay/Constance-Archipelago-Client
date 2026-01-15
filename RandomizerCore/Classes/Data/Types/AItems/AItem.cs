using Constance;
using System;

namespace RandomizerCore.Classes.Data.Types.AItems;

[Serializable]
public abstract class AItem : ISavedDataOwner<AItemSavedData>
{
    public string item;
    public int index;

    public void Init() { }
    public string GetName() => $"{item}-{index}";


    public AItem(string item, int index)
    {
        this.item = item;
        this.index = index;

        SetSavedData(new(GetName()));
    }


    public abstract void GiveToPlayer(IConPlayerEntity player, IConPlayerInventory inventoryManager);


    [NonSerialized]
    private AItemSavedData savedData;
    public AItemSavedData GetSavedData() { return savedData; }
    public void SetSavedData(AItemSavedData savedData, bool save = true)
    {
        if (save) AItemHandler.I.Save(savedData, this);
        this.savedData = savedData;
    }
}
