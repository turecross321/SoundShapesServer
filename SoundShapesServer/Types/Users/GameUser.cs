using Bunkum.HttpServer.RateLimit;
using Realms;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Sessions;
#pragma warning disable CS8618

namespace SoundShapesServer.Types.Users;

public class GameUser : RealmObject, IRateLimitUser
{
    [PrimaryKey] [Required] public string Id { get; init; }
    public string Username { get; set; }

    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with PermissionsType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _PermissionsType { get; set; } = (int)PermissionsType.Default;
    public PermissionsType PermissionsType
    {
        get => (PermissionsType)_PermissionsType;
        set => _PermissionsType = (int)value;
    }
    
    public string? Email { get; set; }
    public string? PasswordBcrypt { get; set; }
    public bool HasFinishedRegistration { get; set; }
    public bool AllowPsnAuthentication { get; set; }
    public bool AllowRpcnAuthentication { get; set; }
    public bool AllowIpAuthentication { get; set; }
    public bool Deleted { get; init; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset LastGameLogin { get; set; }
    public string? SaveFilePath { get; set; }
    
    // ReSharper disable all UnassignedGetOnlyAutoProperty
    [Backlink(nameof(GameIp.User))]
    public IQueryable<GameIp> IpAddresses { get; }

    [Backlink(nameof(LevelLikeRelation.User))] public IQueryable<LevelLikeRelation> LikedLevels { get; }
    public int LikedLevelsCount { get; set; }
    [Backlink(nameof(LevelQueueRelation.User))] public IQueryable<LevelQueueRelation> QueuedLevels { get; }
    public int QueuedLevelsCount { get; set; }
    [Backlink(nameof(LevelUniquePlayRelation.User))] public IQueryable<LevelUniquePlayRelation> PlayedLevels { get; }
    public int PlayedLevelsCount { get; set; }
    [Backlink(nameof(GameLevel.UniqueCompletions))] public IQueryable<GameLevel> CompletedLevels { get; }
    public int CompletedLevelsCount { get; set; }
    [Backlink(nameof(FollowRelation.Recipient))] public IQueryable<FollowRelation> Followers { get; }
    public int FollowersCount { get; set; }
    [Backlink(nameof(FollowRelation.Follower))] public IQueryable<FollowRelation> Following { get; }
    public int FollowingCount { get; set; }
    [Backlink(nameof(GameSession.User))] public IQueryable<GameSession> Sessions { get; }
    [Backlink(nameof(GameLevel.Author))] public IQueryable<GameLevel> Levels { get; }
    public int LevelsCount { get; set; }
    public GameLevel? FeaturedLevel { get; set; }
    [Backlink(nameof(Punishment.Recipient))] public IQueryable<Punishment> Punishments { get; }
    [Backlink(nameof(LeaderboardEntry.User))] public IQueryable<LeaderboardEntry> LeaderboardEntries { get; }
    [Backlink(nameof(GameEvent.Actor))] public IQueryable<GameEvent> Events { get; }
    public int EventsCount { get; set; }
    [Backlink(nameof(GameEvent.ContentUser))] public IQueryable<GameEvent> EventsWhereUserIsRecipient { get; }
    public int Deaths { get; set; }
    public long TotalPlayTime { get; set; }

    // Defined in authentication provider. Avoids Realm threading nonsense.
    public bool RateLimitUserIdIsEqual(object obj)
    {
        if (obj is not string id) return false;
        return Id == id;
    }

    [Ignored] public object RateLimitUserId { get; internal set; } = null!;
}