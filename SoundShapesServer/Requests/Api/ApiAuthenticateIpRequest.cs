namespace SoundShapesServer.Requests.Api;

public class ApiAuthenticateIpRequest
{
    public string IpAddress { get; set; }
    public bool OneTimeUse { get; set; }
}