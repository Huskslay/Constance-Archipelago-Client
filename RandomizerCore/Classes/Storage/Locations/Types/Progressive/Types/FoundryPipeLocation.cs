using Constance;
using FileHandler.Classes;
using RandomizerCore.Classes.Handlers;
using RandomizerCore.Classes.State;
using RandomizerCore.Classes.Storage.Items;
using RandomizerCore.Classes.Storage.Items.Types;
using RandomizerCore.Classes.Storage.Regions;
using RandomizerCore.Classes.Storage.Requirements.Entries;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomizerCore.Classes.Storage.Locations.Types.Progressive.Types;

[Serializable]
public class FoundryPipeLocation : ALocation, IProgressiveLocation
{
    public override RandomizableItems GetItemType() => RandomizableItems.FoundryPipe;
    public static FindObjectsInactive FindInactive => FindObjectsInactive.Exclude;


    protected override string GetDisplayItemNameInner() => ProgressiveItemHandler.GetItemName(this);

    private static int indexes = 0;
    private readonly int progressiveIndex = -1;
    public int GetProgressiveIndex() => progressiveIndex;

    private readonly AItem item;
    public AItem GetProgressiveItem() => item;

    public override AItem GetItem() => ProgressiveItemHandler.GetItem(this);
    public ProgressiveItemType GetProgressiveType() => ProgressiveItemType.FoundryPipes;


    private readonly ConPersistenceId id;


    public FoundryPipeLocation(ConFoundryPaintPipe_Valve valve, Region region) : base(ConvertName(valve), valve.name, region)
    {
        progressiveIndex = indexes++;

        id = valve.PersistenceId;
        item = new ActionItem(this, GetFullName());
        ProgressiveItemHandler.AddToInstance(this);
    }
    private static string ConvertName(ConFoundryPaintPipe_Valve valve)
    {
        string hash = FileSaveLoader.FourDigitHash(valve.PersistenceId.StringValue);
        return $"FoundryPipe-{hash}";
    }

    public override void Init()
    {
        ActionItem.AddAction(GetFullName(), OnCollect);
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


    public static void PatchLoadedLevel(List<ConFoundryPaintPipe_Valve> valves, List<FoundryPipeLocation> valveLocations)
    {
        if (!RandomState.IsRandomized(RandomizableItems.FoundryPipe)) return;
        BasicPatch(valves, valveLocations, (valve, location) =>
        {
            if (!RandomState.TryGetElement(location, out RandomStateElement element))
            {
                Plugin.Logger.LogError($"Could not get element for location {location.GetFullName()}");
                return;
            }

            valve.UpdatePropertyBlock(0f);
        });
    }
}