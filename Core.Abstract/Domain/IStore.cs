namespace Core.Abstract.Domain;

public interface IStore<TValue>
{
    int GetId(TValue value);

    TValue GetValue(int id);
}