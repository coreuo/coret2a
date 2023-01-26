using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface IAccessKey
{
    int AccessKey { set; }

    internal void ReadAccessKey<TData>(TData data)
        where TData : IData
    {
        AccessKey = data.ReadInt();
    }
}