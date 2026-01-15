using System;

namespace RandomizerCore.Classes.Enums;

[Flags]
public enum Skips : ulong
{
    None = 0,
    BlombCloneTp = 1 << 0,
    BlombCloneMidairPogo = 1 << 1,
    EnemyPogo = 1 << 2,
    EnemySlice = 1 << 3,
    EnemyKnockback = 1 << 4,
    SpikeStand = 1 << 5
}
