using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomizerCore.Classes.Data.Rules;

[Serializable]
public class EntranceRules
{
    public bool needsCollectables = false;
    public List<CollectableItemEntry> collectableItems = [];

    public bool needsEvents = false;
    public GameEvents events = GameEvents.None;

    public List<EntranceRule> entranceRules = [];
}
