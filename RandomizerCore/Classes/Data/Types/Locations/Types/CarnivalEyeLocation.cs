using Constance;
using FileHandler.Classes;
using Leo;
using RandomizerCore.Classes.Data.Types.AItems.Types;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using RandomizerCore.Classes.Handlers.State;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomizerCore.Classes.Data.Types.Locations.Types;

[Serializable]
public class CarnivalEyeLocation : ALocation
{
    public static FindObjectsInactive FindInactive => FindObjectsInactive.Exclude;
    public override Randomizable GetRandoType() => Randomizable.CarnivalEye;


    private readonly ConPersistenceId id;


    public CarnivalEyeLocation(CConCarnivalHeadlightEye eye, Region region) : base(GetName(eye), region, eye.name)
    {
        id = eye.persistable.PersistenceId;

        CreateItem(eye, region);
        LocationHandler.I.Save(this);
    }


    public static string GetName(CConCarnivalHeadlightEye eye)
    {
        return FileSaveLoader.FourDigitHash(eye.persistable.PersistenceId.StringValue);
    }

    public Item CreateItem(CConCarnivalHeadlightEye eye, Region region)
    {
        Item item = new(GetName(eye), type, region, "progression", CollectableItems.CarnivalEye);
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

        CConCarnivalHeadlightEye eye = Plugin.FindFirstObjectByType<CConCarnivalHeadlightEye>();
        if (eye != null) eye.LastEyeTimeline();
        else
        {
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



    public static void PatchLoadedLevel(List<CConCarnivalHeadlightEye> eyes, Region region)
    {
        BasicPatch<CConCarnivalHeadlightEye, CarnivalEyeLocation>(eyes, region, (eye, location) =>
        {
            eye.Activated = RandomStateHandler.HasObtainedLocation(location);
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
