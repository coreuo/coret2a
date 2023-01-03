namespace Core.Abstract.Attributes;

public class SizeAttribute : Attribute
{
    public int Size { get; }

    public SizeAttribute(int size)
    {
        Size = size;
    }
}