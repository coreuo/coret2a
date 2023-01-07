﻿using Core.Abstract.Attributes;
using System.Collections.Concurrent;

namespace Packets.Server;

[Entity("Server")]
public interface IServer<in TState, in TData, out TDataConcurrentQueue>
    where TState : IState<TData>
    where TData : IData
    where TDataConcurrentQueue : IProducerConsumerCollection<TData>
{
#if DEBUG
    string Identity { get; }
#endif
    TDataConcurrentQueue InternalQueue { get; }

    void ReleaseData(TData data);

    void InternalReceived(TData data);

    void PacketReceived(TState state, TData data);

    void PacketSeed(TState state);

    [Priority(1.0)]
    public void OnSendInternal(TData data)
    {
        InternalQueue.TryAdd(data);
    }

    [Priority(0.9)]
    public void OnSlice()
    {
        while (InternalQueue.TryTake(out var data))
        {
#if DEBUG
            Debug($"internal received {data.Length} bytes");
#endif
            InternalReceived(data);

            ReleaseData(data);
        }
    }

    [Priority(1.0)]
    public void OnDataReceived(TState state, TData data)
    {
        while (data.Start < data.Length)
        {
            PacketReceived(state, data);
        }
    }

    [Priority(1.1)]
    public void OnPacketReceived(TState state, TData data)
    {
        if (state.Seed != 0) return;
#if DEBUG
        DebugIncoming("0x?? SEED");
#endif
        data.Offset = 0;

        state.ReadSeed(data);

        data.ReadEnd();

        PacketSeed(state);
    }
#if DEBUG
    private static void DebugIncoming(string text)
    {
        Console.WriteLine($"Incoming: {text}");
    }

    private void Debug(string text)
    {
        Console.WriteLine($"[{Identity}] {text}");
    }
#endif
}