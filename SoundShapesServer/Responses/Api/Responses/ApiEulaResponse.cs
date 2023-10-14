namespace SoundShapesServer.Responses.Api.Responses;

public class ApiEulaResponse
{
    public ApiEulaResponse(string eula)
    {
        Content = eula;
    }

    public string Content { get; set; }
}