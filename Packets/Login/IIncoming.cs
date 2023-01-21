using Core.Abstract.Attributes;
using Packets.Attributes.Incoming;
using Packets.Login.Domain;
using Packets.Login.Features;
using Packets.Shared.Features;

namespace Packets.Login;

[Entity("Login", "Server")]
public interface IIncoming<TState, TData, TShard> :
    IName,
    IPassword
    where TState : IState<TData, TShard>
    where TData : Shared.IData
    where TShard : IShard<TData>
{
    [InternalPacket(0x01)]
    public void OnInternalShardAccountOnline(TData data)
    {
        ReadName(data);
    }

    [InternalPacket(0x02)]
    public void OnInternalShardAccountOffline(TData data)
    {
        ReadName(data);
    }

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