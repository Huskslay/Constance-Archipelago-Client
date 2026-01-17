using System;

namespace RandomizerCore.Classes.Data.Types.Entrances;

[Serializable]
public abstract class AEntrance : ISavedDataOwner<AEntranceSavedData>
{
    public void Init() { }
    public abstract string GetName();

    [NonSerialized]
    private AEntranceSavedData savedData;
    public AEntranceSavedData GetSavedData() { return savedData; }
    public void SetSavedData(AEntranceSavedData savedData, bool save = true)
    {
        if (save) EntranceHandler.I.Save(savedData);
        this.savedData = savedData;
    }
}
