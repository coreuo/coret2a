using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Server.Shard;

[Entity("Shard", "Server")]
public interface IShard<in TState, TAccount, TMobile>
    where TState : IState<TAccount, TMobile>
    where TAccount : IAccount
    where TMobile : IMobile
{
    void Listen();

    void Slice();

    void PacketPingResponse(TState state);

    void PacketSkills(TState state);

    [Priority(1.0)]
    [Case("PacketClientQuery", "state", "Mode", 0x05)]
    public void OnPacketClientQuerySkills(TState state)
    {
        PacketSkills(state);
    }

    [Priority(1.0)]
    public void OnPacketPingRequest(TState state)
    {
        PacketPingResponse(state);
    }
}