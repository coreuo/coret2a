using Core.Abstract.Attributes;
using Packets.Attributes.Outgoing;
using Packets.Login.Domain;
using Packets.Login.Features;
using Packets.Shared;

namespace Packets.Login;

[Entity("Login", "Server")]
public interface IOutgoing<TState, TData, TShard, out TShardCollection, TAccount> :
    IShardList<TShard, TShardCollection>
    where TState : IState<TData, TAccount, TShard>
    where TData : IData
    where TShard : IShard<TData>
    where TShardCollection : ICollection<TShard>
    where TAccount : IAccount
{
    [InternalShardPacket(0x00)]
    public void OnInternalShardAuthorization(TState state, TData data)
    {
        state.Account.WriteName(data);

        state.Account.WritePassword(data);

        state.Account.WriteAccessKey(data);
    }

    [Packet(0x82)]
    public void OnPacketAccountLoginFailed(TState state, TData data)
    {
        state.WriteStatus(data);
    }

    [Packet(0xA8)]
    [Sized]
    public void OnPacketBritanniaList(TState state, TData data)
    {
        WriteShards(data);
    }

    [Packet(0x8C)]
    public void OnPacketUserServer(TState state, TData data)
    {
        state.Shard.WriteShardConnection(data);

        state.Account.WriteAccessKey(data);
    }
}