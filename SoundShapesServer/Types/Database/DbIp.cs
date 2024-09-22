using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SoundShapesServer.Types.Database;

/// <summary>
/// Used for IP authentication
/// </summary>
[PrimaryKey(nameof(Id))]
public class DbIp : IDbItem<Guid>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public DbUser User { get; init; } = null!;

    [MaxLength(39)]
    public required string IpAddress { get; init; }
    public required DateTime CreationDate { get; init; }
    public DateTime? AuthorizedDate { get; set; } = null;
    public bool Authorized { get; init; }
    public bool? OneTimeUse { get; set; }
    public ICollection<DbToken> Tokens { get; set; } = null!;
}