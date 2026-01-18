using System;

namespace RandomizerCore.Classes.Enums;

[Flags]
public enum KeyItems : ulong
{
    None = 0,
    Dash = 1UL << 0,
    Stab = 1UL << 1,
    WallDive = 1UL << 2,
    Pogo = 1UL << 3,
    Slice = 1UL << 4,
    BombClone = 1UL << 5,
    DoubleJump = 1UL << 6,
    TpInspiration = 1UL << 7
}
