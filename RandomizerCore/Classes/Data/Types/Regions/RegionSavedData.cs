using RandomizerCore.Classes.Data.Saved;
using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Data.Types.Regions;

[Serializable]
public class RegionSavedData(string connection) : SavedData(connection)
{
    public List<EventGiver> givenEvents = [];
}
