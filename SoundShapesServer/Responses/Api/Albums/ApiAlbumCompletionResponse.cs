namespace SoundShapesServer.Responses.Api.Albums;

public class ApiAlbumCompletionResponse : IApiResponse
{
    public ApiAlbumCompletionResponse(int levelsBeaten, int totalLevels)
    {
        LevelsBeaten = levelsBeaten;
        TotalLevels = totalLevels;
    }

    public int LevelsBeaten { get; set; }
    public int TotalLevels { get; set; }
}