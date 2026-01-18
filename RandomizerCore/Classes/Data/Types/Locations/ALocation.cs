using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomizerCore.Classes.Data.Types.Locations;

[Serializable]
public abstract class ALocation : ISavedDataOwner<ALocationSavedData>
{
    public string name;
    public string type;
    public string region;
    public string goName = "";
    public string baseItem = "";

    public virtual void Init() { }
    public string GetName() => $"{type}-{name}-{region}-Location";
    public abstract Randomizable GetRandoType();
    public Item BaseItem => ItemHandler.I.GetFromName(baseItem);


    public ALocation(string name, Region region, string goName)
    {
        this.name = name;
        this.type = GetRandoType().ToString();
        this.region = region.name;
        this.goName = goName;

        SetSavedData(new(GetName()));
    }


    public static void BasicPatch<T1, T2>(List<T1> objects, Region region, Action<T1, ALocation> onEach = null)
        where T1 : MonoBehaviour where T2 : ALocation
    {
        List<T2> locations = [];
        foreach (string locationName in region.locations)
        {
            ALocation location = LocationHandler.I.GetFromName(locationName);
            if (location is T2 specificLocation) locations.Add(specificLocation);
        }
        if (locations.Count > 0) BasicPatch(objects, locations, onEach);
    }
    public static void BasicPatch<T1, T2>(List<T1> objects, List<T2> locations, Action<T1, ALocation> onEach = null)
        where T1 : MonoBehaviour where T2 : ALocation
    {
        foreach (T1 obj in objects)
        {
            ALocation location = locations.Find(location => location.goName == obj.name);
            if (location == null)
            {
                Plugin.Logger.LogWarning($"Could not find a location for object: {obj.name}");
                return;
            }
            obj.gameObject.AddComponent<LocationComponent>().Set(location);
            onEach?.Invoke(obj, location);
        }
    }


    [NonSerialized]
    private ALocationSavedData savedData;
    public ALocationSavedData GetSavedData() { return savedData; }
    public void SetSavedData(ALocationSavedData savedData, bool save = true)
    {
        if (save) LocationHandler.I.Save(savedData);
        this.savedData = savedData;
    }
}
