using FileHandler.Classes;
using System;

namespace RandomizerCore.Classes.Data.Types.Entrances;

public class EntranceHandler : SavedDataOwnerHandler<AEntrance, AEntranceSavedData>
{
    public override string GetName() => "Entrances";

    public static EntranceHandler I;
    public override void Init()
    {
        I = this;
        base.Init();
    }

    protected override void LoadAll(Action<AEntrance> initiate)
    {
        foreach (AEntrance item in FileSaveLoader.LoadClasses<AEntrance>(FolderPath))
            initiate(item);
    }
}
