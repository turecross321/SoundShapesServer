namespace SoundShapesServer.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class OrderTypeAttribute : Attribute
{
    public OrderTypeAttribute(string parameterName, string summary)
    {
        ParameterName = parameterName;
        Summary = summary;
    }

    public string ParameterName { get; }

    public string Summary { get; }
}