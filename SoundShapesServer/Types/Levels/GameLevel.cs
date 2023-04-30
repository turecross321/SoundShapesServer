using Realms;
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
        CreationDate = DateTimeOffset.UtcNow;
        ModificationDate = DateTimeOffset.UtcNow;
        UniquePlays = Enumerable.Empty<GameUser>().ToList();
        UsersWhoHaveCompletedLevel = Enumerable.Empty<GameUser>().ToList();
        Likes = Enumerable.Empty<LevelLikeRelation>().AsQueryable();
        FileSize = fileSize;
    }
    
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public GameLevel() { }
    #pragma warning restore CS8618

    public string Id { get; set; }
    public GameUser Author { get; init; }
    public string Name { get; set; }
    public int Language { get; set; }
    public DateTimeOffset CreationDate { get; }
    public DateTimeOffset ModificationDate { get; set; }
    public int Plays { get; set; }
    public int Deaths { get; set; }
    public IList<GameUser> UniquePlays { get; }
    public int CompletionCount { get; set; }
    public IList<GameUser> UsersWhoHaveCompletedLevel { get; }
    [Backlink(nameof(LevelLikeRelation.Level))] public IQueryable<LevelLikeRelation> Likes { get; }
    public long FileSize { get; set; }
}