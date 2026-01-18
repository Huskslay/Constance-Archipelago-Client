using RandomizerCore.Classes.Enums;
using System;

namespace RandomizerCore.Classes.Data.Rules;

[Serializable]
public class Rule
{
    public KeyItems items = KeyItems.None;
    public Skips skips = Skips.None;
}
