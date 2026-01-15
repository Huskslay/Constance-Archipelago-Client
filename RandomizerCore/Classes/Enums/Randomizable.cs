using System;

namespace RandomizerCore.Classes.Enums;

[Flags]
public enum Randomizable
{
    None = 0,
    Canvas = 1 << 0,
    Chest = 1 << 1,
    Inspiration = 1 << 2,
    CurrencyFlower = 1 << 3,
    LightStone = 1 << 4,
    Cousin = 1 << 5,
    ShopItem = 1 << 6,
    Pickup = 1 << 7,
    CarnivalEye = 1 << 8,
    FoundryPipe = 1 << 9,
    Tear = 1 << 10
}
