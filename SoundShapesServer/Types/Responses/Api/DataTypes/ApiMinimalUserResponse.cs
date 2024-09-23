using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Responses.Api.DataTypes;

public record ApiMinimalUserResponse : IApiDbResponse<DbUser, ApiMinimalUserResponse>
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required UserRole Role { get; set; }
    public required DateTime CreationDate { get; set; }

    public static ApiMinimalUserResponse FromDb(DbUser user)
    {
        return new ApiMinimalUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            CreationDate = user.CreationDate
        };
    }
}