using Realms;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8618

namespace SoundShapesServer.Types;

public class IpAuthorization : RealmObject
{
    public string IpAddress { get; init; }
    public bool Authorized { get; set; }
    public bool OneTimeUse { get; set; }
    public GameUser User { get; init; }
    [Backlink(nameof(GameSession.Ip))] public IQueryable<GameSession> Sessions { get;}
}