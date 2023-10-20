using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static class EventHelper
{
    public static string EventEnumToGameString(EventType type)
    {
        return type switch
        {
            EventType.LevelPublish => "publish",
            EventType.UserFollow => "follow",
            EventType.LevelLike => "like",
            _ => EventEnumToGameString(EventType.LevelPublish)
        };
    }

    public static EventFilters GetEventFilters(RequestContext context, GameDatabaseContext database)
    {
        List<GameUser>? actors = context.QueryString["actors"].ToUsers(database);
        GameUser? onUser = context.QueryString["onUser"].ToUser(database);
        GameLevel? onLevel = context.QueryString["onLevel"].ToLevel(database);
        List<EventType>? eventTypes = context.QueryString["eventTypes"].ToEnumList<EventType>();

        return new EventFilters(actors?.ToArray(), onUser, onLevel, eventTypes?.ToArray());
    }

    public static EventOrderType GetEventOrder(RequestContext context)
    {
        string? orderString = context.QueryString["orderBy"];
        
        return orderString switch
        {
            "date" => EventOrderType.Date,
            _ => EventOrderType.Date
        };
    }
}