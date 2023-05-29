using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Levels;

public class RelationLevelResponse
{
    public RelationLevelResponse(GameUser user, GameLevel level)
    {
        Id = IdFormatter.FormatRelationLevelId(user.Id, level.Id);
        
        DateTimeOffset? likeDate = level.Likes.FirstOrDefault(r => r.User == user && r.Level == level)?.Date;
        DateTimeOffset? queueDate = level.Likes.FirstOrDefault(r => r.User == user && r.Level == level)?.Date;
        DateTimeOffset mostRecentDate = (likeDate > queueDate ? likeDate : queueDate) ?? DateTimeOffset.UnixEpoch;
        
        Timestamp = mostRecentDate.ToUnixTimeMilliseconds();
        Level = new LevelTargetResponse(level, user);
    }
    
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = GameContentType.queued.ToString();
    [JsonProperty("timestamp")] public long Timestamp { get; set; }
    [JsonProperty("target")] public LevelTargetResponse Level { get; set; }
}