using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.RecentActivity;
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
        string? onUser = context.QueryString["onUser"];
        string? onLevel = context.QueryString["onLevel"];

        string? eventTypesString = context.QueryString["eventTypes"];

        List<EventType>? eventTypes = null;

        if (eventTypesString != null)
        {
            eventTypes = new List<EventType>();
            eventTypes.AddRange(eventTypesString.Split(",").Select(Enum.Parse<EventType>));
        }

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

        IQueryable<GameEvent> events = database.GetEvents();
        IQueryable<GameEvent> filteredEvents = FilterEvents(events, actors?.ToArray(), onUser, onLevel, eventTypes?.ToArray());

        // I know this is utterly pointless, but I want to stay consistent with the rest of the server.
        
        EventOrderType orderType = orderString switch
        {
            "date" => EventOrderType.Date,
            _ => EventOrderType.Date
        };
        
        return new ApiPlayerActivitiesWrapper(filteredEvents, from, count, orderType, descending);
    }
}