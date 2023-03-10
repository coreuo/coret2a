using Core.Abstract.Attributes;

namespace Scripts.CharacterList;

[Entity("Shard", "State")]
public interface IState<TAccount, in TCharacter, TCharacterCollection>
    where TAccount : IAccount<TCharacter, TCharacterCollection>
    where TCharacter : ICharacter
    where TCharacterCollection : ICollection<TCharacter>
{
    TAccount Account { get; set; }

    TCharacter Character { set; }

    uint Slot { get; }

    bool AuthorizationSuccess { get; }
}