using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.IP_Authorization;

public class ApiAuthorizedIpResponse
{
    public ApiAuthorizedIpResponse(IpAuthorization ip)
    {
        IpAddress = ip.IpAddress;
        OneTimeUse = ip.OneTimeUse;
    }

    public string IpAddress { get; set; }
    public bool OneTimeUse { get; set; }
}