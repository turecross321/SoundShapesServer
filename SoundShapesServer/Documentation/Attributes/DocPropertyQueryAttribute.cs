namespace SoundShapesServer.Documentation.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DocPropertyQueryAttribute : Attribute
{
    public DocPropertyQueryAttribute(string parameterName, string summary)
    {
        ParameterName = parameterName;
        Summary = summary;
    }

    public string ParameterName { get; }

    public string Summary { get; }
}