namespace BunkumOpenApi.Attributes;

public class DocDescriptionAttribute(string description) : Attribute
{
    public readonly string Description = description;
}