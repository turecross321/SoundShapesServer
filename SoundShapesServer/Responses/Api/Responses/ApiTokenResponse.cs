using SoundShapesServer.Types.Authentication;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiTokenResponse
{
    [Obsolete("Empty constructor for deserialization.", true)]
    public ApiTokenResponse()
    {
        
    }
    public ApiTokenResponse(GameToken token)
    {
        Id = token.Id;
        CreationDate = token.CreationDate;
        ExpiryDate = token.ExpiryDate;
    }

    public string Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
}