using Bunkum.Core;
using SoundShapesServer.Database;
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
        string? actorIds = context.QueryString["actors"];

        List<GameUser>? actors = null;

        if (actorIds != null)
        {
            actors = new List<GameUser>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (string actorId in actorIds.Split(","))
            {
                GameUser? actor = database.GetUserWithId(actorId);
                if (actor != null) actors.Add(actor);
            }
        }

        string? onUserId = context.QueryString["onUser"];
        GameUser? onUser = null;
        if (onUserId != null)
        {
            onUser = database.GetUserWithId(onUserId);
        }
        
        string? levelIds = context.QueryString["onLevel"];
        GameLevel? onLevel = null;
        if (levelIds != null)
        {
            onLevel = database.GetLevelWithId(levelIds);
        }

        string? eventTypesString = context.QueryString["eventTypes"];
        List<EventType>? eventTypes = null;

        if (eventTypesString != null)
        {
            eventTypes = new List<EventType>();
            eventTypes.AddRange(eventTypesString.Split(",").Select(Enum.Parse<EventType>));
        }

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