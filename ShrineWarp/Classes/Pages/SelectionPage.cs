using CheatMenu.Classes;
using Constance;
using ShrineWarp.Patches;
using System.Collections;
using UnityEngine;

namespace ShrineWarp.Classes.Pages;

public class SelectionPage : GUIPage
{
    public override string Name => "Selection";

    public override void Init(ModGUI modGUI, Transform parent, int id = 1)
    {
        base.Init(modGUI, parent, id);
    }


    public override void Open()
    {

    }


    public override void UpdateOpen()
    {
        Color origColor = GUI.backgroundColor;
        foreach (ShrineData shrine in ShrineDataHandler.loadedData)
        {
            GUI.backgroundColor = OptionsPage.unlockAll ? origColor : (shrine.unlocked ? Color.green : Color.red);
            if (GUILayout.Button(shrine.region))
            {
                if (shrine.unlocked || OptionsPage.unlockAll)
                {
                    CConUiPanel_Journal_Patch.journal.ClosePanel();
                    StartCoroutine(LoadLevel(new(shrine.region), new(shrine.checkpoint)));
                }
            }
        }
        GUI.backgroundColor = origColor;
    }

    public static IEnumerator LoadLevel(ConLevelId levelId, ConCheckPointId id)
    {
        CConPlayerEntity player = Plugin.FindFirstObjectByType<CConPlayerEntity>();
        ConStateAbility_Player_Transition transitionAbility = player.SM.TransitionAbility;
        CConCheckPointManager checkPointManager = CConSceneRegistry.Instance.CheckPointManager as CConCheckPointManager;
        CConTransitionManager transitionManager = transitionAbility.TransitionManager;
        if (levelId.StringValue != "Prod_V01") id = checkPointManager.GetFallbackCheckpointId(levelId);

        ConTransitionCommand_Default trans = new(
            id,
            levelId,
            null,
            Vector2.zero,
            new(0, Color.green, FadeType.Fade, new(0.25f, new()))
        );

        transitionAbility.StartTransitionIn(trans);
        float start = Time.time;
        yield return new WaitUntil(() => !transitionManager.IsRunning || Time.time - start > 15);
        if (transitionManager.IsRunning)
        {
            Plugin.Logger.LogError($"Transition did not end fast enough, forcably ending");
            transitionManager.AbortTransition();
        }
        if (levelId.StringValue != "Prod_V01")
        {
            CConMeditationPointEntity shrine = Plugin.FindFirstObjectByType<CConMeditationPointEntity>(FindObjectsInactive.Include);
            if (shrine != null) player.transform.position = shrine.transform.position;
        }
    }


    public override void Close()
    {

    }
}
