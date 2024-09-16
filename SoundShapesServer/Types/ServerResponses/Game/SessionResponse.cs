using Newtonsoft.Json;

namespace SoundShapesServer.Types.ServerResponses.Game;

public record SessionResponse
{
    [JsonProperty("id")] public required Guid Id { get; set; }
    
    // Game doesn't actually listen to expiry date, until it disconnects, and it will only try to get a new token
    // if the old one is already expired. This can only lead to problems, hence this always being set to 0
    [JsonProperty("expires")] public static long ExpirationDate => 0;
    
    [JsonProperty("person")] public required SessionUserResponse User { get; set; }
}