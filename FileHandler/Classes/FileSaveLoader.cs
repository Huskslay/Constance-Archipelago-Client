using Constance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileHandler.Classes;

public static class FileSaveLoader
{
    private static readonly string assembly = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
    private static readonly string ext = "cls";
    private static readonly string jsonExt = "json";

    public static List<T> LoadClasses<T>(List<string> folders, ConSaveStateId? id = null)
    {
        return LoadClassesInner(folders, LoadClassFromFile<T>, ext, id);
    }
    public static List<T> LoadClassesJson<T>(List<string> folders, ConSaveStateId? id = null)
    {
        return LoadClassesInner(folders, LoadClassFromJson<T>, jsonExt, id);
    }
    private static List<T> LoadClassesInner<T>(List<string> folders, Func<List<string>, string, ConSaveStateId?, T> loader, string extension, ConSaveStateId? id = null)
    {
        string path = GetFolderPath(id, folders);
        if (path == null)
        {
            Plugin.Logger.LogError("An error occured loading class when getting directory");
            return null;
        }

        List<T> clses = [];

        if (!Directory.Exists(path))
        {
            Plugin.Logger.LogWarning($"Path '{path}' does not exist");
            return clses;
        }

        foreach (string file in Directory.EnumerateFiles(path, $"*.{extension}")) clses.Add(loader(folders, Path.GetFileNameWithoutExtension(file), id));

        return clses;
    }

    public static bool TrySaveClassToFile<T>(T cls, List<string> folders, string fileName, ConSaveStateId? id = null, bool logSuccess = true)
    {
        string path = GetFilePath(id, folders, fileName, ext, createDir: true);
        if (path == null)
        {
            Plugin.Logger.LogError("An error occured saving class when getting directory");
            return false;
        }

        try
        {
            BinaryFormatter formatter = new();
            using (FileStream stream = new(path, FileMode.Create))
            {
                formatter.Serialize(stream, cls);
            }
            if (logSuccess) Plugin.Logger.LogMessage($"{nameof(T)} saved to {path}");
            return true;
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogWarning($"Error saving to file: {ex.Message}");
        }
        return false;
    }
    public static T LoadClassFromFile<T>(List<string> folders, string fileName, ConSaveStateId? id = null)
    {
        string path = GetFilePath(id, folders, fileName, ext);
        if (path == null)
        {
            Plugin.Logger.LogError("An error occured loading class when getting directory");
            return default;
        }

        T cls = default;
        try
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(path, FileMode.Open);
            cls = (T)formatter.Deserialize(stream);
        }
        catch (FileNotFoundException)
        {
            Plugin.Logger.LogWarning($"File not found: {path}");
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogWarning($"Error loading from file: {ex.Message}");
        }
        return cls;
    }


    public static bool TrySaveClassToJson<T>(T cls, List<string> folder, string fileName, ConSaveStateId? id = null, bool logSuccess = true, ConSaver conSaver = null)
    {
        string path = GetFilePath(id, folder, fileName, jsonExt, createDir: true, conSaver: conSaver);
        if (path == null)
        {
            Plugin.Logger.LogError("An error occured saving class when getting directory");
            return false;
        }

        try
        {
            var settings = new JsonSerializerSettings
            {
                //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
            };

            string json = JsonConvert.SerializeObject(cls, settings);
            File.WriteAllText(path, json);
            if (logSuccess) Plugin.Logger.LogMessage($"{nameof(T)} saved to {path}");
            return true;
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogWarning($"Error saving to file: {ex.Message}");
        }
        return false;
    }
    public static T LoadClassFromJson<T>(List<string> folders, string fileName, ConSaveStateId? id = null)
    {
        T cls = default;

        string path = GetFilePath(id, folders, fileName, jsonExt);
        if (path == null)
        {
            Plugin.Logger.LogError("An error occured loading class when getting directory");
            return cls;
        }

        try
        {
            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                NullValueHandling = NullValueHandling.Ignore
            };

            string json = File.ReadAllText(path);
            cls = JsonConvert.DeserializeObject<T>(json, settings);
        }
        catch (FileNotFoundException)
        {
            Plugin.Logger.LogWarning($"File not found: {path}");
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogWarning($"Error loading from file: {ex.Message}");
        }
        return cls;
    }


    public static bool ClassExistsInFile(List<string> folders, string fileName, ConSaveStateId? id = null)
    {
        string path = GetFilePath(id, folders, fileName, ext);
        return File.Exists(path);
    }
    public static void DeleteClassInFile(List<string> folders, string fileName, ConSaveStateId? id = null)
    {
        string path = GetFilePath(id, folders, fileName, ext);
        File.Delete(path);
    }
    public static bool ClassExistsInJson(List<string> folders, string fileName, ConSaveStateId? id = null)
    {
        string path = GetFilePath(id, folders, fileName, jsonExt);
        return File.Exists(path);
    }
    public static void DeleteClassInJson(List<string> folders, string fileName, ConSaveStateId? id = null)
    {
        string path = GetFilePath(id, folders, fileName, jsonExt);
        File.Delete(path);
    }


    private static string SaveIdToDir(ConSaveStateId? id, ConSaver conSaver = null)
    {
        if (id == null) return assembly;

        if (conSaver == null)
        {
            CConSaveStateManager saveManager = Plugin.FindFirstObjectByType<CConSaveStateManager>();
            if (saveManager == null)
            {
                Plugin.Logger.LogError($"Could not find CConSaveStateManager");
                return null;
            }
            conSaver = saveManager.Saver;
        }

        return conSaver.BuildSaveDir((ConSaveStateId)id).ToString();
    }
    private static string GetFolderPath(ConSaveStateId? id, List<string> folders, bool createDir = false, ConSaver conSaver = null)
    {
        string dir = SaveIdToDir(id, conSaver);
        if (dir == null) return null;
        string path = dir;
        foreach (string folder in folders) path = Path.Combine(path, folder);
        if (createDir) Directory.CreateDirectory(path);
        return path;
    }
    public static string GetFilePath(ConSaveStateId? id, List<string> folders, string fileName, string ext, bool createDir = false, ConSaver conSaver = null)
    {
        string folderPath = GetFolderPath(id, folders, createDir, conSaver);
        if (folderPath == null) return null;
        return Path.Combine(folderPath, fileName + $".{ext}");
    }

    public static string FourDigitHash(string toHash)
    {
        int hash = toHash.GetHashCode() % 10000;
        return hash.ToString("D4")[^4..];
    }
}
