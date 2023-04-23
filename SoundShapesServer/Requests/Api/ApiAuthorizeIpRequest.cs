namespace SoundShapesServer.Requests.Api;

public class ApiAuthorizeIpRequest
{
    public string IpAddress { get; set; }
    public bool OneTimeUse { get; set; }
}