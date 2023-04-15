using Realms;

namespace SoundShapesServer.Types;

public class IpAuthenticationRequest : RealmObject
{
    public GameUser User { get; set; }
    public string IpAddress { get; set; }
}