using FileHandler.Classes;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomizerCore.Classes.Data.Types.GameEventElements;

public class GameEventHandler : SavedDataOwnerHandler<GameEvent, GameEventSavedData>
{
    public override string GetName() => "Game Events";

    public static GameEventHandler I;
    public override void Init()
    {
        I = this;
        base.Init();
    }

    protected override void LoadAll(Action<GameEvent> initiate)
    {
        foreach (GameEvent item in FileSaveLoader.LoadClasses<GameEvent>(FolderPath))
            initiate(item);
    }
}
