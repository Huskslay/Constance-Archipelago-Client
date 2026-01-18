using System.Collections.Generic;

namespace RandomizerCore.Classes.Enums;

public static class EnumProperties
{
    public static Dictionary<Skips, KeyItems> SkipItems { get; private set; } = new()
    {
        { Skips.BlombCloneTp,           KeyItems.BombClone & KeyItems.TpInspiration },
        { Skips.BlombCloneMidairPogo,   KeyItems.BombClone & KeyItems.Pogo },
        { Skips.EnemyPogo,              KeyItems.Pogo },
        { Skips.EnemySlice,             KeyItems.Slice },
        { Skips.EnemyKnockback,         KeyItems.None },
        { Skips.SpikeStand,             KeyItems.None },
    };

    public static Dictionary<CollectableItems, int> CollectableCounts = new()
    {
        { CollectableItems.Cousin,      4 },
        { CollectableItems.CarnivalEye, 3 },
        { CollectableItems.FoundryPipe, 2 },
        { CollectableItems.Tear,        4 },
    };

    public static Dictionary<GameEvents, string> EventRegions = new()
    {
        { GameEvents.ItemCamera,            "F01" },
        { GameEvents.BossCubicus,           "F21" },
        { GameEvents.BossPalettus,          "F27" },
        { GameEvents.BossBrian,             "F28" },
        { GameEvents.BossAweKing,           "A16" },
        { GameEvents.BossHighPatia,         "A27" },
        { GameEvents.BossChaseNemesis,      "A07" },
        { GameEvents.BossJester,            "C90" },
        { GameEvents.BossJesterEncore,      "C94" },
        { GameEvents.BossManipulator,       "C95" },
        { GameEvents.BossManipulatorEncore, "C96" },
        { GameEvents.BossCornelis,          "C01" },
        { GameEvents.BossLordKoba,          "V11" },
        { GameEvents.BossSirBarfalot,       "V18" },
        { GameEvents.BossPuppetCorruption,  "VD01" },
        { GameEvents.BossPuppetKungfu,      "VD02" },
        { GameEvents.BossPuppetStrings,     "VD03" },
        { GameEvents.BossPuppetMaster,      "VD03" },
    };
}
