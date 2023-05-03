using SoundShapesServer.Authentication;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api;

public class ApiBannedResponse
{
    public ApiBannedResponse(Punishment punishment, GameSession session)
    {
        PunishmentId = punishment.Id;
        Reason = punishment.Reason;
        ExpiresAt = punishment.ExpiresAt;
        SessionId = session.Id;
    }

    public string PunishmentId { get; set; }
    public string Reason { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public string SessionId { get; set; }
}