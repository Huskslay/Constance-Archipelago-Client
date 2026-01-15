using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomizerCore.Classes.Data.Types.GameEventElements;

[Serializable]
public class GameEvent(GameEvents gameEvent) : ISavedDataOwner<GameEventSavedData>
{
    public void Init() { }

    public GameEvents gameEvent = gameEvent;
    public string GetName() => gameEvent.ToString();

    [NonSerialized]
    private GameEventSavedData savedData;
    public GameEventSavedData GetSavedData() { return savedData; }
    public void SetSavedData(GameEventSavedData savedData, bool save = true)
    {
        if (save) GameEventHandler.I.Save(savedData, this);
        this.savedData = savedData;
    }
}
