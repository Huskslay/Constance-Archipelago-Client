using RandomizerCore.Classes.Data.Types.Regions;
using System;

namespace RandomizerCore.Classes.Data.Types.Entrances.Types;

[Serializable]
public class ElevatorEntrance : AEntrance
{
    public string region;

    public override string GetName() => $"{region}-Elevator";

    public ElevatorEntrance(Region region)
    {
        this.region = region.name;

        SetSavedData(new(GetName()));
        EntranceHandler.I.Save(this);
    }
}
