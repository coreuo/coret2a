using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Server.Shard;

[Entity("Shard", "Server")]
public interface IShard<TState, TAccount, TCharacter>
    where TState : IState<TAccount>
    where TAccount : IAccount
    where TCharacter : IMobile
{
    void Listen();

    void Slice();

    void PacketCitiesAndCharacters(TState state);

    void PacketLoginConfirm(TState state);

    void PacketZMove(TState state);

    void PacketSunlight(TState state);

    void PacketLight(TState state);

    void PacketCombat(TState state);

    void PacketEquippedMobile(TState state);

    void PacketLoginComplete(TState state);

    void PacketWeatherChange(TState state);

    void PacketGameTime(TState state);

    void PacketOkMove(TState state);

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

        //PacketLoginConfirm(state);

        PacketCombat(state);

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
}