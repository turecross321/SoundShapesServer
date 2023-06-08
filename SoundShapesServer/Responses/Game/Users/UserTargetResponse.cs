using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Users;

public class UserTargetResponse
{
    public UserTargetResponse(GameUser? user)
    {
        if (user == null)
        {
            Type = ContentHelper.GetContentTypeString(GameContentType.RemovedLevelAuthor);
        }
        
        Id = IdHelper.FormatUserId(user?.Id ?? "");
        DisplayName = user?.Username ?? "";
        Metadata = user != null ? new UserMetadataResponse(user) : new UserMetadataResponse();
    }

    public UserTargetResponse()
    {
        Id = "";
        DisplayName = "";
        Metadata = new UserMetadataResponse();
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; } = ContentHelper.GetContentTypeString(GameContentType.User);

    [JsonProperty("displayName")] public string DisplayName { get; set; }

    [JsonProperty("metadata")] public UserMetadataResponse Metadata { get; set; }
}