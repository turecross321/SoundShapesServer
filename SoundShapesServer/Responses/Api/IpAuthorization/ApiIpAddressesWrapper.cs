using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.IPAuthorization;

public class ApiIpAddressesWrapper
{
    public ApiIpAddressesWrapper(IEnumerable<IpAuthorization> ipAddresses, int totalAddresses)
    {
        IpAddresses = ipAddresses.Select(a=>new ApiIpAddressResponse(a)).ToArray();
        Count = totalAddresses;
    }

    public ApiIpAddressResponse[] IpAddresses { get; }
    public int Count { get; }
}