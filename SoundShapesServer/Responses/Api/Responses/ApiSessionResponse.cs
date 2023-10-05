using SoundShapesServer.Types.Sessions;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiSessionResponse
{
    public ApiSessionResponse(GameSession session)
    {
        Id = session.Id;
        CreationDate = session.CreationDate.ToUnixTimeSeconds();
        ExpiryDate = session.ExpiryDate.ToUnixTimeSeconds();
    }

    public string Id { get; }
    public long CreationDate { get; }
    public long ExpiryDate { get; }
}