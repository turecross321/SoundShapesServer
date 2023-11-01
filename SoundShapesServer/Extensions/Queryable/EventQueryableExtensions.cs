using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions.Queryable;

public static class EventQueryableExtensions
{
    public static IQueryable<GameEvent> FilterEvents(this IQueryable<GameEvent> events, GameDatabaseContext database, EventFilters filters, GameUser? accessor)
    {
        if (filters.CreatedBefore != null)
            events = events.Where(l => l.CreationDate <= filters.CreatedBefore);
        
        if (filters.CreatedAfter != null)
            events = events.Where(l => l.CreationDate >= filters.CreatedAfter);
        
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
            events = events.Where(e => e._DataType == (int)EventDataType.User && e.DataId == filters.OnUser.Id);
        }
        
        if (filters.OnLevel != null)
        {
            events = events.Where(e => e._DataType == (int)EventDataType.Level && e.DataId == filters.OnLevel.Id);
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
        
        if ((accessor?.PermissionsType ?? PermissionsType.Default) < PermissionsType.Moderator
            && filters.OnLevel is not { Visibility: LevelVisibility.Unlisted })
        {
            IQueryable<GameEvent> levelEvents = events.Where(e => e._DataType == (int)EventDataType.Level);
            List<GameEvent> nonPublicLevelEvents = new ();
            foreach (GameEvent e in levelEvents)
            {
                GameLevel? level = database.GetLevelWithId(e.DataId);
                
                if (level != null && level.Author.Id != accessor?.Id &&
                    level._Visibility != (int)LevelVisibility.Public)
                {
                    nonPublicLevelEvents.Add(e);
                }
            }
                
            events = events.AsEnumerable().Except(nonPublicLevelEvents).AsQueryable();
        }

        return events;
    }
    
    public static IQueryable<GameEvent> OrderEvents(this IQueryable<GameEvent> events, EventOrderType order, bool descending)
    {
        return order switch
        {
            EventOrderType.CreationDate => events.OrderByDynamic(e => e.CreationDate, descending),
            _ => events.OrderEvents(EventOrderType.CreationDate, descending)
        };
    }
}