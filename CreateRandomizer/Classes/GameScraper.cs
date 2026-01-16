using Constance;
using FileHandler.Classes;
using RandomizerCore.Classes.Data;
using RandomizerCore.Classes.Data.EntranceRules;
using RandomizerCore.Classes.Data.Saved;
using RandomizerCore.Classes.Data.Types.Entrances;
using RandomizerCore.Classes.Data.Types.Entrances.Types;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Locations.Types;
using RandomizerCore.Classes.Data.Types.Locations.Types.Desposits;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace CreateRandomizer.Classes;

public static class GameScraper
{
    private static List<string> hasShrines;

    public static void Scrape()
    {
        Plugin.I.StartCoroutine(ScrapeLevels());
    }

    private static IEnumerator ScrapeLevels()
    {
        Plugin.FindFirstObjectByType<CConTimelinePlayerController>().enabled = false; // Throws errors
        CConPlayerEntity player = Plugin.FindFirstObjectByType<CConPlayerEntity>();

        // Scenes
        hasShrines = ["Prod_V01:cp_Prod_V01_a15fffec-931b-4c37-8dac-6f4c1e742549"];
        foreach (string scene in SceneHandler.scenes)
            yield return ScrapeLevel(scene, player);
        FileSaveLoader.TrySaveClassToJson(hasShrines, ["Output"], "shrines");

        // Flashbacks
        foreach (string scene in SceneHandler.flashbackScenes)
            yield return ScrapeFlashback(scene, player);
    }


    private static IEnumerator ScrapeLevel(string scene, CConPlayerEntity player)
    {
        Plugin.Logger.LogMessage($"~~~~~~~~~~~~~\nLoading {scene}");

        // Level
        ConLevelId levelId = new(scene);
        yield return RegionHandler.LoadRegion(levelId, player: player);
        Region region = new(scene);

        // Shrines
        CConMeditationPointEntity shrine = Plugin.FindFirstObjectByType<CConMeditationPointEntity>();
        if (shrine != null) hasShrines.Add(scene + ":" + shrine.checkPoint.checkPointId.StringValue);

        // Set
        region.entrances = GetEntrances(region);
        region.locations = CreateLocations(region);

        // Saved Data
        MakeSavedData<AEntranceSavedData, AEntrance, EntranceHandler>(region, skipDupeNames: true, EntranceHandler.I, region.entrances);
        MakeSavedData<ALocationSavedData, ALocation, LocationHandler>(region, skipDupeNames: false, LocationHandler.I, region.locations);

        // Save
        RegionHandler.I.Save(region);
    }

    private static IEnumerator ScrapeFlashback(string scene, CConPlayerEntity player)
    {
        Plugin.Logger.LogMessage($"~~~~~~~~~~~~~\nLoading {scene}");

        // Level
        ConLevelId levelId = new(scene);
        yield return RegionHandler.LoadRegion(levelId, player);

        // Get exit region
        CConLevel_Flashback level = Plugin.FindFirstObjectByType<CConLevel_Flashback>();
        if (!level.exitToCheckPoint.TryExtractLevelId(out ConLevelId exit))
        {
            Plugin.Logger.LogError("Could not get exit id");
            yield break;
        }
        if (!RegionHandler.I.TryGetFromId(exit, out Region exitRegion))
        {
            Plugin.Logger.LogError("Could not get exit region");
            yield break;
        }

        // Add tear to exit region
        TearLocation tearLocation = new(level.tearUnlock, exitRegion);
        exitRegion.locations.Add(tearLocation.GetName());
        RegionHandler.I.Save(exitRegion);
    }





    public static List<string> CreateLocations(Region region)
    {
        List<string> locations = [];

        locations.AddRange(
            GetLocations<CConUnlockAbilityCanvas>(region, "Canvases",
                CanvasLocation.FindInactive, (obj, reg) => new CanvasLocation(obj, reg))
        );
        locations.AddRange(
            GetLocations<CConChestEntity>(region, "Chests",
                ChestLocation.FindInactive, (obj, reg) => new ChestLocation(obj, reg))
        );
        locations.AddRange(
            GetLocations<CConInspirationTriggerBehaviour>(region, "Inspirations",
                InspirationLocation.FindInactive, (obj, reg) => new InspirationLocation(obj, reg))
        );
        locations.AddRange(
            GetLocations<CConCurrencyDepositEntity>(region, "Deposits",
                DepositLocationFactory.FindInactive, DepositLocationFactory.CreateDepositLocation)
        );
        locations.AddRange(
            GetLocations<CConBehaviour_LostShopKeeper>(region, "Cousins",
                CousinLocation.FindInactive, (obj, reg) => new CousinLocation(obj, reg))
        );
        locations.AddRange(
            GetLocations<CConEntityDropBehaviour_TouchToCollect>(region, "Pickups",
                PickupLocation.FindInactive, (obj, reg) => new PickupLocation(obj, reg))
        );
        locations.AddRange(
            GetLocations<CConCarnivalHeadlightEye>(region, "Carnival Eye",
                CarnivalEyeLocation.FindInactive, (obj, reg) => new CarnivalEyeLocation(obj, reg))
        );
        locations.AddRange(
            GetLocations<ConFoundryPaintPipe_Valve>(region, "Foundry Pipe",
                FoundryPipeLocation.FindInactive, (obj, reg) => new FoundryPipeLocation(obj, reg))
        );

        if (region.GetName() == "J04") locations.AddRange(CreateShopLocations(region));


        return locations;
    }
    private static List<string> CreateShopLocations(Region region)
    {
        List<string> locations = [];
        foreach (SConCollectable_ShopItem item in ConMonoBehaviour.SceneRegistry.Collectables.ShopItems)
            locations.Add(new ShopItemLocation(item, region).GetName());
        return locations;
    }



