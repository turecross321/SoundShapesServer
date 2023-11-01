namespace SoundShapesServer.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class OrderTypeAttribute : Attribute
{
    public OrderTypeAttribute(string value, string summary)
    {
        Value = value;
        Summary = summary;
    }

    public string Value { get; }

    public string Summary { get; }
}