using SoundShapesServer.Responses.Api.Framework;

namespace SoundShapesServer.Responses.Api.Responses.Users;

public class ApiUserRelationResponse : IApiResponse
{
    public bool Following { get; set; } 
    public bool Followed { get; set; }
}