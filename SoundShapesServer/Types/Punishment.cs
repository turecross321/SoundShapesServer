using Realms;

namespace SoundShapesServer.Types;

public class Punishment : RealmObject
{
    public string Id { get; init; } = "";
    public GameUser User { get; init; } = new ();
    public int PunishmentType { get; init; }
    public string Reason { get; init; } = "";
    public bool Revoked { get; set; }
    public DateTimeOffset IssuedAt { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
}