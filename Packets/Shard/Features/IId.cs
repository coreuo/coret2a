namespace Packets.Shard.Features;

public interface IId
{
    int Id { get; }

    internal void WriteId<TData>(TData data)
        where TData : IData
    {
        data.WriteInt(Id);
    }
}