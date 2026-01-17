using CheatMenu.Classes;
using Constance;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Locations;
using System.Linq;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Locations;

public class LocationSoloPage : SoloGUIPage
{
    private SavedDataOwnerSoloPage<ALocation, ALocationSavedData> soloPage;
    public override string Name => soloPage.Name;
    private string selectedEntranceRule;

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        soloPage = new();
        base.Init(modGUI, parent, id);
        selectedEntranceRule = null;
    }

    public void Open(ALocation location)
    {
        if (soloPage.OwnerSet && soloPage.Owner == location) return;
        soloPage.Open(location, DrawSavedData);
        Open();
    }
    public override void Open()
    {
        if (!soloPage.OwnerSet) Close();
        else base.Open();
    }

    private void DrawSavedData(ALocationSavedData savedData)
    {
        PageHelpers.DrawEntranceRuleSavedData(savedData, ref selectedEntranceRule);
    }

    public override void UpdateOpen()
    {
        base.UpdateOpen();
        if (!soloPage.OwnerSet) return;

        if (GUILayout.Button("Teleport to"))
            Plugin.I.StartCoroutine(PageHelpers.LoadLocation(soloPage.Owner, () =>
            Plugin.FindObjectsByType<CConTeleportPoint>(FindObjectsSortMode.None).ToList().ConvertAll(x => (MonoBehaviour)x)));

        soloPage.UpdateOpen();
        LocationHandler.I.Save(soloPage.Owner.GetSavedData(), log: false);
    }

    public override void Close()
    {
        soloPage.Close();
        base.Close();
    }
}
