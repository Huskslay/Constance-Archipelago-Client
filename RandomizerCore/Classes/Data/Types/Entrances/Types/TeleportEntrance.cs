using RandomizerCore.Classes.Data.Types.Regions;
using System;

namespace RandomizerCore.Classes.Data.Types.Entrances.Types;

[Serializable]
public class TeleportEntrance : AEntrance
{
    public string name;
    public bool elevator;
    public string parent;
    public string connection;

    public override string GetName() => $"{parent}-{connection}-{name}";

    public TeleportEntrance(string name, bool elevator, Region parent, string connection)
    {
        this.name = name;
        this.elevator = elevator;
        this.parent = parent.name;
        this.connection = connection;

        SetSavedData(new(GetName()));
        EntranceHandler.I.Save(this);
    }
}
