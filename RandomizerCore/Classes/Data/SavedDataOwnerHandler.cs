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
using System.Linq;

namespace RandomizerCore.Classes.Data;

public abstract class SavedDataOwnerHandler<T1, T2> where T1 : ISavedDataOwner<T2> where T2 : SavedData
{
    public List<string> FolderPath => ["Data"];
    public List<string> SavedDataFolderPath => ["Saved Data"];

    public abstract string GetName();

    public Dictionary<string, T1> dataOwners;
    public List<T2> savedDatas;


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
        Plugin.Logger.LogMessage($"Loading {GetName()}");
        LoadAll();
        LoadAllSavedDatas();
        foreach (T1 dataOwner in dataOwners.Values) OnEachLoaded(dataOwner);
    }

    private void LoadAll()
    {
        dataOwners = [];

        List<T1> dataOwnersList = FileSaveLoader.LoadClassFromFile<List<T1>>(FolderPath, GetName());
        if (dataOwnersList != null) LoadAll(dataOwnersList, (dataOwner) => dataOwner.GetName(), OwnerIntiationAction);

        Plugin.Logger.LogMessage($"{dataOwners.Count} {GetName()} found");
    }
    private void OwnerIntiationAction(T1 dataOwner)
    {
        dataOwner.Init();
        dataOwners.Add(dataOwner.GetName(), dataOwner);
    }

    private void LoadAllSavedDatas()
    {
        int foundOwnerCount = 0;

        savedDatas = FileSaveLoader.LoadClassFromJson<List<T2>>(SavedDataFolderPath, GetName());
        savedDatas ??= [];
        LoadAll(savedDatas, (savedData) => savedData.connection, (savedData) => SavedDataIntiationAction(savedData, ref foundOwnerCount));

        Plugin.Logger.LogMessage($"{savedDatas.Count} saved datas found");
        if (foundOwnerCount < dataOwners.Count) Plugin.Logger.LogError($"{dataOwners.Count - foundOwnerCount} owners do not contain saved data");
    }
    private void SavedDataIntiationAction(T2 savedData, ref int foundOwnerCount)
    {
        T1 owner = GetFromName(savedData.connection);
        if (owner == null)
        {
            Plugin.Logger.LogError($"{savedData.connection} saved data can not find connection ");
            return;
        }
        owner.SetSavedData(savedData, save: false);
        foundOwnerCount++;
    }

    private void LoadAll<T>(List<T> objects, Func<T, string> getName, Action<T> onEach)
    {
        HashSet<string> names = [];
        foreach (T obj in objects)
        {
            if (obj == null)
            {
                Plugin.Logger.LogError($"Null found");
                return;
            }
            string name = getName(obj);

            if (names.Contains(name)) Plugin.Logger.LogError($"Name '{name}' is not unique");
            names.Add(name);

            onEach(obj);
        }
    }


    public virtual void OnEachLoaded(T1 loaded)
    {

    }



    public void Save(T1 data, bool log = true)
    {
        if (dataOwners.ContainsKey(data.GetName())) dataOwners[data.GetName()] = data;
        else NewData(data);
        FileSaveLoader.TrySaveClassToFile(dataOwners.Values.ToList(), FolderPath, GetName(), logSuccess: log);
    }
    public virtual void NewData(T1 data)
    {
        dataOwners.Add(data.GetName(), data);
    }
    public void Save(T2 data, bool log = true)
    {
        T2 current = savedDatas.Find(x => x.connection == data.connection);
        if (current != null) savedDatas.Remove(current);
        savedDatas.Add(data);
        FileSaveLoader.TrySaveClassToJson(savedDatas, SavedDataFolderPath, GetName(), logSuccess: log);
    }

    public T1 GetFromName(string name)
    {
        if (dataOwners.ContainsKey(name)) return dataOwners[name];
        return default;
    }
}
