using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Users;

public class UserTargetResponse : IResponse
{
    public UserTargetResponse(GameUser user)
    {
        Id = IdHelper.FormatUserId(user.Id);
        DisplayName = user.Username;
        Metadata = new UserMetadataResponse(user);
    }
    
    public UserTargetResponse()
    {
        Id = IdHelper.FormatUserId("________-____-____-____-____________");
        DisplayName = "Unknown User";
        Metadata = new UserMetadataResponse();
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; } = ContentHelper.GetContentTypeString(GameContentType.User);
    [JsonProperty("displayName")] public string DisplayName { get; set; }
    [JsonProperty("metadata")] public UserMetadataResponse Metadata { get; set; }
}