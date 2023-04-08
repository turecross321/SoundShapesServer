using Newtonsoft.Json;

namespace SoundShapesServer.Responses;

public class UserResponse
{
    public string id { get; set; }
    public string type { get; set; }
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)] public string? displayName { get; set; }
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)] public UserMetadataResponse metadata { get; set; }
}