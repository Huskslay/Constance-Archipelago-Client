using RandomizerCore.Classes.Data.Saved;

namespace RandomizerCore.Classes.Data;

public interface ISavedDataOwner<T> where T : SavedData
{
    public string GetName();

    public void Init();

    T GetSavedData();
    void SetSavedData(T savedData, bool save = true);
}
