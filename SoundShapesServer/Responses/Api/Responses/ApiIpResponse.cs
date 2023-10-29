using SoundShapesServer.Responses.Api.Framework;

namespace SoundShapesServer.Responses.Api.Responses;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiIpResponse : IApiResponse
{
    public ApiIpResponse(Types.GameIp gameIp)
    {
        IpAddress = gameIp.IpAddress;
        Authorized = gameIp.Authorized;
        OneTimeUse = gameIp.OneTimeUse;
        CreationDate = gameIp.CreationDate;
        ModificationDate = gameIp.ModificationDate;
    }

    public string IpAddress { get; set; }
    public bool Authorized { get; set; }
    public bool OneTimeUse { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
}