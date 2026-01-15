using RandomizerCore.Classes.Data.Saved;
using System;

namespace RandomizerCore.Classes.Data.Types.Regions;

[Serializable]
public class RegionSavedData(string connection) : SavedData(connection)
{
}
