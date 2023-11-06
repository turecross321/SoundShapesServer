using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Moderation;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Authentication;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiLoginResponse : IApiResponse, IDataConvertableFrom<ApiLoginResponse, GameToken>
{
    public required ApiTokenResponse AccessToken { get; init; }
    public required ApiTokenResponse? RefreshToken { get; init; }
    public required ApiUserBriefResponse User { get; set; }
    public required IEnumerable<ApiPunishmentResponse> ActivePunishments { get; set; }

    public static ApiLoginResponse FromOld(GameToken old)
    {
        ApiTokenResponse? refresh = null;
        if (old.RefreshToken != null)
            refresh = ApiTokenResponse.FromOld(old.RefreshToken);

        return new ApiLoginResponse
        {
            AccessToken = ApiTokenResponse.FromOld(old),
            RefreshToken = refresh,
            User = ApiUserBriefResponse.FromOld(old.User),
            ActivePunishments = ApiPunishmentResponse.FromOldList(old.User.Punishments.ActivePunishments())
        };
    }

    public static IEnumerable<ApiLoginResponse> FromOldList(IEnumerable<GameToken> oldList)
    {
        return oldList.Select(FromOld);
    }
}