#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiAuthenticateIpRequest
{
    public string IpAddress { get; set; }
    public bool OneTimeUse { get; set; }
}