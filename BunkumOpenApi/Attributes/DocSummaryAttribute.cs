namespace BunkumOpenApi.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DocSummaryAttribute(string summary) : Attribute
{
    public readonly string Summary = summary;
}