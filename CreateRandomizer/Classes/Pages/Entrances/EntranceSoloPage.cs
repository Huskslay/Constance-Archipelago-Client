using CheatMenu.Classes;
using Constance;
using CreateRandomizer.Classes.Pages.Generic;
using RandomizerCore.Classes.Data.Types.Entrances;
using RandomizerCore.Classes.Data.Types.Entrances.Types;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;
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
        windowRect = PageHelpers.NewSoloPageRect;
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
        savedData.overrideConnection = GUIElements.BoolValue("Override connection", savedData.overrideConnection);
        if (savedData.overrideConnection)
        {
            savedData.connectionOverride = GUIElements.StringValue(savedData.connectionOverride);
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

                foreach (TeleportEntrance entrance in EntranceHandler.I.TeleportEntrances)
                {
                    if (entrance.goName == closestTp.name)
                    {
                        savedData.connectionOverride = entrance.GetName();
                        break;
                    }
                }
            }
        }

        GUIElements.ElipseLine();

        savedData.lockState = GUIElements.ListValue("Lock State", savedData.lockState,
            (IEnumerable<EntranceLockState>)Enum.GetValues(typeof(EntranceLockState)),
            (previous, check, _) => previous == check, (value) => value.ToString()
        );

        PageHelpers.DrawEntranceRuleSavedData(savedData, ref selectedEntranceRule);
    }

    public override void UpdateOpen()
    {
        base.UpdateOpen();
        if (!soloPage.OwnerSet) return;

        PageHelpers.LoadEntranceButton(soloPage.Owner);

        soloPage.UpdateOpen();
        EntranceHandler.I.Save(soloPage.Owner.GetSavedData(), log: false);
    }

    public override void Close()
    {
        soloPage.Close();
        base.Close();
    }
}
