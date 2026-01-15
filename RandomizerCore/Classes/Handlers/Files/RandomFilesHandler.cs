using Archipelago.MultiClient.Net;
using Constance;
using FileHandler.Classes;
using RandomizerCore.Classes.Archipelago;
using RandomizerCore.Classes.Handlers.State;
using RandomizerCore.Patches.Files;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace RandomizerCore.Classes.Handlers.Files;

public static class RandomFilesHandler
{
    private static bool loaded = false;
    public static SlotData slotData;

    private static readonly List<string> folders = ["Mods"];
    private static readonly string fileName = "Rando Data";

    public static void Init()
    {
        GameDataActions.OnFileSave.AddListener(OnFileSave);
        GameDataActions.OnFileDelete.AddListener(OnFileDelete);
        GameDataActions.OnFileQuit.AddListener(() => OnFileQuit(CConUiSaveSlot_Patch.latestSave));
    }

    public static void Connect(string url, int port, string slot, string password, Action<string> showErrorOnFail)
    {
        LoginResult result = MultiClient.Connect(url, port, slot, password, out string errorMessage, out SlotData slotData);
        if (!result.Successful)
        {
            showErrorOnFail(errorMessage);
            return;
        }

        RandomFilesHandler.slotData = slotData;
        RandomMenuHandler.LoadSave();
    }

    private static void OnFileSave(ConSaver saver, ConSaveStateId id)
    {
        if (!RandomStateHandler.Randomized) return;

        Plugin.Logger.LogMessage($"Saved file at slot: {id.StringValue}");
        RandomFile file = new(MultiClient.Url, MultiClient.Port, MultiClient.SlotName,
            RandomStateHandler.LocationElements, RandomStateHandler.ItemElements, RandomStateHandler.DisconnectLocations);
        FileSaveLoader.TrySaveClassToJson(file, folders, fileName, id);
    }

    private static void OnFileDelete(ConSaver saver, ConSaveStateId id)
    {
        Plugin.Logger.LogMessage($"Deleted file at slot: {id.StringValue}");
        FileSaveLoader.DeleteClassInJson(folders, fileName, id);
    }


    public static RandomFile GetRandomFile(ConSaveStateId id)
    {
        return FileSaveLoader.LoadClassFromJson<RandomFile>(folders, fileName, id);
    }
    public static void OnFileCreate(ConSaveStateId id)
    {
        Plugin.Logger.LogMessage($"Created file at slot: {id.StringValue} | Randomizing: {RandomMenuHandler.randomizing}");
        if (!RandomMenuHandler.randomizing) return;

        RandomStateHandler.Randomize();
        Loaded();
    }
    public static void OnFileLoad(ConSaveStateId id)
    {
        Plugin.Logger.LogMessage($"Loaded file at slot: {id.StringValue} | Randomizing: {RandomMenuHandler.randomizing}");
        if (!RandomMenuHandler.randomizing) return;

        RandomFile file = GetRandomFile(id);
        RandomStateHandler.Randomize(file);
        Loaded();
    }
    private static void Loaded()
    {
        RandomActionHandler.preOnLoadRandoSave.Invoke();

        loaded = true;

        RandomActionHandler.onLoadRandoSave.Invoke();
    }

    public static void OnFileQuit(ConSaveStateId id)
    {
        if (!loaded) return;
        Plugin.Logger.LogMessage($"Quit file at slot: {id.StringValue}");
        MultiClient.Disconnect();
        RandomStateHandler.UnRandomize();
        loaded = false;
    }

    public static void ForceQuit()
    {
        MultiClient.Disconnect();
        RandomStateHandler.UnRandomize();
        ((CConSceneRegistry)CConSceneRegistry.Instance).OnApplicationQuit();
        SceneManager.LoadScene(1);
    }
}
