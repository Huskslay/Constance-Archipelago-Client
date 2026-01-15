using RandomizerCore.Classes.Archipelago;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Enums;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RandomizerCore.Classes.Handlers.RecentItems;

public static class RecentItemHandler
{
    private static Canvas canvas;
    private static TextMeshProUGUI locationTextPrefab;

    private static List<TrackerText> locationTexts;

    private static TextMeshProUGUI CreateText(float x, float y,
        HorizontalAlignmentOptions horizontal = HorizontalAlignmentOptions.Left,
        VerticalAlignmentOptions vertical = VerticalAlignmentOptions.Top)
    {
        TextMeshProUGUI textMeshProUGUI = new GameObject("Tracker Text").AddComponent<TextMeshProUGUI>();
        textMeshProUGUI.transform.SetParent(canvas.transform);
        textMeshProUGUI.text = "";

        textMeshProUGUI.horizontalAlignment = horizontal;
        textMeshProUGUI.verticalAlignment = vertical;
        textMeshProUGUI.fontSize = 30f;
        textMeshProUGUI.textWrappingMode = TextWrappingModes.NoWrap;

        textMeshProUGUI.margin = new Vector4(0f, 0f, 0f, 0f);
        textMeshProUGUI.rectTransform.anchorMin = Vector2.zero;
        textMeshProUGUI.rectTransform.anchorMax = Vector2.zero;
        textMeshProUGUI.transform.localPosition = new Vector3(x, y, 0f);

        return textMeshProUGUI;
    }

    public static void Init()
    {
        RandomActionHandler.onLocationGet.AddListener(OnLocationGet);
        RandomActionHandler.onItemGet.AddListener(OnItemGet);
        locationTexts = [];

        canvas = new GameObject().AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = Plugin.FindFirstObjectByType<Camera>();
        canvas.sortingOrder = 100;
        canvas.transform.SetParent(Plugin.I.transform);

        locationTextPrefab = CreateText(30, 0);
        locationTextPrefab.gameObject.SetActive(false);
    }

    private static void OnLocationGet(ALocation location)
    {
        string itemName = MultiClient.ScoutItemName(location.GetName(), out string playerName, out bool isCurrentPlayer);
        if (isCurrentPlayer) return; // Text will be created on item get

        CreateText($"Gave '{itemName}' to '{playerName}'");
    }
    private static void OnItemGet(Item item, string playerName, bool isCurrentPlayer)
    {
        string text = $"Got: {item.DisplayName()}";
        if (item.collectable != CollectableItems.None)
            text += $" ({DataStorage.FoundCollectable(item.collectable, item.GetName())}/{EnumProperties.CollectableCounts[item.collectable]})";

        if (isCurrentPlayer) CreateText(text);
        else CreateText($"{text} - From: {playerName}");
    }

    public static void CreateText(string displayText, bool permanent = false, Color? color = null)
    {
        TextMeshProUGUI tmp = Plugin.Instantiate(locationTextPrefab, canvas.transform);
        tmp.transform.position = locationTextPrefab.transform.position;
        TrackerText trackerText = new(tmp, permanent);

        trackerText.Begin(displayText, color == null ? Color.white : (Color)color);
        foreach (TrackerText text in locationTexts) text.Bump();
        locationTexts.Add(trackerText);
    }

    public static void Update()
    {
        int index = 0;
        while (index < locationTexts.Count)
        {
            if (locationTexts[index].Update()) locationTexts.RemoveAt(index);
            else index++;
        }
    }
}
