namespace SoundShapesServer.Responses.Api.IpAuthorization;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiIpAddressResponse
{
    public ApiIpAddressResponse(Types.IpAuthorization ip)
    {
        IpAddress = ip.IpAddress;
        Authorized = ip.Authorized;
        OneTimeUse = ip.OneTimeUse;
        CreationDate = ip.CreationDate.ToUnixTimeSeconds();
        ModificationDate = ip.ModificationDate.ToUnixTimeSeconds();
    }

    public string IpAddress { get; set; }
    public bool Authorized { get; set; }
    public bool OneTimeUse { get; set; }
    public long CreationDate { get; set; }
    public long ModificationDate { get; set; }
}