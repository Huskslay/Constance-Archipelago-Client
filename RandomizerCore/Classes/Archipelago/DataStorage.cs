using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Constance;
using Newtonsoft.Json.Linq;
using RandomizerCore.Classes.Enums;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Archipelago;

public static class DataStorage
{
    private static readonly string foundCousinsKey = "Found Cousins";
    private static readonly string foundCollectablesKey = "Found Collectables";
    private static readonly string foundEventsKey = "Found Events";

    private static ArchipelagoSession session;

    public static void Initialize(ArchipelagoSession apSession)
    {
        session = apSession;

        session.DataStorage[foundCousinsKey].Initialize(new List<string>());
        session.DataStorage[foundCollectablesKey].Initialize(JObject.FromObject(new Dictionary<CollectableItems, List<string>>()));
        long defaultLong = 0;
        session.DataStorage[foundEventsKey].Initialize(JObject.FromObject(defaultLong));
    }

    public static List<string> GetFoundCousins() => session.DataStorage[Scope.Slot, foundCousinsKey].To<List<string>>();
    public static Dictionary<CollectableItems, List<string>> GetFoundCollectables() =>
        session.DataStorage[Scope.Slot, foundCollectablesKey].To<Dictionary<CollectableItems, List<string>>>();
    public static GameEvents GetFoundEvents() => (GameEvents)session.DataStorage[Scope.Slot, foundCollectablesKey].To<ulong>();

    public static void AddFoundCousin(ConLevelId id)
    {
        List<string> foundCousins = GetFoundCousins();
        foundCousins ??= [];
        if (!foundCousins.Contains(id.StringValue)) foundCousins.Add(id.StringValue);
        session.DataStorage[Scope.Slot, foundCousinsKey] = foundCousins;
    }
    public static int FoundCollectable(CollectableItems item, string itemName)
    {
        Dictionary<CollectableItems, List<string>> foundCollectables = GetFoundCollectables();
        foundCollectables ??= [];
        if (!foundCollectables.ContainsKey(item)) foundCollectables.Add(item, []);

        if (!foundCollectables[item].Contains(itemName)) foundCollectables[item].Add(itemName);
        session.DataStorage[Scope.Slot, foundCollectablesKey] = JObject.FromObject(foundCollectables);

        return foundCollectables[item].Count;
    }
    public static void FoundEvent(GameEvents events)
    {
        GameEvents foundEvents = GetFoundEvents();
        foundEvents |= events;
        session.DataStorage[Scope.Slot, foundEventsKey] = JObject.FromObject(foundEvents);
    }
}
