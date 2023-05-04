namespace SoundShapesServer.Responses.Api.Albums;

public class ApiAlbumCompletionResponse
{
    public ApiAlbumCompletionResponse(int levelsBeaten)
    {
        LevelsBeaten = levelsBeaten;
    }

    public int LevelsBeaten { get; set; }
}