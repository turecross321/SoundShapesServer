namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPunishRequest
{
    public ApiPunishRequest(string userId, int punishmentType, string reason, DateTimeOffset expiresAtUtc)
    {
        UserId = userId;
        PunishmentType = punishmentType;
        Reason = reason;
        ExpiresAtUtc = expiresAtUtc;
    }

    public string UserId { get; set; }
    public int PunishmentType { get; }
    public string Reason { get; }
    public DateTimeOffset ExpiresAtUtc { get; }
}