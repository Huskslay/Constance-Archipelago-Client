using System;

namespace RandomizerCore.Classes.Data.Saved;

[Serializable]
public class SavedData(string connection)
{
    public string connection = connection;
}