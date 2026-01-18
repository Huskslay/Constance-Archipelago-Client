using RandomizerCore.Classes.Data.Saved;
using RandomizerCore.Classes.Enums;
using System;
using System.Runtime.Serialization;

namespace RandomizerCore.Classes.Data.Types.Entrances;

[Serializable]
public class AEntranceSavedData(string connection) : EntranceRuleSavedData(connection)
{
    public EntranceLockState lockState = EntranceLockState.Open;
    public bool overrideConnection = false;
    public string connectionOverride = "";
}
