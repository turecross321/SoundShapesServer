using Realms;

namespace SoundShapesServer.Types;

public class IpAuthorization : RealmObject
{
    public string? IpAddress { get; init; }
    public bool Authorized { get; set; }
    public bool OneTimeUse { get; set; }
    public GameUser? User { get; init; }
    public int SessionType { get; init; }
}