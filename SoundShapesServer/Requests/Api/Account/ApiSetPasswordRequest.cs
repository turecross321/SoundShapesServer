// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiSetPasswordRequest
{
    public string SetPasswordTokenId { get; set; }
    public string NewPasswordSha512 { get; set; }
}