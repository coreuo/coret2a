using Core.Abstract.Attributes;
using Packets.Login.Features;

namespace Packets.Login.Domain;

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