namespace SoundShapesServer.Requests.Api.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiSetUsernameRequest
{
    public ApiSetUsernameRequest(string newUsername)
    {
        NewUsername = newUsername;
    }

    public string NewUsername { get; }
}