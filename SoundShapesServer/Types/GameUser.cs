using Bunkum.HttpServer.Authentication;
using Realms;
using SoundShapesServer.Authentication;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;

namespace SoundShapesServer.Types;

public class GameUser : RealmObject, IUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public ResponseType Type = ResponseType.identity;
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PasswordBcrypt { get; set; }
    public bool HasFinishedRegistration { get; set; } = false;
    public IList<string> AuthorizedIPAddresses { get; }

    [Backlink(nameof(LevelLikeRelation.Liker))] public IQueryable<LevelLikeRelation> LikedLevels { get; }
    
    [Backlink(nameof(FollowRelation.Recipient))] public IQueryable<FollowRelation> Followers { get; }
    
    [Backlink(nameof(FollowRelation.Follower))] public IQueryable<FollowRelation> Following { get; }
    [Backlink(nameof(GameSession.User))] public IQueryable<GameSession> Sessions { get; }
    
    [Backlink(nameof(GameLevel.Author))] public IQueryable<GameLevel> Levels { get; }
}