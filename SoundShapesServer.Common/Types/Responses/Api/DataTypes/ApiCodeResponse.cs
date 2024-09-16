using SoundShapesServer.Common.Types.Database;
using SoundShapesServer.Common.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Common.Types.Responses.Api.DataTypes;

public record ApiCodeResponse : IApiDbResponse<DbCode, ApiCodeResponse>
{
    public required string Code { get; init; }
    public required ApiMinimalUserResponse User { get; init; }
    public required DateTimeOffset CreationDate { get; init; }
    public required DateTimeOffset ExpiryDate { get; init; }
    public required CodeType CodeType { get; init; }
    
    public static ApiCodeResponse FromDb(DbCode value)
    {
        return new ApiCodeResponse
        {
            Code = value.Code,
            User = ApiMinimalUserResponse.FromDb(value.User),
            CreationDate = value.CreationDate,
            ExpiryDate = value.ExpiryDate,
            CodeType = value.CodeType
        };
    }

    public static IEnumerable<ApiCodeResponse> FromDbList(IEnumerable<DbCode> value)
    {
        return value.Select(FromDb);
    }
}