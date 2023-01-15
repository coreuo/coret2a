using Core.Abstract.Attributes;

namespace Accounting.Shard;

[Entity("Shard", "Account")]
public interface IAccount<TCharacter, out TCharacterCollection>
    where TCharacter : ICharacter
    where TCharacterCollection : ICollection<TCharacter>
{
    Span<char> Name { get; }

    Span<char> Password { get; }

    int AccessKey { get; set; }

    TCharacterCollection Characters { get; }
}