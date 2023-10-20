using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PaginationHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public LeaderboardEntry CreateLeaderboardEntry(LeaderboardSubmissionRequest request, GameUser user, GameLevel level, PlatformType platformType)
    {
        LeaderboardEntry entry = new()
        {
            Id = GenerateGuid(),
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
        CreateEvent(user, EventType.ScoreSubmission, platformType, null, level, entry);
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
    
    public LeaderboardEntry? GetLeaderboardEntryWithId(string id)
    {
        return _realm.All<LeaderboardEntry>().FirstOrDefault(e => e.Id == id);
    }

    public (LeaderboardEntry[], int) GetPaginatedLeaderboardEntries(LeaderboardOrderType order, bool descending, LeaderboardFilters filters, int from, int count, GameUser? accessor)
    {
        IQueryable<LeaderboardEntry> entries = GetLeaderboardEntries(order, descending, filters);
        return (entries.Paginate(from, count), entries.Count());
    }

    public IQueryable<LeaderboardEntry> GetLeaderboardEntries(LeaderboardOrderType order, bool descending,
        LeaderboardFilters filters)
    {
        return _realm.All<LeaderboardEntry>().FilterLeaderboard(filters).OrderLeaderboard(order, descending);
    }
}