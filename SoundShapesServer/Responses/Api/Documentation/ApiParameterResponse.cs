using AttribDoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SoundShapesServer.Responses.Api.Documentation;

public class ApiParameterResponse : IApiResponse
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
            Type = old.Type,
        };
    }

    public static IEnumerable<ApiParameterResponse> FromParameterList(IEnumerable<Parameter> oldList) => oldList.Select(FromParameter)!;
}