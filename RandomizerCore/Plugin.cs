using BepInEx;
using BepInEx.Logging;
using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Archipelago;
using RandomizerCore.Classes.Data;
using RandomizerCore.Classes.Data.Saved;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Handlers;
using RandomizerCore.Classes.Handlers.Files;
using RandomizerCore.Classes.Handlers.Messages;
using RandomizerCore.Classes.Handlers.State;
using UnityEngine.InputSystem;

namespace RandomizerCore;

[BepInDependency("FileHandler", BepInDependency.DependencyFlags.HardDependency)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    public static Plugin I;

    private void Awake()
    {
        I = this;
        Logger = base.Logger;

        CollectableHandler.Init();
        RandomFilesHandler.Init();
        RandomStateHandler.Init();
        SavedDataOwnerHandler<ISavedDataOwner<SavedData>, SavedData>.InitAll();
        MessageHandler.Init();

        Harmony patcher = new("HarmonyPatcher");
        patcher.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Update()
    {
        MessageHandler.Update();
        MultiClient.Update();
    }

    private void OnApplicationQuit()
    {
        MultiClient.Disconnect();
    }
}
