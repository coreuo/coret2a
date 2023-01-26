using Core.Abstract.Attributes;

namespace Scripts.NetworkEncryption;

[Entity("Server")]
public interface IServer<in TState, in TData>
    where TState : IState
    where TData : IData
{
    [Priority(1.0)]
    public void OnSeed(TState state)
    {
        state.Initialize();
    }

    [Priority(0.9)]
    public void OnBatchReceived(TState state, TData data)
    {
        state.Decrypt(data.Value, data.Start, data.Length);
    }
}