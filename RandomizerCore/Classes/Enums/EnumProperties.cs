using System.Collections.Generic;

namespace RandomizerCore.Classes.Enums;

public static class EnumProperties
{
    public static Dictionary<Skips, KeyItems> SkipItems { get; private set; } = new()
    {
        { Skips.BlombCloneTp, KeyItems.BombClone & KeyItems.TpInspiration },
        { Skips.BlombCloneMidairPogo, KeyItems.BombClone & KeyItems.Pogo },
        { Skips.EnemyPogo, KeyItems.Pogo },
        { Skips.EnemySlice, KeyItems.Slice },
        { Skips.EnemyKnockback, KeyItems.None },
        { Skips.SpikeStand, KeyItems.None },
    };

    public static Dictionary<CollectableItems, int> CollectableCounts = new()
    {
        { CollectableItems.Cousin, 4 },
        { CollectableItems.CarnivalEye, 3 },
        { CollectableItems.FoundryPipe, 2 },
        { CollectableItems.Tear, 4 },
    };
}
