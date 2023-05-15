using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Punishments;

namespace SoundShapesServer.Responses.Api.Moderation;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPunishmentResponse
{
    public ApiPunishmentResponse(Punishment punishment)
    {
        Id = punishment.Id;
        User = new ApiUserResponse(punishment.User);
        PunishmentType = punishment.PunishmentType;
        Reason = punishment.Reason;
        Revoked = punishment.Revoked;
        Issuer = new ApiUserResponse(punishment.Issuer);
        IssuedAt = punishment.IssuedAt;
        ExpiresAt = punishment.ExpiresAt;
    }

    public string Id { get; }
    public ApiUserResponse User { get; set; }
    public int PunishmentType { get; set; }
    public string Reason { get; set; }
    public bool Revoked { get; set; }
    public ApiUserResponse Issuer { get; set; }
    public DateTimeOffset IssuedAt { get; }
    public DateTimeOffset ExpiresAt { get; set; }
}