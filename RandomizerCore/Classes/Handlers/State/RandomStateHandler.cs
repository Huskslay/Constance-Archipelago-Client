using Constance;
using RandomizerCore.Classes.Archipelago;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Enums;
using RandomizerCore.Classes.Handlers.Files;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Handlers.State;

public static class RandomStateHandler
{
    public static bool Randomized => locationState != null && itemState != null;
    public static bool readyForItems = false;

    public static List<RandomStateElement<ALocation>> LocationElements => [.. locationState.Values];
    public static List<RandomStateElement<Item>> ItemElements => [.. itemState.Values];

    private static Dictionary<ALocation, RandomStateElement<ALocation>> locationState = null;
    private static Dictionary<Item, RandomStateElement<Item>> itemState = null;

    public static List<string> DisconnectLocations { get; private set; }

    public static void Init()
    {
        RandomActionHandler.onLocationGet.AddListener(OnLocationGot);
        RandomActionHandler.onItemGet.AddListener(OnItemGet);
    }

    public static void Randomize(RandomFile file = null)
    {
        if (Randomized)
        {
            Plugin.Logger.LogError($"Attempted to randomize when already randod");
            return;
        }
        Plugin.Logger.LogMessage("Randomizing");

        AddToDict(ref locationState, [.. LocationHandler.I.dataOwners.Values], file?.obtainedLocations, "Location");
        AddToDict(ref itemState, [.. ItemHandler.I.dataOwners.Values], file?.obtainedItems, "Item");

        DisconnectLocations = [];
        if (file != null)
        {
            foreach (string locationName in file.disconnectLocations) DisconnectLocations.Add(locationName);
            Plugin.Logger.LogMessage($"{file.disconnectLocations} IADHO:W");
        }
    }
    private static void AddToDict<T>(ref Dictionary<T, RandomStateElement<T>> dict, List<T> list, List<bool> obtainedList, string name)
    {
        dict = [];
        if (obtainedList != null && list.Count != obtainedList.Count)
        {
            Plugin.Logger.LogError($"{name} count ({list.Count}) != File {name} count ({obtainedList.Count})");
            obtainedList = null;
        }
        for (int i = 0; i < list.Count; i++)
            dict.Add(list[i], new(list[i], obtainedList != null && obtainedList[i]));
    }

    public static void UnRandomize()
    {
        if (!Randomized)
        {
            Plugin.Logger.LogError($"Attempted to un-randomize when not randod");
            return;
        }
        Plugin.Logger.LogMessage("Unrandomizing");

        readyForItems = false;
        locationState = null;
        itemState = null;
    }


    public static bool HasObtainedLocation(ALocation location)
    {
        return locationState[location].hasObtainedSource;
    }

    public static void UnObtainLocation(ALocation location)
    {
        RandomStateElement<ALocation> element = locationState[location];
        element.hasObtainedSource = false;
    }


    // Called by location when checked
    public static void CheckLocation(ALocation location)
    {
        Plugin.Logger.LogMessage($"Checked location {location.GetName()}");
        RandomStateElement<ALocation> element = locationState[location];

        if (element.hasObtainedSource)
            Plugin.Logger.LogWarning($"Could not check location `{location.GetName()}` as it has already been obtained");
        else RandomActionHandler.onLocationCheck.Invoke(location);
    }
    public static void AchieveEvents(GameEvents events)
    {
        DataStorage.FoundEvent(events);
    }


    // Called by MultiClient when a location is recieved from the ap
    public static void GetLocation(string locationName)
    {
        ALocation location = LocationHandler.I.GetFromName(locationName);
        if (location == null)
        {
            Plugin.Logger.LogError($"Could not find ALocation of name: {locationName}");
            return;
        }

        RandomStateElement<ALocation> element = locationState[location];
        if (element.hasObtainedSource)
            Plugin.Logger.LogWarning($"Could not get location `{location.GetName()}` as it has already been obtained");
        else RandomActionHandler.onLocationGet.Invoke(location);
    }
    // onLocationGet to mark the source as obtained
    private static void OnLocationGot(ALocation location)
    {
        RandomStateElement<ALocation> element = locationState[location];
        Plugin.Logger.LogMessage($"Got location {element.source.GetName()}");
        element.hasObtainedSource = true;
    }

    // Called by MultiClient when an item is recieved from the ap
    public static void GetItem(string itemName, string playerName, bool isCurrentPlayer)
    {
        Plugin.Logger.LogMessage($"Gettting item {itemName} from {playerName}");

        Item item = ItemHandler.I.GetFromName(itemName);
        if (item == null)
        {
            Plugin.Logger.LogError($"Could not find item of name: {itemName}");
            return;
        }

        RandomStateElement<Item> element = itemState[item];
        if (element.hasObtainedSource)
            Plugin.Logger.LogWarning($"Could not get item `{item.GetName()}` as it has already been obtained");
        else RandomActionHandler.onItemGet.Invoke(item, playerName, isCurrentPlayer);
    }
    // onItemGet to give the item
    private static void OnItemGet(Item item, string playerName, bool isCurrentPlayer)
    {
        Plugin.Logger.LogMessage($"Got item {item.GetName()} from {playerName}");

        IConPlayerEntity player = ConMonoBehaviour.SceneRegistry.PlayerOne;
        IConPlayerInventory inventory = ConMonoBehaviour.SceneRegistry.Inventory;
        item.GiveToPlayer(player, inventory);

        RandomStateElement<Item> element = itemState[item];
        element.hasObtainedSource = true;
    }
}
