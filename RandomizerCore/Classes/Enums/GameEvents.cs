using System;

namespace RandomizerCore.Classes.Enums;

[Flags]
public enum GameEvents : ulong
{
    None = 0,
    ItemCamera =            1UL << 00,
    BossCubicus =           1UL << 01, // Smasher
    BossPalettus =          1UL << 02,
    BossBrian =             1UL << 03, // BrainStoker
    BossAweKing =           1UL << 05,
    BossHighPatia =         1UL << 06, // MothQueen
    BossChaseNemesis =      1UL << 07,
    BossJester =            1UL << 08, // Joker
    BossJesterEncore =      1UL << 09, // JokerInvisible
    BossManipulator =       1UL << 10, // BossBalloon
    BossManipulatorEncore = 1UL << 11, // BulletHellJuggler
    BossCornelis =          1UL << 12,
    BossLordKoba =          1UL << 13, // SlimeNemesis
    BossSirBarfalot =       1UL << 14, // PukeyBoy
    BossPuppetCorruption =  1UL << 15,
    BossPuppetKungfu =      1UL << 16,
    BossPuppetStrings =     1UL << 17,
    BossPuppetMaster =      1UL << 18,
}
