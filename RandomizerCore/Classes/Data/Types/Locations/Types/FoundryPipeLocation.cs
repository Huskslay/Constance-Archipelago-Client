using Constance;
using RandomizerCore.Classes.Data.Types.AItems.Types;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomizerCore.Classes.Data.Types.Locations.Types;

[Serializable]
public class FoundryPipeLocation : ALocation
{
    public static FindObjectsInactive FindInactive => FindObjectsInactive.Exclude;
    public override Randomizable GetRandoType() => Randomizable.FoundryPipe;


    private readonly ConPersistenceId id;


    public FoundryPipeLocation(ConFoundryPaintPipe_Valve valve, Region region) : base(GetName(valve), region, valve.name)
    {
        id = valve.PersistenceId;

        CreateItem(valve, region);
        LocationHandler.I.Save(this);
    }


    public static string GetName(ConFoundryPaintPipe_Valve valve)
    {
        return valve.pipeType.ToString();
    }

    public Item CreateItem(ConFoundryPaintPipe_Valve valve, Region region)
    {
        Item item = new(GetName(valve), type, region, "progression", CollectableItems.FoundryPipe);
        item.aItems = [new ActionItem(GetName(), item.GetName(), 0).GetName()];
        ItemHandler.I.Save(item);
        return item;
    }


    public override void Init()
    {
        ActionItem.AddAction(GetName(), OnCollect);
    }

    private void OnCollect()
    {
        ConMonoBehaviour.SceneRegistry.Persistence.Save.SetBool(id, true, default);
        ConQuestManager questManager = ConMonoBehaviour.SceneRegistry.QuestManager;
        int fixedPipeCount = ConFoundryPaintPipe_Valve.GetFixedPipeCount();
        SConQuest.ConQuestIntel conQuestIntel;
        if (fixedPipeCount != 1)
        {
            if (fixedPipeCount != 2)
            {
                conQuestIntel = SConQuest.ConQuestIntel.None;
            }
            else
            {
                conQuestIntel = SConQuest.ConQuestIntel.C;
            }
        }
        else
        {
            conQuestIntel = SConQuest.ConQuestIntel.B;
        }
        questManager.AddIntel(ConQuests.Foundry, conQuestIntel, false);
    }



    public static void PatchLoadedLevel(List<ConFoundryPaintPipe_Valve> valves, Region region)
    {
        BasicPatch<ConFoundryPaintPipe_Valve, FoundryPipeLocation>(valves, region, (valve, location) =>
        { valve.UpdatePropertyBlock(0f); });
    }
}
