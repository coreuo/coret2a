using Core.Abstract.Attributes;
using Packets.Login.Features;

namespace Packets.Login;

[Entity("Login", "Server")]
public interface ILogin<TState, TData, TShard, out TShardCollection, TAccount> : 
    IUsername,
    IPassword,
    IStatus,
    ITransfer<TData>,
    IShardList<TShard, TShardCollection>
    where TState : IState<TData, TAccount, TShard>
    where TData : IData
    where TShard : IShard<TData>
    where TShardCollection : ICollection<TShard>
    where TAccount : IAccount
{
#if DEBUG
    string Identity { get; }
#endif
    void PacketAccountLoginRequest(TState state);

    void PacketHardwareInfo(TState state);

    void PacketBritanniaSelect(TState state);

    void InternalShardAccountOnline();

    void InternalShardAccountOffline();

    [Priority(1.0)]
    public void OnInternalReceived(TData data)
    {
        var id = BeginInternalIncomingPacket(data);

        switch (id)
        {
            case 0x01:
            {
                ReadUsername(data);

                EndIncomingPacket(data);

                InternalShardAccountOnline();

                return;
            }
            case 0x02:
            {
                ReadUsername(data);

                EndIncomingPacket(data);

                InternalShardAccountOffline();

                return;
            }
            default: throw new InvalidOperationException($"Unknown internal 0x{id:X2}.");
        }
    }

    [Priority(1.0)]
    public void OnPacketReceived(TState state, TData data)
    {
        if (state.Seed == 0) return;

        var id = BeginIncomingPacket(data);

        switch (id)
        {
            case 0x80:
            {
                state.ReadUsername(data);

                state.ReadPassword(data);

                state.ReadLoginKey(data);

                EndIncomingPacket(data);

                PacketAccountLoginRequest(state);

                return;
            }
            case 0xA0:
            {
                state.ReadShard(data);

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

        state.WriteStatus(data);

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

    [Priority(1.0)]
    public void OnInternalShardAuthorization(TState state)
    {
        var data = BeginInternalOutgoingNoSizePacket(0x00);

        state.Account.WriteUsername(data);

        state.Account.WritePassword(data);

        state.Account.WriteAccessKey(data);

        EndOutgoingNoSizePacket(data);

        SendToShard(state, data);
    }

    private void SendToShard(TState state, TData data)
    {
        state.Shard.SendInternal(data);

        Debug($"internal sent {data.Length} bytes");
    }

    private void Debug(string text)
    {
        Console.WriteLine($"[{Identity}] {text}");
    }
}