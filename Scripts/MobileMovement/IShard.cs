using Core.Abstract.Attributes;

namespace Scripts.MobileMovement
{
    [Entity("Shard", "Server")]
    public interface IShard<TState, TMobile>
        where TState : IState<TMobile>
        where TMobile : IMobile
    {
        void PacketZMove(TState state, TMobile mobile);

        void PacketOkMove(TState state, byte notoriety);

        [Priority(2.0)]
        public void OnLoginCharacter(TState state, TMobile character)
        {
            PacketZMove(state, character);
        }

        [Priority(1.0)]
        public void OnPacketRequestMove(TState state)
        {
            var character = state.Character;

            PacketOkMove(state, character.Notoriety);
        }
    }
}
