using System.Diagnostics;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer;

public static class LevelImporting
{
    private static readonly string ImportPath = Path.Combine(".", "levelImport");

    public static void ImportLevels(GameDatabaseContext database, IDataStore dataStore)
    {
        GameUser serverUser = database.GetAdminUser();
        
        if (Directory.Exists(ImportPath))
        {
            Console.WriteLine("Import folder found.");
        }
        else
        {
            Console.WriteLine("No import folder found. Creating one...");
            Directory.CreateDirectory(ImportPath);
            Console.WriteLine($"Created import folder at {ImportPath}");
            return;
        }

        Stopwatch stopwatch = new ();
        stopwatch.Start();
        
        Dictionary<string, List<ImportedLevelDependency>> levels = GetLevelsInDirectory();
        UploadLevels(levels, database, dataStore, serverUser);
        RemoveEmptyDirectories(ImportPath);
        
        stopwatch.Stop();
        
        Console.WriteLine($"Finished importing levels. ({stopwatch.Elapsed})");
    }

    private static Dictionary<string, List<ImportedLevelDependency>> GetLevelsInDirectory()
    {
        Dictionary<string, List<ImportedLevelDependency>> levels = new ();
        
        string[] files = Directory.GetFiles(ImportPath, "*", SearchOption.AllDirectories);
        foreach (string filePath in files)
        {
            string fileName = Path.GetFileName(filePath);

            // e.g. "image"
            string typeString = fileName.Split("-").Last();
            FileType fileType = GetFileTypeFromName(typeString);

            if (fileType == FileType.Unknown)
            {
                Console.WriteLine($"{fileName} is of unknown file type. The name of the file should end with \"-level\", \"-image\", \"-thumbnail\" or \"-sound\". Skipping...");
                continue;
            }

            // e.g. "f2a0ab5d-cf4a-3cb8-bcd3-9586257b8384"
            string identifier = fileName.Remove(fileName.Length - (typeString.Length + 1));

            if (!levels.ContainsKey(identifier)) levels.Add(identifier, new List<ImportedLevelDependency>());
            
            levels[identifier].Add(new ImportedLevelDependency(identifier, fileType, filePath));
        }

        return levels;
    }

    private static void UploadLevels(Dictionary<string, List<ImportedLevelDependency>> levels, GameDatabaseContext database, IDataStore dataStore, GameUser serverUser)
    {
        foreach (string key in levels.Keys)
        {
            IList<ImportedLevelDependency> dependencies = levels[key];

            if (dependencies.Count != 3)
            {
                Console.WriteLine($"{key} has an unexpected amount of dependencies. It should have 3, but has {dependencies.Count}. Skipping...");
                continue;
            }

            ImportedLevelDependency? level = null;
            ImportedLevelDependency? thumbnail = null;
            ImportedLevelDependency? sound = null;
            
            foreach (ImportedLevelDependency dependency in dependencies)
            {
                if (dependency.FileType == FileType.Level) level = dependency;
                if (dependency.FileType == FileType.Image) thumbnail = dependency;
                if (dependency.FileType == FileType.Sound) sound = dependency;
            }
            
            if (level == null || thumbnail == null || sound == null)
            {
                Console.WriteLine($"{key} doesn't have all required files. It should have a level file, an image / thumbnail file, and a sound file. Skipping...");
                continue;
            }
            
            byte[] levelBytes = File.ReadAllBytes(level.FilePath);

            if (levelBytes.Length < 10)
            {
                Console.WriteLine(level.FilePath + " is less than 10 bytes. Skipping...");
                continue;
            }
            
            DateTimeOffset levelCreationDate = File.GetLastWriteTimeUtc(level.FilePath);

            byte[] thumbnailBytes = File.ReadAllBytes(thumbnail.FilePath);
            
            byte[] soundBytes = File.ReadAllBytes(sound.FilePath);

            string levelId = LevelHelper.GenerateLevelId();
            PublishLevelRequest request = new ($"Imported Level ({levelId})", 0, levelBytes.LongLength, levelCreationDate);
            GameLevel publishedLevel = database.CreateLevel(request, serverUser, false, levelId);

            string levelKey = GetLevelResourceKey(publishedLevel.Id, FileType.Level);
            string thumbnailKey = GetLevelResourceKey(publishedLevel.Id, FileType.Image);
            string soundKey = GetLevelResourceKey(publishedLevel.Id, FileType.Sound);

            dataStore.WriteToStore(levelKey, levelBytes);
            dataStore.WriteToStore(thumbnailKey, thumbnailBytes);
            dataStore.WriteToStore(soundKey, soundBytes);
            
            File.Delete(level.FilePath);
            File.Delete(thumbnail.FilePath);
            File.Delete(sound.FilePath);

            string directory = level.FilePath.Split(level.LevelIdentifier)[0];
            if (Directory.GetFiles(directory).Length == 0) Directory.Delete(directory);
            
            Console.WriteLine($"Successfully imported {key} as {publishedLevel.Id}.");
        }
    }
    
    private static void RemoveEmptyDirectories(string directoryPath) {
        string[] subDirectories = Directory.GetDirectories(directoryPath);

        foreach (string subDirectory in subDirectories) {
            RemoveEmptyDirectories(subDirectory);
        }

        if (Directory.GetFiles(directoryPath).Length != 0 ||
            Directory.GetDirectories(directoryPath).Length != 0) return;

        if (directoryPath == ImportPath) return;
        
        Directory.Delete(directoryPath, false);
        Console.WriteLine("Deleted empty directory: {0}", directoryPath);
    }
}