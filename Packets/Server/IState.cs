using Core.Abstract.Attributes;

namespace Packets.Server;

[Entity("State")]
public interface IState<in TData> : ISeed
    where TData : IData
{
    void Send(TData data);
}