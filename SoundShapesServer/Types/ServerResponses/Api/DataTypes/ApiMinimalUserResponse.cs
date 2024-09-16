using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.ServerResponses.Api.ApiTypes;

namespace SoundShapesServer.Types.ServerResponses.Api.DataTypes;

public record ApiMinimalUserResponse: IApiResponse
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