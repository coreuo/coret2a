using Core.Abstract.Attributes;

namespace Packets.Server;

[Entity("Server")]
public interface IServer<in TState, in TData>
    where TState : IState<TData>
    where TData : IData
{
    void PacketReceived(TState state, TData data);

    void PacketSeed(TState state);

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
        state.ReadSeed(data);

        data.ReadEnd();

        PacketSeed(state);
    }
#if DEBUG
    private static void DebugIncoming(string text)
    {
        Console.WriteLine($"Incoming: {text}");
    }
#endif
}