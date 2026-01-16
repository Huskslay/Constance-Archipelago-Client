namespace RandomizerCore.Classes.Data.Types.Items;

public class ItemHandler : SavedDataOwnerHandler<Item, ItemSavedData>
{
    public override string GetName() => "Items";

    public static ItemHandler I;
    public override void Init()
    {
        I = this;
        base.Init();
    }
}
