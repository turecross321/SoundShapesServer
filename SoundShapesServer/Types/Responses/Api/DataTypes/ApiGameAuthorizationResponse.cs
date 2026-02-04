using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Responses.Api.DataTypes;

public record ApiAuthorizationSettingsResponse : IApiResponse<ApiAuthorizationSettingsResponse>
{
    public required bool RpcnAuthorization { get; set; }
    public required bool PsnAuthorization { get; set; }
    public required bool IpAuthorization { get; set; }
    public static ApiAuthorizationSettingsResponse Example()
    {
        return new ApiAuthorizationSettingsResponse
        {
            RpcnAuthorization = false,
            PsnAuthorization = false,
            IpAuthorization = false,
        };
    }
}