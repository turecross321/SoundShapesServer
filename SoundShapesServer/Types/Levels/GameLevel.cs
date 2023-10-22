using Realms;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;
#pragma warning disable CS8618

namespace SoundShapesServer.Types.Levels;

public class GameLevel : RealmObject
{
    [PrimaryKey] [Required] public required string Id { get; init; }
    public required GameUser Author { get; init; }
    public required string Name { get; set; }
    public int Language { get; set; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with PunishmentType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _Visibility { get; set; }
    public LevelVisibility Visibility
    {
        get => (LevelVisibility)_Visibility;
        set => _Visibility = (int)value;
    }
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with PunishmentType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _UploadPlatform { get; set; }
    public PlatformType UploadPlatform
    {
        get => (PlatformType)_UploadPlatform;
        set => _UploadPlatform = (int)value;
    }
    public string? LevelFilePath { get; set; }
    public string? ThumbnailFilePath { get; set; }
    public string? SoundFilePath { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    // ReSharper disable UnassignedGetOnlyAutoProperty
    [Backlink(nameof(LevelPlayRelation.Level))] public IQueryable<LevelPlayRelation> Plays { get; }
    public int PlaysCount { get; set; }
    [Backlink(nameof(LevelUniquePlayRelation.Level))] public IQueryable<LevelUniquePlayRelation> UniquePlays { get; }
    public int UniquePlaysCount { get; set; }
    public IList<GameUser> UniqueCompletions { get; }
    public int CompletionCount { get; set; }
    public int UniqueCompletionsCount { get; set; }
    [Backlink(nameof(LevelLikeRelation.Level))] public IQueryable<LevelLikeRelation> Likes { get; }
    public int LikesCount { get; set; }
    [Backlink(nameof(LevelLikeRelation.Level))] public IQueryable<LevelQueueRelation> Queues { get; }
    public int QueuesCount { get; set; }
    [Backlink(nameof(GameAlbum.Levels))] public IQueryable<GameAlbum> Albums { get; }
    [Backlink(nameof(DailyLevel.Level))] public IQueryable<DailyLevel> DailyLevels { get; }
    [Backlink(nameof(LeaderboardEntry.Level))] public IQueryable<LeaderboardEntry> LeaderboardEntries { get; }
    // ReSharper restore UnassignedGetOnlyAutoProperty
    public long FileSize { get; set; }
    public float Difficulty { get; set; }
    public int Bpm { get; set; }
    public int TransposeValue { get; set; }
    public int ScaleIndex { get; set; }
    public int TotalScreens { get; set; }
    public int TotalEntities { get; set; }
    public bool HasCar { get; set; }
    public bool HasExplodingCar { get; set; }
    public long TotalPlayTime { get; set; }
    public int TotalDeaths { get; set; }
}