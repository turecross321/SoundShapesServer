using Newtonsoft.Json.Linq;

namespace SoundShapesServer.Helpers;

public class CloudSyncHelper
{
    public static byte[] CombineSaves(byte[] save1, byte[] save2)
    {
        JObject result = new JObject();

        string save1String = System.Text.Encoding.UTF8.GetString(save1, 0, save1.Length);
        JObject json1 = JObject.Parse(save1String);
        
        string save2String = System.Text.Encoding.UTF8.GetString(save2, 0, save2.Length);
        JObject json2 = JObject.Parse(save2String);
        
        foreach (JProperty prop in json1.Properties())
        {
            if (json2.Property(prop.Name) != null)
            {
                JToken val1 = prop.Value;
                JToken val2 = json2[prop.Name];

                if (val1.Type == JTokenType.Integer && val2.Type == JTokenType.Integer)
                {
                    result[prop.Name] = (int)val1 > (int)val2 ? val1 : val2;
                }
                else
                {
                    result[prop.Name] = val2;
                }
            }
            else
            {
                result.Add(prop.Name, prop.Value);
            }
        }

        foreach (JProperty prop in json2.Properties())
        {
            if (result.Property(prop.Name) == null)
            {
                result.Add(prop.Name, prop.Value);
            }
        }

        string combinedJson = result.ToString();
        
        byte[] combinedSaves = System.Text.Encoding.UTF8.GetBytes(combinedJson);

        return combinedSaves;
    }
}