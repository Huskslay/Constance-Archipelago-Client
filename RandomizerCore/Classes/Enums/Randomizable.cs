using System;

namespace RandomizerCore.Classes.Enums;

[Flags]
public enum Randomizable : ulong
{
    None = 0,
    Canvas = 1UL << 0,
    Chest = 1UL << 1,
    Inspiration = 1UL << 2,
    CurrencyFlower = 1UL << 3,
    LightStone = 1UL << 4,
    Cousin = 1UL << 5,
    ShopItem = 1UL << 6,
    Pickup = 1UL << 7,
    CarnivalEye = 1UL << 8,
    FoundryPipe = 1UL << 9,
    Tear = 1UL << 10
}
