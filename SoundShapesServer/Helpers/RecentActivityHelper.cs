using SoundShapesServer.Types.RecentActivity;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static class RecentActivityHelper
{
    public static string EventEnumToGameString(EventType type)
    {
        return type switch
        {
            EventType.Publish => "publish",
            EventType.Follow => "follow",
            EventType.Like => "like",
            _ => EventEnumToGameString(EventType.Publish)
        };
    }

    public static IQueryable<GameEvent> FilterEvents(IQueryable<GameEvent> events, GameUser[]? actors, string? onUser, string? onLevel, EventType[]? eventType)
    {
        IQueryable<GameEvent> response = events;

        if (actors != null)
        {
            response = response.AsEnumerable().Where(e => actors.Contains(e.Actor)).AsQueryable();
        }
        
        if (onUser != null)
        {
            response = response.AsEnumerable().Where(e => e.ContentUser?.Id == onUser).AsQueryable();
        }
        
        if (onLevel != null)
        {
            response = response.AsEnumerable().Where(e => e.ContentLevel?.Id == onLevel).AsQueryable();
        }

        if (eventType != null)
        {
            response = response.AsEnumerable().Where(e => eventType.Contains((EventType)e.EventType)).AsQueryable();
        }

        return response;
    }
    
    public static IQueryable<GameEvent> OrderEvents(IQueryable<GameEvent> events, EventOrderType orderType, bool descending)
    {
        IQueryable<GameEvent> response = events;

        switch (orderType)
        {
            case EventOrderType.Date:
                response = response.OrderBy(e => e.Date);
                break;
        }

        if (descending) response = response.AsEnumerable().Reverse().AsQueryable();

        return response;
    }
}