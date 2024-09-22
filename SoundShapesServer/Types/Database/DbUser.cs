using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bunkum.Core.Authentication;
using Microsoft.EntityFrameworkCore;

namespace SoundShapesServer.Types.Database;

[PrimaryKey(nameof(Id))]
public class DbUser: IUser, IDbItem<Guid>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
    
    [MaxLength(16)]
    public required string Name { get; set; }
    
    public required UserRole Role { get; set; }
    
    public bool FinishedRegistration { get; set; }
    public bool VerifiedEmail { get; set; }
    
    [MaxLength(320)]
    public string? EmailAddress { get; set; }
    
    [MaxLength(60)]
    [MinLength(60)]
    public string? PasswordBcrypt { get; set; }

    public bool RpcnAuthorization { get; set; } = false;
    public bool PsnAuthorization { get; set; } = false;
    public bool IpAuthorization { get; set; } = false;
    
    public required DateTimeOffset CreationDate { get; init; }

    public override string ToString()
    {
        string result = $"{this.Name} ({this.Role}";

        if (!FinishedRegistration)
            result += ", Unregistered";
        if (!VerifiedEmail)
            result += ", Unverified email";

        result += ")";

        return result;
    }
}