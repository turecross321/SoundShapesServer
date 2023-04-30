using Bunkum.HttpServer.Authentication;
using Realms;
using SoundShapesServer.Authentication;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;

namespace SoundShapesServer.Types;

public class GameUser : RealmObject, IUser
{
    public GameUser(string id, string? email, string? passwordBcrypt, bool hasFinishedRegistration, IQueryable<IpAuthorization> ipAddresses, IQueryable<LevelLikeRelation> likedLevels, IQueryable<FollowRelation> followers, IQueryable<FollowRelation> following, IQueryable<GameSession> sessions, IQueryable<GameLevel> levels, IQueryable<Punishment> punishments, IQueryable<LeaderboardEntry> leaderboardEntries)
    {
        Id = id;
        Email = email;
        PasswordBcrypt = passwordBcrypt;
        HasFinishedRegistration = hasFinishedRegistration;
        IpAddresses = ipAddresses;
        LikedLevels = likedLevels;
        Followers = followers;
        Following = following;
        Sessions = sessions;
        Levels = levels;
        Punishments = punishments;
        LeaderboardEntries = leaderboardEntries;
    }
    
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public GameUser() { }
    #pragma warning restore CS8618

    public string Id { get; init; }
    public string Username { get; set; } = string.Empty;
    public int PermissionsType { get; set; } = (int)Types.PermissionsType.Default;
    public string? Email { get; set; }
    public string? PasswordBcrypt { get; set; }
    public bool HasFinishedRegistration { get; set; }
    
    [Backlink(nameof(IpAuthorization.User))] public IQueryable<IpAuthorization> IpAddresses { get; }

    [Backlink(nameof(LevelLikeRelation.Liker))] public IQueryable<LevelLikeRelation> LikedLevels { get; }
    
    [Backlink(nameof(FollowRelation.Recipient))] public IQueryable<FollowRelation> Followers { get; }
    
    [Backlink(nameof(FollowRelation.Follower))] public IQueryable<FollowRelation> Following { get; }
    [Backlink(nameof(GameSession.User))] public IQueryable<GameSession> Sessions { get; }
    
    [Backlink(nameof(GameLevel.Author))] public IQueryable<GameLevel> Levels { get; }
    [Backlink(nameof(Punishment.User))] public IQueryable<Punishment> Punishments { get; }
    [Backlink(nameof(LeaderboardEntry.User))] public IQueryable<LeaderboardEntry> LeaderboardEntries { get; }
}