using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Users;

public class UserResponse
{
    public UserResponse(GameUser? user)
    {
        if (user == null)
        {
            Type = GameContentType.alias.ToString();
        }
        
        Id = IdFormatter.FormatUserId(user?.Id ?? "");
        DisplayName = user?.Username ?? "";
        Metadata = user != null ? new UserMetadataResponse(user) : new UserMetadataResponse();
    }

    public UserResponse()
    {
        Id = "";
        DisplayName = "";
        Metadata = new UserMetadataResponse();
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; } = GameContentType.identity.ToString();

    [JsonProperty("displayName")] public string DisplayName { get; set; }

    [JsonProperty("metadata")] public UserMetadataResponse Metadata { get; set; }
}