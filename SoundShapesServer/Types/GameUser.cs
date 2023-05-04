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

    #pragma warning disable CS8618
    // ReSharper disable all UnassignedGetOnlyAutoProperty
    
    [Backlink(nameof(IpAuthorization.User))]
    public IQueryable<IpAuthorization> IpAddresses { get; }

    [Backlink(nameof(LevelLikeRelation.Liker))] public IQueryable<LevelLikeRelation> LikedLevels { get; }
    [Backlink(nameof(GameLevel.UniquePlays))] public IQueryable<GameLevel> PlayedLevels { get; }
    [Backlink(nameof(FollowRelation.Recipient))] public IQueryable<FollowRelation> Followers { get; }
    
    [Backlink(nameof(FollowRelation.Follower))] public IQueryable<FollowRelation> Following { get; }
    [Backlink(nameof(GameSession.User))] public IQueryable<GameSession> Sessions { get; }
    
    [Backlink(nameof(GameLevel.Author))] public IQueryable<GameLevel> Levels { get; }
    [Backlink(nameof(Punishment.User))] public IQueryable<Punishment> Punishments { get; }
    [Backlink(nameof(LeaderboardEntry.User))] public IQueryable<LeaderboardEntry> LeaderboardEntries { get; }
#pragma warning restore CS8618
}