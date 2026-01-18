using BepInEx;
using BepInEx.Logging;
using CheatMenu.Classes;
using Constance;
using CreateRandomizer.Classes;
using CreateRandomizer.Classes.Pages;
using CreateRandomizer.Classes.Pages.Entrances;
using CreateRandomizer.Classes.Pages.Locations;
using CreateRandomizer.Classes.Pages.Regions;
using HarmonyLib;
using RandomizerCore.Classes.Handlers;
using UnityEngine.InputSystem;

namespace CreateRandomizer;

[BepInDependency("FileHandler", BepInDependency.DependencyFlags.HardDependency)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    public static Plugin I;
    private ModGUI modGUI;

    private void Start()
    {
        I = this;
        Logger = base.Logger;

        InitModGUI();
        SceneHandler.Init();

        Harmony patcher = new("HarmonyPatcher");
        patcher.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitModGUI()
    {
        modGUI = ModGUI.Create(UnityEngine.KeyCode.None, windowName: "Create Randomizer");
        modGUI.AddPage<RegionsPage>();
        modGUI.AddPage<LocationsRegionPage>();
        modGUI.AddPage<EntrancesRegionPage>();
        modGUI.AddPage<CheatPage>();
    }

    private void Update()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            GameScraper.Scrape();
        }
        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            DataConverter.Convert();
        }
        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            modGUI.ShowGUI = !modGUI.ShowGUI;
        }

        // Fly
        if (Keyboard.current.f9Key.wasPressedThisFrame)
            ConDebugFlags.DebugFly = !ConDebugFlags.DebugFly;

        // Invulnerability
        if (Keyboard.current.f8Key.wasPressedThisFrame)
            ConDebugFlags.DebugInvulnerability = !ConDebugFlags.DebugInvulnerability;

        // Give all abilities and inspirations
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            CollectableHandler.TrueInit();
            IConPlayerInventory inventoryManager = ConMonoBehaviour.SceneRegistry.Inventory;
            IConPlayerEntity player = ConMonoBehaviour.SceneRegistry.PlayerOne;

            string[] abilities = [
                "slice", "wallDive", "dash", "doubleJump", "pogo", "stab", "bombClone", "fridaMask"
            ];

            foreach (string ability in abilities)
                inventoryManager.Collect(player, CollectableHandler.dict[CollectableHandler.nameDict[ability]], 1);
            foreach (SConCollectable_InspirationDrawing inspiration in CollectableHandler.inspirationCollectables)
                inventoryManager.Collect(player, inspiration, 1);
        }

        // Heal
        if (Keyboard.current.f4Key.wasPressedThisFrame)
        {
            IConPlayerEntity player = ConMonoBehaviour.SceneRegistry.PlayerOne;
            player.HealFully();
            player.RefillPaint();
        }
    }
}
