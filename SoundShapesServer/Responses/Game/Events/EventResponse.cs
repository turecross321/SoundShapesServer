using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Events;

public class EventResponse : IResponse
{
    public EventResponse(GameDatabaseContext database, GameEvent gameEvent)
    {
        Actor = new UserTargetResponse(gameEvent.Actor);

        Content = gameEvent.EventType switch
        {
            Types.Events.EventType.LevelPublish => new EventLevelResponse((GameLevel?)gameEvent.Data(database)!),
            Types.Events.EventType.LevelLike => new EventLevelResponse((GameLevel?)gameEvent.Data(database)!),
            Types.Events.EventType.UserFollow => new UserTargetResponse((GameUser)gameEvent.Data(database)),
            _ => null
        };

        Timestamp = gameEvent.CreationDate.ToUnixTimeMilliseconds().ToString();
        EventType = EventHelper.EventEnumToGameString(gameEvent.EventType);
    }
    
    
    [JsonProperty("actor")] public UserTargetResponse Actor { get; set; }
    
    [JsonProperty("type")] public string ContentType = ContentHelper.GetContentTypeString(Types.GameContentType.Event);
    [JsonProperty("object")] public object? Content { get; set; }
    [JsonProperty("verb")] public string EventType { get; set; }
    [JsonProperty("timestamp")] public string Timestamp { get; set; }
}