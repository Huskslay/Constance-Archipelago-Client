using System;

namespace RandomizerCore.Classes.Enums;

[Flags]
public enum GameEvents : ulong
{
    None = 0,
    AweKingDeath = 1 << 0,
    HighPatiaDeath = 1 << 1
}
