using CheatMenu.Classes;
using Constance;
using RandomizerCore.Classes.Data.Rules;
using RandomizerCore.Classes.Data.Saved;
using RandomizerCore.Classes.Data.Types.Entrances;
using RandomizerCore.Classes.Data.Types.Entrances.Types;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages;

public static class PageHelpers
{
    public static Rect NewSoloPageRect => new(450f, 50f, 400f, 600f);
    public static int KeyItemsPerRow => 6;
    public static int KeyCollectablesPerRow => 6;
    public static int KeyEventsPerRow => 2;
    public static int KeySkipsPerRow => 3;

    public static void DrawEntranceRuleSavedData(EntranceRuleSavedData savedData, ref string selectedEntranceRule, Action onIfUsed = null)
    {
        GUIElements.Line();

        GUILayout.BeginHorizontal();
        GreenRedButton("Used", ref savedData.used);
        if (savedData.used) GreenRedButton("Completed", ref savedData.completed);
        GUILayout.EndHorizontal();

        GUIElements.Line();
        GUIElements.ElipseLine();
        GUIElements.Line();

        if (onIfUsed != null)
        {
            onIfUsed();
            GUIElements.Line();
        }

        DrawEntranceRules(ref savedData.entranceRules, ref selectedEntranceRule);
    }

    public static void DrawEntranceRules(ref EntranceRules entranceRules, ref string selectedEntranceRule)
    {
        GreenRedButton("Needs Collectables", ref entranceRules.needsCollectables);
        if (entranceRules.needsCollectables) DrawCollectables(ref entranceRules);
        if (entranceRules.needsCollectables || entranceRules.needsEvents) GUILayout.Space(20);
        GreenRedButton("Needs Events", ref entranceRules.needsEvents);
        if (entranceRules.needsEvents) DrawEvents(ref entranceRules);

        if (entranceRules.entranceRules.Count == 0)
        {
            GUILayout.Label("No entrance rules found");
            return;
        }

        GUIElements.Line();
        GUIElements.ElipseLine();
        for (int i = 0; i < entranceRules.entranceRules.Count; i++)
        {
            EntranceRule entranceRule = entranceRules.entranceRules[i];
            GUIElements.Line();

            bool wasOpen = entranceRule.entrance == selectedEntranceRule;
            bool open = GUIElements.BoolValue(entranceRule.entrance, wasOpen);

            if (!open)
            {
                if (wasOpen) selectedEntranceRule = null;
                continue;
            }
            if (!wasOpen) selectedEntranceRule = entranceRule.entrance;

            DrawEntranceRule(ref entranceRule);
        }
    }

    private static void DrawCollectables(ref EntranceRules entranceRules)
    {
        foreach (CollectableItems collectableItem in Enum.GetValues(typeof(CollectableItems)))
        {
            if (collectableItem == CollectableItems.None) continue;
            if (entranceRules.collectableItems.Find(x => x.item == collectableItem) != null) continue;
            entranceRules.collectableItems.Add(new(collectableItem));
        }

        foreach (CollectableItemEntry entry in entranceRules.collectableItems)
            GUIElements.IntSelector((_) => entry.item.ToString(), ref entry.count);
    }
    private static void DrawEvents(ref EntranceRules entranceRules)
    {
        EnumFlagButtons(ref entranceRules.events, skip: (item) => { return item == GameEvents.None; }, maxButtonsPerRow: KeyEventsPerRow);
    }



    public static void DrawEntranceRule(ref EntranceRule entranceRule)
    {
        PageHelpers.LoadEntranceButton(EntranceHandler.I.GetFromName(entranceRule.entrance));
        GreenRedButton("Possible", ref entranceRule.possible);
        if (!entranceRule.possible) return;

        GUIElements.ElipseLine();

        foreach (Rule rule in entranceRule.rules)
        {
            GUIElements.ElipseLine();

            if (GUILayout.Button("Remove rule"))
            {
                entranceRule.rules.Remove(rule);
                break;
            }

            DrawRule(rule);
        }

        GUIElements.ElipseLine();
        if (GUILayout.Button("Add rule")) entranceRule.rules.Add(new());
    }

