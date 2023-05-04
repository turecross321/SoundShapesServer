using Newtonsoft.Json;
using SoundShapesServer.Authentication;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api;

public class ApiAuthenticationResponse
{
    public ApiAuthenticationResponse(GameSession session, Punishment? punishment)
    {
        Id = session.Id;
        ExpiresAtUtc = session.ExpiresAt;
        UserId = session.User?.Id ?? "";
        Username = session.User?.Username ?? "";
        PermissionsType = session.User?.PermissionsType ?? (int)Types.PermissionsType.Default;

        IsBanned = punishment != null;
        BanReason = punishment?.Reason;
        BanExpiresAtUtc = punishment?.ExpiresAt;
    }

    public string Id { get; }
    public DateTimeOffset ExpiresAtUtc { get; }
    public string UserId { get; }
    public string Username { get; }
    public int PermissionsType { get; }
    
    public bool IsBanned { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? BanReason { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public DateTimeOffset? BanExpiresAtUtc { get; set; }
}