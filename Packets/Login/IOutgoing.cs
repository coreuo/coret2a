using Packets.Attributes.Outgoing;

// ReSharper disable once CheckNamespace
namespace Packets.Login.Domain;

public partial interface ILogin<TState, TData, TShard, out TShardCollection, TAccount>
{
    #region Iternal
    [InternalShardPacket(0x00)]
    public void OnInternalAuthorization(TState state, TData data)
    {
        state.Account.WriteName(data);

        state.Account.WritePassword(data);

        state.Account.WriteAccessKey(data);
    }
    #endregion

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