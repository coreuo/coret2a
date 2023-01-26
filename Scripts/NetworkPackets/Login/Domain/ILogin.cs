using Core.Abstract.Attributes;
using NetworkPackets.Login.Features;
using NetworkPackets.Shared;
using NetworkPackets.Shared.Features;

namespace NetworkPackets.Login.Domain;

[Entity("Login", "Server")]
public partial interface ILogin<TState, TData, TShard, TShardCollection, TAccount> :
    IName,
    IPassword,
    IEndPoint,
    IStatus,
    ITransfer<TData>,
    IShardList<TShard, TShardCollection>
    where TState : IState<TData, TAccount>
    where TData : IData
    where TShard : IShard<TData>
    where TShardCollection : ICollection<TShard>
    where TAccount : IAccount
{
#if DEBUG
    string Identity { get; }
#endif
    [Priority(2.0)]
    public void OnInternalSendToShard(TShard shard, TData data)
    {
        shard.SendInternal(data);
#if DEBUG
        Debug($"internal sent {data.Length} bytes to shard {shard}");
#endif
    }
#if DEBUG
    private void Debug(string text)
    {
        Console.WriteLine($"[{Identity}] {text}");
    }
#endif
}