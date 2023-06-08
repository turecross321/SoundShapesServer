using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Users;

public class UserBriefResponse
{
    public UserBriefResponse(GameUser follower, GameUser recipient)
    {
        Id = IdHelper.FormatFollowId(follower.Id, recipient.Id);
        UserTarget = new UserTargetResponse(recipient);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; } = ContentHelper.GetContentTypeString(GameContentType.Follow);
    [JsonProperty("target")] public UserTargetResponse UserTarget { get; set; }
}