using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Albums;

public class LinerNoteResponse
{
    [JsonProperty("fontType")] public string FontType { get; set; }
    [JsonProperty("text")] public string Text { get; set; }
}