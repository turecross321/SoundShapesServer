using AttribDoc;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Framework.Documentation;

public class ApiRouteResponse : IApiResponse
{
    public required string Method { get; set; }
    public required string RouteUri { get; set; }
    public required string Summary { get; set; }
    public required bool AuthenticationRequired { get; set; }
    public PermissionsType MinimumPermissionsType { get; set; }
    public required IEnumerable<ApiParameterResponse> Parameters { get; set; }
    public required IEnumerable<ApiErrorResponse> PotentialErrors { get; set; }

    private static ApiRouteResponse FromRoute(Route old)
    {
        return new ApiRouteResponse
        {
            Method = old.Method,
            RouteUri = old.RouteUri,
            Summary = old.Summary,
            AuthenticationRequired = old.AuthenticationRequired,
            MinimumPermissionsType = PermissionsType.Default,  // todo: make this actually work
            Parameters = ApiParameterResponse.FromParameterList(old.Parameters),
            PotentialErrors = ApiErrorResponse.FromErrorList(old.PotentialErrors),
        };
    }

    public static IEnumerable<ApiRouteResponse> FromRouteList(IEnumerable<Route> oldList) => oldList.Select(FromRoute)!;
}