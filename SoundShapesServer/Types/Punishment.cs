using Realms;

namespace SoundShapesServer.Types;

public class Punishment : RealmObject 
{
    public string Id { get; init; } = "";
    public GameUser User { get; set; } = new ();
    public int PunishmentType { get; set; }
    public string Reason { get; set; } = "";
    public bool Dismissed { get; set; }
    public DateTimeOffset IssuedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}