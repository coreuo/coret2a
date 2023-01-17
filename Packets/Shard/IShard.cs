using Core.Abstract.Attributes;
using Packets.Server.Features;
using Packets.Shard.Features;

namespace Packets.Shard;

[Entity("Shard", "Server")]
public interface IShard<TLogin, in TState, TData, TAccount, TMobile, out TMobileCollection, TMap, TSkill, TSkillArray> :
    ITransfer<TData>,
    ICityList,
    IGameTime,
    IName,
    IPassword,
    IAccessKey
    where TLogin : ILogin<TData>
    where TState : IState<TData, TAccount, TMobile, TMobileCollection, TMap, TSkill, TSkillArray>
    where TData : IData
    where TAccount : IAccount<TMobile, TMobileCollection>
    where TMobile : IMobile<TMap, TSkill, TSkillArray>
    where TMobileCollection : ICollection<TMobile>
    where TMap : IMap
    where TSkill : ISkill
    where TSkillArray : IReadOnlyList<TSkill>
{
#if DEBUG
    string Identity { get; }
#endif
    [Link("LoginServer.Shards.Owner")]
    TLogin Login { get; }

    void PacketPreLogin(TState state);

    void PacketPostLogin(TState state);

    void PacketRequestTip(TState state);

    void PacketRequestMove(TState state);

    void PacketRequestObjectUse(TState state);

    void PacketClientQuery(TState state);

    void PacketCombatRequest(TState state);

    void InternalShardAuthorization();

    [Priority(1.0)]
    public void OnInternalReceived(TData data)
    {
        var id = BeginInternalIncomingPacket(data);

        switch (id)
        {
            case 0x00:
            {
                ReadName(data);

                ReadPassword(data);

                ReadAccessKey(data);

                EndIncomingPacket(data);

                InternalShardAuthorization();

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
            case 0x02:
            {
                state.ReadDirection(data);

                state.ReadStatus(data);

                EndIncomingPacket(data);

                PacketRequestMove(state);

                return;
            }
            case 0x06:
            {
                state.ReadTarget(data);

                EndIncomingPacket(data);

                PacketRequestObjectUse(state);

                return;
            }
            case 0x34:
            {
                state.ReadPattern(data);

                state.ReadMode(data);

                state.ReadTarget(data);

                EndIncomingPacket(data);

                PacketClientQuery(state);

                return;
            }
            case 0x5D:
            {
                state.ReadPattern(data);
                
                state.ReadName(data);

                state.ReadPassword(data);

                state.ReadCharacterSlot(data);

                state.ReadSeed(data);

                EndIncomingPacket(data);

                PacketPreLogin(state);

                return;
            }
            case 0x72:
            {
                state.ReadCombat(data);

                EndIncomingPacket(data);

                PacketCombatRequest(state);

                return;
            }
            case 0x91:
            {
                state.ReadAccessKey(data);

                state.ReadName(data);

                state.ReadPassword(data);

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
        var character = state.Character;

        var map = character.Map;

        var data = BeginOutgoingNoSizePacket(0x1B);

        character.WriteId(data);

        data.WriteInt(0);

        character.WriteBody(data);

        character.WriteX(data);

        character.WriteY(data);

        character.WriteShortZ(data);

        character.WriteDirection(data);

        map.WriteByteAreaId(data);

        data.WriteInt(0);

        map.WriteArea(data);

        data.WriteInt(0);

        data.WriteShort(0);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketZMove(TState state)
    {
        var character = state.Character;

        var map = character.Map;

        var data = BeginOutgoingNoSizePacket(0x20);

        character.WriteId(data);

        character.WriteBody(data);

        data.WriteByte(0);

        character.WriteHue(data);

        character.WriteStatus(data);

        character.WriteX(data);

        character.WriteY(data);

        map.WriteUShortAreaId(data);

        character.WriteDirection(data);

        character.WriteSByteZ(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketOkMove(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x22);

        state.WriteStatus(data);

        state.Character.WriteNotoriety(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketSkills(TState state)
    {
        var data = BeginOutgoingPacket(0x3A);

        data.WriteByte(0x00);

        state.Character.WriteSkills(data);

        EndOutgoingPacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketLight(TState state)
    {
        var character = state.Character;

        var data = BeginOutgoingNoSizePacket(0x4E);

        character.WriteId(data);

        character.WriteLight(data);

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
    public void OnPacketCombatResponse(TState state)
    {
        var data = BeginOutgoingNoSizePacket(0x72);

        state.WriteCombat(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketNakedMobile(TState state)
    {
        var character = state.Character;

        var data = BeginOutgoingNoSizePacket(0x77);

        character.WriteId(data);

        character.WriteBody(data);

        character.WriteX(data);

        character.WriteY(data);

        character.WriteSByteZ(data);

        character.WriteDirection(data);

        character.WriteHue(data);

        character.WriteStatus(data);

        character.WriteNotoriety(data);

        EndOutgoingNoSizePacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketEquippedMobile(TState state)
    {
        var character = state.Character;

        var data = BeginOutgoingPacket(0x78);

        character.WriteId(data);

        character.WriteBody(data);

        character.WriteX(data);

        character.WriteY(data);

        character.WriteSByteZ(data);

        character.WriteDirection(data);

        character.WriteHue(data);

        character.WriteStatus(data);

        character.WriteNotoriety(data);

        data.WriteInt(0);

        EndOutgoingPacket(data);

        state.Send(data);
    }

    [Priority(1.0)]
    public void OnPacketOpenPaperDoll(TState state)
    {
        var character = state.Character;

        var data = BeginOutgoingNoSizePacket(0x88);

        character.WriteId(data);

        character.WriteName(data);

        data.Offset += 30;

        character.WriteStatus(data);

        EndOutgoingNoSizePacket(data);

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

    [Priority(1.0)]
    public void OnInternalShardAccountOnline(TState state)
    {
        var data = BeginInternalOutgoingNoSizePacket(0x01);

        state.Account.WriteName(data);

        EndOutgoingNoSizePacket(data);

        SendToLogin(data);
    }

    [Priority(1.0)]
    public void OnInternalShardAccountOffline(TState state)
    {
        var data = BeginInternalOutgoingNoSizePacket(0x02);

        state.Account.WriteName(data);

        EndOutgoingNoSizePacket(data);

        SendToLogin(data);
    }
    private void SendToLogin(TData data)
    {
        Login.SendInternal(data);
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