using Constance;
using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Data.Types.AItems.Types;

[Serializable]
public class ActionItem : AItem
{
    private readonly static Dictionary<string, Action> actionDictionary = [];

    private readonly string onCollect;


    public ActionItem(string onCollect, string item, int index) : base(item, index)
    {
        this.onCollect = onCollect;

        AItemHandler.I.Save(this);
    }


    public override void GiveToPlayer(IConPlayerEntity player, IConPlayerInventory inventoryManager)
    {
        Plugin.Logger.LogMessage($"Calling action for item {GetName()}");
        actionDictionary[onCollect]?.Invoke();
    }

    public static void Reset()
    {
        actionDictionary.Clear();
    }
    public static void AddAction(string name, Action action)
    {
        actionDictionary.Add(name, action);
    }
}
