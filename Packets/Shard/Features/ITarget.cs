using Packets.Shared;

namespace Packets.Shard.Features;

public interface ITarget
{
    int Target { set; }

    internal void ReadTarget<TData>(TData data)
        where TData : IData
    {
        Target = data.ReadInt();
    }
}