using Realms;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.RecentActivity;
using SoundShapesServer.Types.Relations;

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
        UniquePlays = Enumerable.Empty<GameUser>().ToList();
        UsersWhoHaveCompletedLevel = Enumerable.Empty<GameUser>().ToList();
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
    public GameUser? Author { get; init; }
    public string Name { get; set; }
    public int Language { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public int Plays { get; set; }
    public int Deaths { get; set; }
    public IList<GameUser> UniquePlays { get; }
    public int CompletionCount { get; set; }
    public IList<GameUser> UsersWhoHaveCompletedLevel { get; }
    [Backlink(nameof(LevelLikeRelation.Level))] public IQueryable<LevelLikeRelation> Likes { get; }
    [Backlink(nameof(GameAlbum.Levels))] public IQueryable<GameAlbum> Albums { get; }
    [Backlink(nameof(DailyLevel.Level))] public IQueryable<DailyLevel> DailyLevels { get; }
    [Backlink(nameof(GameEvent.ContentLevel))] public IQueryable<GameEvent> Events { get; }
    public long FileSize { get; set; }
    public float Difficulty { get; set; }
}