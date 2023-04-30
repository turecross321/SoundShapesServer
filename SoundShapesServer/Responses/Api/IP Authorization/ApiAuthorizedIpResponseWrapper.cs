namespace SoundShapesServer.Responses.Api.IP_Authorization;

public class ApiAuthorizedIpResponseWrapper
{
    public ApiAuthorizedIpResponseWrapper(ApiAuthorizedIpResponse[] ipAddresses)
    {
        IpAddresses = ipAddresses;
    }

    public ApiAuthorizedIpResponse[] IpAddresses { get; }
}