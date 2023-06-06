using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Following;

public class FollowResponse
{
    public FollowResponse(GameUser recipient)
    {
        Id = recipient.Id;
        User = new UserResponse(recipient);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; } = ContentHelper.GetContentTypeString(GameContentType.Follow);
    [JsonProperty("target")] public UserResponse User { get; set; }
}