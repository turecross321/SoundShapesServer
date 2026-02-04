using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Responses.Api.DataTypes;

public record ApiEulaResponse : IApiResponse<ApiEulaResponse>
{
    public required string CustomText { get; init; }
    public required string License { get; init; }
    public required string RepositoryUrl { get; init; }
    public static ApiEulaResponse Example()
    {
        return new ApiEulaResponse
        {
            CustomText = "Welcome back to Sound Shapes!",
            License = "Yada Yada",
            RepositoryUrl = "https://github.com/turecross321/SoundShapesServer",
        };
        
    }
}