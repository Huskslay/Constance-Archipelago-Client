using CheatMenu.Classes;
using Constance;
using RandomizerCore.Classes.Data;
using RandomizerCore.Classes.Data.Saved;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Selectable;

namespace CreateRandomizer.Classes.Pages.Generic;

public class SavedDataOwnersSelectPage<T1, T2>(int numberPerColumn) where T1 : ISavedDataOwner<T2> where T2 : SavedData
{
    private IEnumerable<T1> options = [];
    private readonly int columnCount = numberPerColumn;

    private Func<T1, Color?> getColor;

    private string name = "SelectPage";
    public string Name => name;

    public void Open(string name, List<T1> options, Func<T1, Color?> getColor)
    {
        this.getColor = getColor;
        this.name = name;
        this.options = options;
    }

    public T1 UpdateOpen()
    {
        return GUIElements.ListValue(default, options, (_, _, _) => false, (x) => x.GetName(),
            numberPerRow: columnCount, setColor: (value, possibleValue, index) => getColor(possibleValue));
    }

    public void Close()
    {
        name = "";
        options = [];
    }
}
