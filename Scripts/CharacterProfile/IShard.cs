using Core.Abstract.Attributes;

namespace Scripts.CharacterProfile
{
    [Entity("Shard", "Server")]
    public interface IShard<TState, TMobile>
        where TState : IState<TMobile>
        where TMobile : IMobile
    {
        TMobile Get(int id);

        void PacketCharacterProfileResponse(TState state, TMobile mobile);

        [Priority(1.0)]
        [Case("PacketCharacterProfileRequest", "state", "Mode", 0x00)]
        public void OnPacketCharacterProfileRequestRead(TState state)
        {
            PacketCharacterProfileResponse(state, state.Character);
        }

        [Priority(1.0)]
        [Case("PacketCharacterProfileRequest", "state", "Mode", 0x01)]
        public void OnPacketCharacterProfileRequestWrite(TState state)
        {
            if (state.Target != state.Character.Id)
                throw new InvalidOperationException("You cannot change someone else character profile.");
        }

        [Priority(1.0)]
        [Case("PacketCharacterProfileRequestWrite", "state", "Command", 0x01)]
        public void OnPacketCharacterProfileRequestWriteDynamicProfileText(TState state)
        {
            var mobile = Get(state.Target);

            var dynamicProfileText = mobile.DynamicProfileText;

            dynamicProfileText.Clear();

            state.Text[..state.Length].CopyTo(dynamicProfileText);
        }
    }
}
