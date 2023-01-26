using Core.Abstract.Attributes;
using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Domain;

[Entity("Login", "Server")]
public interface ILogin<in TData>
    where TData : IData
{
    void SendInternal(TData data);
}