using System.Diagnostics;
using System.Text.RegularExpressions;
using Bunkum.Core.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.LevelImporting;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer;

public static class LevelImporter
{
    public static void ImportCampaignLevels(GameDatabaseContext database, IDataStore dataStore, string path,
        bool overwrite)
    {
        Console.WriteLine("Beginning to import campaign levels.");

        if (Directory.Exists(path))
            Console.WriteLine("Import folder found.");
        else
            throw new Exception("No import folder found. Aborting import.");

        Stopwatch stopwatch = new();
        stopwatch.Start();

        GameUser adminUser = database.GetAdminUser();

        string recordListPath = Path.Combine(path, "Platformer", "recordList.gm");
        string resourcesPath = Path.Combine(path, "Platformer", "levels");
        List<ImportLevelInformation> informationList = GetLevelsFromRecordList(recordListPath, resourcesPath);

        foreach (ImportLevelInformation info in informationList)
        {
            ImportLevelData? data = LoadImportData(info);
            if (data == null)
                continue;

            UploadLevel(info, data, database, dataStore, adminUser, overwrite);
        }


        stopwatch.Stop();
        Console.WriteLine($"Finished importing campaign levels. ({stopwatch.Elapsed})");
    }

    public static void ImportCommunityLevels(GameDatabaseContext database, IDataStore dataStore, string path,
        bool overwrite)
    {
        Console.WriteLine("Beginning to import community levels.");

        if (Directory.Exists(path))
        {
            Console.WriteLine("Import folder found.");
        }
        else
        {
            Console.WriteLine("No import folder found. Aborting import.");
            return;
        }

        Stopwatch stopwatch = new();
        stopwatch.Start();

        GameUser adminUser = database.GetAdminUser();

        List<ImportLevelInformation> informationList = GetCommunityLevelsInDirectory(path);
        foreach (ImportLevelInformation info in informationList)
        {
            ImportLevelData? data = LoadImportData(info);
            if (data == null)
                continue;

            bool success = UploadLevel(info, data, database, dataStore, adminUser, overwrite);
            if (!success)
                continue;

            Console.WriteLine("Deleting imported assets from import folder...");
            File.Delete(info.LevelFilePath);
            File.Delete(info.ThumbnailFilePath);
            if (info.SoundFilePath != null)
                File.Delete(info.SoundFilePath);
        }

        RemoveEmptyDirectories(path, path);
        stopwatch.Stop();
        Console.WriteLine($"Finished importing community levels. ({stopwatch.Elapsed})");
    }

    private static ImportLevelData? LoadImportData(ImportLevelInformation info)
    {
        bool missingRequiredPath =
            string.IsNullOrEmpty(info.LevelFilePath) || string.IsNullOrEmpty(info.ThumbnailFilePath);

        if (missingRequiredPath || !File.Exists(info.LevelFilePath) || !File.Exists(info.ThumbnailFilePath))
        {
            Console.WriteLine(
                $"{info.Name ?? "Level"} doesn't have all required files. It must have a level file and an image / thumbnail file. Skipping...");
            return null;
        }

        byte[] levelBytes = File.ReadAllBytes(info.LevelFilePath);

        if (levelBytes.Length < 300)
        {
            Console.WriteLine(info.LevelFilePath + " is less than 300 bytes. Skipping...");
            return null;
        }

        ImportLevelData level = new()
        {
            CreationDate = File.GetLastWriteTimeUtc(info.LevelFilePath),
            Thumbnail = File.ReadAllBytes(info.ThumbnailFilePath),
            LevelFile = levelBytes
        };

        if (info.SoundFilePath != null && File.Exists(info.SoundFilePath))
            level.SoundFile = File.ReadAllBytes(info.SoundFilePath);

        return level;
    }

    private static bool UploadLevel(ImportLevelInformation info, ImportLevelData data, GameDatabaseContext database,
        IDataStore dataStore,
        GameUser adminUser, bool overwrite)
    {
        PublishLevelRequest request = new(info.Name ?? $"Imported Level ({info.Id})", 0);

        request.CreationDate = !info.CampaignLevel ? data.CreationDate : new DateTimeOffset();

        GameLevel? levelWithSameId = database.GetLevelWithId(info.Id);
        if (levelWithSameId != null)
        {
            if (!overwrite)
            {
                Console.WriteLine($"There is already a level with the id: {info.Id}. Skipping...");
                return false;
            }

            Console.WriteLine($"{info.Id} has already been imported. Overwriting...");
            // TODO: NO NO.... NOT GOOD.
            // TODO: ALSO. DATES STILL FUCKED
            database.RemoveLevel(levelWithSameId, dataStore);
        }

        GameLevel publishedLevel =
            database.CreateLevel(adminUser, request, PlatformType.Unknown, false, info.Id, info.CampaignLevel);
        database.UploadLevelResources(dataStore, publishedLevel, data.LevelFile, data.Thumbnail, data.SoundFile);
        Console.WriteLine($"Successfully imported {publishedLevel.Id}.");
        return true;
    }

