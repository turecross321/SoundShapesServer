using Newtonsoft.Json;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Sessions;

namespace SoundShapesServer.Responses.Api;

public class ApiAuthenticationResponse
{
    public ApiAuthenticationResponse(GameSession session, Punishment? punishment)
    {
        Id = session.Id;
        ExpiresAtUtc = session.ExpiresAt;
        User = new ApiUserResponse(session.User);
        PermissionsType = session.User.PermissionsType;
        IsBanned = punishment != null;
        BanReason = punishment?.Reason;
        BanExpiresAtUtc = punishment?.ExpiresAt;
    }

    public string Id { get; }
    public DateTimeOffset ExpiresAtUtc { get; }
    public ApiUserResponse User { get; set; }
    public int PermissionsType { get; }
    
    public bool IsBanned { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? BanReason { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public DateTimeOffset? BanExpiresAtUtc { get; set; }
}