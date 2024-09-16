using AttribDoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SoundShapesServer.Common.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Common.Types.Responses.Api.DataTypes.Documentation;

public record ApiParameterResponse : IApiResponse
{
    public required string Name { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public required ParameterType Type { get; set; }
    public required string Summary { get; set; }
    
    private static ApiParameterResponse FromParameter(Parameter old)
    {
        return new ApiParameterResponse
        {
            Name = old.Name,
            Summary = old.Summary,
            Type = old.Type
        };
    }

    public static IEnumerable<ApiParameterResponse> FromParameterList(IEnumerable<Parameter> oldList) => oldList.Select(FromParameter);
}