using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Events;

public class EventResponse : IResponse
{
    public EventResponse(GameEvent gameEventObject)
    {
        Actor = new UserTargetResponse(gameEventObject.Actor);
        
        switch (gameEventObject.EventType)
        {
            case Types.Events.EventType.LevelPublish:
                Content = new EventLevelResponse(gameEventObject.ContentLevel ?? new GameLevel());
                Timestamp = (gameEventObject.ContentLevel?.ModificationDate.ToUnixTimeMilliseconds() ?? 0).ToString();
                break;

            case Types.Events.EventType.LevelLike:
                Content = new EventLevelResponse(gameEventObject.ContentLevel ?? new GameLevel());
                Timestamp = (gameEventObject.ContentLevel?.ModificationDate.ToUnixTimeMilliseconds() ?? 0).ToString();
                break;
                
            case Types.Events.EventType.UserFollow:
                Content = new UserTargetResponse(gameEventObject.ContentUser ?? new GameUser());
                Timestamp = gameEventObject.CreationDate.ToString();
                break;
            case Types.Events.EventType.ScoreSubmission:
                break;
            case Types.Events.EventType.AccountRegistration:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Timestamp ??= 0.ToString();
        EventType = EventHelper.EventEnumToGameString(gameEventObject.EventType);
    }
    
    
    [JsonProperty("actor")] public UserTargetResponse Actor { get; set; }
    
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.Event);
    [JsonProperty("object")] public object? Content { get; set; }
    [JsonProperty("verb")] public string EventType { get; set; }
    [JsonProperty("timestamp")] public string Timestamp { get; set; }
}