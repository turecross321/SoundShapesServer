using SoundShapesServer.Types.ServerResponses.Api.ApiTypes.Errors;

namespace SoundShapesServer.Types.ServerResponses.Api.ApiTypes;

public class ApiOkResponse : ApiResponse<ApiEmptyResponse>
{
    public ApiOkResponse() : base(new ApiEmptyResponse())
    {}

    public ApiOkResponse(ApiError error) : base(error)
    {}
    
    public static implicit operator ApiOkResponse(ApiError error) => new(error);
}