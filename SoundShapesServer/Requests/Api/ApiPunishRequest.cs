namespace SoundShapesServer.Requests.Api;

public class ApiPunishRequest
{
    public int PunishmentType { get; set; }
    public string Reason { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
}