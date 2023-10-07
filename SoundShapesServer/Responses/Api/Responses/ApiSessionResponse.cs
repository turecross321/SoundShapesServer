using SoundShapesServer.Types.Sessions;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiSessionResponse
{
    [Obsolete("Empty constructor for deserialization.", true)]
    public ApiSessionResponse()
    {
        
    }
    public ApiSessionResponse(GameSession session)
    {
        Id = session.Id;
        CreationDate = session.CreationDate.ToUnixTimeSeconds();
        ExpiryDate = session.ExpiryDate.ToUnixTimeSeconds();
    }

    public string Id { get; set; }
    public long CreationDate { get; set; }
    public long ExpiryDate { get; set; }
}