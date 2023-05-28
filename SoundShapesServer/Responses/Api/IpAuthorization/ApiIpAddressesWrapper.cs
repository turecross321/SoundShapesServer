namespace SoundShapesServer.Responses.Api.IpAuthorization;

public class ApiIpAddressesWrapper
{
    public ApiIpAddressesWrapper(IEnumerable<Types.IpAuthorization> ipAddresses, int totalAddresses)
    {
        IpAddresses = ipAddresses.Select(a=>new ApiIpAddressResponse(a)).ToArray();
        Count = totalAddresses;
    }

    public ApiIpAddressResponse[] IpAddresses { get; }
    public int Count { get; }
}