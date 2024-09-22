using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SoundShapesServer.Types.Database;

/// <summary>
/// Special type of token that is supposed to be easily entered by a user for two factor purposes, such as changing/verifying e-mail.
/// Don't last as long as normal tokens, and should be included in the request body as opposed to as a header.
/// </summary>
[PrimaryKey(nameof(Id))]
public class DbCode : IDbItem<int>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    [Key]
    [MinLength(6)]
    [MaxLength(6)]
    public required string Code { get; init; }
    
    
    [ForeignKey(nameof(User))]
    public required Guid UserId { get; init; }
    public DbUser User { get; init; } = null!;
    
    public required CodeType CodeType { get; init; }
    
    public required DateTime CreationDate { get; init; }
    public required DateTime ExpiryDate { get; init; }
}