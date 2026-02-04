namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public class ApiEmptyResponse : IApiResponse<ApiEmptyResponse>
{
    public static ApiEmptyResponse Example()
    {
        return new ApiEmptyResponse();
    }
}