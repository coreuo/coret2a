using Core.Abstract.Attributes;
using Packets.Login.Features;

namespace Packets.Login;

[Entity("Shard", "Server")]
public interface IShard : IShardInfo
{
}

[Entity("Shard", "Server")]
public interface IShard<TData> : IShard
    where TData : IData
{
    void SendInternal(TData data);
}