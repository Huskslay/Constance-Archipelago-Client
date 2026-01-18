using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Data.Rules;

[Serializable]
public class EntranceRule(string entrance)
{
    public string entrance = entrance;
    public bool possible = true;
    public List<Rule> rules = [];
}
