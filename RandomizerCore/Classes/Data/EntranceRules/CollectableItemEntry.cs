using RandomizerCore.Classes.Enums;
using System;

namespace RandomizerCore.Classes.Data.EntranceRules;

[Serializable]
public class CollectableItemEntry(CollectableItems item)
{
    public CollectableItems item = item;
    public int count = 0;
}
