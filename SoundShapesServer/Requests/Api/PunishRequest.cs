namespace SoundShapesServer.Requests.Api;

public class PunishRequest
{
    public int PunishmentType { get; set; }
    public string Reason { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
}