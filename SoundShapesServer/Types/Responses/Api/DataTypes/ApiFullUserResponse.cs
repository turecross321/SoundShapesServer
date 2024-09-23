using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Responses.Api.DataTypes;

public record ApiFullUserResponse : IApiDbResponse<DbUser, ApiFullUserResponse>
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required UserRole Role { get; set; }
    public required bool VerifiedEmail { get; set; }
    public required bool FinishedRegistration { get; set; }
    public required DateTime CreationDate { get; set; }
    public required DateTime? RegistrationExpiryDate { get; set; }
    
    public static ApiFullUserResponse FromDb(DbUser user)
    {
        return new ApiFullUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            VerifiedEmail = user.VerifiedEmail,
            FinishedRegistration = user.FinishedRegistration,
            CreationDate = user.CreationDate,
            RegistrationExpiryDate = user.RegistrationExpiryDate
        };
    }
}