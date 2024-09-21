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
    
    public static ApiFullUserResponse FromDb(DbUser value)
    {
        return new ApiFullUserResponse
        {
            Id = value.Id,
            Name = value.Name,
            Role = value.Role,
            VerifiedEmail = value.VerifiedEmail,
            FinishedRegistration = value.FinishedRegistration
        };
    }
}