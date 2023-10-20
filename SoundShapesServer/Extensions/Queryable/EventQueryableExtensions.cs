using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions.Queryable;

public static class EventQueryableExtensions
{
    public static IQueryable<GameEvent> FilterEvents(this IQueryable<GameEvent> events, EventFilters filters, GameUser? accessor)
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
    
    public static IQueryable<GameEvent> OrderEvents(this IQueryable<GameEvent> events, EventOrderType order, bool descending)
    {
        return order switch
        {
            EventOrderType.Date => events.OrderByDynamic(e => e.CreationDate, descending),
            _ => events.OrderEvents(EventOrderType.Date, descending)
        };
    }
}