namespace SoundShapesServer.Responses.Api.Users;

public class ApiUserRelationResponse : IApiResponse
{
    public bool Following { get; set; } 
    public bool Followed { get; set; }
}