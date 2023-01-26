using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface IHits
{
    ushort Hits { get; }

    ushort HitsMaximum { get; }

    internal void WriteHits<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Hits);

        data.WriteUShort(HitsMaximum);
    }
}