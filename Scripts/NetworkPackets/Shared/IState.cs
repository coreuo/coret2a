using Core.Abstract.Attributes;

namespace NetworkPackets.Shared;

[Entity("State")]
public interface IState<TData> : ISeed
    where TData : IData
{
    void Send(TData data);
}