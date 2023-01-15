namespace Core.Launcher.Domain;

public interface IObject
{
    int Id { get; }

    Pointer Pointer { get; }

    public Pool GetPool();
}