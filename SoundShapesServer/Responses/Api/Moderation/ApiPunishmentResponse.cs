using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Moderation;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPunishmentResponse
{
    public ApiPunishmentResponse(Punishment punishment)
    {
        Id = punishment.Id;
        UserId = punishment.User.Id;
        PunishmentType = punishment.PunishmentType;
        Reason = punishment.Reason;
        Revoked = punishment.Revoked;
        IssuedAtUtc = punishment.IssuedAt;
        ExpiresAtUtc = punishment.ExpiresAt;
    }

    public string Id { get; }
    public string UserId { get; }
    public int PunishmentType { get; set; }
    public string Reason { get; set; }
    public bool Revoked { get; set; }
    public DateTimeOffset IssuedAtUtc { get; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
}