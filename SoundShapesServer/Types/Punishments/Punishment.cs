using Realms;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Punishments;

public class Punishment : RealmObject 
{
    public Punishment(string id, GameUser recipient, int punishmentType, string reason, bool revoked, GameUser author, DateTimeOffset issuedAt, DateTimeOffset expiresAt)
    {
        Id = id;
        Recipient = recipient;
        PunishmentType = punishmentType;
        Reason = reason;
        Revoked = revoked;
        Author = author;
        IssuedAt = issuedAt;
        ExpiresAt = expiresAt;
    }
    
    // Realm cries if this doesn't exist
#pragma warning disable CS8618
    public Punishment() {}
#pragma warning restore CS8618

    [PrimaryKey] [Required] public string Id { get; init; }
    public GameUser Recipient { get; set; }
    public int PunishmentType { get; set; }
    public string Reason { get; set; }
    public bool Revoked { get; set; }
    // TODO: revoked date
    public GameUser Author { get; init; }
    public DateTimeOffset IssuedAt { get; init; }
    public DateTimeOffset ExpiresAt { get; set; }
}