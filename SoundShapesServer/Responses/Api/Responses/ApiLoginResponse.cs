using SoundShapesServer.Extensions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Responses.Moderation;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiLoginResponse
{    
    [Obsolete("Empty constructor for deserialization.", true)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApiLoginResponse() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    public ApiLoginResponse(GameUser user, GameToken accessToken, GameToken? refreshToken)
    {
        AccessToken = new ApiTokenResponse(accessToken);
        if (refreshToken != null) 
            RefreshToken = new ApiTokenResponse(refreshToken);
        User = new ApiUserBriefResponse(accessToken.User);
        ActivePunishments = user.Punishments.ActivePunishments().AsEnumerable().Select(p => new ApiPunishmentResponse(p)).ToArray();
    }

    public ApiTokenResponse AccessToken { get; set; }
    public ApiTokenResponse? RefreshToken { get; set; }
    public ApiUserBriefResponse User { get; set; }
    public ApiPunishmentResponse[] ActivePunishments { get; set; }
}