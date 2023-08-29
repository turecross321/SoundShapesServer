namespace SoundShapesServer.Responses.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiIpResponse : IApiResponse
{
    public ApiIpResponse(Types.GameIp gameIp)
    {
        IpAddress = gameIp.IpAddress;
        Authorized = gameIp.Authorized;
        OneTimeUse = gameIp.OneTimeUse;
        CreationDate = gameIp.CreationDate.ToUnixTimeSeconds();
        ModificationDate = gameIp.ModificationDate.ToUnixTimeSeconds();
    }

    public string IpAddress { get; set; }
    public bool Authorized { get; set; }
    public bool OneTimeUse { get; set; }
    public long CreationDate { get; set; }
    public long ModificationDate { get; set; }
}