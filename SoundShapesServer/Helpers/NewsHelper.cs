using Newtonsoft.Json.Linq;

namespace SoundShapesServer.Helpers;

public class NewsHelper
{
    public static JArray SerializeNews(string? text = null, string? title = null, string? fullText = null, string? url = null)
    {
        // We can't return the news directly because some dumbass decided the variable names should start with 00_, hence this being a thing

        JArray json = new JArray();
        JObject jObject = new JObject();
        
        if (text != null) jObject.Add("00_text", text);
        if (title != null) jObject.Add("00_title", title);
        if (fullText != null) jObject.Add("00_fullText", fullText);
        if (text != null) jObject.Add("00_url", url);
        
        json.Add(jObject);

        return json;
    }
}