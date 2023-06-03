using SoundShapesServer.Responses.Api.Moderation;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Sessions;

namespace SoundShapesServer.Responses.Api;

public class ApiSessionResponse
{
    public ApiSessionResponse(GameSession session, IEnumerable<Punishment> activePunishments)
    {
        Id = session.Id;
        ExpiresAt = session.ExpiresAt;
        User = new ApiUserBriefResponse(session.User);
        ActivePunishments = activePunishments.Select(p => new ApiPunishmentResponse(p)).ToArray();
    }

    public string Id { get; }
    public DateTimeOffset ExpiresAt { get; }
    public ApiUserBriefResponse User { get; set; }
    public ApiPunishmentResponse[] ActivePunishments { get; set; }
}