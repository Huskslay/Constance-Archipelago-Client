using System;
using UnityEngine;

namespace RandomizerCore.Classes.Data.Saved;

[Serializable]
public class SavedData(string connection)
{
    public string connection = connection;

    public virtual Color? GetColor() => null;
}