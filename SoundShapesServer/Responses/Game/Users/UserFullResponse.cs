using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Users;

public class UserFullResponse : IResponse
{
    public UserFullResponse(GameUser recipient)
    {
        Id = recipient.Id;
        User = new UserTargetResponse(recipient);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; } = ContentHelper.GetContentTypeString(GameContentType.Follow);
    [JsonProperty("target")] public UserTargetResponse User { get; set; }
}