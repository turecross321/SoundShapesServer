using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.PlayerActivity;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.RecentActivity;

public class ActivityResponse
{
    public ActivityResponse(GameEvent gameEventObject)
    {
        Actor = new UserResponse(gameEventObject.Actor);
        
        switch ((EventType)gameEventObject.EventType)
        {
            case Types.PlayerActivity.EventType.Publish:
                Content = new RecentActivityLevelResponse(gameEventObject.ContentLevel ?? new GameLevel());
                Timestamp = (gameEventObject.ContentLevel?.ModificationDate.ToUnixTimeMilliseconds() ?? 0).ToString();
                break;

            case Types.PlayerActivity.EventType.Like:
                Content = new RecentActivityLevelResponse(gameEventObject.ContentLevel ?? new GameLevel());
                Timestamp = (gameEventObject.ContentLevel?.ModificationDate.ToUnixTimeMilliseconds() ?? 0).ToString();
                break;
                
            case Types.PlayerActivity.EventType.Follow:
                Content = new UserResponse(gameEventObject.ContentUser ?? new GameUser());
                Timestamp = gameEventObject.Date.ToString();
                break;
            case Types.PlayerActivity.EventType.ScoreSubmission:
                break;
            case Types.PlayerActivity.EventType.AccountRegistration:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Timestamp ??= 0.ToString();
        EventType = RecentActivityHelper.EventEnumToGameString((EventType)gameEventObject.EventType);
    }
    
    
    [JsonProperty("actor")] public UserResponse Actor { get; set; }
    
    [JsonProperty("type")] public string Type = GameContentType.activity.ToString();
    [JsonProperty("object")] public object? Content { get; set; }
    [JsonProperty("verb")] public string EventType { get; set; }
    [JsonProperty("timestamp")] public string Timestamp { get; set; }
}