using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SoundShapesServer.Attributes;

namespace SoundShapesServer.Responses.Api.Framework.Documentation;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class ApiOrderTypeResponse
{
    public ApiOrderTypeResponse(OrderTypeAttribute attribute)
    {
        ParameterName = attribute.ParameterName;
        Summary = attribute.Summary;
    }

    public string ParameterName { get; set; }

    public string Summary { get; set; }
}