using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SoundShapesServer.Attributes;

namespace SoundShapesServer.Responses.Api.Framework.Documentation;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class ApiOrderTypeResponse
{
    public ApiOrderTypeResponse(OrderTypeAttribute attribute)
    {
        Value = attribute.Value;
        Summary = attribute.Summary;
    }

    public string Value { get; set; }

    public string Summary { get; set; }
}