using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface ITarget
{
    int Target { set; }

    internal void ReadTarget<TData>(TData data)
        where TData : IData
    {
        Target = data.ReadInt();
    }
}