    #region Campaign Levels

    private static List<ImportLevelInformation> GetLevelsFromRecordList(string recordListPath, string resourcesPath)
    {
        List<ImportLevelInformation> levels = new();

        string recordListScript = File.ReadAllText(recordListPath);

        string recordListGlobalName = "campaignList = {";
        int startIndex = recordListScript.IndexOf(recordListGlobalName, StringComparison.Ordinal);

        if (startIndex == -1)
            throw new Exception($"campaignList could not be found in {recordListPath}");

        string recordList = recordListScript.Substring(startIndex + recordListGlobalName.Length).Split("};").First();
        // Use regex to remove all comments.
        recordList = Regex.Replace(recordList, @"/\*(.|\n)*?\*/", string.Empty);

        List<string> records = recordList.Split("recordName").ToList();
        // First element won't be a record, but rather stuff before it, so remove it.
        records.RemoveAt(0);

        foreach (string record in records)
        {
            // ReSharper disable once CommentTypo
            // Remove "bsides" levels (aka. trophy levels).
            // These don't have any server interaction, so there's no reason to import them
            string filtered = record.Split("bsides").First();

            List<string> tracks = filtered.Split("trackName").ToList();
            // First element won't be a track, but rather stuff before the first it, so remove it.
            tracks.RemoveAt(0);
            foreach (string track in tracks)
            {
                string[] rows = ("trackName" + track).Split("\n");
                string? name = rows.FirstOrDefault(r => r.Contains("trackName ="))?.Split("\"")[1];
                string? baseFileName = rows.FirstOrDefault(r => r.Contains("baseFilename ="))?.Split("\"")[1];

                if (name == null || baseFileName == null)
                    continue;

                levels.Add(new ImportLevelInformation
                {
                    Name = name,
                    Id = baseFileName,
                    LevelFilePath = Path.Combine(resourcesPath, baseFileName + ".json.z"),
                    ThumbnailFilePath = Path.Combine(resourcesPath, baseFileName + ".png")
                });
            }
        }

        return levels;
    }

    #endregion

    #region Community Levels

    private static List<ImportLevelInformation> GetCommunityLevelsInDirectory(string path)
    {
        List<ImportLevelInformation> list = new();

        string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        foreach (string filePath in files)
        {
            string fileName = Path.GetFileName(filePath);

            // e.g. "image"
            string typeString = fileName.Split("-").Last();
            FileType fileType = GetFileTypeFromName(typeString);

            if (fileType == FileType.Unknown)
            {
                Console.WriteLine(
                    $"{fileName} is of unknown file type. The name of the file should end with \"-level\", \"-image\", \"-thumbnail\" or \"-sound\". Skipping...");
                continue;
            }

            // e.g. "f2a0ab5d-cf4a-3cb8-bcd3-9586257b8384"
            string identifier = fileName.Remove(fileName.Length - (typeString.Length + 1));

            ImportLevelInformation? info = list.FirstOrDefault(i => i.Id == identifier);
            if (info == null)
            {
                list.Add(new ImportLevelInformation());
                info = list.First(e => e.Id == identifier);
            }

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (fileType)
            {
                case FileType.Level:
                    info.LevelFilePath = filePath;
                    break;
                case FileType.Image:
                    info.ThumbnailFilePath = filePath;
                    break;
                case FileType.Sound:
                    info.SoundFilePath = filePath;
                    break;
            }
        }

        return list;
    }

    #endregion

    private static void RemoveEmptyDirectories(string rootDirectory, string directoryPath)
    {
        string[] subDirectories = Directory.GetDirectories(directoryPath);

        foreach (string subDirectory in subDirectories) RemoveEmptyDirectories(rootDirectory, subDirectory);

        if (Directory.GetFiles(directoryPath).Length != 0 ||
            Directory.GetDirectories(directoryPath).Length != 0) return;

        if (directoryPath == rootDirectory) return;

        Directory.Delete(directoryPath, false);
        Console.WriteLine("Deleted empty directory: {0}", directoryPath);
    }
}