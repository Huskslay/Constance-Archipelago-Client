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
}
