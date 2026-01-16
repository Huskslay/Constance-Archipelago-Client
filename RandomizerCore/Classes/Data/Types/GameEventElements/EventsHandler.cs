namespace RandomizerCore.Classes.Data.Types.GameEventElements;

public class GameEventHandler : SavedDataOwnerHandler<GameEvent, GameEventSavedData>
{
    public override string GetName() => "Game Events";

    public static GameEventHandler I;
    public override void Init()
    {
        I = this;
        base.Init();
    }
}
