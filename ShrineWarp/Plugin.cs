using BepInEx;
using BepInEx.Logging;
using CheatMenu.Classes;
using FileHandler.Classes;
using HarmonyLib;
using ShrineWarp.Classes;
using ShrineWarp.Classes.Pages;
using UnityEngine;

namespace ShrineWarp;

[BepInDependency("CheatMenu", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("FileHandler", BepInDependency.DependencyFlags.HardDependency)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    public static ModGUI modGUI;
    public static Transform Transform { get; private set; }


    private void Awake()
    {
        Logger = base.Logger;
        Transform = transform;

        InitializeModGUI();
        ShrineDataHandler.Init();

        Harmony patcher = new("HarmonyPatcher");
        patcher.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitializeModGUI()
    {
        modGUI = ModGUI.Create(KeyCode.Home, windowRect: new(Screen.currentResolution.width - 300f, 140f, 200f, 600f), windowName: "Shrine Warp");
        modGUI.AddPage<SelectionPage>();
        modGUI.AddPage<OptionsPage>();

        GameDataActions.OnFileQuit.AddListener(() => { modGUI.ShowGUI = false; });
    }
}
