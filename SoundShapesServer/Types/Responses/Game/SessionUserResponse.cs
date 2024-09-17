using Newtonsoft.Json;

namespace SoundShapesServer.Types.Responses.Game;

/// <summary>
/// Special user response type only used in the authentication endpoint
/// </summary>
public record SessionUserResponse
{
    [JsonProperty("id")] public required Guid Id { get; set; }
    [JsonProperty("display_name")] public required string UserName { get; set; }
}