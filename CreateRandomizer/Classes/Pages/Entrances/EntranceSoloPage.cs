using CheatMenu.Classes;
using Constance;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Entrances;
using RandomizerCore.Classes.Data.Types.Entrances.Types;
using System.Linq;
using UnityEngine;

namespace CreateRandomizer.Classes.Pages.Entrances;

public class EntrancesSoloPage : SoloGUIPage
{
    private SavedDataOwnerSoloPage<AEntrance, AEntranceSavedData> soloPage;
    public override string Name => soloPage.Name;
    private string selectedEntranceRule;

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        soloPage = new();
        base.Init(modGUI, parent, id);
        selectedEntranceRule = null;
    }

    public void Open(AEntrance entrance)
    {
        if (soloPage.OwnerSet && soloPage.Owner == entrance) return;
        soloPage.Open(entrance, DrawSavedData);
        Open();
    }
    public override void Open()
    {
        if (!soloPage.OwnerSet) Close();
        base.Open();
    }

    private void DrawSavedData(AEntranceSavedData savedData)
    {
        PageHelpers.DrawEntranceRuleSavedData(savedData, ref selectedEntranceRule);
    }

    public override void UpdateOpen()
    {
        base.UpdateOpen();
        if (!soloPage.OwnerSet) return;

        if (soloPage.Owner is TeleportEntrance teleportEntrance)
        {
            if (GUILayout.Button($"Teleport to {(teleportEntrance.GetSavedData().overrideEntrance || teleportEntrance.GetConnection() == null ? "Jank" : "")}"))
                StartCoroutine(PageHelpers.LoadEntrance(teleportEntrance));
        }
        else if (soloPage.Owner is ElevatorEntrance elevatorEntrance && GUILayout.Button("Teleport to elevator"))
            StartCoroutine(PageHelpers.LoadEntrance(elevatorEntrance));

        soloPage.Owner.GetSavedData().overrideEntrance = GUIElements.BoolValue("Override entrance", soloPage.Owner.GetSavedData().overrideEntrance);
        if (soloPage.Owner.GetSavedData().overrideEntrance)
        {
            soloPage.Owner.GetSavedData().entranceOverride = GUIElements.StringValue(soloPage.Owner.GetSavedData().entranceOverride);
            if (GUILayout.Button("To nearest"))
            {
                CConPlayerEntity player = FindFirstObjectByType<CConPlayerEntity>();

                float closest = float.MaxValue;
                CConTeleportPoint closestTp = null;
                foreach (CConTeleportPoint tp in FindObjectsByType<CConTeleportPoint>(FindObjectsSortMode.None))
                {
                    float distance = Vector3.Distance(tp.transform.position, player.transform.position);
                    if (distance < closest)
                    {
                        closest = distance;
                        closestTp = tp;
                    }
                }

                foreach (TeleportEntrance entrance in EntranceHandler.I.dataOwners.Values.ToList().FindAll(x => x is TeleportEntrance)
                    .ConvertAll(x => (TeleportEntrance)x))
                {
                    if (entrance.goName == closestTp.name)
                    {
                        soloPage.Owner.GetSavedData().entranceOverride = entrance.GetName();
                        break;
                    }
                }
            }
        }

        soloPage.UpdateOpen();
        EntranceHandler.I.Save(soloPage.Owner.GetSavedData(), log: false);
    }

    public override void Close()
    {
        soloPage.Close();
        base.Close();
    }
}
