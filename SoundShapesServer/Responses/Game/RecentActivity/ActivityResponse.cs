using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Responses.Game.RecentActivity;

public class ActivityResponse
{
    public ActivityResponse(GameEvent gameEventObject)
    {
        Actor = new UserResponse(gameEventObject.Actor);
        
        Content = (EventType)gameEventObject.EventType switch
        {
            Types.RecentActivity.EventType.Publish => 
                new RecentActivityLevel(gameEventObject.ContentLevel ?? new GameLevel()),
                
            Types.RecentActivity.EventType.Like => 
                new RecentActivityLevel(gameEventObject.ContentLevel ?? new GameLevel()),
                
            Types.RecentActivity.EventType.Follow => 
                new UserResponse(gameEventObject.ContentUser ?? new GameUser()),

            _ => null
        };

        EventType = RecentActivityHelper.EventEnumToGameString((EventType)gameEventObject.EventType);
        Timestamp = gameEventObject.Date.ToUnixTimeMilliseconds().ToString();
    }
    
    
    [JsonProperty("actor")] public UserResponse Actor { get; set; }
    
    [JsonProperty("type")] public string Type = GameContentType.activity.ToString();
    [JsonProperty("object")] public object? Content { get; set; }
    [JsonProperty("verb")] public string EventType { get; set; }
    [JsonProperty("timestamp")] public string Timestamp { get; set; }
}