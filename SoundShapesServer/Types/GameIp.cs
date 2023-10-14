using Realms;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;
// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8618

namespace SoundShapesServer.Types;

public class GameIp : RealmObject
{
    public string IpAddress { get; init; }
    public bool Authorized { get; set; }
    public bool OneTimeUse { get; set; }
    public GameUser User { get; init; }
    public IList<GameToken> Tokens { get; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
}