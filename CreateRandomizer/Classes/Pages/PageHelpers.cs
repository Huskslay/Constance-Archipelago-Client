using CheatMenu.Classes;
using Constance;
using RandomizerCore.Classes.Data.EntranceRules;
using RandomizerCore.Classes.Data.Saved;
using RandomizerCore.Classes.Data.Types.Entrances;
using RandomizerCore.Classes.Data.Types.Entrances.Types;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages;

public static class PageHelpers
{
    public static void DrawEntranceRuleSavedData(EntranceRuleSavedData savedData, ref string selectedEntranceRule)
    {
        GUILayout.BeginHorizontal();
        GreenRedButton("Used", ref savedData.used);
        if (savedData.used) GreenRedButton("Completed", ref savedData.completed);
        GUILayout.EndHorizontal();

        if (savedData.entranceRules.Count == 0)
        {
            GUILayout.Label("No entrance rules found");
            return;
        }

        foreach (EntranceRule rule in savedData.entranceRules)
        {
            bool wasOpen = rule.entrance == selectedEntranceRule;
            bool open = GUIElements.BoolValue(rule.entrance, wasOpen);

            if (!open)
            {
                if (wasOpen) selectedEntranceRule = null;
                continue;
            }
            if (!wasOpen) selectedEntranceRule = rule.entrance;

            GUILayout.Label("AAA");
        }
    }

    public static void GreenRedButton(string text, ref bool value)
    {
        Color bg = GUI.backgroundColor;

        GUI.backgroundColor = value ? Color.green : Color.red;
        if (GUILayout.Button(text)) value = !value;

        GUI.backgroundColor = bg;
    }





    public static IEnumerator LoadTransition(TeleportEntrance entrance)
    {
        //if (entrance.GetSavedData().doOverrideTransition)
        //{
        //    yield return RegionsHandler.I.LoadLevel(entrance.GetRegion());

        //    CConTeleportPoint tp = Plugin.FindObjectsByType<CConTeleportPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList().Find(x => x.teleportTo.StringValue == entrance.teleportToCheckPoint);

        //    CConPlayerEntity player = Plugin.FindFirstObjectByType<CConPlayerEntity>();
        //    player.transform.position = tp.transform.position;
        //}
        //else 
        TeleportEntrance connection = entrance.GetConnection();
        if (connection == null) yield break;
        yield return RegionHandler.LoadRegion(connection.teleportToCheckpoint);
    }
    public static IEnumerator LoadTransition(ElevatorEntrance entrance)
    {
        yield return RegionHandler.LoadRegion(RegionHandler.I.GetFromName(entrance.region));

        CConElevatorBehaviour tp = Plugin.FindFirstObjectByType<CConElevatorBehaviour>();

        CConPlayerEntity player = Plugin.FindFirstObjectByType<CConPlayerEntity>();
        player.transform.position = tp.transform.position;
    }
    public static IEnumerator LoadLocation(ALocation location, Func<List<MonoBehaviour>> getLocations)
    {
        yield return RegionHandler.LoadRegion(RegionHandler.I.GetFromName(location.region));

        List<MonoBehaviour> behaviours = getLocations();
        MonoBehaviour tp = behaviours.Find(x => x.name == location.goName);

        CConPlayerEntity player = Plugin.FindFirstObjectByType<CConPlayerEntity>();
        player.transform.position = tp.transform.position;
    }
}
