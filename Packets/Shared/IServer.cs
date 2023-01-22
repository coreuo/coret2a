using System.Collections.Concurrent;
using Core.Abstract.Attributes;
using Packets.Login.Features;

namespace Packets.Shared;

[Entity("Server")]
public interface IServer<in TState, TData, out TDataConcurrentQueue> : ITransfer<TData>,
    IEndPoint
    where TState : IState<TData>
    where TData : IData
    where TDataConcurrentQueue : IProducerConsumerCollection<TData>
{
#if DEBUG
    string Identity { get; }
#endif
    TDataConcurrentQueue InternalQueue { get; }

    void ReleaseData(TData data);

    void InternalPacketReceived(TData data, byte id);

    void BatchReceived(TState state, TData data);

    void PacketReceived(TState state, TData data, byte id);

    void Seed(TState state, TData data);

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
            var id = BeginIncomingInternal(data);

            InternalPacketReceived(data, id);
        }
    }

    [Priority(1.0)]
    public void OnDataReceived(TState state, TData data)
    {
        if (state.Seed == 0) Seed(state, data);

        BatchReceived(state, data);
    }

    [Priority(1.0)]
    public void OnBatchReceived(TState state, TData data)
    {
        while (data.Start < data.Length)
        {
            var id = BeginIncomingPacket(data);

            PacketReceived(state, data, id);
        }
    }

    [Priority(0.001)]
    [Link("ReceiveSize")]
    public void OnReceiveSizeBegin(TState state, TData data)
    {
        data.OffsetSize();
    }

    [Priority(0.01)]
    [Link("InternalSend")]
    public void OnInternalSendBegin(TState state, TData data, byte id)
    {
        BeginOutgoingInternal(data, id);
    }

    [Priority(0.01)]
    [Link("PacketSend")]
    public void OnPacketSendBegin(TState state, TData data, byte id)
    {
        BeginOutgoingPacket(data, id);
    }

    [Priority(0.02)]
    [Link("SendSize")]
    public void OnSendSizeBegin(TState state, TData data)
    {
        data.OffsetSize();
    }

    [Priority(0.1)]
    public void OnSeed(TState state, TData data)
    {
#if DEBUG
        DebugIncoming("0x?? SEED");
#endif
        data.Offset = 0;

        state.ReadSeed(data);

        data.ReadEnd();
    }

    [Priority(0.2)]
    [Link("InternalPacketReceived")]
    public void OnInternalReceivedEnd(TData data)
    {
        EndIncomingInternal(data);

        ReleaseData(data);
    }

    [Priority(0.2)]
    [Link("PacketReceived")]
    public void OnPacketReceivedEnd(TData data)
    {
        EndIncomingPacket(data);
    }

    [Priority(0.2)]
    [Link("InternalSend")]
    public void OnInternalSendEnd(TState state, TData data)
    {
        EndOutgoingInternal(data);
    }

    [Priority(0.2)]
    [Link("PacketSend")]
    public void OnPacketSendEnd(TState state, TData data)
    {
        EndOutgoingPacket(data);
    }

    [Priority(0.3)]
    [Link("SendSize")]
    public void OnSendSizeEnd(TState state, TData data)
    {
        data.WriteSize((ushort)data.Length);
    }

    [Priority(2.0)]
    public void OnPacketSend(TState state, TData data)
    {
        state.Send(data);
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