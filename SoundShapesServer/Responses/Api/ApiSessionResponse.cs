using Newtonsoft.Json;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Sessions;

namespace SoundShapesServer.Responses.Api;

public class ApiSessionResponse
{
    public ApiSessionResponse(GameSession session, Punishment? punishment)
    {
        Id = session.Id;
        ExpiresAt = session.ExpiresAt;
        User = new ApiUserBriefResponse(session.User);
        IsBanned = punishment != null;
        BanReason = punishment?.Reason;
        BanExpiresAt = punishment?.ExpiresAt;
    }

    public string Id { get; }
    public DateTimeOffset ExpiresAt { get; }
    public ApiUserBriefResponse User { get; set; }
    public bool IsBanned { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? BanReason { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public DateTimeOffset? BanExpiresAt { get; set; }
}