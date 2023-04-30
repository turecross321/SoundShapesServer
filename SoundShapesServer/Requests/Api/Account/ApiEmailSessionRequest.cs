namespace SoundShapesServer.Requests.Api.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiEmailSessionRequest
{
    public ApiEmailSessionRequest(string newEmail)
    {
        NewEmail = newEmail;
    }

    public string NewEmail { get; }
}