using Core.Abstract.Attributes;

namespace Scripts.MobileSkills
{
    [Entity("Shard", "Server")]
    public interface IShard<TState, TMobile>
        where TState : IState<TMobile>
        where TMobile : IMobile
    {
        void PacketSkills(TState state, TMobile character);

        [Priority(1.0)]
        [Case("PacketClientQuery", "state", "Mode", 0x05)]
        public void OnPacketClientQuerySkills(TState state)
        {
            var character = state.Character;

            PacketSkills(state, character);
        }
    }
}
