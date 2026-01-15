using Newtonsoft.Json;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Handlers.State;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Handlers.Files;

[System.Serializable]
public class RandomFile
{
    public string url;
    public int port;
    public string slotName;

    public List<bool> obtainedLocations;
    public List<bool> obtainedItems;
    public List<string> disconnectLocations;

    [JsonConstructor]
    public RandomFile(string url, int port, string slotName, List<bool> obtainedLocations, List<bool> obtainedItems, List<string> disconnectLocations)
    {
        this.url = url;
        this.port = port;
        this.slotName = slotName;

        this.obtainedLocations = obtainedLocations;
        this.obtainedItems = obtainedItems;
        this.disconnectLocations = disconnectLocations;
    }

    public RandomFile(string url, int port, string slotName, List<RandomStateElement<ALocation>> locationElements,
                        List<RandomStateElement<Item>> itemElements, List<string> disconnectLocations)
    {
        this.url = url;
        this.port = port;
        this.slotName = slotName;

        obtainedLocations = [];
        foreach (RandomStateElement<ALocation> element in locationElements)
            obtainedLocations.Add(element.hasObtainedSource);

        obtainedItems = [];
        foreach (RandomStateElement<Item> element in itemElements)
            obtainedItems.Add(element.hasObtainedSource);
        this.disconnectLocations = disconnectLocations;
    }
}
