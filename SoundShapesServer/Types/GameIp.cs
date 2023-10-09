using Realms;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;
// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8618

namespace SoundShapesServer.Types;

public class GameIp : RealmObject
{
    public GameIp(string ipAddress, GameUser user)
    {
        IpAddress = ipAddress;
        User = user;
        CreationDate = DateTimeOffset.UtcNow;
        ModificationDate = DateTimeOffset.UtcNow;
    }
    
    // Realm cries if this doesn't exist
    public GameIp() {}
    
    public string IpAddress { get; init; }
    public bool Authorized { get; set; }
    public bool OneTimeUse { get; set; }
    public GameUser User { get; init; }
    public IList<AuthToken> Tokens { get; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
}