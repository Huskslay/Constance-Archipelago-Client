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
public class CanvasLocation : ALocation
{
    public static FindObjectsInactive FindInactive => FindObjectsInactive.Exclude;
    public override Randomizable GetRandoType() => Randomizable.Canvas;


    public CanvasLocation(CConUnlockAbilityCanvas canvas, Region region) : base(GetName(canvas), region, canvas.name)
    {
        CreateItem(canvas, region);
        LocationHandler.I.Save(this);
    }


    public static string GetName(CConUnlockAbilityCanvas canvas)
    {
        SConCollectable collectable = canvas.collectable;
        return collectable.name.Replace("unlock_Ability_", "");
    }

    public Item CreateItem(CConUnlockAbilityCanvas canvas, Region region)
    {
        Item item = new(GetName(canvas), type, region, "progression", CollectableItems.None);
        item.aItems = [new CollectableItem(canvas.collectable, item.GetName(), 0).GetName()];
        ItemHandler.I.Save(item);
        return item;
    }

    public static void PatchLoadedLevel(List<CConUnlockAbilityCanvas> canvases, Region region)
    {
        BasicPatch<CConUnlockAbilityCanvas, CanvasLocation>(canvases, region, (canvas, location) =>
        {
            canvas.collectable = null;

            bool flag = RandomStateHandler.HasObtainedLocation(location);
            if (canvas.canvasDrawing)
            {
                canvas.canvasDrawing.SetActive(flag);
            }
            if (flag)
            {
                foreach (GameObject gameObject in canvas.deactivateIfCollected)
                {
                    if (gameObject)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        });
    }
}
