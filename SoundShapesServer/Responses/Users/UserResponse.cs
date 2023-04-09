using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Users;

public class UserResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    
    [JsonProperty("displayName", NullValueHandling=NullValueHandling.Ignore)]
    public string? DisplayName { get; set; }
    
    [JsonProperty("metadata", NullValueHandling=NullValueHandling.Ignore)] 
    public UserMetadataResponse Metadata { get; set; }
}