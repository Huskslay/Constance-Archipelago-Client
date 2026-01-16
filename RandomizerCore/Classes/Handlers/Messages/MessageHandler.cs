using FileHandler.Classes;
using RandomizerCore.Classes.Archipelago;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Enums;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RandomizerCore.Classes.Handlers.Messages;

public static class MessageHandler
{
    private static Canvas canvas;
    private static TextMeshProUGUI messageTextPrefab;

    private static List<MessageText> messageObjects;

    private static TextMeshProUGUI CreateText(float x, float y,
        HorizontalAlignmentOptions horizontal = HorizontalAlignmentOptions.Left,
        VerticalAlignmentOptions vertical = VerticalAlignmentOptions.Top)
    {
        TextMeshProUGUI textMeshProUGUI = new GameObject("Message Text").AddComponent<TextMeshProUGUI>();
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
        messageObjects = [];

        canvas = new GameObject().AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = Plugin.FindFirstObjectByType<Camera>();
        canvas.sortingOrder = 100;
        canvas.transform.SetParent(Plugin.I.transform);

        messageTextPrefab = CreateText(30, 0);
        messageTextPrefab.gameObject.SetActive(false);

        GameDataActions.OnFileQuit.AddListener(OnFileQuit);
    }
    private static void OnFileQuit()
    {
        Plugin.Logger.LogMessage("Destroying all message text objects");
        while (messageObjects.Count > 0)
        {
            Plugin.Destroy(messageObjects[0].GameObject);
            messageObjects.RemoveAt(0);
        }
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
        Plugin.Logger.LogMessage($"Creating {(permanent ? "permanent" : "")} text '{displayText}'");

        TextMeshProUGUI tmp = Plugin.Instantiate(messageTextPrefab, canvas.transform);
        tmp.transform.position = messageTextPrefab.transform.position;
        MessageText messageText = new(tmp, permanent);

        messageText.Begin(displayText, color == null ? Color.white : (Color)color);
        foreach (MessageText text in messageObjects) text.Bump();
        messageObjects.Add(messageText);
    }

    public static void Update()
    {
        int index = 0;
        while (index < messageObjects.Count)
        {
            if (messageObjects[index].Update()) messageObjects.RemoveAt(index);
            else index++;
        }
    }
}
