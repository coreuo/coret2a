using Core.Abstract.Attributes;

namespace Packets.Shared;

[Entity("State")]
public interface IState<TData> : ISeed
    where TData : IData
{
    void Send(TData data);
}