namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPunishRequest
{
    public ApiPunishRequest(int punishmentType, string reason, DateTimeOffset expiresAtUtc)
    {
        PunishmentType = punishmentType;
        Reason = reason;
        ExpiresAtUtc = expiresAtUtc;
    }

    public int PunishmentType { get; }
    public string Reason { get; }
    public DateTimeOffset ExpiresAtUtc { get; }
}