using AttribDoc;

namespace SoundShapesServer.Responses.Api.Documentation;

public class ApiErrorResponse : IApiResponse
{
    public required string Name { get; set; }
    public required string OccursWhen { get; set; }
    
    private static ApiErrorResponse FromError(Error old)
    {
        return new ApiErrorResponse
        {
            Name = old.Name,
            OccursWhen = old.OccursWhen,
        };
    }
    
    public static IEnumerable<ApiErrorResponse> FromErrorList(IEnumerable<Error> oldList) => oldList.Select(FromError);
}