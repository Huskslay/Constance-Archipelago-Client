using RandomizerCore.Classes.Data.EntranceRules;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomizerCore.Classes.Data.Saved;

[Serializable]
public class EntranceRuleSavedData(string connection) : SavedData(connection)
{
    public bool completed = false;
    public bool used = false;

    public List<EntranceRule> entranceRules = [];

    public override Color? GetColor() => completed ? null : Color.red;
}
