using Newtonsoft.Json;
using SoundShapesServer.Types;
namespace SoundShapesServer.Responses.Game.Users;

public class UserResponse
{
    [JsonProperty("id")] public string? Id { get; set; } = "";
    [JsonProperty("type")] public string Type { get; } = ResponseType.identity.ToString();

    [JsonProperty("displayName", NullValueHandling=NullValueHandling.Ignore)] public string? DisplayName { get; set; } = null;

    [JsonProperty("metadata")] public UserMetadataResponse Metadata { get; set; } = new UserMetadataResponse();
}