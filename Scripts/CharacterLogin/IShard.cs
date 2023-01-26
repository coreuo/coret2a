using Core.Abstract.Attributes;

namespace Scripts.CharacterLogin
{
    [Entity("Shard", "Server")]
    public interface IShard<in TState, TAccount, out TAccountCollection, TCharacter, TCharacterCollection>
        where TState : IState<TAccount, TCharacter, TCharacterCollection>
        where TAccount : IAccount<TCharacter, TCharacterCollection>
        where TAccountCollection : ICollection<TAccount>
        where TCharacter : ICharacter
        where TCharacterCollection : ICollection<TCharacter>
    {
        void LoginCharacter(TState state, TCharacter character);

        void PacketLoginConfirm(TState state, TCharacter character);

        void PacketLoginComplete(TState state);

        [Priority(1.0)]
        public void OnPacketPreLogin(TState state)
        {
            var characters = state.Account.Characters;

            var slot = state.Slot;

            if (!(slot < characters.Count)) return;

            var character = state.Character = characters.ElementAt((int)slot);

            LoginCharacter(state, character);
        }

        [Priority(1.0)]
        [Link("LoginCharacter")]
        public void OnLoginBegin(TState state, TCharacter character)
        {
            PacketLoginConfirm(state, character);
        }

        [Priority(10.0)]
        [Link("LoginCharacter")]
        public void OnLoginEnd(TState state)
        {
            PacketLoginComplete(state);
        }
    }
}
