namespace SoundShapesServer.Requests.Api.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiLoginRequest
{
    public ApiLoginRequest(string email, string passwordSha512)
    {
        Email = email;
        PasswordSha512 = passwordSha512;
    }

    public string Email { get; }
    public string PasswordSha512 { get; }
}