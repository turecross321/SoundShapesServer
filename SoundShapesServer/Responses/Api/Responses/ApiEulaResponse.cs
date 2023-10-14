namespace SoundShapesServer.Responses.Api.Responses;

public class ApiEulaResponse
{
    public ApiEulaResponse(string content)
    {
        CustomContent = content;
        License = Globals.AGPLLicense;
        GitRepo = "https://github.com/turecross321/SoundShapesServer"; // TODO: THIS SHOULD OBVIOUSLY BE AUTOMATIC
    }

    public string CustomContent { get; set; }
    public string License { get; set; }
    public string GitRepo { get; set; }
}