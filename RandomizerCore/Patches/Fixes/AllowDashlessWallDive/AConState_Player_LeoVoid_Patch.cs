using Constance;
using HarmonyLib;
using Leo;
using RandomizerCore.Classes.Handlers.State;

namespace RandomizerCore.Patches.Fixes.AllowDashlessWallDive;

[HarmonyPatch(typeof(AConState_Player<Void>))]
internal class AConState_Player_LeoVoid_Patch
{
    public static bool FakeDash = false;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(AConState_Player<Void>.TryInitDashState), [typeof(IConStateUpdate), typeof(float), typeof(DirectionX?)], [ArgumentType.Out, ArgumentType.Normal, ArgumentType.Normal])]
    private static bool TryInitDashState_Prefix(AConState_Player<Void> __instance, ref bool __result, out IConStateUpdate dashState, float buffer, DirectionX? forceDir = null)
    {
        dashState = null;
        __result = false;

        if (!RandomStateHandler.Randomized) return true;
        if (__instance.SceneRegistry.Inventory.Has(__instance.Unlocks.Dash)) return true;


        if (__instance.Entity.Transitioning || __instance.Entity.StatusModifiers.InVoidGlimpse || __instance.Entity.StatusEffect.Modifiers.IsStunned || !__instance.Input.DiveDash.JustPressed(buffer, false))
            return false;

        DirectionX directionX2 = __instance.FaceDir;

        if (__instance.SM.WallDive.TryInit(out dashState, __instance.MovementData.wallDiveMaxXDistance, directionX2))
        {
            FakeDash = true;
            __result = true;
            return true;
        }


        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(AConState_Player<Void>.TryInitDashState), [typeof(IConStateUpdate), typeof(float), typeof(DirectionX?)], [ArgumentType.Out, ArgumentType.Normal, ArgumentType.Normal])]
    private static void TryInitDashState_Postfix()
    {
        FakeDash = false;
    }
}