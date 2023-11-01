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
            EventType.LevelPublish => new EventLevelResponse((GameLevel?)gameEvent.Data(database)!),
            EventType.LevelLike => new EventLevelResponse((GameLevel?)gameEvent.Data(database)!),
            EventType.UserFollow => new UserTargetResponse((GameUser)gameEvent.Data(database)),
            _ => null
        };

        Timestamp = gameEvent.CreationDate.ToUnixTimeMilliseconds().ToString();
        Type = gameEvent.EventType switch
        {
            EventType.LevelPublish => "publish",
            EventType.UserFollow => "follow",
            EventType.LevelLike => "like",
            _ => "publish"
        };
    }
    
    
    [JsonProperty("actor")] public UserTargetResponse Actor { get; set; }
    
    [JsonProperty("type")] public string ContentType = ContentHelper.GetContentTypeString(Types.GameContentType.Event);
    [JsonProperty("object")] public object? Content { get; set; }
    [JsonProperty("verb")] public string Type { get; set; }
    [JsonProperty("timestamp")] public string Timestamp { get; set; }
}