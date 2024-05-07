using System.Diagnostics;
using System.Text.RegularExpressions;
using Bunkum.Core.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Levels.Importing;
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
            ImportLevelData? data = LoadLevelDependencies(info);
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
        for (int i = 0; i < informationList.Count; i++)
        {
            ImportLevelInformation info = informationList[i];
            ImportLevelData? data = LoadLevelDependencies(info);
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

    private static ImportLevelData? LoadLevelDependencies(ImportLevelInformation info)
    {
        bool missingRequiredPath =
            string.IsNullOrEmpty(info.LevelFilePath) || string.IsNullOrEmpty(info.ThumbnailFilePath);

        if (!info.UploadIfMissingFiles && (missingRequiredPath || !File.Exists(info.LevelFilePath) ||
                                           !File.Exists(info.ThumbnailFilePath)))
        {
            Console.WriteLine(
                $"{info.Name ?? "Level"} doesn't have all required files. It must have a level file and an image / thumbnail file. Skipping...");
            return null;
        }

        byte[]? levelBytes = null;
        DateTimeOffset lastWriteDate = default;
        if (File.Exists(info.LevelFilePath))
        {
            levelBytes = File.ReadAllBytes(info.LevelFilePath);
            File.GetLastWriteTimeUtc(info.LevelFilePath);
        }

        if (levelBytes is { Length: < 300 })
        {
            Console.WriteLine(info.LevelFilePath + " is less than 300 bytes. Skipping...");
            return null;
        }

        byte[]? thumbnailBytes = null;
        if (File.Exists(info.ThumbnailFilePath))
            thumbnailBytes = File.ReadAllBytes(info.ThumbnailFilePath);

        byte[]? soundBytes = null;
        if (File.Exists(info.SoundFilePath))
            soundBytes = File.ReadAllBytes(info.SoundFilePath);
        
        ImportLevelData level = new()
        {
            LevelWriteDate = lastWriteDate,
            LevelFile = levelBytes,
            Thumbnail = thumbnailBytes,
            SoundFile = soundBytes
        };

        return level;
    }

    #region Community Levels

    private static List<ImportLevelInformation> GetCommunityLevelsInDirectory(string path)
    {
        List<ImportLevelInformation> list = new();

        Console.WriteLine("Beginning to scan community import directory...");
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
                list.Add(new ImportLevelInformation
                {
                    Id = identifier,
                    CampaignLevel = false,
                    UploadIfMissingFiles = false
                });
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

            Console.WriteLine($"Added {filePath} to import queue");
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

    #region Campaign Levels

    private static bool UploadLevel(ImportLevelInformation info, ImportLevelData data, GameDatabaseContext database,
        IDataStore dataStore,
        GameUser adminUser, bool overwrite)
    {
        string name = info.Name ?? $"Imported Level ({info.Id})";
        DateTimeOffset date = !info.CampaignLevel ? data.LevelWriteDate : new DateTimeOffset();
        const LevelVisibility visibility = LevelVisibility.Public;

        GameLevel? level = database.GetLevelWithId(info.Id);
        if (level != null)
        {
            if (!overwrite)
            {
                Console.WriteLine($"There is already a level with the id: {info.Id}. Skipping...");
                return false;
            }

            Console.WriteLine($"{info.Id} has already been imported. Editing metadata...");
            database.EditLevel(level, name, 0, visibility, date, info.CampaignLevel);
        }
        else
        {
            level = new GameLevel
            {
                Id = info.Id,
                Author = adminUser,
                Name = name,
                Visibility = visibility,
                UploadPlatform = PlatformType.Unknown,
                CreationDate = date,
                ModificationDate = date,
                CampaignLevel = info.CampaignLevel
            };

            database.AddLevel(level, false);
        }

        if (data.LevelFile != null)
            database.UploadLevelResource(dataStore, level, data.LevelFile, FileType.Level);
        if (data.Thumbnail != null)
            database.UploadLevelResource(dataStore, level, data.Thumbnail, FileType.Image);
        if (data.SoundFile != null)
            database.UploadLevelResource(dataStore, level, data.SoundFile, FileType.Sound);

        Console.WriteLine($"Successfully imported {level.Id}.");
        return true;
    }

    private static List<ImportLevelInformation> GetLevelsFromRecordList(string recordListPath, string resourcesPath)
    {
        Console.WriteLine("Beginning to scan campaign import directory...");

        List<ImportLevelInformation> levels = new();

        string recordListScript = File.ReadAllText(recordListPath);

        const string recordListGlobalName = "campaignList = ";
        int startIndex = recordListScript.IndexOf(recordListGlobalName, StringComparison.Ordinal);

        if (startIndex == -1)
            throw new Exception($"campaignList could not be found in {recordListPath}");

        string recordList = recordListScript[(startIndex + recordListGlobalName.Length)..].Split("};").First();
        // Remove all /* */ comments
        recordList = Regex.Replace(recordList, @"/\*(.|\n)*?\*/", string.Empty);
        // Remove all single line comments
        recordList =
            Regex.Replace(recordList, @"//.*$", string.Empty,
                RegexOptions.Multiline); // Remove "//" and everything after it

        List<string> records = GetChildrenElements(recordList);

        foreach (string record in records)
        {
            // ReSharper disable once CommentTypo
            // Remove "bsides" levels (aka. trophy levels).
            // These don't have any server interaction, so there's no reason to import them
            string filtered = record.Split("bsides").First();
            List<string> elements = GetChildrenElements(filtered);
            List<string> tracks = GetChildrenElements(elements.First());


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
                    LevelFilePath = Path.Combine(resourcesPath,
                        baseFileName + ".json.z"),
                    ThumbnailFilePath = Path.Combine(resourcesPath,
                        baseFileName + ".png"),
                    CampaignLevel = true,
                    SoundFilePath = null,
                    UploadIfMissingFiles = true
                });

                Console.WriteLine($"Added {name} to import queue)");
            }
        }

        return levels;
    }

    private static List<string> GetChildrenElements(string input)
    {
        // Remove root {
        int firstOpeningBracketIndex = input.IndexOf("{", StringComparison.Ordinal);
        input = input.Substring(firstOpeningBracketIndex + 1);

        List<int> openedBracketPositions = new();
        List<string> elements = new();
        for (int i = 0; i < input.Length; i++)
        {
            char character = input[i];
            switch (character)
            {
                case '{':
                    openedBracketPositions.Add(i);
                    break;
                case '}':
                    if (openedBracketPositions.Count == 1)
                    {
                        int openingPosition = openedBracketPositions.First();
                        int length = i - openingPosition + 1;
                        elements.Add(input.Substring(openingPosition, length));
                        openedBracketPositions.Remove(openedBracketPositions.First());
                    }
                    else if (openedBracketPositions.Any())
                    {
                        openedBracketPositions.RemoveAt(openedBracketPositions.Count - 1);
                    }

                    break;
            }
        }

        return elements;
    }

    #endregion
}