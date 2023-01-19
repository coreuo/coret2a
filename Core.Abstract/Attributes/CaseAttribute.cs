namespace Core.Abstract.Attributes;

public class CaseAttribute : Attribute
{
    public string Method { get; }

    public string Subject { get; }

    public string Property { get; }

    public int Value { get; }

    public CaseAttribute(string method, string subject, string property, int value)
    {
        Method = method;
        Subject = subject;
        Property = property;
        Value = value;
    }
}