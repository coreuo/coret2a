using Core.Abstract.Attributes;

namespace Scripts.CharacterList
{
    [Entity("Shard", "Server")]
    public interface IShard<in TState, TAccount, out TAccountCollection, TCharacter, TCharacterCollection>
        where TState : IState<TAccount, TCharacter, TCharacterCollection>
        where TAccount : IAccount<TCharacter, TCharacterCollection>
        where TAccountCollection : ICollection<TAccount>
        where TCharacter : ICharacter
        where TCharacterCollection : ICollection<TCharacter>
    {
        void PacketCitiesAndCharacters(TState state, TCharacterCollection characters);

        [Priority(2.0)]
        [Case("PacketPostLogin", nameof(state), nameof(state.AuthorizationSuccess), true)]
        public void OnSuccessfulShardAuthorization(TState state)
        {
            var characters = state.Account.Characters;

            PacketCitiesAndCharacters(state, characters);
        }
    }
}
