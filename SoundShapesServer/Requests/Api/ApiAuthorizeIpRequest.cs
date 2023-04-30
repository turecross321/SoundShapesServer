namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiAuthorizeIpRequest
{
    public ApiAuthorizeIpRequest(string ipAddress, bool oneTimeUse)
    {
        IpAddress = ipAddress;
        OneTimeUse = oneTimeUse;
    }

    public string IpAddress { get; }
    public bool OneTimeUse { get; }
}