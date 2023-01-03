using Core.Abstract.Attributes;
using Packets.Login.Outgoing;

namespace Packets.Login;

[Entity("Login", "Server")]
public interface ILogin<in TState, TData, TShard, out TShardCollection, TAccount> : 
    ITransfer<TData>,
    IShardList<TShard, TShardCollection>
    where TState : IState<TData, TAccount, TShard>
    where TData : IData
    where TShard : IShard
    where TShardCollection : ICollection<TShard>
    where TAccount : IAccount
{
    void PacketAccountLoginRequest(TState state);

    void PacketHardwareInfo(TState state);

    void PacketBritanniaSelect(TState state);

    [Priority(1.0)]
    public void OnPacketReceived(TState state, TData data)
    {
        if (state.Seed == 0) return;

        var id = BeginIncomingPacket(data);

        switch (id)
        {
            case 0x80:
            {
                state.ReadLoginRequest(data);

                EndIncomingPacket(data);

                PacketAccountLoginRequest(state);

                return;
            }
            case 0xA0:
            {
                state.ReadSelectedShard(data);

                EndIncomingPacket(data);

                PacketBritanniaSelect(state);

                return;
            }
            case 0xA4:
            {
                state.ReadHardwareInfo(data);

                EndIncomingPacket(data);

                PacketHardwareInfo(state);

                return;
            }
            default: throw new InvalidOperationException($"Unknown packet 0x{id:X2}.");
        }
    }

    [Priority(1.0)]
    public void OnPacketAccountLoginFailed(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x82);

        state.WriteLoginFailedReason(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketBritanniaList(TState state)
    {
        var data = BeginOutgoingPacket(0xA8);

        WriteShards(data);

        EndOutgoingPacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketUserServer(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x8C);

        state.Shard.WriteShardConnection(data);

        state.Account.WriteAccessKey(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }
}