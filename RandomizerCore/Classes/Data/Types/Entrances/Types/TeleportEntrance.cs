using Constance;
using RandomizerCore.Classes.Data.Types.Regions;
using System;

namespace RandomizerCore.Classes.Data.Types.Entrances.Types;

[Serializable]
public class TeleportEntrance : AEntrance
{
    public string name;
    public string region;
    public string connectionName;
    public string connectionRegion;
    public ConCheckPointId teleportToCheckpoint;

    public override string GetName() => GetName(name, region, connectionRegion);

    public TeleportEntrance GetConnection()
    {
        if (connectionName == null) return null;
        return (TeleportEntrance)EntranceHandler.I.GetFromName(connectionName);
    }

    public TeleportEntrance(string name, Region region, string connectionName, string connectionRegion, ConCheckPointId teleportToCheckpoint)
    {
        this.name = name;
        this.region = region.name;
        this.connectionName = connectionName == null ? null : GetName(connectionName, connectionRegion, region.name);
        this.connectionRegion = connectionRegion;
        this.teleportToCheckpoint = teleportToCheckpoint;

        SetSavedData(new(GetName()));
        EntranceHandler.I.Save(this);
    }

    public static string GetName(string name, string region, string connectionRegion)
    {
        return $"{region}-{connectionRegion ?? "???"}-{name}";
    }
}
