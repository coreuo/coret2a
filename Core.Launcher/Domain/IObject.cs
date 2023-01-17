namespace Core.Launcher.Domain;

public interface IObject<TSave> : IObject
{
    TSave Save { get; }
}

public interface IObject
{
    Pointer Pointer { get; }

    public static abstract int GetSize();
}