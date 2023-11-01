namespace SoundShapesServer.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FilterPropertyAttribute : Attribute
{
    public FilterPropertyAttribute(string parameterName, string summary)
    {
        ParameterName = parameterName;
        Summary = summary;
    }

    public string ParameterName { get; }

    public string Summary { get; }
}