using System;
using System.Collections.Generic;

namespace RandomizerCore.Classes.Handlers.Files;

public class SlotData
{
    public bool skipIntro;

    public SlotData(Dictionary<string, object> data)
    {
        if (data.ContainsKey("skip_intro")) skipIntro = (Int64)data["skip_intro"] > 0;
        else Plugin.Logger.LogWarning($"Data does not contain key: skip_intro");
    }
}
