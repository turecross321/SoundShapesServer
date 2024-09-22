using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SoundShapesServer.Types.Database;

[PrimaryKey(nameof(Id))]
public class DbRefreshToken : IDbItem<Guid>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; } 
    
    [ForeignKey(nameof(User))]
    public required Guid UserId { get; init; }

    public DbUser User { get; init; } = null!;

    public ICollection<DbToken> Tokens { get; init; } = null!;
    
    public required DateTimeOffset CreationDate { get; init; }
    public required DateTimeOffset ExpiryDate { get; set; }
}