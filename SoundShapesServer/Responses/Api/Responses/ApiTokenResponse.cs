using SoundShapesServer.Types.Authentication;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiTokenResponse
{
    [Obsolete("Empty constructor for deserialization.", true)]
    public ApiTokenResponse()
    {
        
    }
    public ApiTokenResponse(AuthToken token)
    {
        Id = token.Id;
        CreationDate = token.CreationDate.ToUnixTimeSeconds();
        ExpiryDate = token.ExpiryDate.ToUnixTimeSeconds();
    }

    public string Id { get; set; }
    public long CreationDate { get; set; }
    public long ExpiryDate { get; set; }
}