using RandomizerCore.Classes.Data.Types.Entrances.Types;

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
}
