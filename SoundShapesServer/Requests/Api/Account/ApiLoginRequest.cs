// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiLoginRequest
{
    public string Email { get; init; }
    public string PasswordSha512 { get; init; }
}