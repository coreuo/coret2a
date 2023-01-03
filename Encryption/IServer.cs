using Core.Abstract.Attributes;

namespace Encryption;

[Entity("Server")]
public interface IServer<in TState, in TData>
    where TState : IState
    where TData : IData
{
    [Priority(1.0)]
    public void OnPacketSeed(TState state)
    {
        state.Initialize();
    }

    [Priority(0.9)]
    public void OnPacketReceived(TState state, TData data)
    {
        if (state.Seed == 0) return;

        state.Decrypt(data.Value, data.Start, data.Length);
    }
}