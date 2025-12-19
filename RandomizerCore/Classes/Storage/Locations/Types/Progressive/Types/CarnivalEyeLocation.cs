using Constance;
using FileHandler.Classes;
using Leo;
using RandomizerCore.Classes.Adapters;
using RandomizerCore.Classes.Handlers;
using RandomizerCore.Classes.State;
using RandomizerCore.Classes.Storage.Items;
using RandomizerCore.Classes.Storage.Items.Types;
using RandomizerCore.Classes.Storage.Regions;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace RandomizerCore.Classes.Storage.Locations.Types.Progressive.Types;

[Serializable]
public class CarnivalEyeLocation : ALocation, IProgressiveLocation
{
    public override RandomizableItems GetItemType() => RandomizableItems.CarnivalEyes;
    public static FindObjectsInactive FindInactive => FindObjectsInactive.Exclude;


    protected override string GetDisplayItemNameInner() => ProgressiveItemHandler.GetItemName(this);


    private readonly int progressiveIndex = -1;
    public int GetProgressiveIndex() => progressiveIndex;

    private readonly ActionItem item;
    public AItem GetProgressiveItem() => item;

    public override AItem GetItem() => ProgressiveItemHandler.GetItem(this);
    public ProgressiveItemType GetProgressiveType() => ProgressiveItemType.CarnivalEyes;


    private readonly ConPersistenceId id;


    public CarnivalEyeLocation(CConCarnivalHeadlightEye eye, Region region) : base(ConvertName(eye), eye.name, region)
    {
        switch (region.GetFullName())
        {
            case "C03": progressiveIndex = 0; break;
            case "C04": progressiveIndex = 1; break;
            case "C05": progressiveIndex = 2; break;
        }
        if (progressiveIndex == -1) Plugin.Logger.LogError("Index was not found");

        id = eye.persistable.PersistenceId;
        item = new ActionItem(this, GetFullName());
        ProgressiveItemHandler.AddToInstance(this);
    }
    private static string ConvertName(CConCarnivalHeadlightEye eye)
    {
        string hash = FileSaveLoader.FourDigitHash(eye.persistable.PersistenceId.StringValue);
        return $"CarnivalEye-{hash}";
    }

    public override void Init()
    {
        ActionItem.AddAction(GetFullName(), OnCollect);
    }

    private void OnCollect()
    {
        ConMonoBehaviour.SceneRegistry.Persistence.Save.SetBool(id, true, default);

        CConCarnivalHeadlightEye eye = Plugin.FindFirstObjectByType<CConCarnivalHeadlightEye>();
        if (eye != null) eye.LastEyeTimeline();
        else {
            if (CConCarnivalHeadlightEye.AreAllHeadLightsActive()) CConCarnivalHeadlightEye.JackieTentSpawned = true;

            ConQuestManager questManager = ConMonoBehaviour.SceneRegistry.QuestManager;
            var conQuestIntel = CConCarnivalHeadlightEye.GetHeadLightsActiveCount() switch
            {
                1 => SConQuest.ConQuestIntel.A,
                2 => SConQuest.ConQuestIntel.B,
                3 => SConQuest.ConQuestIntel.C,
                _ => SConQuest.ConQuestIntel.None,
            };
            questManager.AddIntel(ConQuests.Carnival, conQuestIntel, false);
        }
    }


    public static void PatchLoadedLevel(List<CConCarnivalHeadlightEye> eyes, List<CarnivalEyeLocation> eyeLocations)
    {
        if (!RandomState.IsRandomized(RandomizableItems.CarnivalEyes)) return;
        BasicPatch(eyes, eyeLocations, (eye, location) =>
        {
            if (!RandomState.TryGetElement(location, out RandomStateElement element))
            {
                Plugin.Logger.LogError($"Could not get element for location {location.GetFullName()}");
                return;
            }

            eye.Activated = element.hasObtainedSource;
            eye.interactable.interactable = !eye.Activated;
            if (eye.Activated)
            {
                eye.animator.ConPlay(new ConAnimationClipId("Activate", false), IConClipType.Ended, -1, false);
                eye.onActivateFromPersistence.InvokeSafe(null);
            }
            bool flag = CConCarnivalHeadlightEye.JackieTentSpawned && CConCarnivalHeadlightEye.AreAllHeadLightsActive();
            eye.jackieSpawnTimeline.SetGoActive(flag);
            eye.tentSpawnTimeline.SetGoActive(flag);
            if (eye.gachaJackie)
            {
                eye.gachaJackie.SetGoActive(!flag);
            }
        });
    }

}