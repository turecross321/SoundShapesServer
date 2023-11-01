using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Types.Authentication;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiTokenResponse : IApiResponse, IDataConvertableFrom<ApiTokenResponse, GameToken>
{
    public required string Id { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset ExpiryDate { get; set; }

    public static ApiTokenResponse FromOld(GameToken old)
    {
        return new ApiTokenResponse
        {
            Id = old.Id,
            CreationDate = old.CreationDate,
            ExpiryDate = old.ExpiryDate
        };
    }

    public static IEnumerable<ApiTokenResponse> FromOldList(IEnumerable<GameToken> oldList)
    {
        return oldList.Select(FromOld);
    }
}