namespace Core.Launcher.Domain;

public class Property
{
    internal int Offset { get; set; } = -1;

    public string Name { get; }

    public int Size { get; }

    public Property(string name, int size)
    {
        Name = name;
        Size = size;
    }
}