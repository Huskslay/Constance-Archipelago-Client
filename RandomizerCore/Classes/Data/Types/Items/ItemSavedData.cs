using RandomizerCore.Classes.Data.Saved;
using System;

namespace RandomizerCore.Classes.Data.Types.Items;

[Serializable]
public class ItemSavedData(string connection) : SavedData(connection)
{
}
