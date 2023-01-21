using Core.Abstract.Attributes;
using Packets.Attributes.Outgoing;
using Packets.Login.Features;
using Packets.Shared;
using Packets.Shared.Features;

namespace Packets.Login.Domain;

[Entity("Login", "Server")]
public interface ILogin<TState, TData, TShard, TAccount>
    where TState : IState<TData, TAccount, TShard>
    where TData : IData
    where TShard : IShard<TData>
    where TAccount : IAccount
{
#if DEBUG
    string Identity { get; }
#endif
    [Priority(2.0)]
    public void OnInternalSendToShard(TState state, TData data)
    {
        state.Shard.SendInternal(data);
#if DEBUG
        Debug($"internal sent {data.Length} bytes to shard");
#endif
    }
#if DEBUG
    private void Debug(string text)
    {
        Console.WriteLine($"[{Identity}] {text}");
    }
#endif
}