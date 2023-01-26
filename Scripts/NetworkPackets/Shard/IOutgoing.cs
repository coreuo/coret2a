using NetworkPackets.Attributes.Outgoing;

// ReSharper disable once CheckNamespace
namespace NetworkPackets.Shard.Domain;

public partial interface IShard<out TLogin, in TState, TData, TAccount, TMobile, out TMobileCollection, TMap, TSkill, TSkillArray>
{
    #region Internal
    [InternalLoginPacket(0x01)]
    public void OnInternalAccountOnline(TState state, TData data)
    {
        state.Account.WriteName(data);
    }

    [InternalLoginPacket(0x02)]
    public void OnInternalAccountOffline(TState state, TData data)
    {
        state.Account.WriteName(data);
    }
    #endregion

    [Packet(0x11)]
    [Sized]
    public void OnPacketMobileStatus(TState state, TMobile mobile, TData data)
    {
        mobile.WriteId(data);

        mobile.WriteName(data);

        mobile.WriteHits(data);

        mobile.WriteMode(data);

        state.WriteExpansions(data);

        if (!state.T2A) return;

        mobile.WriteGender(data);

        mobile.WriteStrength(data);

        mobile.WriteDexterity(data);

        mobile.WriteIntelligence(data);

        mobile.WriteStamina(data);

        mobile.WriteMana(data);

        mobile.WriteGold(data);

        mobile.WriteArmor(data);

        mobile.WriteWeight(data);
    }

    [Packet(0x1B)]
    public void OnPacketLoginConfirm(TState state, TMobile character, TData data)
    {
        var map = character.Map;

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
    }

    [Packet(0x20)]
    public void OnPacketZMove(TState state, TMobile mobile, TData data)
    {
        var map = mobile.Map;

        mobile.WriteId(data);

        mobile.WriteBody(data);

        data.WriteByte(0);

        mobile.WriteHue(data);

        mobile.WriteStatus(data);

        mobile.WriteX(data);

        mobile.WriteY(data);

        map.WriteUShortAreaId(data);

        mobile.WriteDirection(data);

        mobile.WriteSByteZ(data);
    }

    [Packet(0x22)]
    public void OnPacketOkMove(TState state, byte notoriety, TData data)
    {
        state.WriteStatus(data);

        data.WriteByte(notoriety);
    }

    [Packet(0x3A)]
    [Sized]
    public void OnPacketSkills(TState state, TMobile character, TData data)
    {
        data.WriteByte(0x00);

        character.WriteSkills(data);
    }

    [Packet(0x4E)]
    public void OnPacketLight(TState state, TMobile mobile, byte light, TData data)
    {
        mobile.WriteId(data);

        data.WriteByte(light);
    }

    [Packet(0x4F)]
    public void OnPacketSunlight(TState state, byte sunLight, TData data)
    {
        data.WriteByte(sunLight);
    }

    [Packet(0x55)]
    public void OnPacketLoginComplete(TState state, TData data)
    {
    }

    [Packet(0x5B)]
    public void OnPacketGameTime(TState state, byte hour, byte minute, byte second, TData data)
    {
        data.WriteByte(hour);

        data.WriteByte(minute);

        data.WriteByte(second);
    }

    [Packet(0x65)]
    public void OnPacketWeatherChange(TState state, byte type, byte particles, byte temperature, TData data)
    {
        data.WriteByte(type);

        data.WriteByte(particles);

        data.WriteByte(temperature);
    }

    [Packet(0x72)]
    public void OnPacketCombatResponse(TState state, bool combat, TData data)
    {
        data.WriteBool(combat); //IsFighting

        data.WriteByte(0); //AutoFight

        data.WriteByte(0x32); //Aggressiveness

        data.WriteByte(0); //RetreatSensitivity
    }

    [Packet(0x73)]
    public void OnPacketPingResponse(TState state, TData data)
    {
        state.WritePing(data);
    }

    [Packet(0x77)]
    public void OnPacketNakedMobile(TState state, TData data)
    {
        var character = state.Character;

        character.WriteId(data);

        character.WriteBody(data);

        character.WriteX(data);

        character.WriteY(data);

        character.WriteSByteZ(data);

        character.WriteDirection(data);

        character.WriteHue(data);

        character.WriteStatus(data);

        character.WriteNotoriety(data);
    }

    [Packet(0x78)]
    [Sized]
    public void OnPacketEquippedMobile(TState state, TMobile mobile, TData data)
    {
        mobile.WriteId(data);

        mobile.WriteBody(data);

        mobile.WriteX(data);

        mobile.WriteY(data);

        mobile.WriteSByteZ(data);

        mobile.WriteDirection(data);

        mobile.WriteHue(data);

        mobile.WriteStatus(data);

        mobile.WriteNotoriety(data);

        data.WriteInt(0);
    }

    [Packet(0x88)]
    public void OnPacketOpenPaperDoll(TState state, TData data)
    {
        var character = state.Character;

        character.WriteId(data);

        character.WriteFullName(data);

        character.WriteStatus(data);
    }

    [Packet(0xA9)]
    [Sized]
    public void OnPacketCitiesAndCharacters(TState state, TData data)
    {
        state.Account.WriteCharacters(data);

        WriteCities(data);
    }

    [Packet(0xB8)]
    [Sized]
    public void OnPacketCharacterProfileResponse(TState state, TMobile mobile, TData data)
    {
        mobile.WriteId(data);

        data.WriteAsciiTerminated(mobile.Name);

        data.WriteBigUnicodeTerminated(mobile.StaticProfileText);

        data.WriteBigUnicodeTerminated(mobile.DynamicProfileText);
    }
}