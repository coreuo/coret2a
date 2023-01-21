using Core.Abstract.Attributes;
using Packets.Shared;

namespace Packets.Shard.Domain;

[Entity("Login", "Server")]
public interface ILogin<in TData>
    where TData : IData
{
    void SendInternal(TData data);
}