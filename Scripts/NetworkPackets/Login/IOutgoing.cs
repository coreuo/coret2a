using NetworkPackets.Attributes.Outgoing;

// ReSharper disable once CheckNamespace
namespace NetworkPackets.Login.Domain;

public partial interface ILogin<TState, TData, TShard, TShardCollection, TAccount>
{
    #region Iternal
    [InternalShardPacket(0x00)]
    public void OnInternalShardAuthorization(TShard shard, TAccount account, TData data)
    {
        account.WriteName(data);

        account.WritePassword(data);

        account.WriteAccessKey(data);
    }
    #endregion

    [Packet(0x82)]
    public void OnPacketAccountLoginFailed(TState state, TData data)
    {
        state.WriteStatus(data);
    }

    [Packet(0xA8)]
    [Sized]
    public void OnPacketBritanniaList(TShardCollection shards, TData data)
    {
        WriteShards(shards, data);
    }

    [Packet(0x8C)]
    public void OnPacketUserServer(TShard shard, TAccount account, TData data)
    {
        shard.WriteShardConnection(data);

        account.WriteAccessKey(data);
    }
}