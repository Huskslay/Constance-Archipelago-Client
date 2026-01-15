using CheatMenu.Classes;
using RandomizerCore.Classes.Data;
using RandomizerCore.Classes.Data.Saved;

namespace CreateRandomizer.Classes.Pages.Generic;

public class SavedDataOwnersPage<T1, T2>(SavedDataOwnerHandler<T1, T2> handler, int numberPerColumn) 
    where T1 : ISavedDataOwner<T2> where T2 : SavedData
{
    private readonly SavedDataOwnerHandler<T1, T2> handler = handler;
    private readonly int columnCount = numberPerColumn;

    public void Open()
    {

    }

    public T1 UpdateOpen()
    {
        return GUIElements.ListValue(default, handler.dataOwners.Values, (_, _, _) => false, (x) => x.GetName(), 
            numberPerRow: columnCount, setColor: (value, possibleValue, index) => possibleValue.GetSavedData().GetColor());
    }

    public void Close()
    {

    }
}
