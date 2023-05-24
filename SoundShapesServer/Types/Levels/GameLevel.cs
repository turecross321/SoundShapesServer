using Realms;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.PlayerActivity;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Levels;

public class GameLevel : RealmObject
{
    public GameLevel(string id, GameUser author, string name, int language, long fileSize, DateTimeOffset creationDate)
    {
        Id = id;
        Author = author;
        Name = name;
        Language = language;
        CreationDate = creationDate;
        ModificationDate = creationDate;
        Plays = Enumerable.Empty<LevelPlayRelation>().AsQueryable();
        UniquePlays = Enumerable.Empty<LevelUniquePlayRelation>().AsQueryable();
        UniqueCompletions = Enumerable.Empty<GameUser>().ToList();
        Likes = Enumerable.Empty<LevelLikeRelation>().AsQueryable();
        Albums = Enumerable.Empty<GameAlbum>().AsQueryable();
        DailyLevels = Enumerable.Empty<DailyLevel>().AsQueryable();
        Events = Enumerable.Empty<GameEvent>().AsQueryable();
        FileSize = fileSize;
    }
    
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public GameLevel() { }
    #pragma warning restore CS8618

    [PrimaryKey] [Required] public string Id { get; set; }
    public GameUser Author { get; init; }
    public string Name { get; set; }
    public int Language { get; set; }
    public string? LevelFilePath { get; set; }
    public string? ThumbnailFilePath { get; set; }
    public string? SoundFilePath { get; set; }
    public int Bpm { get; set; }
    public int TransposeValue { get; set; }
    public int ScaleIndex { get; set; }
    public int TotalScreens { get; set; }
    public int TotalEntities { get; set; }
    public bool HasCar { get; set; }
    public bool HasExplodingCar { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public long TotalPlayTime { get; set; }
    public int TotalDeaths { get; set; }
    [Backlink(nameof(LevelPlayRelation.Level))] public IQueryable<LevelPlayRelation> Plays { get; }
    public int PlaysCount { get; set; }
    [Backlink(nameof(LevelUniquePlayRelation.Level))] public IQueryable<LevelUniquePlayRelation> UniquePlays { get; }
    public int UniquePlaysCount { get; set; }
    public IList<GameUser> UniqueCompletions { get; }
    public int CompletionCount { get; set; }
    public int UniqueCompletionsCount { get; set; }
    [Backlink(nameof(LevelLikeRelation.Level))] public IQueryable<LevelLikeRelation> Likes { get; }
    public int LikesCount { get; set; }
    [Backlink(nameof(GameAlbum.Levels))] public IQueryable<GameAlbum> Albums { get; }
    [Backlink(nameof(DailyLevel.Level))] public IQueryable<DailyLevel> DailyLevels { get; }
    [Backlink(nameof(GameEvent.ContentLevel))] public IQueryable<GameEvent> Events { get; }
    public long FileSize { get; set; }
    public float Difficulty { get; set; }
}