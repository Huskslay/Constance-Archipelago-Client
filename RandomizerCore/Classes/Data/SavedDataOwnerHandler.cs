using FileHandler.Classes;
using RandomizerCore.Classes.Data.Saved;
using RandomizerCore.Classes.Data.Types.AItems;
using RandomizerCore.Classes.Data.Types.AItems.Types;
using RandomizerCore.Classes.Data.Types.Entrances;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Regions;
using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Data;

public abstract class SavedDataOwnerHandler<T1, T2> where T1 : ISavedDataOwner<T2> where T2 : SavedData
{
    public List<string> FolderPath => ["Data", GetName()];
    public List<string> SavedDataFolderPath => ["Saved Data", GetName()];

    public abstract string GetName();

    public Dictionary<string, T1> dataOwners;


    public static void InitAll()
    {
        ActionItem.Reset();
        new RegionHandler().Init();
        new LocationHandler().Init();
        new ItemHandler().Init();
        new AItemHandler().Init();
        new EntranceHandler().Init();
    }


    public virtual void Init()
    {
        LoadAll();
        LoadSavedDatas();
    }

    private void LoadAll()
    {
        Plugin.Logger.LogMessage($"Loading {GetName()}");
        dataOwners = [];
        HashSet<string> names = [];

        LoadAll((dataOwner) => OwnerIntiationAction(ref names, dataOwner));
        Plugin.Logger.LogMessage($"{dataOwners.Count} {GetName()} found");
    }
    private void OwnerIntiationAction(ref HashSet<string> names, T1 dataOwner)
    {
        if (dataOwner == null) Plugin.Logger.LogError($"Null {GetName()} found");
        else
        {
            if (names.Contains(dataOwner.GetName()))
                Plugin.Logger.LogError($"{GetName()} name '{dataOwner.GetName()}' is not unique");
            else names.Add(dataOwner.GetName());

            dataOwner.Init();
            dataOwners.Add(dataOwner.GetName(), dataOwner);
        }
    }
    protected abstract void LoadAll(Action<T1> initiate);


    private void LoadSavedDatas()
    {
        List<T2> savedDatas = FileSaveLoader.LoadClassesJson<T2>(SavedDataFolderPath);

        HashSet<string> names = [];
        int foundOwnerCount = 0;
        foreach (T2 savedData in savedDatas)
        {
            if (savedData == null) Plugin.Logger.LogError($"Null saved data found");
            else
            {
                if (names.Contains(savedData.connection))
                    Plugin.Logger.LogError($"Connection '{savedData.connection}' is not unique");
                names.Add(savedData.connection);

                T1 owner = GetFromName(savedData.connection);
                if (owner == null)
                {
                    Plugin.Logger.LogError($"{savedData.connection} saved data can not find connection ");
                    continue;
                }
                else
                {
                    owner.SetSavedData(savedData, save: false);
                    foundOwnerCount++;
                }
            }
        }

        Plugin.Logger.LogMessage($"{savedDatas.Count} saved datas found");
        if (foundOwnerCount < dataOwners.Count) Plugin.Logger.LogError($"{dataOwners.Count - foundOwnerCount} owners do not contain saved data");
    }



    public void Save(T1 data)
    {
        FileSaveLoader.TrySaveClassToFile(data, FolderPath, data.GetName());
        if (dataOwners.ContainsKey(data.GetName())) dataOwners[data.GetName()] = data;
        else dataOwners.Add(data.GetName(), data);
    }
    public void Save(T2 data, T1 owner)
    {
        FileSaveLoader.TrySaveClassToJson(data, SavedDataFolderPath, owner.GetName());
    }

    public T1 GetFromName(string name)
    {
        if (dataOwners.ContainsKey(name)) return dataOwners[name];
        return default;
    }
}
