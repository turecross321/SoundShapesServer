using AttribDoc;
using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Responses.Api.DataTypes.Documentation;

public record ApiRouteResponse : IApiResponse
{
    public required string Method { get; set; }
    public required string RouteUri { get; set; }
    public required string Summary { get; set; }
    public required bool AuthenticationRequired { get; set; }
    public required UserRole? MinimumRole { get; set; }
    public required IEnumerable<ApiParameterResponse> Parameters { get; set; }
    public required IEnumerable<ApiDocumentationErrorResponse> PotentialErrors { get; set; }

    public static ApiRouteResponse FromRoute(Route route)
    {
        const string minPermsKey = "MinimumRole";
        UserRole? minRole = null;
        if (route.ExtraProperties.TryGetValue(minPermsKey, out object? property))
        {
            minRole = (UserRole?)property; // todo: implement a MinimumRole attribute and use this
        }

        return new ApiRouteResponse
        {
            Method = route.Method,
            RouteUri = route.RouteUri,
            Summary = route.Summary,
            AuthenticationRequired = route.AuthenticationRequired,
            MinimumRole = minRole,
            Parameters = ApiParameterResponse.FromParameterList(route.Parameters),
            PotentialErrors = ApiDocumentationErrorResponse.FromErrorList(route.PotentialErrors),
        };
    }
}