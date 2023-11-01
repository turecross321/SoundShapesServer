using SoundShapesServer.Responses.Api.Framework;

namespace SoundShapesServer.Responses.Api.Responses.Albums;

public class ApiAlbumCompletionResponse : IApiResponse
{
    public required int LevelsBeaten { get; set; }
    public required int TotalLevels { get; set; }
}