using Bunkum.Core.Storage;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.LevelHelper;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public GameLevel CreateLevel(GameUser user, PublishLevelRequest request, PlatformType uploadPlatform, bool createEvent = true, string? levelId = null)
    {
        levelId ??= GenerateLevelId();
        GameLevel level = new GameLevel()
        {
            Id = levelId,
            Author = user,
            Name = request.Name,
            Language = request.Language,
            CreationDate = request.CreationDate,
            FileSize = request.FileSize,
            ModificationDate = DateTimeOffset.UtcNow,
            Visibility = LevelVisibility.Public,
            UploadPlatform = uploadPlatform,
        };

        _realm.Write(() =>
        {
            _realm.Add(level);
            user.LevelsCount = user.Levels.Count();
        });

        if (createEvent) CreateEvent(user, EventType.LevelPublish, uploadPlatform, EventDataType.Level, level.Id);
        
        return level;
    }

    public GameLevel EditLevel(PublishLevelRequest updatedPublishLevel, GameLevel level)
    {
        _realm.Write(() =>
        {
            level.Name = AdhereToLevelNameCharacterLimit(updatedPublishLevel.Name);
            level.Visibility = updatedPublishLevel.Visibility;
            level.ModificationDate = DateTimeOffset.UtcNow;
        });

        return level;
    }

    private void SetLevelPlayTime(GameLevel level)
    {
        long totalPlayTime = level.LeaderboardEntries.AsEnumerable().Sum(e => e.PlayTime);

        _realm.Write(() =>
        {
            level.TotalPlayTime = totalPlayTime;
        });
    }

    public void UploadLevelResources(IDataStore dataStore, GameLevel level, byte[] levelFile, byte[] thumbnailFile,
        byte[] soundFile)
    {
        UploadLevelResource(dataStore, level, levelFile, FileType.Level);
        UploadLevelResource(dataStore, level, thumbnailFile, FileType.Image);
        UploadLevelResource(dataStore, level, soundFile, FileType.Sound);
    }

    private bool SetLevelInfo(GameLevel level, byte[] levelFile)
    {
        JObject? deCompressedLevel = levelFile.LevelFileToJObject();
        if (deCompressedLevel == null)
            return false;
        
        int bpm = deCompressedLevel.Value<int?>("bpm") ?? 120;
        int transposeValue = deCompressedLevel.Value<int?>("transposeValue") ?? 0;
        int scaleIndex = deCompressedLevel.Value<int?>("scaleIndex") ?? 0;
        int screensCount = (deCompressedLevel.GetValue("screenData") ?? throw new InvalidOperationException()).Count();

        int totalEntities = deCompressedLevel.GetValue("entities")?.Count() ?? 0;
        // This was added in later versions of the game, and it replaces entities
        int totalEntitiesB = deCompressedLevel.GetValue("entitiesB")?.Count() ?? 0;

        int entitiesCount = Math.Max(totalEntities, totalEntitiesB);

        bool hasCar = deCompressedLevel.GetValue("entityTypesUsed")
            ?.Values<string>().FirstOrDefault
                // ReSharper disable once StringLiteralTypo
                (e => e == "Platformer_EntityPacks_GameStuff_CarCheckpoint") != null;
        
        bool hasExplodingCar = deCompressedLevel.GetValue("entityTypesUsed")
            ?.Values<string>().FirstOrDefault
                // ReSharper disable once StringLiteralTypo
                (e => e == "Platformer_EntityPacks_GameStuff_ExplodingCarCheckpoint") != null;

        _realm.Write(() =>
        {
            level.FileSize = levelFile.Length;
            level.Bpm = bpm;
            level.TransposeValue = transposeValue;
            level.ScaleIndex = scaleIndex;
            level.TotalScreens = screensCount;
            level.TotalEntities = entitiesCount;
            level.HasCar = hasCar;
            level.HasExplodingCar = hasExplodingCar;
        });

        return true;
    }
    
    public ApiOkResponse UploadLevelResource(IDataStore dataStore, GameLevel level,
        byte[] file, FileType fileType)
    {
        if (fileType == FileType.Image && !file.IsPng())
            return ApiBadRequestError.FileIsNotPng;

        if (fileType == FileType.Level)
        {
            if (!SetLevelInfo(level, file))
                return ApiBadRequestError.CorruptLevel;
        }
        
        string key = GetLevelResourceKey(level, fileType);
        dataStore.WriteToStore(key, file);
        
        SetLevelFilePath(level, fileType, key);
        return new ApiOkResponse();
    }

    private void SetLevelFilePath(GameLevel level, FileType fileType, string path)
    {
        _realm.Write(() =>
        {
            switch (fileType)
            {
                case FileType.Level:
                    level.LevelFilePath = path;
                    break;
                case FileType.Image:
                    level.ThumbnailFilePath = path;
                    break;
                case FileType.Sound:
                    level.SoundFilePath = path;
                    break;
                case FileType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }
        });
    }

    private static void RemoveLevelResources(GameLevel level, IDataStore dataStore)
    {
        if (level.LevelFilePath != null) dataStore.RemoveFromStore(level.LevelFilePath);
        if (level.ThumbnailFilePath != null) dataStore.RemoveFromStore(level.ThumbnailFilePath);
        if (level.SoundFilePath != null) dataStore.RemoveFromStore(level.SoundFilePath);
    }
    
    public void RemoveLevel(GameLevel level, IDataStore dataStore)
    {
        RemoveLevelResources(level, dataStore);
        RemoveAllReportsWithContentLevel(level);
        RemoveEventsOnLevel(level);
        
        _realm.Write(() =>
        {
            foreach (GameAlbum album in level.Albums)
            {
                album.Levels.Remove(level);
            }
            _realm.RemoveRange(level.DailyLevels);
            _realm.RemoveRange(level.Likes);
            _realm.RemoveRange(level.LeaderboardEntries);
            _realm.Remove(level);
        });
    }
    
    public void AddCompletionToLevel(GameUser user, GameLevel level)
    {
        if (!level.UniqueCompletions.Contains(user)) AddUniqueCompletion(user, level);
        
        _realm.Write(() =>
        {
            level.CompletionCount++;
        });
    }

    private void AddUniqueCompletion(GameUser user, GameLevel level)
    {
        _realm.Write(() =>
        {
            level.UniqueCompletions.Add(user);
            level.UniqueCompletionsCount = level.UniqueCompletions.Count;
            user.CompletedLevelsCount = user.CompletedLevels.Count();
        });
    }
    
    public void SetLevelDifficulty(GameLevel level)
    {
        _realm.Refresh();

        _realm.Write(() =>
        {
            level.Difficulty = CalculateLevelDifficulty(level);
        });
    }
    public void AddDeathsToLevel(GameUser user, GameLevel level, int deaths)
    {
        _realm.Write(() =>
        {
            level.TotalDeaths += deaths;
            user.Deaths += deaths;
        });
    }
    
    public GameLevel? GetLevelWithId(string id) => _realm.All<GameLevel>().FirstOrDefault(l => l.Id == id);

    private IQueryable<GameLevel> GetLevelsWithIds(IEnumerable<string> ids)
    {
        List<GameLevel> levels = new();
        
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (string levelId in ids) 
        {
            GameLevel? level = GetLevelWithId(levelId);
            if (level != null) levels.Add(level);
        }

        return levels.AsQueryable();
    }

    public (GameLevel[], int) GetPaginatedLevels(LevelOrderType order, bool descending, LevelFilters filters, int from, int count, GameUser? accessor)
    {
        IQueryable<GameLevel> levels = GetLevels(order, descending, filters, accessor);
        return (levels.Paginate(from, count), levels.Count());
    }

    public IQueryable<GameLevel> GetLevels(LevelOrderType order, bool descending, LevelFilters filters, GameUser? accessor)
    {
        return _realm.All<GameLevel>().FilterLevels(this, filters, accessor).OrderLevels(order, descending);
    }
}