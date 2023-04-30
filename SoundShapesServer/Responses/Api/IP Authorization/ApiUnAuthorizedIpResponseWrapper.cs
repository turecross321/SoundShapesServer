namespace SoundShapesServer.Responses.Api.IP_Authorization;

public class ApiUnAuthorizedIpResponseWrapper
{
    public ApiUnAuthorizedIpResponseWrapper(ApiUnAuthorizedIpResponse[] ipAddresses)
    {
        IpAddresses = ipAddresses;
    }

    public ApiUnAuthorizedIpResponse[] IpAddresses { get; }
}