using Core.Abstract.Attributes;
using Packets.Login.Features;
using Packets.Server.Features;

namespace Packets.Login;

[Entity("Login", "Server")]
public interface ILogin<TState, TData, TShard, out TShardCollection, TAccount> :
    IName,
    IPassword,
    IEndPoint,
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
    [Priority(0.1)]
    [Case("InternalPacketReceived", "id", 0x01)]
    public void OnInternalShardAccountOnline(TData data)
    {
        ReadName(data);
    }

    [Priority(0.1)]
    [Case("InternalPacketReceived", "id", 0x02)]
    public void OnInternalShardAccountOffline(TData data)
    {
        ReadName(data);
    }

    [Priority(0.1)]
    [Case("PacketReceived", "id", 0x80)]
    public void OnPacketAccountLoginRequest(TState state, TData data)
    {
        state.ReadName(data);

        state.ReadPassword(data);

        state.ReadLoginKey(data);
    }

    [Priority(0.1)]
    [Case("PacketReceived", "id", 0xA0)]
    public void OnPacketBritanniaSelect(TState state, TData data)
    {
        state.ReadShard(data);
    }

    [Priority(0.1)]
    [Case("PacketReceived", "id", 0xA4)]
    public void OnPacketHardwareInfo(TState state, TData data)
    {
        state.ReadHardwareInfo(data);
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

        state.Account.WriteName(data);

        state.Account.WritePassword(data);

        state.Account.WriteAccessKey(data);

        EndOutgoingNoSizePacket(data);

        SendToShard(state, data);
    }

    private void SendToShard(TState state, TData data)
    {
        state.Shard.SendInternal(data);
#if DEBUG
        Debug($"internal sent {data.Length} bytes");
#endif
    }
#if DEBUG
    private void Debug(string text)
    {
        Console.WriteLine($"[{Identity}] {text}");
    }
#endif
}