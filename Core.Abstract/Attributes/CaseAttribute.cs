namespace Core.Abstract.Attributes;

public class CaseAttribute : Attribute
{
    public string Method { get; }

    public string Property { get; }

    public string Value { get; }

    public CaseAttribute(string method, string property, string value)
    {
        Method = method;
        Property = property;
        Value = value;
    }
}