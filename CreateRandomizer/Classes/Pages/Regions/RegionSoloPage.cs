using CheatMenu.Classes;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Regions;

public class RegionSoloPage : SoloGUIPage
{
    private SavedDataOwnerSoloPage<Region, RegionSavedData> soloPage;
    private RegionEventPage eventPage;

    public override string Name => soloPage.Name;

    public ulong selectedGameEvent;
    public string selectedEntranceRule;

    public Region Region => soloPage.Owner;

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        soloPage = new();
        base.Init(modGUI, parent, id);
        eventPage = new GameObject().AddComponent<RegionEventPage>();
        eventPage.Init(modGUI, parent, ModGUI.winId++);
    }

    public void Open(Region region)
    {
        if (soloPage.OwnerSet && region == soloPage.Owner) return;
        soloPage.Open(region, DrawSavedData);

        selectedGameEvent = (ulong)GameEvents.None;
        selectedEntranceRule = null;

        Open();
    }
    public override void Open()
    {
        if (!soloPage.OwnerSet) Close();

        base.Open();
    }

    private void DrawSavedData(RegionSavedData savedData)
    {
        EventGiver giver = GUIElements.ListValue("Events", eventPage.Giver, savedData.givenEvents,
            (current, possible, _) => current == possible, (giver) => giver.gameEvent.ToString(), numberPerRow: 1);
        if (giver != null && giver != eventPage.Giver) eventPage.Open(giver);
    }

    public override void UpdateOpen()
    {
        base.UpdateOpen();
        if (!soloPage.OwnerSet) return;

        if (GUILayout.Button("Teleport")) StartCoroutine(PageHelpers.LoadRegion(soloPage.Owner));
        GUIElements.Line();

        soloPage.UpdateOpen();
        RegionHandler.I.Save(soloPage.Owner.GetSavedData(), log: false);
    }

    public override void Close()
    {
        eventPage.Close();
        soloPage.Close();
        base.Close();
    }
}
