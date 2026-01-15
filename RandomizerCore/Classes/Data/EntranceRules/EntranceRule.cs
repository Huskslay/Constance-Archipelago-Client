using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Data.EntranceRules;

[Serializable]
public class EntranceRule(string entrance)
{
    public string entrance = entrance;
    public KeyItems items = KeyItems.None;
    public Skips skips = Skips.None;
    public List<CollectableItemEntry> collectableItems = [];
}
