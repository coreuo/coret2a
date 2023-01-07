using Core.Abstract.Attributes;

namespace Accounting.Shard;

[Entity("Shard", "Account")]
public interface IAccount<TCharacter, out TCharacterCollection>
    where TCharacter : ICharacter
    where TCharacterCollection : ICollection<TCharacter>
{
    [Size(30)]
    Span<char> Username { get; }

    [Size(30)]
    Span<char> Password { get; }

    int AccessKey { get; set; }

    TCharacterCollection Characters { get; }
}