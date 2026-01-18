using System;

namespace RandomizerCore.Classes.Enums;

[Flags]
public enum Skips : ulong
{
    None = 0,
    BlombCloneTp = 1UL << 0,
    BlombCloneMidairPogo = 1UL << 1,
    EnemyPogo = 1UL << 2,
    EnemySlice = 1UL << 3,
    EnemyKnockback = 1UL << 4,
    SpikeStand = 1UL << 5
}
