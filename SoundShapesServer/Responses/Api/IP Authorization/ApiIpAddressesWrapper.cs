using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.IP_Authorization;

public class ApiIpAddressesWrapper
{
    public ApiIpAddressesWrapper(IpAuthorization[] ipAddresses, int totalAddresses)
    {
        IpAddresses = ipAddresses.Select(a=>new ApiIpAddressResponse(a)).ToArray();
        Count = totalAddresses;
    }

    public ApiIpAddressResponse[] IpAddresses { get; }
    public int Count { get; }
}