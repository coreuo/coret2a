using System.Collections.Concurrent;
using Core.Abstract.Attributes;

namespace Network;

[Entity("Server")]
public interface IServer<TState, out TStateConcurrentQueue, out TStateCollection, TData, out TDataConcurrentQueue> : IListener<TState, TStateConcurrentQueue>, ITransfer<TData>
    where TState : IState<TData, TDataConcurrentQueue>
    where TStateConcurrentQueue : IProducerConsumerCollection<TState>
    where TStateCollection : ICollection<TState>
    where TData : IData
    where TDataConcurrentQueue : IProducerConsumerCollection<TData>
{
    TStateCollection States { get; }

    void Connected(TState state);

    void Disconnected(TState state);

    void DataReceived(TState state, TData data);

    [Priority(1.0)]
    public void OnSlice()
    {
        while (ListenQueue.TryTake(out var state))
        {
            States.Add(state);

            Connected(state);

            state.OnReceive();
        }

        foreach (var state in States)
        {
            if (state.Sending <= 0 && !state.Receiving)
            {
                Disconnected(state);

                States.Remove(state);
            }

            else Process(state);
        }

        if (!Locked && !Listening && !States.Any())
            Running = false;
    }

    private void Process(TState state)
    {
        while (state.ReceiveQueue.TryTake(out var data))
        {
            try
            {
                DataReceived(state, data);
            }
            catch
#if DEBUG
                (Exception exception)
#endif
            {
#if DEBUG
                Debug("cannot process data", exception);
#endif
                Locked = false;
            }

            ReleaseData(data);
        }
    }

    [Priority(1.0)]
    public void OnStop()
    {
        if (Listening) Close();

        foreach (var state in States)
        {
            state.Close();
        }
    }

#if DEBUG
    private void Debug(string text, Exception? exception = null)
    {
        Console.WriteLine($"[{Identity}] {text}{(exception != null ? $"\n{exception}" : null)}");
    }
#endif
}