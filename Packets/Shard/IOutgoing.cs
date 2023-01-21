using Packets.Attributes.Outgoing;

// ReSharper disable once CheckNamespace
namespace Packets.Shard.Domain;

public partial interface IShard<out TLogin, in TState, TData, TAccount, TMobile, out TMobileCollection, TMap, TSkill, TSkillArray>
{
    [Packet(0x11)]
    [Sized]
    public void OnPacketMobileStatus(TState state, TData data)
    {
        var character = state.Character;

        character.WriteId(data);

        character.WriteName(data);

        character.WriteHits(data);

        character.WriteMode(data);

        state.WriteExpansions(data);

        if (!state.T2A) return;

        character.WriteGender(data);

        character.WriteStrength(data);

        character.WriteDexterity(data);

        character.WriteIntelligence(data);

        character.WriteStamina(data);

        character.WriteMana(data);

        character.WriteGold(data);

        character.WriteArmor(data);

        character.WriteWeight(data);
    }

    [Packet(0x1B)]
    public void OnPacketLoginConfirm(TState state, TData data)
    {
        var character = state.Character;

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
    public void OnPacketZMove(TState state, TData data)
    {
        var character = state.Character;

        var map = character.Map;

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
    }

    [Packet(0x22)]
    public void OnPacketOkMove(TState state, TData data)
    {
        state.WriteStatus(data);

        state.Character.WriteNotoriety(data);
    }

    [Packet(0x3A)]
    [Sized]
    public void OnPacketSkills(TState state, TData data)
    {
        data.WriteByte(0x00);

        state.Character.WriteSkills(data);
    }

    [Packet(0x4E)]
    public void OnPacketLight(TState state, TData data)
    {
        var character = state.Character;

        character.WriteId(data);

        character.WriteLight(data);
    }

    [Packet(0x4F)]
    public void OnPacketSunlight(TState state, TData data)
    {
        state.Character.WriteSunlight(data);
    }

    [Packet(0x55)]
    public void OnPacketLoginComplete(TState state, TData data)
    {
    }

    [Packet(0x5B)]
    public void OnPacketGameTime(TState state, TData data)
    {
        WriteGameTime(data);
    }

    [Packet(0x65)]
    public void OnPacketWeatherChange(TState state, TData data)
    {
        state.Character.WriteWeather(data);
    }

    [Packet(0x72)]
    public void OnPacketCombatResponse(TState state, TData data)
    {
        state.WriteCombat(data);
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
    public void OnPacketEquippedMobile(TState state, TData data)
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

        data.WriteInt(0);
    }

    [Packet(0x88)]
    public void OnPacketOpenPaperDoll(TState state, TData data)
    {
        var character = state.Character;

        character.WriteId(data);

        character.WriteName(data);

        data.Offset += 30;

        character.WriteStatus(data);
    }

    [Packet(0xA9)]
    [Sized]
    public void OnPacketCitiesAndCharacters(TState state, TData data)
    {
        state.Account.WriteCharacters(data);

        WriteCities(data);
    }
}