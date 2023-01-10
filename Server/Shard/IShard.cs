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

    void PacketCitiesAndCharacters(TState state);

    void PacketLoginConfirm(TState state);

    void PacketZMove(TState state);

    void PacketSunlight(TState state);

    void PacketLight(TState state);

    void PacketCombatResponse(TState state);

    void PacketEquippedMobile(TState state);

    void PacketLoginComplete(TState state);

    void PacketWeatherChange(TState state);

    void PacketGameTime(TState state);

    void PacketOkMove(TState state);

    void PacketOpenPaperDoll(TState state);

    void PacketSkills(TState state);

    void PacketNakedMobile(TState state);

    [Priority(1.0)]
    public void OnPacketPostLogin(TState state)
    {
        if (Is.Default(state.Account)) throw new InvalidOperationException("Invalid account.");

        PacketCitiesAndCharacters(state);
    }

    [Priority(1.0)]
    public void OnPacketPreLogin(TState state)
    {
        if (Is.Default(state.Account)) throw new InvalidOperationException("Invalid account.");

        PacketLoginConfirm(state);

        PacketZMove(state);

        PacketSunlight(state);

        PacketLight(state);

        PacketLoginConfirm(state);

        PacketCombatResponse(state);

        PacketEquippedMobile(state);

        PacketLoginComplete(state);

        PacketWeatherChange(state);

        PacketGameTime(state);
    }

    [Priority(1.0)]
    public void OnPacketRequestMove(TState state)
    {
        PacketOkMove(state);
    }

    [Priority(1.0)]
    public void OnPacketRequestObjectUse(TState state)
    {
        if (state.IsSelfTargeted()) PacketOpenPaperDoll(state);
    }

    [Priority(1.0)]
    public void OnPacketClientQuery(TState state)
    {
        switch (state.Mode)
        {
            case 0x05:
            {
                PacketSkills(state);

                return;
            }
        }
    }

    [Priority(1.0)]
    public void OnPacketCombatRequest(TState state)
    {
        state.TransferCombat();

        PacketNakedMobile(state);

        PacketCombatResponse(state);
    }
}