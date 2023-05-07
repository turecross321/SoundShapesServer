using Realms;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types;

public class Punishment : RealmObject 
{
    public Punishment(string id, GameUser user, int punishmentType, string reason, bool revoked, GameUser issuer, DateTimeOffset issuedAt, DateTimeOffset expiresAt)
    {
        Id = id;
        User = user;
        PunishmentType = punishmentType;
        Reason = reason;
        Revoked = revoked;
        Issuer = issuer;
        IssuedAt = issuedAt;
        ExpiresAt = expiresAt;
    }
    
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public Punishment() {}
    #pragma warning restore CS8618

        [PrimaryKey]
    [Required] public string Id { get; init; }
    public GameUser User { get; set; }
    public int PunishmentType { get; set; }
    public string Reason { get; set; }
    public bool Revoked { get; set; }
    public GameUser Issuer { get; init; }
    public DateTimeOffset IssuedAt { get; init; }
    public DateTimeOffset ExpiresAt { get; set; }
}