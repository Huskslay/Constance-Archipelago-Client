using Constance;
using RandomizerCore.Classes.Data.Types.Locations.Types;
using RandomizerCore.Classes.Data.Types.Locations.Types.Desposits;
using RandomizerCore.Classes.Data.Types.Regions;
using System.Collections.Generic;
using UnityEngine;

namespace RandomizerCore.Classes.Components;

public static class RegionLivePatcher
{
    public static void PatchLoadedRegion()
    {
        IConPlayerEntity player = ConMonoBehaviour.SceneRegistry.PlayerOne;
        ConLevelId levelId = player.Level.Current;

        if (levelId.IsEmpty()) return;
        Plugin.Logger.LogMessage($"Patching {levelId.StringValue}");

        if (levelId.StringValue.Contains("Flashback")) PatchFlashback(levelId);
        else PatchNormalLevel(levelId);
    }

    private static void PatchFlashback(ConLevelId levelId)
    {
        CConLevel_Flashback flashback = Plugin.FindFirstObjectByType<CConLevel_Flashback>();

        if (flashback == null || !flashback.exitToCheckPoint.TryExtractLevelId(out ConLevelId exitRegion))
        {
            Plugin.Logger.LogMessage($"Could not find region from flashback at {levelId.StringValue}");
            return;
        }
        if (!RegionHandler.I.TryGetFromId(exitRegion, out Region region))
        {
            Plugin.Logger.LogMessage($"Could not find region from flashback exit at {exitRegion.StringValue}");
            return;
        }

        TearLocation.PatchLoadedLevel(flashback, region);
    }

    private static void PatchNormalLevel(ConLevelId levelId)
    {

        if (!RegionHandler.I.TryGetFromId(levelId, out Region region))
        {
            Plugin.Logger.LogMessage($"Could not find region for id {levelId.StringValue}");
            return;
        }


        List<CConUnlockAbilityCanvas> canvases =
            [.. Plugin.FindObjectsByType<CConUnlockAbilityCanvas>(CanvasLocation.FindInactive, FindObjectsSortMode.None)];
        CanvasLocation.PatchLoadedLevel(canvases, region);

        List<CConChestEntity> chests =
            [.. Plugin.FindObjectsByType<CConChestEntity>(ChestLocation.FindInactive, FindObjectsSortMode.None)];
        ChestLocation.PatchLoadedLevel(chests, region);

        List<CConInspirationTriggerBehaviour> inspirations =
            [.. Plugin.FindObjectsByType<CConInspirationTriggerBehaviour>(InspirationLocation.FindInactive, FindObjectsSortMode.None)];
        InspirationLocation.PatchLoadedLevel(inspirations, region);

        List<CConCurrencyDepositEntity> deposits =
            [.. Plugin.FindObjectsByType<CConCurrencyDepositEntity>(DepositLocationFactory.FindInactive, FindObjectsSortMode.None)];
        DepositLocationFactory.PatchLoadedLevel(deposits, region);

        List<CConBehaviour_LostShopKeeper> cousins =
            [.. Plugin.FindObjectsByType<CConBehaviour_LostShopKeeper>(CousinLocation.FindInactive, FindObjectsSortMode.None)];
        CousinLocation.PatchLoadedLevel(cousins, region);

        List<CConEntityDropBehaviour_TouchToCollect> pickups =
            [.. Plugin.FindObjectsByType<CConEntityDropBehaviour_TouchToCollect>(PickupLocation.FindInactive, FindObjectsSortMode.None)];
        PickupLocation.PatchLoadedLevel(pickups, region);

        List<CConCarnivalHeadlightEye> carnivalEyes =
            [.. Plugin.FindObjectsByType<CConCarnivalHeadlightEye>(CarnivalEyeLocation.FindInactive, FindObjectsSortMode.None)];
        CarnivalEyeLocation.PatchLoadedLevel(carnivalEyes, region);

        List<ConFoundryPaintPipe_Valve> valves =
            [.. Plugin.FindObjectsByType<ConFoundryPaintPipe_Valve>(FoundryPipeLocation.FindInactive, FindObjectsSortMode.None)];
        FoundryPipeLocation.PatchLoadedLevel(valves, region);


        CConUiPanel_Shop shop = Plugin.FindFirstObjectByType<CConUiPanel_Shop>(FindObjectsInactive.Include);
        ShopItemLocation.PatchLoadedLevel(shop, region);
    }

}