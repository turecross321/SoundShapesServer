using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiGameAuthenticationSettingsResponse : IApiResponse,
    IDataConvertableFrom<ApiGameAuthenticationSettingsResponse, GameUser>
{
    public required bool AllowPsnAuthentication { get; set; }
    public required bool AllowRpcnAuthentication { get; set; }
    public required bool AllowIpAuthentication { get; set; }

    public static ApiGameAuthenticationSettingsResponse FromOld(GameUser old)
    {
        return new ApiGameAuthenticationSettingsResponse
        {
            AllowPsnAuthentication = old.AllowPsnAuthentication,
            AllowRpcnAuthentication = old.AllowRpcnAuthentication,
            AllowIpAuthentication = old.AllowIpAuthentication
        };
    }

    public static IEnumerable<ApiGameAuthenticationSettingsResponse> FromOldList(IEnumerable<GameUser> oldList)
    {
        return oldList.Select(FromOld);
    }
}