using CheatMenu.Classes;
using Constance;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages;
public class CheatPage : GUIPage
{
    public override string Name => "Cheats";

    public override void UpdateOpen()
    {
        if (GUILayout.Button("One shot")) ConDebugFlags.DebugOneShot(true);
        if (GUILayout.Button("No One shot")) ConDebugFlags.DebugOneShot(false);
    }
}
