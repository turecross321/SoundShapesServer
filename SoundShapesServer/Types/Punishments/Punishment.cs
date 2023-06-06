using Realms;
using SoundShapesServer.Types.Users;
#pragma warning disable CS8618

namespace SoundShapesServer.Types.Punishments;

public class Punishment : RealmObject 
{
    [PrimaryKey] [Required] public string Id { get; init; }
    public GameUser Recipient { get; set; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with TokenType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _PunishmentType { get; set; }
    public PunishmentType PunishmentType
    {
        get => (PunishmentType)_PunishmentType;
        set => _PunishmentType = (int)value;
    }
    
    public string Reason { get; set; }
    public bool Revoked { get; set; }
    public GameUser Author { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
    public DateTimeOffset? RevokeDate { get; set; }
}