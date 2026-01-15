using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Handlers.State;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomizerCore.Patches.Map;

[HarmonyPatch(typeof(CConUiMapRoot))]
public class CConUiMapRoot_Patch
{
    public readonly static List<CConUiMapIcon> icons = [];

    [HarmonyPrefix]
    [HarmonyPatch(nameof(CConUiMapRoot.RebuildSelectTargets))]
    private static void RebuildSelectTargets_Prefix(CConUiMapRoot __instance)
    {
        if (!RandomStateHandler.Randomized) return;

        foreach (CConUiMapIcon icon in icons) Plugin.Destroy(icon.gameObject);
        icons.Clear();

        Region startRegion = RegionHandler.I.GetFromName("V01");
        if (startRegion == null)
        {
            Plugin.Logger.LogError("Could not find region to start from");
            return;
        }

        IConUiMapSelectTarget a = __instance.iconParent.GetComponentsInChildren<IConUiMapSelectTarget>(includeInactive: true).First(x =>
        {
            if (x.RectTransform.name != "Custom" && x.RectTransform.gameObject.TryGetComponent(out CConUiMapIcon icon))
            { return icon.icon == ConMapIcon.Shrine; }
            return false;
        });
        foreach (RandomStateElement<ALocation> element in RandomStateHandler.LocationElements)
        {
            if (element.hasObtainedSource) continue;
            MakeIcon(__instance, a, new($"Prod_{element.source.region}"), true);
        }
    }

    private static CConUiMapIcon MakeIcon(CConUiMapRoot __instance, IConUiMapSelectTarget a, ConLevelId level, bool reachable)
    {
        CConUiMapIcon icon = UnityEngine.Object.Instantiate(a.RectTransform.gameObject).GetComponent<CConUiMapIcon>();
        IConUiMapSelectTarget target = icon.GetComponent<IConUiMapSelectTarget>();

        icon.level = __instance.id2Level.Dictionary[level];

        icon.transform.SetParent(icon.level.iconParent);
        icon.transform.localScale = a.RectTransform.localScale;
        icon.transform.localPosition = a.RectTransform.localPosition;
        target.RectTransform.sizeDelta /= 1.75f;

        if (!reachable) icon.image.color = Color.gray;
        icon.name = "Custom";
        icon.gameObject.SetActive(true);

        return icon;
    }
}
