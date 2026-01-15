using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Data.Types.Regions;

[Serializable]
public class Region : ISavedDataOwner<RegionSavedData>
{
    public string name;
    public List<string> entrances = [];
    public List<string> locations = [];

    public void Init() { }
    public string GetName() => $"{name}";


    public Region(string name)
    {
        this.name = name.Replace("Prod_", "");

        SetSavedData(new(GetName()));
    }


    [NonSerialized]
    private RegionSavedData savedData;
    public RegionSavedData GetSavedData() { return savedData; }
    public void SetSavedData(RegionSavedData savedData, bool save = true)
    {
        if (save) RegionHandler.I.Save(savedData, this);
        this.savedData = savedData;
    }
}
