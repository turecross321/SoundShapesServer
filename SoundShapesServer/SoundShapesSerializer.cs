using Bunkum.Core.Responses;

namespace SoundShapesServer;

public class SoundShapesSerializer : IBunkumSerializer
{
    private static readonly JsonSerializer JsonSerializer = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public string[] ContentTypes { get; } =
    [
        "application/json",
    ];

    public byte[] Serialize(object data)
    {
        using MemoryStream memoryStream = new();
        using StreamWriter streamWriter = new(memoryStream);
        using JsonWriter jsonWriter = new JsonTextWriter(streamWriter);
        
        JsonSerializer.Serialize(jsonWriter, data);
        jsonWriter.Flush();
        return memoryStream.ToArray();
    }
}