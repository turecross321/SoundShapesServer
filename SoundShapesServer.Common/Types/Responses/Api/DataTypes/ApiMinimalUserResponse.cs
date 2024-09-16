using SoundShapesServer.Common.Types.Database;
using SoundShapesServer.Common.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Common.Types.Responses.Api.DataTypes;

public record ApiMinimalUserResponse : IApiDbResponse<DbUser, ApiMinimalUserResponse>
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public UserRole Role { get; set; }

    public static ApiMinimalUserResponse FromDb(DbUser user)
    {
        return new ApiMinimalUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role
        };
    }
    
    public static IEnumerable<ApiMinimalUserResponse> FromDbList(IEnumerable<DbUser> dbList)
    {
        return dbList.Select(FromDb);
    }
}