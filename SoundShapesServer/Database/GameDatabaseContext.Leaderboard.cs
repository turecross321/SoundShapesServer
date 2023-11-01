using MongoDB.Bson;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public LeaderboardEntry CreateLeaderboardEntry(LeaderboardSubmissionRequest request, GameUser user, GameLevel level, PlatformType platformType)
    {
        LeaderboardEntry entry = new()
        {
            User = user,
            Level = level,
            Score = request.Score,
            PlayTime = Math.Max(request.PlayTime, 0),
            Deaths = Math.Max(request.Deaths, 0),
            Golded = request.Golded,
            Notes = request.Notes,
            Completed = request.Completed,
            CreationDate = DateTimeOffset.UtcNow,
            PlatformType = platformType
        };

        _realm.Write(() =>
        {
            _realm.Add(entry);
        });
        
        SetLevelPlayTime(level);
        CreateEvent(user, EventType.ScoreSubmission, platformType, EventDataType.LeaderboardEntry, entry.Id.ToString()!);
        SetUserPlayTime(user);
        
        return entry;
    }
    
    public void RemoveLeaderboardEntry(LeaderboardEntry entry)
    {
        RemoveAllReportsWithContentLeaderboardEntry(entry);
        
        _realm.Write(() =>
        {
            _realm.Remove(entry);
        });
    }
    
    public LeaderboardEntry? GetLeaderboardEntry(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId)) 
            return null;
        return _realm.All<LeaderboardEntry>().FirstOrDefault(e => e.Id == objectId);
    }

    public PaginatedList<LeaderboardEntry> GetPaginatedLeaderboardEntries(GameLevel level, LeaderboardOrderType order, bool descending, LeaderboardFilters filters, int from, int count, GameUser? accessor)
    {
        return new PaginatedList<LeaderboardEntry>(GetLeaderboardEntries(level, order, descending, filters), from, count);
    }

    public IQueryable<LeaderboardEntry> GetLeaderboardEntries(GameLevel level, LeaderboardOrderType order, bool descending,
        LeaderboardFilters filters)
    {
        return _realm.All<LeaderboardEntry>().FilterLeaderboard(level, filters).OrderLeaderboard(order, descending);
    }
}