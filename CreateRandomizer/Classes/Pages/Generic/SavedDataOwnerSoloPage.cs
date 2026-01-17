using RandomizerCore.Classes.Data;
using RandomizerCore.Classes.Data.Saved;
using System;

namespace CreateRandomizer.Classes.Pages.Generic;

public class SavedDataOwnerSoloPage<T1, T2> where T1 : ISavedDataOwner<T2> where T2 : SavedData
{
    public T1 Owner { get; private set; }
    public bool OwnerSet { get; private set; } = false;

    private Action<T2> drawSavedData;

    public string Name => OwnerSet ? Owner.GetName() : "null";

    public void Open(T1 owner, Action<T2> drawSavedData)
    {
        Owner = owner;
        OwnerSet = true;
        this.drawSavedData = drawSavedData;
    }

    public void UpdateOpen()
    {
        if (OwnerSet) drawSavedData?.Invoke(Owner.GetSavedData());
    }

    public void Close()
    {
        Owner = default;
        OwnerSet = false;
    }
}
