using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Archipelago;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Handlers.State;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace RandomizerCore.Patches.Locations.ShopItem;

[HarmonyPatch(typeof(CConUiPanel_Shop))]
public class CConUiPanel_Shop_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConUiPanel_Shop.OnBuyItem))]
    private static bool OnBuyItem_Prefix(CConUiPanel_Shop __instance, CConUiShopItemButton itemButton)
    {
        if (!RandomStateHandler.Randomized) return true;

        IConPlayerInventory inventory = ConMonoBehaviour.SceneRegistry.Inventory;
        ManyLocationComponent manyLocationComponent = __instance.GetComponent<ManyLocationComponent>();
        if (manyLocationComponent == null)
        {
            Plugin.Logger.LogError("No ManyLocationComponent on shop");
            return false;
        }
        List<ALocation> locations = manyLocationComponent.Locations;

        ALocation location = locations.Find(x => x.goName == itemButton.ShopItem.name);

        if (RandomStateHandler.HasObtainedLocation(location))
        {
            Plugin.Logger.LogWarning($"Already obtained source: {location.GetName()}");
            return false;
        }
        if (!inventory.Pay(__instance.ShoppingPlayer, inventory.Catalog.Currency, itemButton.ShopItem.Price)) return false;

        inventory.Collect(__instance.ShoppingPlayer, itemButton.ShopItem, 1);
        RandomStateHandler.CheckLocation(location);

        __instance.ShowSelectionMenu();
        if (__instance.IsShopEmpty()) ConMonoBehaviour.SceneRegistry.UiPanelManager.ClosePanel(__instance);
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConUiPanel_Shop.OnSelectionItemSelected))]
    private static bool OnSelectionItemSelected_Prefix(CConUiPanel_Shop __instance, SConCollectable_ShopItem item)
    {
        if (!RandomStateHandler.Randomized) return true;

        List<ALocation> locations = __instance.GetComponent<ManyLocationComponent>().Locations;
        ALocation location = locations.Find(x => x.goName == item.name);


        if (location == null)
        {
            Plugin.Logger.LogWarning($"Could not find location for shop item: {item.name}");
            return true;
        }
        string itemName = MultiClient.ScoutItemName(location.GetName(), out _, out _);


        __instance.imgItem.sprite = item.ImageSprite;
        __instance.txtName.text = itemName;
        LocalizedString descriptionText = item.DescriptionText;
        descriptionText.SetStringVariable("Amount", "");
        __instance.textDescription.text = descriptionText.TryGetText("");

        return false;
    }
}