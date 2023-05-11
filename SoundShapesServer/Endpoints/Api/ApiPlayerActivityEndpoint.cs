using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.RecentActivity;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.RecentActivity;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.RecentActivityHelper;

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
        
        string? actorsString = context.QueryString["actors"];
        string? onUserString = context.QueryString["onUser"];
        string? onLevelString = context.QueryString["onLevel"];
        
        List<GameUser>? actors = null;

        if (actorsString != null)
        {
            actors = new List<GameUser>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (string actorString in actorsString.Split(","))
            {
                GameUser? actor = database.GetUserWithId(actorString);
                if (actor != null) actors.Add(actor);
            }
        }

        GameUser? onUser = null;
        if (onUserString != null)
        {
            onUser = database.GetUserWithId(onUserString);
        }
        
        GameLevel? onLevel = null;
        if (onLevelString != null)
        {
            onLevel = database.GetLevelWithId(onLevelString);
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
        
        return new ApiPlayerActivitiesWrapper(events, totalEvents);
    }
}