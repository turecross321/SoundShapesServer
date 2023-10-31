using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions.RequestContextExtensions;

public static class EventRequestContextExtensions
{
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