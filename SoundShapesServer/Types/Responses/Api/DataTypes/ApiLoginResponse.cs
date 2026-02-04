using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Responses.Api.DataTypes;

public record ApiLoginResponse : IApiResponse<ApiLoginResponse>
{
    public required ApiFullUserResponse User { get; set; }
    public required ApiTokenResponse AccessToken { get; set; }
    public required ApiRefreshTokenResponse RefreshToken { get; set; }
    public static ApiLoginResponse Example()
    {
        return new ApiLoginResponse
        {
            User = ApiFullUserResponse.Example(),
            AccessToken = ApiTokenResponse.Example(),
            RefreshToken = ApiRefreshTokenResponse.Example(),
        };
    }
}