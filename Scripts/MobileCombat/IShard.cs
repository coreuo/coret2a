using Core.Abstract.Attributes;

namespace Scripts.MobileCombat
{
    [Entity("Shard", "Server")]
    public interface IShard<TState, TMobile>
        where TState : IState<TMobile>
        where TMobile : IMobile
    {
        void PacketCombatResponse(TState state, bool combat);

        void PacketNakedMobile(TState state, TMobile mobile);

        [Priority(5.0)]
        public void OnLoginCharacter(TState state, TMobile character)
        {
            PacketCombatResponse(state, character.Combat);
        }

        [Priority(1.0)]
        public void OnPacketCombatRequest(TState state)
        {
            var character = state.Character;

            var combat = character.Combat = state.Combat;

            PacketNakedMobile(state, character);

            PacketCombatResponse(state, combat);
        }
    }
}
