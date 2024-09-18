using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Responses.Api.DataTypes;

public record ApiTokenResponse : IApiDbResponse<DbToken, ApiTokenResponse>
{
    public required Guid Id { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset ExpiryDate { get; set; }
    public required TokenType TokenType { get; set; }
    
    public static ApiTokenResponse FromDb(DbToken value)
    {
        return new ApiTokenResponse
        {
            Id = value.Id,
            CreationDate = value.CreationDate,
            ExpiryDate = value.ExpiryDate,
            TokenType = value.TokenType
        };
    }
}