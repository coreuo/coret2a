namespace Core.Launcher.Domain;

public interface IObject
{
    int Id { get; }

    int GetOffset(int index);

    public Pool GetPool();
}