    private static List<string> GetEntrances(Region region)
    {
        List<string> entrances = [];
        CConLevel_Adventure level = Plugin.FindFirstObjectByType<CConLevel_Adventure>();

        foreach (CConTeleportPoint tp_point in
            Plugin.FindObjectsByType<CConTeleportPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            bool completed = false;
            foreach (SConLevelInfo.TransitionInfo info in level.LevelInfo.transitionsTo)
            {
                string region1 = info.id.Level1.StringValue.Replace("Prod_", "");
                string region2 = info.id.Level2.StringValue.Replace("Prod_", "");

                if (region1 == region.name || region2 == region.name)
                {
                    completed = true;

                    string connectionRegion = region1 == region.name ? region2 : region1;
                    string chpt = region1 == region.name ? info.id.checkPoint2.StringValue : info.id.checkPoint1.StringValue;
                    string connectionChpt = region1 == region.name ? info.id.checkPoint1.StringValue : info.id.checkPoint2.StringValue;

                    string name = FileSaveLoader.FourDigitHash(chpt);
                    string connectionName = FileSaveLoader.FourDigitHash(connectionChpt);
                    TeleportEntrance entrance = new(name, region, connectionName, connectionRegion, tp_point.teleportTo);
                    entrances.Add(entrance.GetName());

                    break;
                }
            }
            if (!completed)
            {
                Plugin.Logger.LogWarning($"Could not find connection for transtion: {tp_point.name}");
                string name = FileSaveLoader.FourDigitHash(tp_point.teleportTo.StringValue);
                TeleportEntrance entrance = new(name, region, null, null, tp_point.teleportTo);
                entrances.Add(entrance.GetName());
            }
        }

        CConElevatorBehaviour elevator = Plugin.FindFirstObjectByType<CConElevatorBehaviour>(FindObjectsInactive.Exclude);
        if (elevator != null)
        {
            ElevatorEntrance entrance = new(region);
            entrances.Add(entrance.GetName());
        }

        Plugin.Logger.LogMessage($"Found: {entrances.Count} Entrances, Elevator: {elevator != null}");
        return entrances;
    }

    private static void MakeSavedData<T1, T2, T3>(Region region, bool skipDupeNames, T3 handler, List<string> owners)
        where T1 : EntranceRuleSavedData where T2 : ISavedDataOwner<T1> where T3 : SavedDataOwnerHandler<T2, T1>
    {
        foreach (string ownerName in owners)
        {
            T2 owner = handler.GetFromName(ownerName);
            T1 savedData = owner.GetSavedData();
            foreach (string entranceName in region.entrances)
            {
                if (skipDupeNames && entranceName == ownerName) continue;
                EntranceRule rule = savedData.entranceRules.Find(x => x.entrance == entranceName);
                if (rule == null)
                {
                    rule ??= new(entranceName);
                    savedData.entranceRules.Add(rule);
                }
                foreach (CollectableItems collectableItem in Enum.GetValues(typeof(CollectableItems)))
                {
                    CollectableItemEntry entry = rule.collectableItems.Find(x => x.item == collectableItem);
                    if (entry != null) continue;
                    rule.collectableItems.Add(new(collectableItem));
                }
            }
            owner.SetSavedData(savedData);
        }
    }
    public static List<string> GetLocations<T1>(Region region, string name, FindObjectsInactive findInactive, Func<T1, Region, ALocation> constructor)
        where T1 : MonoBehaviour
    {
        List<string> locations = [];
        foreach (T1 obj in Plugin.FindObjectsByType<T1>(findInactive, FindObjectsSortMode.None))
        {
            ALocation location = constructor(obj, region);
            locations.Add(location.GetName());
        }
        Plugin.Logger.LogMessage($"Found: {locations.Count} {name}");
        return locations;
    }
}
