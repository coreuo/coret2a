using Core.Abstract.Attributes;
using NetworkPackets.Login.Features;

namespace NetworkPackets.Login.Domain;

[Entity("Shard", "Server")]
public interface IShard : IShardInfo
{
}

[Entity("Shard", "Server")]
public interface IShard<TData> : IShard
    where TData : Shared.IData
{
    void SendInternal(TData data);
}