using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Locations;
using UnityEngine.Events;

namespace RandomizerCore.Classes.Handlers;

public class RandomActionHandler
{
    // When loading into a rando, before arch connection and after
    public static UnityEvent preOnLoadRandoSave = new();
    public static UnityEvent onLoadRandoSave = new();
    // When checking a location
    public static UnityEvent<ALocation> onLocationCheck = new();
    // After recieving a location
    public static UnityEvent<ALocation> onLocationGet = new();
    // After getting an item
    public static UnityEvent<Item, string, bool> onItemGet = new();
}
