using Core.Abstract.Attributes;

namespace Accounting.Shard;

[Entity("Account")]
public interface IAccount<TCharacter, out TCharacterCollection>
    where TCharacter : ICharacter
    where TCharacterCollection : ICollection<TCharacter>
{
    [Size(30)]
    Span<char> Name { get; }

    [Size(30)]
    Span<char> Password { get; }

    int AccessKey { get; }

    TCharacterCollection Characters { get; }
}