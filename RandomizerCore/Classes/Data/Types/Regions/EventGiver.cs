using RandomizerCore.Classes.Data.Rules;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Data.Types.Regions;

[Serializable]
public class EventGiver(GameEvents gameEvent)
{
    public GameEvents gameEvent = gameEvent;
    public EntranceRules entranceRules = new();
}
