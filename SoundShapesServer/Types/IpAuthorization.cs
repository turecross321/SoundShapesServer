using Realms;

namespace SoundShapesServer.Types;

public class IpAuthorization : RealmObject
{
    public string IpAddress { get; set; }
    public bool Authorized { get; set; }
    public bool OneTimeUse { get; set; }
    public GameUser User { get; set; }
    public int SessionType { get; set; }
}