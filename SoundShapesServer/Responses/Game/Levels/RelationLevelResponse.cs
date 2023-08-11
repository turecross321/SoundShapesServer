using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Levels;

public class RelationLevelResponse : IResponse
{
    public RelationLevelResponse(GameLevel level, GameUser user)
    {
        Id = IdHelper.FormatRelationLevelId(user.Id, level.Id);
        
        DateTimeOffset likeDate = level.Likes.FirstOrDefault(r => r.User == user && r.Level == level)?.Date ?? DateTimeOffset.UnixEpoch;
        DateTimeOffset queueDate = level.Queues.FirstOrDefault(r => r.User == user && r.Level == level)?.Date ?? DateTimeOffset.UnixEpoch;
        DateTimeOffset mostRecentDate = likeDate > queueDate ? likeDate : queueDate;
        if (mostRecentDate == DateTimeOffset.UnixEpoch) throw new Exception();
        
        Timestamp = mostRecentDate.ToUnixTimeMilliseconds();
        Level = new LevelTargetResponse(level, user);
    }
    
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.Queued);
    [JsonProperty("timestamp")] public long Timestamp { get; set; }
    [JsonProperty("target")] public LevelTargetResponse Level { get; set; }
}