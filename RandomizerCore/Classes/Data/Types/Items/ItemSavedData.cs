using RandomizerCore.Classes.Data.Saved;
using RandomizerCore.Classes.Enums;
using System;

namespace RandomizerCore.Classes.Data.Types.Items;

[Serializable]
public class ItemSavedData(string connection) : SavedData(connection)
{
    public KeyItems givenItems = KeyItems.None;
    public CollectableItems givenCollectables = CollectableItems.None;
}
