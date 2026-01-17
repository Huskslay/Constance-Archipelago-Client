using RandomizerCore.Classes.Data.EntranceRules;
using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Data.Saved;

[Serializable]
public class EntranceRuleSavedData(string connection) : SavedData(connection)
{
    public bool completed = false;
    public bool used = true;

    public List<EntranceRule> entranceRules = [];
}
