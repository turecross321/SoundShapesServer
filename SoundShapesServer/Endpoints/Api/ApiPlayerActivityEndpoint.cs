using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.RecentActivity;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.PlayerActivity;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api;

public class ApiPlayerActivityEndpoint : EndpointGroup
{
    [ApiEndpoint("activities")]
    [Authentication(false)]
    public ApiPlayerActivitiesWrapper GetActivities(RequestContext context, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];
        
        string? actorIds = context.QueryString["actors"];
        string? onUserId = context.QueryString["onUser"];
        string? levelIds = context.QueryString["onLevel"];
        
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

        GameUser? onUser = null;
        if (onUserId != null)
        {
            onUser = database.GetUserWithId(onUserId);
        }
        
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

        // I know this is utterly pointless, but I want to stay consistent with the rest of the server.
        EventOrderType orderType = orderString switch
        {
            "date" => EventOrderType.Date,
            _ => EventOrderType.Date
        };

        EventFilters filters = new (actors?.ToArray(), onUser, onLevel, eventTypes?.ToArray());
        (GameEvent[] events, int totalEvents) = database.GetEvents(orderType, descending, filters, from, count);
        
        return new ApiPlayerActivitiesWrapper(database, events, totalEvents);
    }
}