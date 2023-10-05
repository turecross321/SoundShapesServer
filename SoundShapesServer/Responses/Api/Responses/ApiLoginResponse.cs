using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Responses.Moderation;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiLoginResponse
{
    public ApiLoginResponse(GameUser user, GameSession session, GameSession? refreshSession)
    {
        Session = new ApiSessionResponse(session);
        if (refreshSession != null) 
            RefreshSession = new ApiSessionResponse(refreshSession);
        User = new ApiUserBriefResponse(session.User);
        ActivePunishments = PunishmentHelper.GetActivePunishments(user).AsEnumerable().Select(p => new ApiPunishmentResponse(p)).ToArray();
    }

    public ApiSessionResponse Session { get; set; }
    public ApiSessionResponse? RefreshSession { get; set; }
    public ApiUserBriefResponse User { get; set; }
    public ApiPunishmentResponse[] ActivePunishments { get; set; }
}