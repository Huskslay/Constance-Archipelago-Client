using Constance;
using RandomizerCore.Classes.Archipelago;
using RandomizerCore.Classes.Data.Types.AItems.Types;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using RandomizerCore.Classes.Handlers.State;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomizerCore.Classes.Data.Types.Locations.Types;

[Serializable]
public class CousinLocation : ALocation
{
    public static FindObjectsInactive FindInactive => FindObjectsInactive.Include;
    public override Randomizable GetRandoType() => Randomizable.Cousin;


    private readonly ConLevelId id;
    private readonly SConQuest.ConQuestIntel questIntel;


    public CousinLocation(CConBehaviour_LostShopKeeper cousin, Region region) : base(GetName(cousin), region, cousin.name)
    {
        id = cousin.GetLevelId();
        questIntel = cousin.config.shopKeeperData.ToList().Find(x => x.levelId == id).questIntel;

        CreateItem(cousin, region);
        LocationHandler.I.Save(this);
    }


    public static string GetName(CConBehaviour_LostShopKeeper cousin)
    {
        return cousin.config.shopKeeperData.ToList().Find(x => x.levelId == cousin.GetLevelId()).questIntel.ToString();
    }

    public Item CreateItem(CConBehaviour_LostShopKeeper cousin, Region region)
    {
        Item item = new(GetName(cousin), type, region, "useful", CollectableItems.Cousin);
        item.aItems = [new ActionItem(GetName(), item.GetName(), 0).GetName()];
        ItemHandler.I.Save(item);
        return item;
    }

    public static void PatchLoadedLevel(List<CConBehaviour_LostShopKeeper> cousins, Region region)
    {
        BasicPatch<CConBehaviour_LostShopKeeper, CousinLocation>(cousins, region, (cousin, location) =>
        { cousin.gameObject.SetActive(!RandomStateHandler.HasObtainedLocation(location)); });
    }


    public override void Init()
    {
        ActionItem.AddAction(GetName(), OnCollect);
    }
    private void OnCollect()
    {
        DataStorage.AddFoundCousin(id);

        ConPersistenceId conPersistenceId = SConConfig_QuestFindShopKeepers.BuildKeeperFoundPersistenceId(id);
        ConScriptableObject.SceneRegistry.Save.SetBool(conPersistenceId, true, default);
        if (!ConMonoBehaviour.SceneRegistry.QuestManager.TryGetQuest(new ConQuestId("quest_ShopKeeper"), out SConQuest quest))
        {
            Plugin.Logger.LogWarning("Could not get shopkeeper quest!");
            return;
        }
        quest.AddIntel(questIntel);
    }
}
