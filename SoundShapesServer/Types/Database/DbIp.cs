using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SoundShapesServer.Types.Database;

/// <summary>
/// Used for IP authentication
/// </summary>
[PrimaryKey(nameof(Id))]
public class DbIp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    public required Guid UserId { get; init; }
    public DbUser User { get; init; } = null!;

    [MaxLength(39)]
    public required string IpAddress { get; init; }
    public required DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset AuthorizedDate { get; set; }
    public bool Authorized { get; init; }
    public bool OneTimeUse { get; set; }
    public ICollection<DbToken> Tokens { get; set; } = null!;
}