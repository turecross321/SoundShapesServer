using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bunkum.Core.Authentication;
using Microsoft.EntityFrameworkCore;

namespace SoundShapesServer.Types.Database;

[PrimaryKey(nameof(Id))]
public class DbToken : IToken<DbUser>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; } 
    
    [ForeignKey(nameof(User))]
    public required Guid UserId { get; init; }

    public DbUser User { get; init; } = null!;
    
    public required DateTimeOffset CreationDate { get; init; }
    public required DateTimeOffset ExpiryDate { get; init; }
    
    public required TokenType TokenType { get; set; }
    public required PlatformType? Platform { get; set; }
}