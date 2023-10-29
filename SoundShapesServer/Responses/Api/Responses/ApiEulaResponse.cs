namespace SoundShapesServer.Responses.Api.Responses;

public class ApiEulaResponse
{
    public ApiEulaResponse(string content, string sourceCodeUrl)
    {
        CustomContent = content;
        License = Globals.AGPLLicense;
        SourceCodeUrl = sourceCodeUrl;
    }

    public string CustomContent { get; set; }
    public string License { get; set; }
    public string SourceCodeUrl { get; set; }
}