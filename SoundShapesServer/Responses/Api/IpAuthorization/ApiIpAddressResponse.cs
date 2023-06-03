namespace SoundShapesServer.Responses.Api.IpAuthorization;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiIpAddressResponse
{
    public ApiIpAddressResponse(Types.IpAuthorization ip)
    {
        IpAddress = ip.IpAddress;
        Authorized = ip.Authorized;
        OneTimeUse = ip.OneTimeUse;
        CreationDate = ip.CreationDate;
        ModificationDate = ip.ModificationDate;
    }

    public string IpAddress { get; set; }
    public bool Authorized { get; set; }
    public bool OneTimeUse { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
}