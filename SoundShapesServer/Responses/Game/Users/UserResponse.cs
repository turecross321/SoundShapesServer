using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
namespace SoundShapesServer.Responses.Game.Users;

public class UserResponse
{
    public UserResponse(GameUser user)
    {
        Id = IdFormatter.FormatUserId(user.Id);
        DisplayName = user.Username;
        Metadata = new UserMetadataResponse(user);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; } = GameContentType.identity.ToString();

    [JsonProperty("displayName", NullValueHandling=NullValueHandling.Ignore)] public string DisplayName { get; set; }

    [JsonProperty("metadata")] public UserMetadataResponse Metadata { get; set; }
}