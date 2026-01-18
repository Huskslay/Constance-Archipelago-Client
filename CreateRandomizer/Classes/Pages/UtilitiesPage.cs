using CheatMenu.Classes;
using Constance;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages;
public class UtilitiesPage : GUIPage
{
    public override string Name => "Cheats";

    public override void UpdateOpen()
    {
        if (GUILayout.Button("Scrape")) GameScraper.Scrape();
        if (GUILayout.Button("Convert")) DataConverter.Convert();

        GUIElements.Line();

        if (GUILayout.Button("One shot")) ConDebugFlags.DebugOneShot(true);
        if (GUILayout.Button("No One shot")) ConDebugFlags.DebugOneShot(false);
    }
}
