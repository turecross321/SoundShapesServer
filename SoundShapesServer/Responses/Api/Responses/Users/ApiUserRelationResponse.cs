using SoundShapesServer.Responses.Api.Framework;

namespace SoundShapesServer.Responses.Api.Responses.Users;

public class ApiUserRelationResponse : IApiResponse
{
    public required bool Following { get; set; }
    public required bool Followed { get; set; }
}