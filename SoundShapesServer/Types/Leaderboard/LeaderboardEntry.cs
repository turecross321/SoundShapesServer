using MongoDB.Bson;
using Realms;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Leaderboard;

public class LeaderboardEntry : RealmObject
{
    [PrimaryKey] public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public GameUser User { get; init; }
    public GameLevel Level { get; set; }
    public long Score { get; init; }
    public long PlayTime { get; init; }
    public int Deaths { get; init; }
    // ReSharper disable once IdentifierTypo
    public int Golded { get; init; }
    public int Notes { get; set; }
    public bool Completed { get; init; }
    public DateTimeOffset CreationDate { get; set; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with PlatformType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _PlatformType { get; set; } = (int)PlatformType.Unknown;
    public PlatformType PlatformType
    {
        get => (PlatformType)_PlatformType;
        init => _PlatformType = (int)value;
    }
    
    public int Position()
    {
        IQueryable<LeaderboardEntry> entries = Level.LeaderboardEntries
            .FilterLeaderboard(new LeaderboardFilters(Level, obsolete: false))
            .OrderLeaderboard(LeaderboardOrderType.Score, false);
        
        return entries.ToList().IndexOf(this);
    }
}