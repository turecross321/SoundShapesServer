using Bunkum.Core.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SoundShapesServer.Responses;

public class CustomJsonSerializer : IBunkumSerializer
{
    private static readonly JsonSerializer JsonSerializer = new JsonSerializer()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public string[] ContentTypes { get; } = {
        "application/json"
    };

    public byte[] Serialize(object data)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
                    JsonSerializer.Serialize(jsonWriter, data);
                    jsonWriter.Flush();
                    return memoryStream.ToArray();
                }
            }
        }
    }
}