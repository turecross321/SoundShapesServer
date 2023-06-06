using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Events;

public class EventResponse
{
    public EventResponse(GameEvent gameEventObject)
    {
        Actor = new UserResponse(gameEventObject.Actor);
        
        switch ((EventType)gameEventObject.EventType)
        {
            case Types.Events.EventType.Publish:
                Content = new EventLevelResponse(gameEventObject.ContentLevel ?? new GameLevel());
                Timestamp = (gameEventObject.ContentLevel?.ModificationDate.ToUnixTimeMilliseconds() ?? 0).ToString();
                break;

            case Types.Events.EventType.Like:
                Content = new EventLevelResponse(gameEventObject.ContentLevel ?? new GameLevel());
                Timestamp = (gameEventObject.ContentLevel?.ModificationDate.ToUnixTimeMilliseconds() ?? 0).ToString();
                break;
                
            case Types.Events.EventType.Follow:
                Content = new UserResponse(gameEventObject.ContentUser ?? new GameUser());
                Timestamp = gameEventObject.Date.ToString();
                break;
            case Types.Events.EventType.ScoreSubmission:
                break;
            case Types.Events.EventType.AccountRegistration:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Timestamp ??= 0.ToString();
        EventType = EventHelper.EventEnumToGameString((EventType)gameEventObject.EventType);
    }
    
    
    [JsonProperty("actor")] public UserResponse Actor { get; set; }
    
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.Activity);
    [JsonProperty("object")] public object? Content { get; set; }
    [JsonProperty("verb")] public string EventType { get; set; }
    [JsonProperty("timestamp")] public string Timestamp { get; set; }
}