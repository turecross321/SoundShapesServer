using AttribDoc;
using SoundShapesServer.Attributes;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Framework.Documentation;

public class ApiRouteResponse : IApiResponse
{
    public required string Method { get; set; }
    public required string RouteUri { get; set; }
    public required string Summary { get; set; }
    public required bool AuthenticationRequired { get; set; }
    public required PermissionsType? MinimumPermissionsType { get; set; }
    public required IEnumerable<ApiParameterResponse> Parameters { get; set; }
    public required IEnumerable<ApiDocumentationErrorResponse> PotentialErrors { get; set; }
    public required IEnumerable<ApiOrderTypeResponse>? OrderTypes { get; set; }

    private static ApiRouteResponse FromRoute(Route old)
    {
        const string minPermsKey = "MinimumPermissionsType";
        PermissionsType? minPerms = null;
        if (old.ExtraProperties.TryGetValue(minPermsKey, out object? property))
        {
            minPerms = (PermissionsType?)property;
        }

        IEnumerable<ApiOrderTypeResponse>? orderTypes = null;
        if (old.ExtraProperties.TryGetValue("orderTypes", out object? types))
        {
            List<OrderTypeAttribute> list = (List<OrderTypeAttribute>)types;
            orderTypes = list.Select(a => new ApiOrderTypeResponse(a));   
        }

        return new ApiRouteResponse
        {
            Method = old.Method,
            RouteUri = old.RouteUri,
            Summary = old.Summary,
            AuthenticationRequired = old.AuthenticationRequired,
            MinimumPermissionsType = minPerms,
            Parameters = ApiParameterResponse.FromParameterList(old.Parameters),
            PotentialErrors = ApiDocumentationErrorResponse.FromErrorList(old.PotentialErrors),
            OrderTypes = orderTypes
        };
    }

    public static IEnumerable<ApiRouteResponse> FromRouteList(IEnumerable<Route> oldList) => oldList.Select(FromRoute);
}