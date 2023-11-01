using SoundShapesServer.Responses.Api.Framework;

namespace SoundShapesServer.Responses.Api.Responses.Levels;

public class ApiLevelRelationResponse : IApiResponse
{
    public required bool Completed { get; set; }
    public required bool Liked { get; init; }
    public required bool Queued { get; init; }
}