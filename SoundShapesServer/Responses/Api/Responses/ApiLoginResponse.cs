using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Responses.Moderation;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiLoginResponse
{    
    [Obsolete("Empty constructor for deserialization.", true)]
    public ApiLoginResponse() {}
    
    public ApiLoginResponse(GameUser user, GameToken accessToken, GameToken? refreshToken)
    {
        AccessToken = new ApiTokenResponse(accessToken);
        if (refreshToken != null) 
            RefreshToken = new ApiTokenResponse(refreshToken);
        User = new ApiUserBriefResponse(accessToken.User);
        ActivePunishments = PunishmentHelper.GetActivePunishments(user).AsEnumerable().Select(p => new ApiPunishmentResponse(p)).ToArray();
    }

    public ApiTokenResponse AccessToken { get; set; }
    public ApiTokenResponse? RefreshToken { get; set; }
    public ApiUserBriefResponse User { get; set; }
    public ApiPunishmentResponse[] ActivePunishments { get; set; }
}