    private static void DrawRule(Rule rule)
    {
        GUILayout.Label("Items: ");
        EnumFlagButtons(ref rule.items, skip: (item) => { return item == KeyItems.None; }, maxButtonsPerRow: KeyItemsPerRow);

        GUILayout.Label("Skips: ");
        EnumFlagButtons(ref rule.skips, skip: (item) => { return item == Skips.None; }, maxButtonsPerRow: KeySkipsPerRow);
    }





    public static void EnumFlagButtons<T>(ref T flags, Action<T, T> onFlagsChanged = null, int maxButtonsPerRow = 6, Func<T, bool> skip = null)
        where T : Enum
    {
        Color bg = GUI.backgroundColor;
        int index = 0;
        GUILayout.BeginHorizontal();

        foreach (T item in Enum.GetValues(typeof(T)))
        {
            if (skip != null && skip.Invoke(item)) continue;

            if (index++ >= maxButtonsPerRow)
            {
                index = 1;
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }

            GUI.backgroundColor = flags.HasFlag(item) ? Color.green : Color.red;
            if (GUILayout.Button(item.ToString()))
            {
                flags = (T)Enum.ToObject(typeof(T), Convert.ToUInt64(flags) ^ Convert.ToUInt64(item));
                onFlagsChanged?.Invoke(flags, item);
            }
        }

        GUILayout.EndHorizontal();
        GUI.backgroundColor = bg;
    }

    public static void GreenRedButton(string text, ref bool value)
    {
        Color bg = GUI.backgroundColor;

        GUI.backgroundColor = value ? Color.green : Color.red;
        if (GUILayout.Button(text)) value = !value;

        GUI.backgroundColor = bg;
    }





    public static void LoadEntranceButton(AEntrance entrance)
    {
        if (entrance is TeleportEntrance teleportEntrance)
        {
            if (GUILayout.Button($"Teleport to {(teleportEntrance.GetSavedData().overrideConnection || teleportEntrance.GetConnection() == null ? "Jank" : "")}"))
                Plugin.I.StartCoroutine(PageHelpers.LoadEntrance(teleportEntrance));
        }
        else if (entrance is ElevatorEntrance elevatorEntrance && GUILayout.Button("Teleport to elevator"))
            Plugin.I.StartCoroutine(PageHelpers.LoadEntrance(elevatorEntrance));
    }
    public static IEnumerator LoadEntrance(TeleportEntrance entrance)
    {
        TeleportEntrance connection = entrance.GetConnection();

        if (entrance.GetSavedData().overrideConnection || entrance.GetConnection() == null)
        {
            yield return LoadRegion(RegionHandler.I.GetFromName(entrance.region));

            CConTeleportPoint tp = Plugin.FindObjectsByType<CConTeleportPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .ToList().Find(x => x.teleportTo.StringValue == entrance.teleportToCheckpoint.stringValue);

            CConPlayerEntity player = Plugin.FindFirstObjectByType<CConPlayerEntity>();
            player.transform.position = tp.transform.position;
            yield break;
        }

        yield return RegionHandler.LoadRegion(connection.teleportToCheckpoint);
    }
    public static IEnumerator LoadEntrance(ElevatorEntrance entrance)
    {
        yield return LoadRegion(RegionHandler.I.GetFromName(entrance.region));

        CConElevatorBehaviour tp = Plugin.FindFirstObjectByType<CConElevatorBehaviour>();

        CConPlayerEntity player = Plugin.FindFirstObjectByType<CConPlayerEntity>();
        player.transform.position = tp.transform.position;
    }
    public static IEnumerator LoadLocation(ALocation location, Func<List<MonoBehaviour>> getLocations)
    {
        yield return LoadRegion(RegionHandler.I.GetFromName(location.region));

        List<MonoBehaviour> behaviours = getLocations();
        MonoBehaviour tp = behaviours.Find(x => x.name == location.goName);

        CConPlayerEntity player = Plugin.FindFirstObjectByType<CConPlayerEntity>();
        player.transform.position = tp.transform.position;
    }
    public static IEnumerator LoadRegion(Region owner)
    {
        yield return RegionHandler.LoadRegion(owner);
    }
}
