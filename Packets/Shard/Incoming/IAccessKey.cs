namespace Packets.Shard.Incoming;

public interface IAccessKey
{
    int AccessKey { set; }

    internal void ReadAccessKey<TData>(TData data)
        where TData : IData
    {
        AccessKey = data.ReadInt();
    }
}