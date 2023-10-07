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
        IQueryable<GameEvent> orderedEvents = GetEvents(order, descending, filters, accessor);
        GameEvent[] paginatedEvents = PaginationHelper.PaginateEvents(orderedEvents, from, count);

        return (paginatedEvents, orderedEvents.Count());
    }

    private IQueryable<GameEvent> GetEvents(EventOrderType order, bool descending, EventFilters filters, GameUser? accessor)
    {
        IQueryable<GameEvent> events = _realm.All<GameEvent>();
        IQueryable<GameEvent> filteredEvents = FilterEvents(events, filters, accessor);
        IQueryable<GameEvent> orderedEvents = OrderEvents(filteredEvents, order, descending);

        return orderedEvents;
    }

    private static IQueryable<GameEvent> FilterEvents(IQueryable<GameEvent> events, EventFilters filters, GameUser? accessor)
    {
        if (filters.Actors != null)
        {
            IEnumerable<GameEvent> tempResponse = new List<GameEvent>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (GameUser actor in filters.Actors)
            {
                tempResponse = tempResponse.Concat(events.Where(e=> e.Actor == actor));
            }

            events = tempResponse.AsQueryable();
        }
        
        if (filters.OnUser != null)
        {
            events = events.Where(e => e.ContentUser == filters.OnUser);
        }
        
        if (filters.OnLevel != null)
        {
            events = events.Where(e => e.ContentLevel == filters.OnLevel);
        }

        if (filters.EventTypes != null)
        {
            IEnumerable<GameEvent> tempEvents = new List<GameEvent>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (EventType eventType in filters.EventTypes)
            {
                tempEvents = tempEvents.Concat(events.Where(e=> e._EventType == (int)eventType));
            }

            events = tempEvents.AsQueryable();
        }
        
        // Automatically remove private results, and remove unlisted results if the level hasn't been specified
        if ((accessor?.PermissionsType ?? PermissionsType.Default) < PermissionsType.Moderator)
        {
            List<GameEvent> nonPublicLevelEventsList = new ();

            foreach (GameEvent e in events)
            {
                if (e.ContentLevel != null && e.ContentLevel.Author.Id != accessor?.Id &&
                    e.ContentLevel._Visibility != (int)LevelVisibility.Public)
                {
                    nonPublicLevelEventsList.Add(e);
                }
            }

            IQueryable<GameEvent> nonPublicLevelEvents = nonPublicLevelEventsList.AsQueryable();

            // If an OnLevel has been specified and the specified level is unlisted, don't count it as non public.
            if (filters.OnLevel is { Visibility: LevelVisibility.Unlisted })
            {
                nonPublicLevelEvents = nonPublicLevelEvents.Where(e => e.ContentLevel != null && e.ContentLevel.Id != filters.OnLevel.Id);
            }
                
            events = events.AsEnumerable().Except(nonPublicLevelEvents).AsQueryable();
        }

        return events;
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
        if (descending) return events.OrderByDescending(e => e.CreationDate);
        return events.OrderBy(e => e.CreationDate);
    }

    #endregion
}