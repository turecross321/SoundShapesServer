// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiSetEmailRequest
{
    public string SetEmailTokenId { get; set; }
    public string NewEmail { get; set; }
}