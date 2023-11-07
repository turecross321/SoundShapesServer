using Bunkum.Core.Storage;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Levels.SSLevel;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.LevelHelper;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public void AddLevel(GameLevel level, bool createEvent)
    {
        GameUser author = level.Author;

        _realm.Write(() =>
        {
            _realm.Add(level);
            author.LevelsCount = author.Levels.Count();
        });

        if (createEvent)
            CreateEvent(author, EventType.LevelPublish, level.UploadPlatform, EventDataType.Level, level.Id);
    }

    public GameLevel EditLevel(GameLevel level, string name, int? language = null, LevelVisibility? visibility = null,
        DateTimeOffset? creationDate = null, bool? campaignLevel = null)
    {
        _realm.Write(() =>
        {
            level.Name = AdhereToLevelNameCharacterLimit(name);
            level.Language = language ?? level.Language;
            level.Visibility = visibility ?? level.Visibility;
            level.CreationDate = creationDate ?? level.CreationDate;
            level.ModificationDate = creationDate ?? DateTimeOffset.UtcNow;
            level.CampaignLevel = campaignLevel ?? level.CampaignLevel;
        });

        return level;
    }

    private void SetLevelPlayTime(GameLevel level)
    {
        long totalPlayTime = level.LeaderboardEntries.AsEnumerable().Sum(e => e.PlayTime);

        _realm.Write(() => { level.TotalPlayTime = totalPlayTime; });
    }

    public bool AnalyzeLevel(GameLevel level, byte[] levelFile)
    {
        SSLevel? ssLevel = SSLevel.FromLevelFile(levelFile);
        if (ssLevel == null)
            return false;

        _realm.Write(() =>
        {
            level.FileSize = levelFile.Length;
            level.Bpm = ssLevel.Bpm;
            level.TransposeValue = ssLevel.TransposeValue;
            level._Scale = ssLevel.ScaleIndex;
            level.TotalScreens = ssLevel.ScreenData.Count();
            level.TotalEntities = ssLevel.Entities.Count() + ssLevel.EntitiesB?.Count() ?? 0;
            level.HasCar =
                ssLevel.EntitiesB?.Any(e => e.EntityType == "Platformer_EntityPacks_GameStuff_CarCheckpoint") ?? false;
            level.HasExplodingCar = ssLevel.EntitiesB?.Any(e =>
                e.EntityType == "Platformer_EntityPacks_GameStuff_ExplodingCarCheckpoint") ?? false;
            level.HasUfo =
                ssLevel.EntitiesB?.Any(e => e.EntityType == "Platformer_EntityPacks_GameStuff_UFOCheckpoint") ?? false;
            level.HasFirefly =
                ssLevel.EntitiesB?.Any(e => e.EntityType == "Platformer_EntityPacks_GameStuff_FireflyCheckpoint") ??
                false;
        });

        return true;
    }

    public ApiOkResponse UploadLevelResource(IDataStore dataStore, GameLevel level,
        byte[] file, FileType fileType)
    {
        if (fileType == FileType.Image && !file.IsPng())
            return ApiBadRequestError.FileIsNotPng;

        if (fileType == FileType.Level)
            if (!AnalyzeLevel(level, file))
                return ApiBadRequestError.CorruptLevel;

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
            foreach (GameAlbum album in level.Albums) album.Levels.Remove(level);
            _realm.RemoveRange(level.DailyLevels);
            _realm.RemoveRange(level.Likes);
            _realm.RemoveRange(level.LeaderboardEntries);
            _realm.Remove(level);
        });
    }

    public void AddCompletionToLevel(GameUser user, GameLevel level)
    {
        if (!level.UniqueCompletions.Contains(user)) AddUniqueCompletion(user, level);

        _realm.Write(() => { level.CompletionCount++; });
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

        _realm.Write(() => { level.Difficulty = CalculateLevelDifficulty(level); });
    }

    public void AddDeathsToLevel(GameUser user, GameLevel level, int deaths)
    {
        _realm.Write(() =>
        {
            level.TotalDeaths += deaths;
            user.Deaths += deaths;
        });
    }

    public GameLevel? GetLevelWithId(string id)
    {
        return _realm.All<GameLevel>().FirstOrDefault(l => l.Id == id);
    }

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

    public PaginatedList<GameLevel> GetPaginatedLevels(LevelOrderType order, bool descending, LevelFilters filters,
        int from, int count, GameUser? accessor)
    {
        return new PaginatedList<GameLevel>(GetLevels(order, descending, filters, accessor), from, count);
    }

    public IQueryable<GameLevel> GetLevels(LevelOrderType order, bool descending, LevelFilters filters,
        GameUser? accessor)
    {
        return _realm.All<GameLevel>().FilterLevels(this, filters, accessor).OrderLevels(order, descending);
    }
}