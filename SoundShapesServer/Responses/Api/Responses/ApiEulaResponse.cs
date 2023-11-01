using SoundShapesServer.Responses.Api.Framework;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiEulaResponse : IApiResponse
{
    public required string CustomContent { get; set; }
    public required string License { get; set; }
    public required string SourceCodeUrl { get; set; }

    public static ApiEulaResponse FromOld(string content, string sourceCodeUrl)
    {
        return new ApiEulaResponse
        {
            CustomContent = content,
            License = Globals.AGPLLicense,
            SourceCodeUrl = sourceCodeUrl
        };
    }
}