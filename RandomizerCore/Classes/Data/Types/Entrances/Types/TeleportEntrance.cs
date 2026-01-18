using Constance;
using RandomizerCore.Classes.Data.Types.Regions;
using System;

namespace RandomizerCore.Classes.Data.Types.Entrances.Types;

[Serializable]
public class TeleportEntrance(string name, Region region,
    ConCheckPointId teleportToCheckpoint, ConCheckPointId teleportFromCheckpoint, string goName) : AEntrance
{
    public string name = name;
    public string region = region.name;
    public string connectionName = null;
    public string connectionRegion = null;

    public string goName = goName;
    public ConCheckPointId teleportToCheckpoint = teleportToCheckpoint;
    public ConCheckPointId teleportFromCheckpoint = teleportFromCheckpoint;

    public override string GetName() => $"{region}-{connectionRegion ?? "???"}-{name}";

    public TeleportEntrance GetConnection()
    {
        string connectionName = this.connectionName;
        if (GetSavedData().overrideConnection) connectionName = GetSavedData().connectionOverride;
        if (connectionName == null) return null;

        AEntrance connection = EntranceHandler.I.GetFromName(connectionName);
        if (connection is not TeleportEntrance teleportEntrance) return null;
        return teleportEntrance;
    }

    public void Connect(string connectionName, string connectionRegion)
    {
        this.connectionName = $"{connectionRegion}-{region}-{connectionName}";
        this.connectionRegion = connectionRegion;
    }
}
