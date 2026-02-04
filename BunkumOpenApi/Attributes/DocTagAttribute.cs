namespace BunkumOpenApi.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class DocTagAttribute(string tagName) : Attribute
{
    public string TagName { get; set; } = tagName;
}