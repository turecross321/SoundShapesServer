using Bunkum.HttpServer.Authentication;
using Realms;
using SoundShapesServer.Authentication;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;

namespace SoundShapesServer.Types;

public class GameUser : RealmObject, IUser
{
    public string Id { get; init; } = "";
    public string Username { get; set; } = "";
    public int PermissionsType { get; set; } = (int)Types.PermissionsType.Default;
    public string? Email { get; set; }
    public string? PasswordBcrypt { get; set; }
    public bool HasFinishedRegistration { get; set; }
    public DateTimeOffset CreationDate { get; set; }

    [Backlink(nameof(IpAuthorization.User))]
    public IQueryable<IpAuthorization> IpAddresses { get; } = Enumerable.Empty<IpAuthorization>().AsQueryable();

    [Backlink(nameof(LevelLikeRelation.Liker))] public IQueryable<LevelLikeRelation> LikedLevels { get; } = Enumerable.Empty<LevelLikeRelation>().AsQueryable();
    [Backlink(nameof(GameLevel.UniquePlays))] public IQueryable<GameLevel> PlayedLevels { get; } = Enumerable.Empty<GameLevel>().AsQueryable();

    [Backlink(nameof(FollowRelation.Recipient))] public IQueryable<FollowRelation> Followers { get; } = Enumerable.Empty<FollowRelation>().AsQueryable();
    
    [Backlink(nameof(FollowRelation.Follower))] public IQueryable<FollowRelation> Following { get; } = Enumerable.Empty<FollowRelation>().AsQueryable();
    [Backlink(nameof(GameSession.User))] public IQueryable<GameSession> Sessions { get; } = Enumerable.Empty<GameSession>().AsQueryable();
    
    [Backlink(nameof(GameLevel.Author))] public IQueryable<GameLevel> Levels { get; } = Enumerable.Empty<GameLevel>().AsQueryable();
    [Backlink(nameof(Punishment.User))] public IQueryable<Punishment> Punishments { get; } = Enumerable.Empty<Punishment>().AsQueryable();
    [Backlink(nameof(LeaderboardEntry.User))] public IQueryable<LeaderboardEntry> LeaderboardEntries { get; } = Enumerable.Empty<LeaderboardEntry>().AsQueryable();
}