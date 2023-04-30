namespace SoundShapesServer.Requests.Api.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiSetPasswordRequest
{
    public ApiSetPasswordRequest(string newPasswordSha512)
    {
        NewPasswordSha512 = newPasswordSha512;
    }

    public string NewPasswordSha512 { get; }
}