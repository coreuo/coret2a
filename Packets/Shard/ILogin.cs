using Core.Abstract.Attributes;

namespace Packets.Shard;

[Entity("Login", "Server")]
public interface ILogin<in TData>
    where TData : IData
{
    void SendInternal(TData data);
}