using RandomizerCore.Classes.Data.Saved;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Data.Types.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreateRandomizer.Classes.Pages.Generic;

public static class SavedDataDrawer
{
    public static void Draw(SavedData savedData)
    {
        if (savedData is ALocationSavedData aLocationSavedData) Draw(aLocationSavedData);
        else if (savedData is RegionSavedData regionSavedData) Draw(regionSavedData);
    }

    public static void Draw(RegionSavedData savedData)
    {
        
    }

    public static void Draw(ALocationSavedData savedData)
    {
        Draw((EntranceRuleSavedData)savedData);
    }

    public static void Draw(EntranceRuleSavedData savedData)
    {

    }
}
