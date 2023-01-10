namespace Core.Launcher.Domain;

public class Property
{
    public int Offset { get; set; }

    public string Name { get; }

    public int Size { get; }

    public Property(string name, int size, int offset = -1)
    {
        Name = name;
        Size = size;
        Offset = offset;
    }

    public override string ToString()
    {
        return $"{Name} {Size} at {Offset}";
    }
}