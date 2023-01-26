using NetworkPackets.Attributes.Incoming;

// ReSharper disable once CheckNamespace
namespace NetworkPackets.Login.Domain;

public partial interface ILogin<TState, TData, TShard, TShardCollection, TAccount>
{
    #region Iternal
    [InternalPacket(0x01)]
    public void OnInternalAccountOnline(TData data)
    {
        ReadName(data);
    }

    [InternalPacket(0x02)]
    public void OnInternalAccountOffline(TData data)
    {
        ReadName(data);
    }
    #endregion

    [Packet(0x80)]
    public void OnPacketAccountLoginRequest(TState state, TData data)
    {
        state.ReadName(data);

        state.ReadPassword(data);

        state.ReadLoginKey(data);
    }

    [Packet(0xA0)]
    public void OnPacketBritanniaSelect(TState state, TData data)
    {
        state.ReadShard(data);
    }

    [Packet(0xA4)]
    public void OnPacketHardwareInfo(TState state, TData data)
    {
        state.ReadHardwareInfo(data);
    }
}