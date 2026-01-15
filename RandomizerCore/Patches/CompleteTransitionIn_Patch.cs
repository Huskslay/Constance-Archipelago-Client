using Constance;
using HarmonyLib;
using RandomizerCore.Classes.Components;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Handlers.Files;
using RandomizerCore.Classes.Handlers.State;
using System.Collections;
using UnityEngine;

namespace RandomizerCore.Patches;

[HarmonyPatch(typeof(ConStateAbility_Player_Transition))]
public class ConStateAbility_Player_Transition_Patch
{
    public static readonly ConCheckPointId startCheckpointId = new("cp_Prod_V01_7d6bf550-4ce0-11ef-a7cf-35ea7ade9f40");

    [HarmonyPrefix]
    [HarmonyPatch(nameof(ConStateAbility_Player_Transition.StartTransitionIn), [typeof(IConTransitionCommand), typeof(float)])]
    private static void StartTransitionIn_Prefix()
    {
        RandomStateHandler.readyForItems = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(ConStateAbility_Player_Transition.CompleteTransitionIn))]
    private static void CompleteTransitionIn_Postfix()
    {
        if (!RandomStateHandler.Randomized) return;

        string level = ConMonoBehaviour.SceneRegistry.PlayerOne.Level.Current.StringValue;
        if (level == "Prod_Flashback_IntroCutscene" && RandomFilesHandler.slotData.skipIntro)
        {
            Plugin.I.StartCoroutine(Skip());
            return;
        }

        RegionLivePatcher.PatchLoadedRegion();
        RandomStateHandler.readyForItems = true;
    }

    private static IEnumerator Skip()
    {
        // Get references
        CConPlayerEntity player = Plugin.FindFirstObjectByType<CConPlayerEntity>();
        ConStateAbility_Player_Transition transitionAbility = player.SM.TransitionAbility;
        CConCheckPointManager checkPointManager = CConSceneRegistry.Instance.CheckPointManager as CConCheckPointManager;
        CConTransitionManager transitionManager = transitionAbility.TransitionManager;

        // Load level and set player to start
        yield return new WaitUntil(() => !transitionManager.IsRunning);
        yield return new WaitForSeconds(0.1f);
        yield return RegionHandler.LoadRegion(startCheckpointId, player);
        player.transform.position += new Vector3(50, 0, 0);
    }
}