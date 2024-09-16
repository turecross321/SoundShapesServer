using AttribDoc;
using SoundShapesServer.Common.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Common.Types.Responses.Api.DataTypes.Documentation;

public record ApiRouteResponse : IApiResponse
{
    public required string Method { get; set; }
    public required string RouteUri { get; set; }
    public required string Summary { get; set; }
    public required bool AuthenticationRequired { get; set; }
    public required UserRole? MinimumRole { get; set; }
    public required IEnumerable<ApiParameterResponse> Parameters { get; set; }
    public required IEnumerable<ApiDocumentationErrorResponse> PotentialErrors { get; set; }

    private static ApiRouteResponse FromRoute(Route old)
    {
        const string minPermsKey = "MinimumRole";
        UserRole? minRole = null;
        if (old.ExtraProperties.TryGetValue(minPermsKey, out object? property))
        {
            minRole = (UserRole?)property;
        }

        return new ApiRouteResponse
        {
            Method = old.Method,
            RouteUri = old.RouteUri,
            Summary = old.Summary,
            AuthenticationRequired = old.AuthenticationRequired,
            MinimumRole = minRole,
            Parameters = ApiParameterResponse.FromParameterList(old.Parameters),
            PotentialErrors = ApiDocumentationErrorResponse.FromErrorList(old.PotentialErrors),
        };
    }

    public static IEnumerable<ApiRouteResponse> FromRouteList(IEnumerable<Route> oldList) => oldList.Select(FromRoute);
}