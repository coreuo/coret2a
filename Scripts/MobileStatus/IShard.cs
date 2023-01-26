using Core.Abstract.Attributes;

namespace Scripts.MobileStatus
{
    [Entity("Shard", "Server")]
    public interface IShard<TState, TMobile>
        where TState : IState<TMobile>
        where TMobile : IMobile
    {
        void PacketMobileStatus(TState state, TMobile mobile);

        [Priority(1.0)]
        [Case("PacketClientQuery", "state", "Mode", 0x04)]
        public void OnPacketClientQueryMobileStatus(TState state)
        {
            var character = state.Character;

            PacketMobileStatus(state, character);
        }
    }
}
