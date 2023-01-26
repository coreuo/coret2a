using NetworkPackets.Attributes.Incoming;

// ReSharper disable once CheckNamespace
namespace NetworkPackets.Shard.Domain;

public partial interface IShard<out TLogin, in TState, TData, TAccount, TMobile, out TMobileCollection, TMap, TSkill, TSkillArray>
{
    #region Internal
    [InternalPacket(0x00)]
    public void OnInternalShardAuthorization(TData data)
    {
        ReadName(data);

        ReadPassword(data);

        ReadAccessKey(data);
    }
    #endregion

    [Packet(0x00)]
    public void OnPacketLogin(TState state, TData data)
    {
        state.ReadPattern(data);

        state.ReadX(data);

        state.ReadY(data);

        state.ReadSByteZ(data);

        state.ReadName(data);

        state.ReadPassword(data);

        state.ReadGender(data);

        state.ReadStrength(data);

        state.ReadDexterity(data);

        state.ReadIntelligence(data);

        state.ReadFirstSkillId(data);

        state.ReadFirstSkillValue(data);

        state.ReadSecondSkillId(data);

        state.ReadSecondSkillValue(data);

        state.ReadThirdSkillId(data);

        state.ReadThirdSkillValue(data);

        state.ReadSkinHue(data);

        state.ReadHairId(data);

        state.ReadHairHue(data);

        state.ReadBeardId(data);

        state.ReadBeardHue(data);

        state.ReadTown(data);

        state.ReadCity(data);

        state.ReadCharacterSlot(data);

        state.ReadSeed(data);
    }

    [Packet(0x02)]
    public void OnPacketRequestMove(TState state, TData data)
    {
        state.ReadDirection(data);

        state.ReadStatus(data);
    }

    [Packet(0x06)]
    public void OnPacketRequestObjectUse(TState state, TData data)
    {
        state.ReadTarget(data);
    }

    [Packet(0x34)]
    public void OnPacketClientQuery(TState state, TData data)
    {
        state.ReadPattern(data);

        state.ReadMode(data);

        state.ReadTarget(data);
    }

    [Packet(0x5D)]
    public void OnPacketPreLogin(TState state, TData data)
    {
        state.ReadPattern(data);

        state.ReadName(data);

        state.ReadPassword(data);

        state.ReadCharacterSlot(data);

        state.ReadSeed(data);
    }

    [Packet(0x72)]
    public void OnPacketCombatRequest(TState state, TData data)
    {
        state.ReadCombat(data);
    }

    [Packet(0x73)]
    public void OnPacketPingRequest(TState state, TData data)
    {
        state.ReadPing(data);
    }

    [Packet(0x91)]
    public void OnPacketPostLogin(TState state, TData data)
    {
        state.ReadAccessKey(data);

        state.ReadName(data);

        state.ReadPassword(data);
    }

    [Packet(0xA7)]
    public void OnPacketRequestTip(TState state, TData data)
    {
        state.ReadTipRequest(data);

        state.ReadDirection(data);
    }

    [Packet(0xB8)]
    [Sized]
    public void OnPacketCharacterProfileRequest(TState state, TData data)
    {
        state.ReadMode(data);

        state.ReadTarget(data);

        if (state.Mode == 0) return;

        state.ReadCommand(data);

        state.ReadText(data);
    }
}