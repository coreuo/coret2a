using Packets.Attributes.Incoming;

// ReSharper disable once CheckNamespace
namespace Packets.Login.Domain;

public partial interface ILogin<TState, TData, TShard, out TShardCollection, TAccount>
{
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