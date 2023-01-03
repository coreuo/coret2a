using Core.Abstract.Attributes;
using Packets.Shard.Outgoing;

namespace Packets.Shard;

[Entity("Shard", "Server")]
public interface IShard<in TState, TData, TAccount, TMobile, out TMobileCollection> :
    ITransfer<TData>,
    ICityList,
    IGameTime
    where TState : IState<TData, TAccount, TMobile, TMobileCollection>
    where TData : IData
    where TAccount : IAccount<TMobile, TMobileCollection>
    where TMobile : IMobile
    where TMobileCollection : ICollection<TMobile>
{
    void PacketPreLogin(TState state);

    void PacketPostLogin(TState state);

    void PacketRequestTip(TState state);

    void PacketRequestMove(TState state);

    [Priority(1.0)]
    public void OnPacketReceived(TState state, TData data)
    {
        if (state.Seed == 0) return;

        var id = BeginIncomingPacket(data);

        switch (id)
        {
            case 0x02:
            {
                state.ReadDirection(data);

                state.ReadSequence(data);

                EndIncomingPacket(data);

                PacketRequestMove(state);

                return;
            }
            case 0x5D:
            {
                state.ReadCharacterPattern(data);
                
                state.ReadCredentials(data);

                state.ReadCharacterSlot(data);

                state.ReadClientSeed(data);

                EndIncomingPacket(data);

                PacketPreLogin(state);

                return;
            }
            case 0x91:
            {
                state.ReadAccessKey(data);

                state.ReadCredentials(data);

                EndIncomingPacket(data);

                PacketPostLogin(state);

                return;
            }
            case 0xA7:
            {
                state.ReadTipRequest(data);

                state.ReadDirection(data);

                EndIncomingPacket(data);

                PacketRequestTip(state);

                return;
            }
            default: throw new InvalidOperationException($"Unknown packet 0x{id:X2}.");
        }
    }

    [Priority(1.0)]
    public void OnPacketLoginConfirm(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x1B);

        state.Character.WriteLoginConfirm(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketZMove(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x20);

        state.Character.WriteZMove(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketOkMove(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x22);

        state.WriteSequence(data);

        state.Character.WriteNotoriety(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketLight(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x4E);

        state.Character.WriteSerial(data);

        state.Character.WriteLight(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketSunlight(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x4F);

        state.Character.WriteSunlight(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketLoginComplete(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x55);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketGameTime(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x5B);

        WriteGameTime(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketWeatherChange(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x65);

        state.Character.WriteWeather(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketCombat(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x72);

        state.WriteCombat(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketEquippedMobile(TState state)
    {
        var data = BeginOutgoingPacket(0x78);

        state.Character.WriteEquippedMobile(data);

        EndOutgoingPacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketCitiesAndCharacters(TState state)
    {
        var data = BeginOutgoingPacket(0xA9);

        state.Account.WriteCharacters(data);

        WriteCities(data);

        EndOutgoingPacket(data);

        state.Send(data);
    }
}