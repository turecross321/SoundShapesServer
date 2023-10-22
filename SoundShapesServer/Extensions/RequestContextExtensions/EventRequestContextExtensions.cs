using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions.RequestContextExtensions;

public static class EventRequestContextExtensions
{
    public static EventFilters GetEventFilters(this RequestContext context, GameDatabaseContext database)
    {
        return new EventFilters
        {
            Actors = context.QueryString["actors"].ToUsers(database),
            OnUser = context.QueryString["onUser"].ToUser(database),
            OnLevel = context.QueryString["onLevel"].ToLevel(database),
            EventTypes = context.QueryString["eventTypes"].ToEnumList<EventType>(),
            CreatedBefore = context.QueryString["createdBefore"].ToDateFromUnix(),
            CreatedAfter = context.QueryString["createdAfter"].ToDateFromUnix(),
        };
    }

    public static EventOrderType GetEventOrder(this RequestContext context)
    {
        string? orderString = context.QueryString["orderBy"];
        
        return orderString switch
        {
            "date" => EventOrderType.Date,
            _ => EventOrderType.Date
        };
    }
}