using RandomizerCore.Classes.Data.Types.Entrances.Types;
using System.Collections.Generic;
using System.Linq;

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



    private List<T> ConvertEntrances<T>() where T : AEntrance
    {
        return I.dataOwners.Values.ToList()
               .FindAll(x => x.GetType() == typeof(T))
               .ConvertAll(x => (T)x);
    }

    private List<TeleportEntrance> teleportEntrances = null;
    public List<TeleportEntrance> TeleportEntrances
    {
        get
        {
            teleportEntrances ??= ConvertEntrances<TeleportEntrance>();
            return teleportEntrances;
        }
    }

    private List<ElevatorEntrance> elevatorEntrances = null;
    public List<ElevatorEntrance> ElevatorEntrances
    {
        get
        {
            elevatorEntrances ??= ConvertEntrances<ElevatorEntrance>();
            return elevatorEntrances;
        }
    }


    public override void NewData(AEntrance data)
    {
        base.NewData(data);
        if (teleportEntrances != null && data is TeleportEntrance teleportEntrance)
            teleportEntrances.Add(teleportEntrance);
        if (elevatorEntrances != null && data is ElevatorEntrance elevatorEntrance)
            elevatorEntrances.Add(elevatorEntrance);
    }
}
