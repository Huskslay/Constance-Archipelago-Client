using CheatMenu.Classes;
using RandomizerCore.Classes.Data.Types.Regions;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Regions;
public class RegionEventPage : SoloGUIPage
{
    public EventGiver Giver { get; private set; } = null;


    public override string Name => Giver == null ? "null" : Giver.gameEvent.ToString();
    private string selectedEntranceRule;

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        base.Init(modGUI, parent, id);
        windowRect = PageHelpers.NewSoloPageRect;
    }

    public void Open(EventGiver giver)
    {
        Giver = giver;
        selectedEntranceRule = null;
        Open();
    }
    public override void Open()
    {
        base.Open();
    }

    public override void UpdateOpen()
    {
        base.UpdateOpen();
        if (Giver == null) return;

        PageHelpers.DrawEntranceRules(ref Giver.entranceRules, ref selectedEntranceRule);
    }

    public override void Close()
    {
        Giver = null;
        base.Close();
    }
}
