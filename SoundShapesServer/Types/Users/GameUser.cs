using Bunkum.Core.RateLimit;
using Realms;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Relations;

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
        get
        {
            if (Deleted)
                return PermissionsType.Deleted;
            
            if (Punishments.ActiveBans().Any())
                return PermissionsType.Banned;
            
            return (PermissionsType)_PermissionsType;
        }
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
    public DateTimeOffset? LastGameLogin { get; set; }
    public string? SaveFilePath { get; set; }
    
    // ReSharper disable all UnassignedGetOnlyAutoProperty
    [Backlink(nameof(GameIp.User))]
    public IQueryable<GameIp> IpAddresses { get; }

    [Backlink(nameof(LevelLikeRelation.User))] public IQueryable<LevelLikeRelation> LikedLevelRelations { get; }
    public int LikedLevelsCount { get; set; }
    [Backlink(nameof(LevelQueueRelation.User))] public IQueryable<LevelQueueRelation> QueuedLevelRelations { get; }
    public int QueuedLevelsCount { get; set; }
    [Backlink(nameof(LevelUniquePlayRelation.User))] public IQueryable<LevelUniquePlayRelation> PlayedLevelRelations { get; }
    public int PlayedLevelsCount { get; set; }
    [Backlink(nameof(GameLevel.UniqueCompletions))] public IQueryable<GameLevel> CompletedLevels { get; }
    public int CompletedLevelsCount { get; set; }
    [Backlink(nameof(UserFollowRelation.Recipient))] public IQueryable<UserFollowRelation> FollowersRelations { get; }
    public int FollowersCount { get; set; }
    [Backlink(nameof(UserFollowRelation.Follower))] public IQueryable<UserFollowRelation> FollowingRelations { get; }
    public int FollowingCount { get; set; }
    [Backlink(nameof(GameToken.User))] public IQueryable<GameToken> Tokens { get; }
    [Backlink(nameof(GameLevel.Author))] public IQueryable<GameLevel> Levels { get; }
    public int LevelsCount { get; set; }
    [Backlink(nameof(Punishment.Recipient))] public IQueryable<Punishment> Punishments { get; }
    [Backlink(nameof(LeaderboardEntry.User))] public IQueryable<LeaderboardEntry> LeaderboardEntries { get; }
    [Backlink(nameof(GameEvent.Actor))] public IQueryable<GameEvent> Events { get; }
    public int EventsCount { get; set; }
    [Backlink(nameof(NewsEntry.Author))] public IQueryable<NewsEntry> NewsEntries { get; }
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