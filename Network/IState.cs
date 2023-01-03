using System.Collections.Concurrent;
using Core.Abstract.Attributes;

namespace Network;

[Entity("State")]
public interface IState : ISocket
{
    [Size(30)]
    Span<char> Identity { get; }
}

[Entity("State")]
public interface IState<TData, out TDataConcurrentQueue> : IReceiver<TData, TDataConcurrentQueue>, ISender<TData>
    where TData : IData
    where TDataConcurrentQueue : IProducerConsumerCollection<TData>
{
    [Priority(1.0)]
    public void OnStop()
    {
        if (!Locked || Sending < 0 || !Receiving)
            return;

        Locked = false;

        Close();
    }
}