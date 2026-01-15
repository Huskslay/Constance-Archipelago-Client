using FileHandler.Classes;
using System;

namespace RandomizerCore.Classes.Data.Types.Locations;

public class LocationHandler : SavedDataOwnerHandler<ALocation, ALocationSavedData>
{
    public override string GetName() => "Locations";

    public static LocationHandler I;
    public override void Init()
    {
        I = this;
        base.Init();
    }

    protected override void LoadAll(Action<ALocation> initiate)
    {
        foreach (ALocation item in FileSaveLoader.LoadClasses<ALocation>(FolderPath))
            initiate(item);
    }
}
