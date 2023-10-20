using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    private void CreateEvent(GameUser actor, EventType eventType, PlatformType platformType, GameUser? user = null, GameLevel? level = null, LeaderboardEntry? leaderboardEntry = null)
    {
        GameEvent eventObject = new()
        {
            Id = Guid.NewGuid().ToString(),
            Actor = actor,

            ContentUser = user,
            ContentLevel = level,
            ContentLeaderboardEntry = leaderboardEntry,
        
            EventType = eventType,
            CreationDate = DateTimeOffset.UtcNow,
            PlatformType = platformType
        };
        
        GameEvent? previousEvent = _realm
            .All<GameEvent>()
            .AsEnumerable()
            .Where(e => e.Actor.Id == eventObject.Actor.Id)
            .FirstOrDefault(e => 
                e.EventType == eventObject.EventType 
                && Equals(e.ContentUser, user) 
                && Equals(e.ContentLevel, level) 
                && Equals(e.ContentLeaderboardEntry, leaderboardEntry));

        if (previousEvent != null) return;
        
        _realm.Write(() =>
        {
            _realm.Add(eventObject);
            actor.EventsCount = actor.Events.Count();
        });
    }

    public void RemoveEvent(GameEvent eventObject)
    {
        _realm.Write(() =>
        {
            _realm.Remove(eventObject);
            eventObject.Actor.EventsCount = eventObject.Actor.Events.Count();
        });
    }
    
    public GameEvent? GetEventWithId(string id)
    {
        return _realm.All<GameEvent>().FirstOrDefault(e => e.Id == id);
    }
    
    public (GameEvent[], int) GetPaginatedEvents(EventOrderType order, bool descending,  EventFilters filters, int from, int count, GameUser? accessor)
    {
        IQueryable<GameEvent> events = _realm.All<GameEvent>().FilterEvents(filters, accessor)
            .OrderEvents(order, descending);

        return (events.Paginate(from, count), events.Count());
    }
}