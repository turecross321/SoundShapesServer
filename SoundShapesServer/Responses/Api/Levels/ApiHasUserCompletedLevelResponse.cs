namespace SoundShapesServer.Responses.Api.Levels;

public class ApiHasUserCompletedLevelResponse
{
    public ApiHasUserCompletedLevelResponse(bool hasCompleted)
    {
        HasCompleted = hasCompleted;
    }

    public bool HasCompleted { get; set; }
}