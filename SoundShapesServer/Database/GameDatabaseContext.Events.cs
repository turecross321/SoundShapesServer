using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    private void CreateEvent(GameUser actor, EventType eventType, GameUser? user = null, GameLevel? level = null, LeaderboardEntry? leaderboardEntry = null)
    {
        GameEvent eventObject = new (actor, user, level, leaderboardEntry, eventType);
        
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
        });
    }

    public void RemoveEvent(GameEvent eventObject)
    {
        _realm.Write(() => _realm.Remove(eventObject));
    }
    
    public GameEvent? GetEventWithId(string id)
    {
        return _realm.All<GameEvent>().FirstOrDefault(e => e.Id == id);
    }
    
    public (GameEvent[], int) GetEvents(EventOrderType order, bool descending,  EventFilters filters, int from, int count)
    {
        IQueryable<GameEvent> events = _realm.All<GameEvent>();

        IQueryable<GameEvent> filteredEvents = FilterEvents(events, filters);
        IQueryable<GameEvent> orderedEvents = OrderEvents(filteredEvents, order, descending);
        
        GameEvent[] paginatedEvents = PaginationHelper.PaginateEvents(orderedEvents, from, count);

        return (paginatedEvents, filteredEvents.Count());
    }
    
    private static IQueryable<GameEvent> FilterEvents(IQueryable<GameEvent> events, EventFilters filters)
    {
        IQueryable<GameEvent> response = events;

        if (filters.Actors != null)
        {
            IEnumerable<GameEvent> tempResponse = new List<GameEvent>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (GameUser actor in filters.Actors)
            {
                tempResponse = tempResponse.Concat(response.Where(e=> e.Actor == actor));
            }

            response = tempResponse.AsQueryable();
        }
        
        if (filters.OnUser != null)
        {
            response = response.Where(e => e.ContentUser == filters.OnUser);
        }
        
        if (filters.OnLevel != null)
        {
            response = response.Where(e => e.ContentLevel == filters.OnLevel);
        }

        if (filters.EventTypes != null)
        {
            IEnumerable<GameEvent> tempResponse = new List<GameEvent>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (EventType eventType in filters.EventTypes)
            {
                tempResponse = tempResponse.Concat(response.Where(e=> e._EventType == (int)eventType));
            }

            response = tempResponse.AsQueryable();
        }

        return response;
    }

    #region Event Ordering

    private IQueryable<GameEvent> OrderEvents(IQueryable<GameEvent> events, EventOrderType order, bool descending)
    {
        return order switch
        {
            EventOrderType.Date => OrderEventsByDate(events, descending),
            _ => OrderEventsByDate(events, descending)
        };
    }
    
    private IQueryable<GameEvent> OrderEventsByDate(IQueryable<GameEvent> events, bool descending)
    {
        if (descending) return events.OrderByDescending(e => e.Date);
        return events.OrderBy(e => e.Date);
    }

    #endregion
}