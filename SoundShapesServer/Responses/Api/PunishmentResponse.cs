using MongoDB.Bson;

namespace SoundShapesServer.Responses.Api;

public class PunishmentResponse
{
    public ObjectId Id { get; set; }
    public string UserId { get; set; }
    public int PunishmentType { get; set; }
    public string Reason { get; set; }
    public bool Revoked { get; set; }
    public DateTimeOffset IssuedAtUtc { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
}