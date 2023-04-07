using System.ComponentModel;
using System.Reflection;
using SoundShapesServer.Requests;

namespace SoundShapesServer.Helpers;

public class LeaderboardHelper
{
    public static LeaderboardSubmissionRequest DeSerializeSubmission(string str)
    {
        LeaderboardSubmissionRequest response = new LeaderboardSubmissionRequest();
        
        string[] queries = str.Split("&");

        foreach (string? query in queries)
        {
            string[] nameAndValue = query.Split("=");
            string name = nameAndValue[0];
            string value = nameAndValue[1];

            PropertyInfo? propertyInfo = response.GetType().GetProperty(name);
            
            System.Type? propertyType = propertyInfo?.PropertyType;

            if (propertyType == null) continue;
            
            TypeConverter? converter = TypeDescriptor.GetConverter(propertyType);
            object? convertedValue = converter.ConvertFromString(value);
            
            propertyInfo?.SetValue(response, convertedValue);
        }

        return response;
    }
}