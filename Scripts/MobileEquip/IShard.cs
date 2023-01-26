using Core.Abstract.Attributes;

namespace Scripts.MobileEquip
{
    [Entity("Shard", "Server")]
    public interface IShard<TState, TMobile>
        where TState : IState<TMobile>
        where TMobile : IMobile
    {
        void PacketEquippedMobile(TState state, TMobile character);

        [Priority(5.0)]
        public void OnLoginCharacter(TState state, TMobile character)
        {
            PacketEquippedMobile(state, character);
        }
    }
}
