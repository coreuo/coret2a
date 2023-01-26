using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface IGold
{
    uint Gold { get; }

    internal void WriteGold<TData>(TData data)
        where TData : IData
    {
        data.WriteUInt(Gold);
    }
}