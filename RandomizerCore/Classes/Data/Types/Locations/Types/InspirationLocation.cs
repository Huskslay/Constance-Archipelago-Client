using Constance;
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
public class InspirationLocation : ALocation
{
    public static FindObjectsInactive FindInactive => FindObjectsInactive.Exclude;
    public override Randomizable GetRandoType() => Randomizable.Inspiration;


    public InspirationLocation(CConInspirationTriggerBehaviour inspiration, Region region) : base(GetName(inspiration), region, inspiration.name)
    {
        baseItem = CreateItem(inspiration, region).GetName();
        LocationHandler.I.Save(this);
    }


    public static string GetName(CConInspirationTriggerBehaviour inspiration)
    {
        return inspiration.inspiration.name.Replace("inspDrawing_", "");
    }

    public Item CreateItem(CConInspirationTriggerBehaviour inspiration, Region region)
    {
        Item item = new(GetName(inspiration), type, region, "useful", CollectableItems.None);
        item.aItems = [new CollectableItem(inspiration.inspiration, item.GetName(), 0).GetName()];
        ItemHandler.I.Save(item);
        return item;
    }

    public static void PatchLoadedLevel(List<CConInspirationTriggerBehaviour> inspirations, Region region)
    {
        BasicPatch<CConInspirationTriggerBehaviour, InspirationLocation>(inspirations, region, (inspiration, location) =>
        {
            inspiration.inspiration = null;

            // Hide the inspiration if it has been collected already
            if (RandomStateHandler.HasObtainedLocation(location))
            {
                inspiration.gameObject.SetActive(false);
                inspiration.SetOwned();
            }
            if (!inspiration.cinematicOnly)
            {
                inspiration.vfxFloating.Play();
                inspiration.vfxGroundGlow.Play();
                inspiration.sfxIdle.TryPlay(inspiration.Entity);
            }
        });
    }
}
