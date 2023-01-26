using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface IBody
{
    ushort Body { get; }

    internal void WriteBody<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Body);
    }
}