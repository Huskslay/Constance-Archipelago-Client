using FileHandler.Classes;
using RandomizerCore.Classes.Data.Types.Entrances;
using RandomizerCore.Classes.Data.Types.Entrances.Types;
using RandomizerCore.Classes.Data.Types.Items;
using RandomizerCore.Classes.Data.Types.Regions;

namespace CreateRandomizer.Classes;

public static class DataConverter
{
    public static void Convert()
    {
        CheckData();
        EntranceData();
        RegionData();
    }

    public static void CheckData()
    {
        string output = "def item_full_name(name: tuple[str, str, str, str]) -> str:\n    return f\"{_to_full_name(name)}-Item\"\n" +
            "def location_full_name(name: tuple[str, str, str, str]) -> str:\n    return f\"{_to_full_name(name)}-Location\"\n" +
            "def _to_full_name(name: tuple[str, str, str, str]) -> str:\n    return f\"{name[1]}-{name[0]}-{name[2]}\"\n\n";

        output += "# name, type, region, classification\n";
        foreach (Item item in ItemHandler.I.dataOwners.Values)
        {
            output += $"{item.GetName().Replace("-Item", "-Check").Replace("-", "_")} = (" +
                $"\"{item.name}\", " +
                $"\"{item.type}\", " +
                $"\"{item.region}\", " +
                $"\"{item.classification}\"" +
            $")\n";
        }
        FileSaveLoader.SaveTxt(["Output"], "checks", output);
    }

    public static void EntranceData()
    {
        string output = "# name, region, connection, connectionRegion\n";
        foreach (TeleportEntrance entrance in EntranceHandler.I.TeleportEntrances)
        {
            TeleportEntrance connection = entrance.GetConnection();

            output += $"{entrance.GetName().Replace("-", "_")} = (" +
                $"\"{entrance.GetName()}\", " +
                $"\"{entrance.region}\", " +
                $"\"{(connection == null ? "null" : connection.GetName())}\", " +
                $"\"{(connection == null ? "null" : connection.region)}\", " +
            $")\n";
        }
        foreach (ElevatorEntrance elevator1 in EntranceHandler.I.ElevatorEntrances)
        {
            foreach (ElevatorEntrance elevator2 in EntranceHandler.I.ElevatorEntrances)
            {
                if (elevator1 == elevator2) continue;
                output += $"{elevator1.region}_{elevator2.region}_Elevator = (" +
                    $"\"{elevator1.region}-{elevator2.region}-Elevator\", " +
                    $"\"{elevator1.region}\", " +
                    $"\"{elevator2.region}\", " +
                $")\n";
            }
        }
        FileSaveLoader.SaveTxt(["Output"], "entrance", output);
    }

    public static void RegionData()
    {
        string output = "# name, entrances, locations\n";
        foreach (Region region in RegionHandler.I.dataOwners.Values)
        {
            output += $"{region.GetName()} = (" +
                $"\"{region.GetName()}\", " +
                $"[";
            foreach (string entrance in region.entrances)
                output += $"\"{entrance}\", ";
            output += "], [";
            foreach (string location in region.locations)
                output += $"\"{location}\", ";
            output += $"])\n";
        }
        FileSaveLoader.SaveTxt(["Output"], "regions", output);
    }
}
