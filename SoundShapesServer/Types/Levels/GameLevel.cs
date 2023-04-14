using Realms;
using SoundShapesServer.Types.Relations;

namespace SoundShapesServer.Types.Levels;

public class GameLevel : RealmObject
{
    public string Id { get; set; }
    public GameUser Author { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Language { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public int Plays { get; set; }
    public int Deaths { get; set; }
    public IList<GameUser> UniquePlays { get; }
    public int CompletionCount { get; set; }
    public IList<GameUser> UsersWhoHaveCompletedLevel { get; }
    [Backlink(nameof(LevelLikeRelation.Level))] public IQueryable<LevelLikeRelation> Likes { get; }
    public long FileSize { get; set; }
}