using Core.Abstract.Attributes;

namespace Scripts.CharacterLogin;

[Entity("Shard", "Account")]
public interface IAccount<TCharacter, out TCharacterCollection>
    where TCharacter : ICharacter
    where TCharacterCollection : ICollection<TCharacter>
{
    TCharacterCollection Characters { get; }
}