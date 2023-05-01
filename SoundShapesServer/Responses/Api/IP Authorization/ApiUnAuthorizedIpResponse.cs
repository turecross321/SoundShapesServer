using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.IP_Authorization;

public class ApiUnAuthorizedIpResponse
{
    public ApiUnAuthorizedIpResponse(IpAuthorization ip)
    {
        IpAddress = ip.IpAddress;
    }

    public string IpAddress { get; set; }
}