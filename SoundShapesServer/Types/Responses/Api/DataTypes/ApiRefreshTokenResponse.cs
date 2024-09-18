using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Responses.Api.DataTypes;

public record ApiRefreshTokenResponse : IApiDbResponse<DbRefreshToken, ApiRefreshTokenResponse>
{
    public required Guid Id { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset ExpiryDate { get; set; }
    public static ApiRefreshTokenResponse FromDb(DbRefreshToken value)
    {
        return new ApiRefreshTokenResponse
        {
            Id = value.Id,
            CreationDate = value.CreationDate,
            ExpiryDate = value.ExpiryDate
        };
    }
}