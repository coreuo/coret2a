using Core.Abstract.Attributes;

namespace Scripts.CharacterProfile
{
    [Entity("Shard", "Server")]
    public interface IShard<TState, TMobile>
        where TState : IState<TMobile>
        where TMobile : IMobile
    {
        TMobile Get(int id);

        void PacketOpenPaperDoll(TState state);

        void PacketCharacterProfileResponse(TState state, TMobile mobile);

        [Priority(1.0)]
        public void OnPacketRequestObjectUse(TState state)
        {
            if (state.Character.Id == state.Target) PacketOpenPaperDoll(state);
        }

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
        }

        [Priority(1.0)]
        [Case("PacketCharacterProfileRequestWrite", "state", "Command", 0x01)]
        public void OnPacketCharacterProfileRequestWriteDynamicProfileText(TState state)
        {
            //You cannot change someone else character profile.
            if (state.Target != state.Character.Id) return;

            var mobile = Get(state.Target);

            var dynamicProfileText = mobile.DynamicProfileText;

            dynamicProfileText.Clear();

            state.Text[..state.Length].CopyTo(dynamicProfileText);
        }
    }
}
