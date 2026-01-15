using System;

namespace RandomizerCore.Classes.Enums;

[Flags]
public enum KeyItems : ulong
{
    None = 0,
    Dash = 1 << 0,
    Stab = 1 << 1,
    WallDive = 1 << 2,
    Pogo = 1 << 3,
    Slice = 1 << 4,
    BombClone = 1 << 5,
    DoubleJump = 1 << 6,
    TpInspiration = 1 << 7
}
