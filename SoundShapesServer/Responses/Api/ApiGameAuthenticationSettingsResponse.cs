using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api;

public class ApiGameAuthenticationSettingsResponse
{
    public ApiGameAuthenticationSettingsResponse(GameUser user)
    {
        AllowPsnAuthentication = user.AllowPsnAuthentication;
        AllowRpcnAuthentication = user.AllowRpcnAuthentication;
        AllowIpAuthentication = user.AllowIpAuthentication;
    }
    
    public bool AllowPsnAuthentication { get; set; }
    public bool AllowRpcnAuthentication { get; set; }
    public bool AllowIpAuthentication { get; set; }
}