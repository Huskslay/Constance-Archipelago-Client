using RandomizerCore.Classes.Data;
using RandomizerCore.Classes.Data.Saved;

namespace CreateRandomizer.Classes.Pages.Generic;

public class SavedDataOwnerSoloPage<T1, T2> where T1 : ISavedDataOwner<T2> where T2 : SavedData
{
    public T1 Owner { get; private set; }
    public bool OwnerSet { get; private set; } = false;

    public string Name => OwnerSet ? Owner.GetName() : "null";

    public void Open(T1 owner)
    {
        Owner = owner;
        OwnerSet = true;
    }

    public void UpdateOpen()
    {
        if (!OwnerSet) return;
        SavedDataDrawer.Draw(Owner.GetSavedData());
    }

    public void Close()
    {
        Owner = default;
        OwnerSet = false;
    }
}
