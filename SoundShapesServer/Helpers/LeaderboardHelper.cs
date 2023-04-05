using System.ComponentModel;
using SoundShapesServer.Requests;

namespace SoundShapesServer.Helpers;

public class LeaderboardHelper
{
    public static LeaderboardSubmissionRequest DeSerializeSubmission(string str)
    {
        LeaderboardSubmissionRequest response = new LeaderboardSubmissionRequest();
        
        string[] queries = str.Split("&");

        foreach (var query in queries)
        {
            string[] nameAndValue = query.Split("=");
            string name = nameAndValue[0];
            string value = nameAndValue[1];

            var propertyInfo = response.GetType().GetProperty(name);
            
            var propertyType = propertyInfo?.PropertyType;

            if (propertyType == null) continue;
            
            var converter = TypeDescriptor.GetConverter(propertyType);
            var convertedValue = converter.ConvertFromString(value);
            
            propertyInfo?.SetValue(response, convertedValue);
        }

        return response;
    }
}