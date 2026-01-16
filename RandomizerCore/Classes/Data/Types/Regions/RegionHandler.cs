using Constance;
using Leo;
using System.Collections;
using UnityEngine;

namespace RandomizerCore.Classes.Data.Types.Regions;

public class RegionHandler : SavedDataOwnerHandler<Region, RegionSavedData>
{
    public override string GetName() => "Regions";

    public static RegionHandler I;
    public override void Init()
    {
        I = this;
        base.Init();
    }


    public bool TryGetFromId(ConLevelId id, out Region region)
    {
        region = GetFromName(id.StringValue.Replace("Prod_", ""));
        return region != null;
    }


    public static IEnumerator LoadRegion(Region region, CConPlayerEntity player = null, Direction direction = null)
    {
        yield return LoadRegion(new ConLevelId($"Prod_{region.GetName()}"), player: player, direction: direction);
    }
    public static IEnumerator LoadRegion(ConCheckPointId id, CConPlayerEntity player = null, Direction direction = null)
    {
        yield return LoadRegion(id.ExtractLevelId(), player: player, id: id, direction: direction);
    }
    public static IEnumerator LoadRegion(ConLevelId levelId, CConPlayerEntity player = null, ConCheckPointId? id = null, Direction direction = null)
    {
        player = Plugin.FindFirstObjectByType<CConPlayerEntity>();
        ConStateAbility_Player_Transition transitionAbility = player.SM.TransitionAbility;
        CConCheckPointManager checkPointManager = CConSceneRegistry.Instance.CheckPointManager as CConCheckPointManager;
        CConTransitionManager transitionManager = transitionAbility.TransitionManager;
        id ??= checkPointManager.GetFallbackCheckpointId(levelId);

        ConTransitionCommand_Default trans = new(
            (ConCheckPointId)id,
            levelId,
            direction,
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
        yield return new WaitForSeconds(0.1f);
    }
}
