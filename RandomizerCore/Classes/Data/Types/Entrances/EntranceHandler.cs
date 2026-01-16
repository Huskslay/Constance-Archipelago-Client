using RandomizerCore.Classes.Data.Types.Entrances.Types;
using RandomizerCore.Classes.Data.Types.Locations;

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

    public override void OnEachLoaded(AEntrance loaded)
    {
        if (loaded is not TeleportEntrance teleportEntrance) return;
        if (teleportEntrance.GetConnection() == null)
            Plugin.Logger.LogWarning($"Entrance '{loaded.GetName()}' does not have a connection");
    }
}
