using Core.Abstract.Attributes;
using Packets.Server.Incoming;

namespace Packets.Server;

[Entity("State")]
public interface IState<TData> : IClientSeed
    where TData : IData
{
    void Send(TData data);
}